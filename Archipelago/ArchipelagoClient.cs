using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.MessageLog.Messages;
using Archipelago.MultiClient.Net.Packets;
using GnosiaArchipelagoRandomizer.Utils;
using HarmonyLib;
using UnityEngine;
using GnosiaArchipelagoRandomizer.Patches.Optional;
using Archipelago.MultiClient.Net.Models;
using HarmonyLib.Tools;
using gnosia;
using coreSystem;

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
            session.MessageLog.OnMessageReceived += message => ArchipelagoConsole.LogMessage(message.ToString());
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
                    //Get settings
                    Dictionary<string, object> slotData = ServerData.GetSlotData();
                    if (slotData["death_link"] != null)
                        enableDeathLink = Convert.ToBoolean(slotData["death_link"]);

                    //Apply optional patches (except Death Link)
                    var harmony = new Harmony(Plugin.PluginGUID + ".optional");
                    if (Convert.ToBoolean(slotData["randomize_character_unlocks"]))
                    {
                        harmony.CreateClassProcessor(typeof(CharacterRandomizerPatch)).Patch();
                        Plugin.BepinLogger.LogInfo("CharacterRandomizerPatch Applied!");
                    }
                    if (Convert.ToBoolean(slotData["allow_gender_specific_logic"]))
                    {
                        harmony.CreateClassProcessor(typeof(MoreRespecPatch)).Patch();
                        harmony.CreateClassProcessor(typeof(ReCharacterCreationPatch)).Patch();
                        Plugin.BepinLogger.LogInfo("MoreRespecPatch Applied!");
                    }
                    switch (Convert.ToInt64(slotData["goal"]))
                    {
                        case 0:
                            harmony.CreateClassProcessor(typeof(NormalEndingGoal)).Patch();
                            Plugin.BepinLogger.LogInfo("NormalEndingGoal Patch Applied!");
                            break;

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
            //Unsubscribe from invalid session facts
            session.MessageLog.OnMessageReceived -= message => ArchipelagoConsole.LogMessage(message.ToString());
            session.Items.ItemReceived -= OnItemReceived;
            session.Socket.ErrorReceived -= OnSessionErrorReceived;
            session.Socket.SocketClosed -= OnSessionSocketClosed;
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
                    switch (id)
                    {
                        //TODO: Move messages from LocationPatches to here
                        default:
                            sp.SetDialogScreen(50400U, "You got a skill!\nAnd I forgot to replace this message!", 2, false);
                            break;
                    }                    
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
    }
}