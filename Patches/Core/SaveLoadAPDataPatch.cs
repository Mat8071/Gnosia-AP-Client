using System.Collections.Generic;
using System.IO;
using System.Linq;
using GnosiaArchipelagoRandomizer.Archipelago;
using GnosiaArchipelagoRandomizer.Patches.Optional;
using HarmonyLib;
using Newtonsoft.Json;
using systemService.saveData;

namespace GnosiaArchipelagoRandomizer.Patches
{
    [HarmonyPatch]
    class SaveLoadAPDataPatch
    {
        [HarmonyPatch(typeof(SaveDataManager), "CreateNewSlot")]
        [HarmonyPrefix]
        static void NewGame(SaveDataManager __instance, int slotId)
        {
            //Reset items used
            Plugin.items_used.Clear();
            //Add all items to the queue
            foreach (long item in Plugin.inventory)
            {
                //But only if they're consumables/instant effects
                if (item >= 11000)
                    Plugin.instant_item_queue.Enqueue(item);
            }
        }

        [HarmonyPatch(typeof(SaveDataManager), "LoadSlot")]
        [HarmonyPrefix]
        static void LoadData(SaveDataManager __instance, int slotId)
        {
            //Get path
            string dir = Traverse.Create(__instance).Field("SaveDirectory").GetValue<string>();
            string file_path = $"{dir}/save/slot{slotId}/ap_data.json";
            if (!File.Exists(file_path))
            {
                //No save data found, let's just add all items to the queue
                foreach (long item in Plugin.inventory)
                {
                    //But only if they're consumables/instant effects
                    if (item >= 11000)
                        Plugin.instant_item_queue.Enqueue(item);
                }
                return;
            }
            //Load data
            string json = File.ReadAllText(file_path);
            APData data = JsonConvert.DeserializeObject<APData>(json);
            //Sync Items
            Plugin.items_used.Clear();
            Plugin.items_used.AddRange(data.UsedItems);
            List<long> items_to_award = new List<long>(Plugin.inventory);
            foreach (long item in data.UsedItems)
            {
                items_to_award.Remove(item);
            }
            foreach (long item in items_to_award)
            {
                //If instant effect
                if (item >= 11000)
                {
                    //Add all items that weren't used on this save file to the queue
                    Plugin.instant_item_queue.Enqueue(item);
                }
            }
            //Check all loaded locations (without any messages)
            _ = Plugin.CheckLocations(data.CheckedLocations.ToArray());
            //Restore completed achievements set
            if (data.CompletedAchievements != null)
                HandleAchievementsPatch.completedAchievements.UnionWith(data.CompletedAchievements);
            //Check if goaled on role achievements goal
            var options = ArchipelagoClient.ServerData.SlotData.Options;
            if ((options?.Goal ?? ArchipelagoData.Goal.NormalEnding) == ArchipelagoData.Goal.RoleAchievements)
                HandleAchievementsPatch.CheckGoal();
        }

        [HarmonyPatch(typeof(SaveDataManager), "SaveData")]
        [HarmonyPostfix]
        static void SaveData(SaveDataManager __instance)
        {
            //Get path
            string dir = Traverse.Create(__instance).Field("SaveDirectory").GetValue<string>();
            dir = $"{dir}/save/slot{__instance.GetCurrentSlotId()}";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            string file_path = $"{dir}/ap_data.json";
            //Save data
            APData data = new APData
            {
                UsedItems = Plugin.items_used,
                CheckedLocations = ArchipelagoClient.ServerData.CheckedLocations,
                CompletedAchievements = HandleAchievementsPatch.completedAchievements,
            };
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(file_path, json);
        }
    }

    class APData
    {
        public List<long> UsedItems;
        public HashSet<long> CheckedLocations;
        public HashSet<int> CompletedAchievements;
    }
}