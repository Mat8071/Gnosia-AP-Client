using System;
using System.Collections.Generic;
using gnosia;
using GnosiaArchipelagoRandomizer.Archipelago;
using HarmonyLib;
using util;

namespace GnosiaArchipelagoRandomizer.Patches.Core
{
    [HarmonyPatch(typeof(SearchScenario), "Select")]
    class EventSearchPatch
    {
        static readonly Dictionary<int, List<int>> needCharacterMap = new Dictionary<int, List<int>>
        {
            { 0, new List<int> { } },
            { 36, new List<int> { 6 } },
            { 60, new List<int> { 6 } },
            { 62, new List<int> { 6, 11 } },
            { 41, new List<int> { 8 } },
            { 75, new List<int> { 8, 3 } },
            { 76, new List<int> { 8 } },
            { 78, new List<int> { 8, 4, 5, 7, 10, 11, 13 } },
            { 79, new List<int> { 8 } },
            { 53, new List<int> { 1 } },
            { 80, new List<int> { 1 } },
            { 81, new List<int> { 1, 8, 11 } },
            { 83, new List<int> { 1, 4 } },
            { 45, new List<int> { 10, 7 } },
            { 111, new List<int> { 10, 4 } },
            { 109, new List<int> { 14, 7 } },
            { 114, new List<int> { 14, 10, 11 } },
            { 115, new List<int> { 14, 10, 11 } },
            { 99, new List<int> { 12, 4, 14 } },
            { 67, new List<int> { 3, 2 } },
            { 68, new List<int> { 3 } },
            { 118, new List<int> { 3, 11 } },
            { 119, new List<int> { 3, 9 } },
            { 91, new List<int> { 7, 3, 4, 8 } },
            { 92, new List<int> { 7 } },
            { 102, new List<int> { 11 } },
            { 127, new List<int> { 11, 3, 4, 5, 9, 13 } },
            { 74, new List<int> { 13, 12 } },
            { 73, new List<int> { 13, 11 } },
            { 96, new List<int> { 5, 7, 10, 11 } },
            { 54, new List<int> { 2, 7 } },
            { 121, new List<int> { 2 } },
            { 125, new List<int> { 2, 7 } },
            { 133, new List<int> { 2, 3, 7 } },
            { 116, new List<int> { 4, 5 } },
            { 124, new List<int> { 4, 7, 9, 10, 11 } },
            { 87, new List<int> { 9, 11 } },
            { 88, new List<int> { 9, 11 } },
            { 33, new List<int> { 3 } },
            { 49, new List<int> { 12 } },
            { 98, new List<int> { 12, 3 } },
            { 66, new List<int> { 3 } },
            { 86, new List<int> { 9, 1, 7 } },
            { 63, new List<int> { 5, 6, 11 } },
            { 110, new List<int> { 10, 1, 2 } },
            { 113, new List<int> { 10, 14, 11 } },
            { 108, new List<int> { 14, 2, 7, 9, 10 } },
            { 71, new List<int> { 12, 13, 7 } },
            { 90, new List<int> { 7, 2, 3 } },
            { 95, new List<int> { 5, 3, 13 } },
            { 97, new List<int> { 5, 4 } },
            { 122, new List<int> { 4 } },
            { 94, new List<int> { 5 } },
            { 64, new List<int> { 3, 2, 11 } },
            { 107, new List<int> { 12, 14 } },
            { 61, new List<int> { 6, 8 } },
            { 106, new List<int> { 11, 9 } },
        };
        static ScenarioContents GetScenario(Array scenario, int sid)
        {
            return (ScenarioContents)scenario.GetValue(sid);
        }
        static bool Prefix(SearchScenario __instance, ref bool __result, ref gnosia.GameData gd)
        {
            //My stuff
            Dictionary<int, List<int>> forbiddenMap = new Dictionary<int, List<int>>();
            Type dataType = AccessTools.TypeByName("gnosia.Data");
            Array scenario = (Array)AccessTools.Field(dataType, "Scenario").GetValue(null);
            //End of my stuff here
            float num = 0f;
            List<int> list = new List<int>();
            int i = 0;
            foreach (SearchScenario.SearchContents searchContents in __instance.sc)
            {
                if (!searchContents.IsGot(gd) && searchContents.CanGet(gd))
                {
                    bool flag = true;
                    if (searchContents.scenarioId > 0)
                    {
                        int zeroNum = gd.GetZeroNum((long)((ulong)GetScenario(scenario, searchContents.scenarioId).selMainFlg));
                        if (zeroNum >= 0 && zeroNum < 15 && (float)gd.baseData.s_chara_resource[zeroNum] < GetScenario(scenario, searchContents.scenarioId).eatMain * 255f)
                        {
                            flag = false;
                        }
                        int zeroNum2 = gd.GetZeroNum((long)((ulong)GetScenario(scenario, searchContents.scenarioId).selTgtFlg));
                        if (zeroNum2 >= 0 && zeroNum2 < 15 && (float)gd.baseData.s_chara_resource[zeroNum2] < GetScenario(scenario, searchContents.scenarioId).eatTgt * 255f)
                        {
                            flag = false;
                        }
                    }
                    //Added stuff here
                    //Check crew number
                    if (searchContents.pMin > Plugin.crew_max)
                    {
                        flag = false;
                    }
                    //Check characters
                    var options = ArchipelagoClient.ServerData.SlotData.Options;
                    if (options?.RandomizeCharacterUnlocks ?? false)
                    {
                        foreach (int chara in needCharacterMap[searchContents.scenarioId])
                        {
                            if (!Plugin.found_characters[chara])
                            {
                                flag = false;
                                break;
                            }
                        }
                    }
                    //Check roles
                    List<int> forbidden = new List<int>(searchContents.kinsiYaku);
                    if (options?.RandomizeRoleUnlocks ?? true)
                    {
                        for (int role = 1; role < Plugin.found_roles.Length; role++)
                        {
                            if (!Plugin.found_roles[role])
                            {
                                if (searchContents.needYaku.Contains(role))
                                {
                                    flag = false;
                                    break;
                                }
                                forbidden.Add(role);
                            }
                        }
                    }
                    forbiddenMap[i] = forbidden;
                    //End of modifications
                    if (flag)
                    {
                        list.Add(i);
                        num += searchContents.power;
                        //Optional logging
                        if (Plugin.debug_mode)
                            Plugin.BepinLogger.LogInfo($"Found Event Id: {searchContents.scenarioId}");
                    }
                }
                i++;
            }
            float num2 = Util.GetRandF() * num;
            for (i = 0; i < list.Count; i++)
            {
                if (__instance.sc[list[i]].power > num2)
                {
                    __instance.badYaku = __instance.sc[list[i]].badYaku;
                    __instance.goodYaku = __instance.sc[list[i]].goodYaku;
                    __instance.needYaku = __instance.sc[list[i]].needYaku;
                    __instance.kinsiYaku = forbiddenMap[list[i]]; //Modified thing
                    __instance.gmin = __instance.sc[list[i]].gMin;
                    __instance.gmax = __instance.sc[list[i]].gMax;
                    __instance.scenarioId = __instance.sc[list[i]].scenarioId;
                    __instance.ninzu = __instance.sc[list[i]].pMin;
                    __result = true;
                    //Optional logging
                    if (Plugin.debug_mode)
                        Plugin.BepinLogger.LogInfo($"Selected Event: {__instance.sc[list[i]].scenarioId}");
                    return false;
                }
                num2 -= __instance.sc[list[i]].power;
            }
            __result = false;
            return false;
        }
    }
}
