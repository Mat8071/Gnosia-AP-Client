using System;
using System.Linq;
using System.Threading;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.MessageLog.Messages;
using Archipelago.MultiClient.Net.Models;
using Archipelago.MultiClient.Net.Packets;
using coreSystem;
using GnosiaArchipelagoRandomizer.Patches.Optional;
using GnosiaArchipelagoRandomizer.Utils;
using HarmonyLib;
using UnityEngine;

namespace GnosiaArchipelagoRandomizer.Archipelago
{
    public class ArchipelagoClient
    {
        public const string APVersion = "0.6.7";
        private const string Game = "Gnosia";

        public static bool Authenticated;
        private bool attemptingConnection;

        public static ArchipelagoData ServerData = new();
        private DeathLinkHandler DeathLinkHandler;
        private ArchipelagoSession session;

        /// <summary>
        /// call to connect to an Archipelago session. Connection info should already be set up on ServerData
        /// </summary>
        /// <returns></returns>
        public void Connect()
        {
            if (Authenticated || attemptingConnection) return;

            attemptingConnection = true;

            try
            {
                session = ArchipelagoSessionFactory.CreateSession(ServerData.Uri);
                SetupSession();
            }
            catch (Exception e)
            {
                Plugin.BepinLogger.LogError(e);
            }

            TryConnect();
        }

        /// <summary>
        /// add handlers for Archipelago events
        /// </summary>
        private void SetupSession()
        {
            session.MessageLog.OnMessageReceived += OnMessageReceived;
            session.Items.ItemReceived += OnItemReceived;
            session.Socket.ErrorReceived += OnSessionErrorReceived;
            session.Socket.SocketClosed += OnSessionSocketClosed;
        }

        /// <summary>
        /// attempt to connect to the server with our connection info
        /// </summary>
        private void TryConnect()
        {
            try
            {
                // it's safe to thread this function call but unity notoriously hates threading so do not use excessively
                ThreadPool.QueueUserWorkItem(
                    _ => HandleConnectResult(
                        session.TryConnectAndLogin(
                            Game,
                            ServerData.SlotName,
                            ItemsHandlingFlags.AllItems,
                            new Version(APVersion),
                            password: ServerData.Password,
                            requestSlotData: ServerData.NeedSlotData
                        )));
            }
            catch (Exception e)
            {
                Plugin.BepinLogger.LogError(e);
                HandleConnectResult(new LoginFailure(e.ToString()));
                attemptingConnection = false;
            }
        }

