using System;
using setting;
using GnosiaArchipelagoRandomizer.Archipelago;
using HarmonyLib;
using gnosia;
using util;
using System.Runtime.CompilerServices;

namespace GnosiaArchipelagoRandomizer.Patches.Core
{
    [HarmonyPatch]
    class EventRequirementChangePatches
    {
        static byte GetCharaTotalNotes(Array chara, int id)
        {
            var entry = chara.GetValue(id);
            var nameField = AccessTools.Field(entry.GetType(), "d_tokkiNum");
            return (byte)nameField.GetValue(entry);
        }


        [HarmonyReversePatch]
        [HarmonyPatch(typeof(ScenarioContents), "CanOpen")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        static bool BaseCanOpen(ScenarioContents instance, ref gnosia.GameData gd)
        {
            throw new NotImplementedException();
        }

        [HarmonyReversePatch]
        [HarmonyPatch(typeof(ScenarioContents), "Close")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        static void BaseClose(ScenarioContents instance, ref gnosia.GameData gd, ref gnosia.GameData.scenarioData sd)
        {
            throw new NotImplementedException();
        }


        [HarmonyPatch(typeof(MainGnosMaxScenario), "CanOpen")]
        [HarmonyPrefix]
        static bool NormalEnding(MainGnosMaxScenario __instance, ref bool __result, ref gnosia.GameData gd)
        {
            //Get internal stuff
            Type dataType = AccessTools.TypeByName("gnosia.Data");
            Array chara = (Array)AccessTools.Field(dataType, "Chara").GetValue(null);
            //Check conditions
            bool baseResult = BaseCanOpen(__instance, ref gd);
            if (baseResult && gd.baseData.day == 1 && (gd.baseData.sce_all_flg & 1073741824UL) > 0UL)
            {
                int found_notes = 0;
                int total_notes = 0;
                for (int i = 1; i < 15; i++)
                {
                    byte notes = GetCharaTotalNotes(chara, i);
                    total_notes += notes;
                    for (int j = 0; j < notes; j++)
                    {
                        if ((gd.baseData.s_chara_all_flg[i] & (1UL << j)) > 0UL)
                        {
                            found_notes += 1;
                        }
                    }
                }
                __result = found_notes >= (total_notes * (Convert.ToSingle(ArchipelagoClient.ServerData.GetSlotData()["required_note_percent"]) / 100f));
                return false;
            }
            __result = false;
            return false;
        }


        [HarmonyPatch(typeof(Gina2Scenario), "CanOpen")]
        [HarmonyPrefix]
        static bool DontBeFooled(Gina2Scenario __instance, ref bool __result, ref gnosia.GameData gd)
        {
            //Call base method
            bool baseResult = BaseCanOpen(__instance, ref gd);
            //Original (Modified)
            if (baseResult && gd.personFromId[8] >= 0 && gd.personFromId[1] >= 0 && gd.personFromId[11] >= 0 && Util.Count16((ushort)(gd.chara[gd.personFromId[1]].allFlg & 255UL)) >= 4 && gd.chara[gd.personFromId[1]].i_yaku <= Setting.Yakuwari.y_Murabito && gd.chara[gd.personFromId[1]].buf[11] > 0 && gd.GetInsideTrust(gd.personFromId[1], 0, false) > 0.45f && gd.GetFriend(gd.personFromId[1], 0) > 0.15f)
            {
                for (int i = 1; i < (int)gd.baseData.totalNum; i++)
                {
                    if (((long)(gd.peopleFlg[0] & gd.chara[gd.personFromId[1]].buf[11]) & (long)(1UL << (i & 31))) > 0L && (gd.chara[i].i_yaku == Setting.Yakuwari.y_Jinro || gd.chara[0].i_yaku <= Setting.Yakuwari.y_Murabito) && gd.chara[0].p_rate[i] > 0f)
                    {
                        __result = !ArchipelagoClient.ServerData.CheckedLocations.Contains(104) || gd.chara[0].i_yaku == Setting.Yakuwari.y_Jinro;
                        return false;
                    }
                }
            }
            __result = false;
            return false;
        }

        [HarmonyPatch(typeof(SQ4Scenario), "CanOpen")]
        [HarmonyPrefix]
        static bool SQ2GnosiaIntro(SQ4Scenario __instance, ref bool __result, ref gnosia.GameData gd)
        {
            bool baseResult = BaseCanOpen(__instance, ref gd);
            __result = baseResult && gd.baseData.day == 1 && gd.baseData.loop >= 25 && gd.personFromId[2] >= 0 && gd.personFromId[7] >= 0 && gd.personFromId[3] >= 0 && !ArchipelagoClient.ServerData.CheckedLocations.Contains(202) && gd.chara[gd.personFromId[2]].i_yaku == Setting.Yakuwari.y_Jinro && gd.chara[0].i_yaku == Setting.Yakuwari.y_Jinro && gd.chara[gd.personFromId[7]].i_yaku != Setting.Yakuwari.y_Jinro && gd.chara[gd.personFromId[3]].i_yaku != Setting.Yakuwari.y_Jinro && gd.chara[gd.personFromId[7]].i_yaku != Setting.Yakuwari.y_Fox;
            return false;
        }

        [HarmonyPatch(typeof(TutorialLoop19Scenario), "CanOpen")]
        [HarmonyPrefix]
        static bool BugLoop(TutorialLoop19Scenario __instance, ref bool __result, ref gnosia.GameData gd)
        {
            bool baseResult = BaseCanOpen(__instance, ref gd);
            __result = baseResult && gd.baseData.loop >= 16 && !ArchipelagoClient.ServerData.CheckedLocations.Contains(1508) && Plugin.found_characters[11];
            return false;
        }

        [HarmonyPatch(typeof(SQ4Scenario), "Close")]
        [HarmonyPrefix]
        static bool SQ2GnosiaIntroClose(SQ4Scenario __instance, ref gnosia.GameData gd, ref gnosia.GameData.scenarioData sd)
        {
            gd.baseData.sce_flg = gd.baseData.sce_flg ^ 16384UL;
            if (ArchipelagoClient.ServerData.CheckedLocations.Contains(202) && ArchipelagoClient.ServerData.CheckedLocations.Contains(703))
            {
                ScenarioContents.ChangeSceOnFlg(ref gd, ref sd, 24U);
            }
            BaseClose(__instance, ref gd, ref sd);
            return false;
        }

        [HarmonyPatch(typeof(Stella4Scenario), "Close")]
        [HarmonyPrefix]
        static bool Stella5Close(Stella4Scenario __instance, ref gnosia.GameData gd, ref gnosia.GameData.scenarioData sd)
        {
            gd.baseData.sce_flg = gd.baseData.sce_flg ^ 16384UL;
            if (ArchipelagoClient.ServerData.CheckedLocations.Contains(405)) //Changed condition
            {
                ScenarioContents.ChangeSceOnFlg(ref gd, ref sd, 24U);
            }
            BaseClose(__instance, ref gd, ref sd);
            return false;
        }
    }
}