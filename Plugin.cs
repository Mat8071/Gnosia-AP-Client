using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Models;
using Archipelago.MultiClient.Net.Packets;
using BepInEx;
using BepInEx.Logging;
using coreSystem;
using GnosiaArchipelagoRandomizer.Archipelago;
using GnosiaArchipelagoRandomizer.Utils;
using HarmonyLib;
using UnityEngine;
using sce.SampleUtil.Input;
using Mono.Cecil;
using GnosiaArchipelagoRandomizer.Patches.Core;
using GnosiaArchipelagoRandomizer.Patches;
using HarmonyLib.Tools;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.IO;
using Newtonsoft.Json;

namespace GnosiaArchipelagoRandomizer
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGUID = "com.mat8071.gnosiaAP";
        public const string PluginName = "GnosiaArchipelagoRandomizer";
        public const string PluginVersion = "0.1.4";

        public const string ModDisplayInfo = $"{PluginName} v{PluginVersion}";
        private const string APDisplayInfo = $"Archipelago v{ArchipelagoClient.APVersion}";
        public static ManualLogSource BepinLogger;
        public static ArchipelagoClient ArchipelagoClient;

        //My variables
        public static application.Application Application;
        public static bool applicationInitialized = false;
        public static bool loadedSavesAtLeastOnce = false;

        //AP Variables
        public static List<long> inventory = new List<long>();
        public static bool[] found_characters = new bool[15];
        public static bool[] found_skills = new bool[19];
        public static bool[] found_roles = new bool[9];
        public static bool[,] found_notes = new bool[15, 8];

        private static ushort[] skill_flags = new ushort[19] 
        { 
            0, 0, 0, 0, 28, 38, 33, 27, 0, 0, 0, 0, 0, 0, 23, 0, 0, 0, 0,
        };

        public static int crew_max = 5;

        public static Queue<long> instant_item_queue = new Queue<long>();
        public static List<long> items_used = new List<long>();

        private static bool goal_completed = false;

        public static bool debug_mode = false;
        private void Awake()
        {
            // Plugin startup logic
            BepinLogger = Logger;
            ArchipelagoClient = new ArchipelagoClient();
            ArchipelagoConsole.Awake();

            ArchipelagoConsole.LogMessage($"{ModDisplayInfo} loaded!");

            //Apply core patches
            var harmony = new Harmony(PluginGUID + ".core");

            harmony.CreateClassProcessor(typeof(AfterBugAllRolesPatch)).Patch();
            harmony.CreateClassProcessor(typeof(CrewLimitCursorPatch)).Patch();
            harmony.CreateClassProcessor(typeof(CrewLimitPatch)).Patch();
            harmony.CreateClassProcessor(typeof(DefaultToSlotNamePatch)).Patch();
            harmony.CreateClassProcessor(typeof(EventRequirementChangePatches)).Patch();
            harmony.CreateClassProcessor(typeof(EventSearchInitializePatch)).Patch();
            harmony.CreateClassProcessor(typeof(EventSearchPatch)).Patch();
            harmony.CreateClassProcessor(typeof(LoadDataPatch)).Patch();
            harmony.CreateClassProcessor(typeof(LocationPatches)).Patch();
            harmony.CreateClassProcessor(typeof(MustConnectBeforeTitlePatch)).Patch();
            harmony.CreateClassProcessor(typeof(NextDayPatch)).Patch();
            harmony.CreateClassProcessor(typeof(NextLoopPatch)).Patch();
            harmony.CreateClassProcessor(typeof(NoTrophyPatch)).Patch();
            harmony.CreateClassProcessor(typeof(SaveLoadAPDataPatch)).Patch();
            harmony.CreateClassProcessor(typeof(SeparateSavesPatch)).Patch();
            harmony.CreateClassProcessor(typeof(SimplifyMessagesPatch)).Patch();
            harmony.CreateClassProcessor(typeof(WWGRequirementsPatch)).Patch();

            BepinLogger.LogInfo("Core Patches Applied!");

            //Load last-used connection info
            ConnectionInfo info = LoadConnectionInfo();
            if (info != null)
            {
                ArchipelagoClient.ServerData.Uri = info.Uri;
                ArchipelagoClient.ServerData.SlotName = info.SlotName;
            }
        }

        private void Update()
        {
            ArchipelagoConsole.Update();
        }

        private void OnGUI()
        {
            // show the mod is currently loaded in the corner
            GUI.Label(new Rect(16, 16, 300, 20), ModDisplayInfo);
            ArchipelagoConsole.OnGUI();

            string statusMessage;
            // show the Archipelago Version and whether we're connected or not
            if (ArchipelagoClient.Authenticated)
            {
                // if your game doesn't usually show the cursor this line may be necessary
                // Cursor.visible = false;

                statusMessage = " Status: Connected";
                GUI.Label(new Rect(16, 50, 300, 20), APDisplayInfo + statusMessage);
            }
            else
            {
                // if your game doesn't usually show the cursor this line may be necessary
                // Cursor.visible = true;

                statusMessage = " Status: Disconnected";
                GUI.Label(new Rect(16, 50, 300, 20), APDisplayInfo + statusMessage);
                GUI.Label(new Rect(16, 70, 150, 20), "Host: ");
                GUI.Label(new Rect(16, 90, 150, 20), "Player Name: ");
                GUI.Label(new Rect(16, 110, 150, 20), "Password: ");

                ArchipelagoClient.ServerData.Uri = GUI.TextField(new Rect(150, 70, 150, 20),
                    ArchipelagoClient.ServerData.Uri);
                ArchipelagoClient.ServerData.SlotName = GUI.TextField(new Rect(150, 90, 150, 20),
                    ArchipelagoClient.ServerData.SlotName);
                ArchipelagoClient.ServerData.Password = GUI.TextField(new Rect(150, 110, 150, 20),
                    ArchipelagoClient.ServerData.Password);

                // requires that the player at least puts *something* in the slot name
                if (GUI.Button(new Rect(16, 130, 100, 20), "Connect") &&
                    !ArchipelagoClient.ServerData.SlotName.IsNullOrWhiteSpace())
                {
                    ArchipelagoClient.Connect();
                }
            }
            // this is a good place to create and add a bunch of debug buttons
            if (debug_mode)
            {
                if (GUI.Button(new Rect(16, 170, 120, 25), "Trigger DeathLink"))
                {
                    try
                    {
                        ArchipelagoClient.GetDeathLinkHandler().KillPlayer("You pressed the DeathLink button");
                    }
                    catch (Exception e)
                    {
                        BepinLogger.LogError(e);
                    }
                }
            }
        }

        public static void CompleteGoal()
        {
            try
            {
                ArchipelagoSession session = ArchipelagoClient.GetSession();
                session.SetGoalAchieved();
            }
            catch (Exception e)
            {
                BepinLogger.LogError(e);
            }
            goal_completed = true;
        }

        public static bool IsGoalCompleted()
        {
            return goal_completed;
        }

        public static void UpdateItems()
        {
            //Get settings
            Dictionary<string, object> slotData = ArchipelagoClient.ServerData.GetSlotData();
            bool randomize_character_unlocks = false;
            if (slotData != null && slotData.ContainsKey("randomize_character_unlocks"))
            {
                randomize_character_unlocks = Convert.ToBoolean(slotData["randomize_character_unlocks"]);
            }
            else
            {
                BepinLogger.LogWarning("Update Items failed!");
                return;
            }
            //Reset crew max temporarily
            crew_max = randomize_character_unlocks ? 1 : 5;
            foreach (long item in inventory)
            {
                if (item < 100)
                {
                    //It's a skill / player flag
                    if (item < 20)
                    {
                        found_skills[item - 1] = true;
                        //Check if skill is Definite Human/Enemy
                        if (item == 2)
                        {
                            //Give Definite Enemy alternate versions
                            found_skills[2] = true;
                            found_skills[3] = true;
                        }
                    }
                }
                else if (item < 1500)
                {
                    //It's a Character / Character Note / Character Flag
                    if (item % 100 == 0)
                    {
                        //It's a Character
                        found_characters[item / 100] = true;
                        if (randomize_character_unlocks)
                            crew_max += 1;
                    }
                    else
                    {
                        //It's a Character Note / Character Flag
                        found_notes[item / 100, (item % 100) - 1] = true;
                    }
                }
                else if (item < 1600)
                {
                    //It's a Role
                    found_roles[item % 1500] = true;
                }
                else if (item == 10000)
                {
                    //It's a progressive crew max item
                    if (!randomize_character_unlocks)
                        crew_max += 1;
                }
            }
            //Try updating items right now!
            try
            {
                if (!loadedSavesAtLeastOnce) //This is to not spam errors on first connection
                    return;
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                UpdateSafeGDItems(gd);
            }
            catch (Exception e)
            {
                BepinLogger.LogError(e);
            }
        }

        public static void UpdateSafeGDItems(gnosia.GameData gd)
        {
            //Set character flags
            for (int i = 1; i < found_notes.GetLength(0); i++)
            {
                if (gd.personFromId[i] >= 0)
                {
                    //Character is present in this loop
                    gnosia.GameData.character current = gd.chara[gd.personFromId[i]];
                    for (int j = 0; j < found_notes.GetLength(1); j++)
                    {
                        if (found_notes[i, j])
                        {
                            //Update flags
                            current.allFlg |= (1UL << j);
                        }
                    }
                    //Set character back and update stats
                    gd.chara[gd.personFromId[i]] = current;
                    gd.CalGnos(gd.personFromId[i]);
                }
                else
                {
                    //Character is NOT present in this loop
                    for (int j = 0; j < found_notes.GetLength(1); j++)
                    {
                        if (found_notes[i, j])
                            gd.baseData.s_chara_all_flg[i] |= (1UL << j);
                    }
                }
            }
            //Set role flags
            if (gd.baseData.loop >= 14)
            {
                for (int i = 1; i < found_roles.Length; i++)
                {
                    if (found_roles[i])
                        gd.baseData.sce_all_flg |= (1UL << i);
                }
            }
        }

        public static void UpdateSkills(gnosia.GameData gd)
        {
            //Get player
            if (gd.personFromId[0] >= 0)
            {
                gnosia.GameData.character player = gd.chara[gd.personFromId[0]];
                //Set skill flags
                for (int i = 0; i < found_skills.Length; i++)
                {
                    if (found_skills[i])
                    {
                        //Give the skill to the player
                        player.allFlg |= (1UL << i);
                        if (skill_flags[i] > 0)
                            //Activate the flag that lets characters use the skill
                            gd.baseData.sce_all_flg |= (1UL << (skill_flags[i] - 1));
                    }
                }
                gd.chara[gd.personFromId[0]] = player;
            }
        }

        public static bool CanAccessWorldWithoutGnosia()
        {
            if (crew_max < 15)
                return false;
            return true;
        }

        public static void ResetRoleFlags(gnosia.GameData gd)
        {
            for (int i = 1; i < found_roles.Length; i++)
            {
                if (found_roles[i])
                    gd.baseData.sce_all_flg |= (1UL << i);
                else
                {
                    //Only Reset stuff if roles are not crew or gnosia
                    if (i < 5 || i == 6 || i == 8)
                        gd.baseData.sce_all_flg &= ~(1UL << i);
                }
            }
        }

        public static void ResetSetupSettings(gnosia.GameData gd)
        {
            for (int i = 1; i < found_roles.Length; i++)
                if (!found_roles[i] && (i < 5 || i == 6 || i == 8))
                    gd.baseData.yakuNum[i] = 0;
        }
        
        public static void ActivateInstantUseItem(gnosia.GameData gd, long itemId)
        {
            if (itemId == 11000)
            {
                gd.baseData.gainExp += 50;
            }
            items_used.Add(itemId);
        }

        public static async Task<MessageData> ScoutSingleLocationAndGetMessage(long id)
        {
            //Check if location has already been completed
            if (ArchipelagoClient.ServerData.CheckedLocations.Contains(id))
                return null;
            //This is in a try catch cause you might've disconnected but still need to mark the location
            try
            {
                //Do the funny message stuff
                ArchipelagoSession session = ArchipelagoClient.GetSession();
                string message = "";
                int type = 1;
                Dictionary<long, ScoutedItemInfo> dict = await session.Locations.ScoutLocationsAsync(HintCreationPolicy.None, id);
                ScoutedItemInfo info = dict[id];
                //If it's a progression item, change the type
                if (info.Flags.HasFlag(ItemFlags.Advancement))
                    type = 0;
                //Check who the item belongs to
                if (info.Player.Slot != session.ConnectionInfo.Slot)
                {
                    //It's not an item from this game
                    string player_name = info.Player.Name;
                    string item_name = info.ItemDisplayName;
                    message = $"You found {player_name}'s {item_name}";
                }
                else
                {
                    //The item is from this game
                    string item_name = info.ItemDisplayName;
                    message = $"You found your {item_name}";
                }
                return new MessageData { message = message, type = type };
            }
            catch (Exception e)
            {
                BepinLogger.LogError(e);
                return new MessageData { message = "You completed a location! (Please reconnect)" , type = 1};
            }
        }
        
        public static async Task<MessageData> ScoutMultipleLocationsAndGetMessage(params long[] ids)
        {
            //Try catch cause the client might've disconnected
            try
            {
                //Get the session
                ArchipelagoSession session = ArchipelagoClient.GetSession();
                //Determine soundfx type
                Dictionary<long, ScoutedItemInfo> dict = await session.Locations.ScoutLocationsAsync(HintCreationPolicy.None, ids);
                int type = 1;
                foreach (long id in dict.Keys)
                {
                    ScoutedItemInfo info = dict[id];
                    if (info.Flags.HasFlag(ItemFlags.Advancement))
                    {
                        type = 0;
                        break;
                    }
                }
                //Display Message
                string message = "You found a bunch of items!";
                return new MessageData { message = message, type = type };
            }
            catch (Exception e)
            {
                BepinLogger.LogError(e);
                return new MessageData{message = "You completed some locations! (Please reconnect)", type = 1};
            }
        }

        public static async Task<MessageData> ScoutVariableLocationsAndGetMessage(params long[] ids)
        {
            List<long> temp = new List<long>(ids);
            foreach (long id in ids)
            {
                if (ArchipelagoClient.ServerData.CheckedLocations.Contains(id))
                    temp.Remove(id);
            }
            if (temp.Count == 0)
                return null;
            else if (temp.Count == 1)
                return await ScoutSingleLocationAndGetMessage(temp[0]).ConfigureAwait(false);
            else
                return await ScoutMultipleLocationsAndGetMessage(temp.ToArray()).ConfigureAwait(false);
        }

        public static async Task CheckLocations(params long[] ids)
        {
            ArchipelagoClient.ServerData.CheckedLocations.UnionWith(ids);
            try
            {
                ArchipelagoSession session = ArchipelagoClient.GetSession();
                await session.Locations.CompleteLocationChecksAsync(ids);
            }
            catch (Exception e)
            {
                BepinLogger.LogError(e);
            }
        }

        public static void CheckLocationsInScript(params long[] ids)
        {
            CheckLocationsInScript(true, ids);
        }

        public static void CheckLocationsInScript(bool withMessage, params long[] ids)
        {
            //Get sp
            ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
            //Do stuff
            sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
            {
                _ = CheckLocations(ids);
                return true;
            }, (float e) => true, false));
            if (withMessage)
            {
                MessageData messageData = Task.Run(() => ScoutVariableLocationsAndGetMessage(ids)).GetAwaiter().GetResult();
                if (messageData != null)
                {
                    sp.ShowInfoUpdateMes(messageData.message, 45002, messageData.type, true);
                }
            }
        }

        public static void SaveConnectionInfo(ConnectionInfo info)
        {
            string SaveDirectory = $"{UnityEngine.Application.persistentDataPath}/Archipelago";
            string FilePath = $"{SaveDirectory}/last_connection.json";
            if (!Directory.Exists(SaveDirectory))
            {
                //Archipelago Directory does not exist. Let's create it
                Directory.CreateDirectory(SaveDirectory);
            }
            string json = JsonConvert.SerializeObject(info, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        public static ConnectionInfo LoadConnectionInfo()
        {
            string SaveDirectory = $"{UnityEngine.Application.persistentDataPath}/Archipelago";
            string FilePath = $"{SaveDirectory}/last_connection.json";
            if (!Directory.Exists(SaveDirectory))
            {
                //Archipelago Directory does not exist. Let's create it
                Directory.CreateDirectory(SaveDirectory);
            }
            if (!File.Exists(FilePath))
            {
                //File Does Not Exist! Nothing to load
                return null;
            }
            string json = File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<ConnectionInfo>(json);
        }
    }
    public class MessageData
    {
        public string message;
        public int type;
    }

    public class ConnectionInfo
    {
        public string Uri;
        public string SlotName;
        //Do not save the password
    }
}