        /// <summary>
        /// handle the connection result and do things
        /// </summary>
        /// <param name="result"></param>
        private void HandleConnectResult(LoginResult result)
        {
            string outText;
            if (result.Successful)
            {
                var success = (LoginSuccessful)result;

                //Save connection info for next time
                ConnectionInfo info = new ConnectionInfo { Uri = ServerData.Uri, SlotName = ServerData.SlotName };
                Plugin.SaveConnectionInfo(info);

                //Check if trying to change slot
                if (ServerData.GetSeed() != null && session.RoomState.Seed != ServerData.GetSeed())
                {
                    //Prevent connection and send message through console
                    Disconnect();
                    ArchipelagoConsole.LogMessage("The slot you were trying to connect to is using a different seed from the loaded data. If you are trying to change slots, please close and reopen the game first so that save data doesn't get mixed up");
                    attemptingConnection = false;
                    return;
                }

                ServerData.SetupSession(success.SlotData, session.RoomState.Seed);
                Authenticated = true;

                //Goal if somehow you lost connection, goaled offline and reconnected
                if (Plugin.IsGoalCompleted())
                {
                    Plugin.CompleteGoal();
                }

                bool enableDeathLink = false;

                //We need to get the settings before starting the title screen or the patches won't apply
                try
                {
                    //Get Slot Data
                    var slotData = ServerData.SlotData;
                    CheckVersionCompatibility(slotData.Version);
                    var options = slotData.Options;
                    enableDeathLink = options?.DeathLink ?? false;

                    //Apply optional patches (except Death Link)
                    var harmony = new Harmony(Plugin.PluginGUID + ".optional");
                    bool achievementsPatchAlreadyApplied = false;
                    //Apply Location Patches
                    if ((options?.RandomizeNotes ?? true) || (options?.RandomizeSkills ?? true))
                    {
                        harmony.CreateClassProcessor(typeof(MixedLocationsPatch)).Patch();
                        Plugin.BepinLogger.LogInfo("MixedLocationsPatch Applied!");
                    }
                    if (options?.RandomizeNotes ?? true)
                    {
                        harmony.CreateClassProcessor(typeof(NoteLocationsPatch)).Patch();
                        harmony.CreateClassProcessor(typeof(EventSearchInitializePatch)).Patch();
                        Plugin.BepinLogger.LogInfo("NoteLocationsPatch Applied!");
                        Plugin.BepinLogger.LogInfo("EventSearchInitializePatch Applied!");
                    }
                    if (options?.RandomizeSkills ?? true)
                    {
                        harmony.CreateClassProcessor(typeof(NextDayPatch)).Patch();
                        harmony.CreateClassProcessor(typeof(SkillLocationsPatch)).Patch();
                        Plugin.BepinLogger.LogInfo("NextDayPatch Applied!");
                        Plugin.BepinLogger.LogInfo("SkillLocationsPatch Applied!");
                    }
                    if (options?.AddRoleAchievementLocations ?? false)
                    {
                        harmony.CreateClassProcessor(typeof(HandleAchievementsPatch)).Patch();
                        Plugin.BepinLogger.LogInfo("HandleAchievementsPatch Applied!");
                        achievementsPatchAlreadyApplied = true;
                    }
                    if ((options?.AddWinWithCharacterLocations ?? false) ||
                        (options?.AddWinAgainstCharacterLocations ?? false) ||
                        (options?.AddWinAsRoleLocations ?? false) ||
                        (options?.AddWinAgainstRoleLocations ?? false))
                    {
                        harmony.CreateClassProcessor(typeof(WinLocationsPatch)).Patch();
                        Plugin.BepinLogger.LogInfo("WinLocationsPatch Applied!");
                    }
                    //Apply direct setting-based patches
                    if (options?.RandomizeCharacterUnlocks ?? false)
                    {
                        harmony.CreateClassProcessor(typeof(CharacterRandomizerPatch)).Patch();
                        Plugin.BepinLogger.LogInfo("CharacterRandomizerPatch Applied!");
                    }
                    if ((options?.ExpMultiplier ?? 1) != 1)
                    {
                        harmony.CreateClassProcessor(typeof(ExpMultiplierPatch)).Patch();
                        Plugin.BepinLogger.LogInfo("ExpMultiplierPatch Applied!");
                    }
                    if (options?.AllowGenderSpecificLogic ?? false)
                    {
                        harmony.CreateClassProcessor(typeof(MoreRespecPatch)).Patch();
                        harmony.CreateClassProcessor(typeof(ReCharacterCreationPatch)).Patch();
                        Plugin.BepinLogger.LogInfo("MoreRespecPatch Applied!");
                        Plugin.BepinLogger.LogInfo("ReCharacterCreationPatch Applied!");
                    }
                    //Apply other patches that depend on multiple settings
                    if ((options?.TutorialHandling ?? ArchipelagoData.TutorialHandling.Vanilla) == ArchipelagoData.TutorialHandling.Vanilla
                        && (options?.RandomizeRoleUnlocks ?? true))
                    {
                        harmony.CreateClassProcessor(typeof(AfterBugAllRolesPatch)).Patch();
                        harmony.CreateClassProcessor(typeof(NextLoopPatch)).Patch();
                        Plugin.BepinLogger.LogInfo("AfterBugAllRolesPatch Applied!");
                        Plugin.BepinLogger.LogInfo("NextLoopPatch Applied!");
                    }
                    //Apply patches based on choice-based options
                    switch (options?.TutorialHandling)
                    {
                        case ArchipelagoData.TutorialHandling.Vanilla:
                            harmony.CreateClassProcessor(typeof(TutorialLocationsPatch)).Patch();
                            Plugin.BepinLogger.LogInfo("TutorialLocationsPatch Applied!");
                            break;

                        case ArchipelagoData.TutorialHandling.Skip:
                        case ArchipelagoData.TutorialHandling.SkipAndRemoveLocations:
                            harmony.CreateClassProcessor(typeof(SkipTutorialPatch)).Patch();
                            Plugin.BepinLogger.LogInfo("SkipTutorialPatch Applied!");
                            break;

                        case null:
                            Plugin.BepinLogger.LogError("Tutorial Handling Option not found! Treating as Vanilla");
                            goto case ArchipelagoData.TutorialHandling.Vanilla;

                        default:
                            Plugin.BepinLogger.LogError("Unknown Tutorial Handling Setting! Treating as Vanilla!");
                            goto case ArchipelagoData.TutorialHandling.Vanilla;
                    }
                    switch (options?.Goal)
                    {
                        case ArchipelagoData.Goal.NormalEnding:
                            harmony.CreateClassProcessor(typeof(NormalEndingGoal)).Patch();
                            Plugin.BepinLogger.LogInfo("NormalEndingGoal Patch Applied!");
                            break;

                        case ArchipelagoData.Goal.RoleAchievements:
                            if (!achievementsPatchAlreadyApplied)
                            {
                                harmony.CreateClassProcessor(typeof(HandleAchievementsPatch)).Patch();
                                Plugin.BepinLogger.LogInfo("HandleAchievementsPatch Applied!");
                            }
                            break;

                        case null:
                            Plugin.BepinLogger.LogError("Goal Option not found! Treating as Normal Ending!");
                            goto case ArchipelagoData.Goal.NormalEnding;

                        default:
                            Plugin.BepinLogger.LogError("Unknown goal setting! No goal patch applied!");
                            break;
                    }
                }
                catch (Exception e)
                {
                    Plugin.BepinLogger.LogError(e);
                }

                if (Plugin.Application != null && Plugin.applicationInitialized == false)
                {
                    //Start the title screen and stuff
                    Traverse.Create(Plugin.Application).Method("Start").GetValue();
                    Plugin.applicationInitialized = true;
                    Plugin.BepinLogger.LogInfo("Game should start now");
                }

                DeathLinkHandler?.Unsubscribe();
                DeathLinkHandler = new(session.CreateDeathLinkService(), ServerData.SlotName, enableDeathLink);
                //Reset inventory
                Plugin.inventory.Clear();
                foreach (ItemInfo itemInfo in session.Items.AllItemsReceived)
                {
                    long id = itemInfo.ItemId;
                    Plugin.inventory.Add(id);
                }
                //Send checks for locations completed before connection
                session.Locations.CompleteLocationChecksAsync(ServerData.CheckedLocations.ToArray());
                //Sync locations completed with server
                foreach (long location in session.Locations.AllLocationsChecked)
                {
                    //We don't need to check if location is already in there cause it's a set now
                    ServerData.CheckedLocations.Add(location);
                }
                //Update item state
                Plugin.UpdateItems();
                outText = $"Successfully connected to {ServerData.Uri} as {ServerData.SlotName}!";

                ArchipelagoConsole.LogMessage(outText);
            }
            else
            {
                var failure = (LoginFailure)result;
                outText = $"Failed to connect to {ServerData.Uri} as {ServerData.SlotName}.";
                outText = failure.Errors.Aggregate(outText, (current, error) => current + $"\n    {error}");

                Plugin.BepinLogger.LogError(outText);

                Authenticated = false;
                Disconnect();
            }

            ArchipelagoConsole.LogMessage(outText);
            attemptingConnection = false;
        }

