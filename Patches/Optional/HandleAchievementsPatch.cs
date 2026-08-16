using System.Collections.Generic;
using System.Linq;
using coreSystem;
using GnosiaArchipelagoRandomizer.Archipelago;
using HarmonyLib;
using systemService.trophy;
using UnityEngine;

namespace GnosiaArchipelagoRandomizer.Patches.Optional
{
    [HarmonyPatch(typeof(GameLogManager), "CheckTrophy")]
    class HandleAchievementsPatch
    {
        public static readonly HashSet<int> allRoleAchievements = new HashSet<int>
        {
            GameLogManager.TROPHY_ID_ENGINEERPLAY,
            GameLogManager.TROPHY_ID_SHUGOPLAY,
            GameLogManager.TROPHY_ID_HUMANPLAY,
            GameLogManager.TROPHY_ID_ACPLAY,
            GameLogManager.TROPHY_ID_GNOSIAPLAY,
            GameLogManager.TROPHY_ID_BUGPLAY,
        };
        public static readonly Dictionary<int, string> roleAchievementIdToName = new Dictionary<int, string>
        {
            { GameLogManager.TROPHY_ID_ENGINEERPLAY, "Intrepid Investigator" },
            { GameLogManager.TROPHY_ID_SHUGOPLAY, "Guardian Angel" },
            { GameLogManager.TROPHY_ID_HUMANPLAY, "Hero" },
            { GameLogManager.TROPHY_ID_ACPLAY, "Loyal Servant" },
            { GameLogManager.TROPHY_ID_GNOSIAPLAY, "Lonely Battle" },
            { GameLogManager.TROPHY_ID_BUGPLAY, "Destroyer of the Universe" },
        };
        public static readonly HashSet<int> completedAchievements = new HashSet<int>();
        private static readonly Dictionary<string, int> roleAchievementNameToId = new Dictionary<string, int>
        {
            { "Intrepid Investigator Achievement", GameLogManager.TROPHY_ID_ENGINEERPLAY },
            { "Guardian Angel Achievement", GameLogManager.TROPHY_ID_SHUGOPLAY },
            { "Hero Achievement", GameLogManager.TROPHY_ID_HUMANPLAY },
            { "Loyal Servant Achievement", GameLogManager.TROPHY_ID_ACPLAY },
            { "Lonely Battle Achievement", GameLogManager.TROPHY_ID_GNOSIAPLAY },
            { "Destroyer of the Universe Achievement", GameLogManager.TROPHY_ID_BUGPLAY },
        };
        private static readonly Dictionary<int, int> roleAchievementIdToLocationId = new Dictionary<int, int>
        {
            { GameLogManager.TROPHY_ID_ENGINEERPLAY, 1601 },
            { GameLogManager.TROPHY_ID_SHUGOPLAY, 1603 },
            { GameLogManager.TROPHY_ID_HUMANPLAY, 1605 },
            { GameLogManager.TROPHY_ID_ACPLAY, 1606 },
            { GameLogManager.TROPHY_ID_GNOSIAPLAY, 1607 },
            { GameLogManager.TROPHY_ID_BUGPLAY, 1608 },
        };
        public static void CheckGoal()
        {
            var options = ArchipelagoClient.ServerData.SlotData.Options;
            //Get needed achievements
            HashSet<int> excludedAchievements = new HashSet<int>(
                (options?.ExcludedAchievements ?? Enumerable.Empty<string>())
                .Concat(options?.ExcludeLocations ?? Enumerable.Empty<string>())
                .Where(name => roleAchievementNameToId.ContainsKey(name))
                .Select(name => roleAchievementNameToId[name])
            );
            HashSet<int> neededAchievements = new HashSet<int>(allRoleAchievements.Except(excludedAchievements));
            //Check if goaled
            if (neededAchievements.IsSubsetOf(completedAchievements))
            {
                Plugin.CompleteGoal();
            }
        }
        static void Prefix(int tid)
        {
            ArchipelagoData.OptionsContents options = ArchipelagoClient.ServerData.SlotData.Options;
            bool goal = options?.Goal == ArchipelagoData.Goal.RoleAchievements;
            bool location = options?.AddRoleAchievementLocations ?? false;
            if (!roleAchievementIdToName.TryGetValue(tid, out string achievementName))
            {
                return;
            }
            //Get sp
            ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
            //Show message for completing achievement
            sp.ShowInfoUpdateMes($"Achievement Completed: {achievementName}");
            if (location)
            {
                _ = Plugin.CheckLocations(roleAchievementIdToLocationId[tid]);
            }
            if (goal)
            {
                //Add achievement to list of completed achievements
                completedAchievements.Add(tid);
                //Check if goaled
                CheckGoal();
            }
        }
    }
}