        /// <summary>
        /// something went wrong, or we need to properly disconnect from the server. cleanup and re null our session
        /// </summary>
        private void Disconnect()
        {
            if (session != null)
            {
                //Unsubscribe from invalid session facts
                session.MessageLog.OnMessageReceived -= OnMessageReceived;
                session.Items.ItemReceived -= OnItemReceived;
                session.Socket.ErrorReceived -= OnSessionErrorReceived;
                session.Socket.SocketClosed -= OnSessionSocketClosed;
            }
            //Base function from the template
            Plugin.BepinLogger.LogDebug("disconnecting from server...");
            session?.Socket.DisconnectAsync();
            session = null;
            Authenticated = false;
        }

        public void SendMessage(string message)
        {
            session.Socket.SendPacketAsync(new SayPacket { Text = message });
        }

        private void OnMessageReceived(LogMessage message)
        {
            ArchipelagoConsole.LogMessage(message.ToString());
        }

        /// <summary>
        /// we received an item so reward it here
        /// </summary>
        /// <param name="helper">item helper which we can grab our item from</param>
        private void OnItemReceived(ReceivedItemsHelper helper)
        {
            var receivedItem = helper.DequeueItem();

            if (helper.Index <= ServerData.Index) return;

            ServerData.Index++;

            //Log for debugging
            Plugin.BepinLogger.LogInfo($"Received {receivedItem.ItemDisplayName}");

            //Reward the item here
            long id = receivedItem.ItemId;
            Plugin.inventory.Add(id);
            //Check if it's a permanent upgrade
            if (id < 11000)
            {
                //Check if we're fully done connecting to the server and can read SlotData from Plugin
                if (Authenticated && !attemptingConnection)
                {
                    //Update gamedata since we received a permanent item
                    Plugin.UpdateItems();
                }
            }
            else
            {
                //It's a consumable/instant effect item
                //Try awarding the item immediately
                try
                {
                    if (!Plugin.loadedSavesAtLeastOnce) //This is to not spam errors on first connection
                        return;
                    gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                    //We found gd so the player has loaded a save file.
                    Plugin.ActivateInstantUseItem(gd, id);
                }
                catch (Exception e)
                {
                    //The player has not loaded a save file yet and there's no gd.
                    //Items will be automatically handled when they load a save file
                    Plugin.BepinLogger.LogError(e);
                }
            }
            //Display a message (under certain conditions)
            try
            {
                if (receivedItem.Player.Slot == session.ConnectionInfo.Slot) //The item is local
                    return;
                if (!receivedItem.Flags.HasFlag(ItemFlags.NeverExclude)) //The item is not useful
                    return;
                if (!Plugin.loadedSavesAtLeastOnce) //This is to prevent message spam on first connection
                    return;
                //Let's display this message
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                string playerName = receivedItem.Player.Name;
                string itemName = receivedItem.ItemDisplayName;
                string message = $"{playerName} sent you {itemName}";
                int type = receivedItem.Flags.HasFlag(ItemFlags.Advancement) ? 0 : 1;
                sp.ShowInfoUpdateMes(message, 45002U, type, true);
                //If the item is a skill, show its dialog screen
                if (id < 20)
                {
                    sp.PlaySeInScript("se_square", 1f);
                    switch (id)
                    {
                        case 1:
                            sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioTutorialText(15, 26, -1), 2, false);
                            break;
                        case 2:
                            sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioRakioText(1, 18, -1), 3, false);
                            break;
                        case 5:
                            sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioCommetText(3, 28, -1), 3, false);
                            break;
                        case 6:
                            sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioStellaText(2, 19, -1), 3, false);
                            break;
                        case 7:
                            sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioOtomeText(0, 41, -1), 3, false);
                            break;
                        case 8:
                            sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioShaminText(2, 23, -1), 3, false);
                            break;
                        case 9:
                            sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioRakioText(5, 26, -1), 3, false);
                            break;
                        case 10:
                            sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioCipiText(0, 25, -1), 3, false);
                            break;
                        case 11:
                            sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioShigeText(0, 18, -1), 3, false);
                            break;
                        case 12:
                            sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioYurikoText(0, 12, -1), 3, false);
                            break;
                        case 13:
                            sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioSetsuText(0, 22, -1), 3, false);
                            break;
                        case 14:
                            sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioJonasText(2, 16, -1), 3, false);
                            break;
                        case 15:
                            sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioSQText(1, 24, -1), 3, false);
                            break;
                        case 16:
                            sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioKukulText(5, 12, -1), 3, false);
                            break;
                        case 17:
                            sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioSQText(0, 33, -1), 3, false);
                            break;
                        case 18:
                            sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioGinaText(1, 15, -1), 3, false);
                            break;
                        case 19:
                            sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioShaminText(0, 27, -1), 3, false);
                            break;
                        default:
                            sp.SetDialogScreen(50400U, "You got a skill!\nAnd I forgot to replace this message!", 2, false);
                            break;
                    }
                    sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                }
            }
            catch (Exception e)
            {
                Plugin.BepinLogger.LogError(e);
            }
        }

        /// <summary>
        /// something went wrong with our socket connection
        /// </summary>
        /// <param name="e">thrown exception from our socket</param>
        /// <param name="message">message received from the server</param>
        private void OnSessionErrorReceived(Exception e, string message)
        {
            Plugin.BepinLogger.LogError(e);
            ArchipelagoConsole.LogMessage(message);
            //Show error message
            try
            {
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                sp.SetDialogScreen(50400U, $"Error:\n{message}\nThe game will now disconnect from the server.\nYou can reconnect the same way you connected initially", 5, false);
            }
            catch (Exception ex)
            {
                Plugin.BepinLogger.LogError(ex);
            }
            //Disconnect
            Disconnect();
        }

        /// <summary>
        /// something went wrong closing our connection. disconnect and clean up
        /// </summary>
        /// <param name="reason"></param>
        private void OnSessionSocketClosed(string reason)
        {
            Plugin.BepinLogger.LogError($"Connection to Archipelago lost: {reason}");
            //Show error message
            try
            {
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                sp.SetDialogScreen(50400U, $"Connection to Archipelago lost:\n{reason}.\nYou can reconnect the same way you connected initially", 5, false);
            }
            catch (Exception e)
            {
                Plugin.BepinLogger.LogError(e);
            }
            Disconnect();
        }

        public ArchipelagoSession GetSession()
        {
            return session;
        }

        public DeathLinkHandler GetDeathLinkHandler()
        {
            return DeathLinkHandler;
        }

        private bool CheckVersionCompatibility(string version)
        {
            string newerClientMessage =
                "The version of the client you're using is much newer " +
                "than the version of the AP World used to generate the MultiWorld " +
                "and is almost certainly incompatible. Please either regenerate the " +
                "MultiWorld with a newer version of the AP World (recommended) or " +
                "downgrade your client.";

            string olderClientMessage =
                "The version of the client you're using is much older " +
                "than the version of the AP World used to generate the MultiWorld " +
                "and is almost certainly incompatible. Please update your client to " +
                "avoid compatibility issues.";

            if (version == null)
            {
                ArchipelagoConsole.LogMessage(newerClientMessage);
                Plugin.BepinLogger.LogError(newerClientMessage);
                return false;
            }

            Version pluginVersion = new Version(Plugin.PluginVersion);
            Version apWorldVersion = new Version(version);

            if (pluginVersion.Major != apWorldVersion.Major ||
                pluginVersion.Minor != apWorldVersion.Minor)
            {
                string message = pluginVersion > apWorldVersion
                    ? newerClientMessage
                    : olderClientMessage;

                ArchipelagoConsole.LogMessage(message);
                Plugin.BepinLogger.LogError(message);
                return false;
            }

            return true;
        }
    }
}