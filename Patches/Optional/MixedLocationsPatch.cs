using System;
using System.Collections.Generic;
using coreSystem;
using gnosia;
using GnosiaArchipelagoRandomizer.Archipelago;
using GnosiaArchipelagoRandomizer.Utils;
using HarmonyLib;
using UnityEngine;
using util;

namespace GnosiaArchipelagoRandomizer.Patches.Optional
{
    [HarmonyPatch]
    class MixedLocationsPatch
    {
        [HarmonyPatch(typeof(Gina2Scenario), "SetParam")]
        [HarmonyPostfix]
        static void DontBeFooled(Gina2Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[4];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Get Other Stuff
                Type dataType = AccessTools.TypeByName("gnosia.Data");
                Array chara = (Array)AccessTools.Field(dataType, "Chara").GetValue(null);
                var options = ArchipelagoClient.ServerData.SlotData.Options;
                //Base
                List<string> list4 = Util.Split(sp.m_rs.GetScenarioGinaText(1, 11, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list4, true, true, false, true);
                list4 = Util.Split(sp.m_rs.GetScenarioGinaText(1, 12, 5), new char[] { '|' });
                string text4 = list4[0];
                Util.Replace(ref text4, "{1}", MyUtils.GetCharaName(chara, gd, ad.targetP));
                list4[0] = text4;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list4, true, true, false, true);
                list4 = Util.Split(sp.m_rs.GetScenarioGinaText(1, 13, 0), new char[] { '|' });
                text4 = list4[0];
                Util.Replace(ref text4, "{1}", MyUtils.GetCharaName(chara, gd, ad.targetP));
                list4[0] = text4;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list4, true, true, false, true);
                if (options?.RandomizeSkills ?? true)
                {
                    if (!ArchipelagoClient.ServerData.CheckedLocations.Contains(18)) //Changed condition
                    {
                        sp.WaitSec(0.1f, false);
                        Plugin.CheckLocationsInScript(18);
                        gd.baseData.gainExp += 50U;
                    }
                }
                else
                {
                    //Original
                    if ((gd.chara[0].allFlg & 131072UL) == 0UL)
                    {
                        sp.WaitSec(0.1f, false);
                        sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                        {
                            GameData.character character2 = gd.chara[0];
                            character2.allFlg |= 131072UL;
                            gd.chara[0] = character2;
                            return true;
                        }, (float e) => true, false));
                        sp.ShowInfoUpdateMes(sp.m_rs.GetScenarioGinaText(1, 14, -1), 45002U, 1, true);
                        gd.baseData.gainExp += 50U;
                        sp.PlaySeInScript("se_square", 1f);
                        sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioGinaText(1, 15, -1), 3, false);
                        sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                    }
                }
                list4 = Util.Split(sp.m_rs.GetScenarioGinaText(1, 16, 3), new char[] { '|' });
                text4 = list4[0];
                Util.Replace(ref text4, "{0}", gd.takashiName);
                list4[0] = text4;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list4, true, true, false, true);
                list4 = Util.Split(sp.m_rs.GetScenarioGinaText(1, 17, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list4, true, true, false, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 3.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                if (options?.RandomizeNotes ?? true)
                {
                    if (!ArchipelagoClient.ServerData.CheckedLocations.Contains(104)) //Changed condition
                    {
                        sp.WaitSec(0.75f, true);
                        int mainP = ad.mainP;
                        Plugin.CheckLocationsInScript(104);
                        gd.baseData.gainExp += 50U;
                    }
                }
                else
                {
                    //Original
                    if ((gd.chara[ad.mainP].allFlg & 8UL) == 0UL)
                    {
                        sp.WaitSec(0.75f, true);
                        int mainP = ad.mainP;
                        sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                        {
                            GameData.character character3 = gd.chara[mainP];
                            character3.allFlg |= 8UL;
                            gd.chara[mainP] = character3;
                            gd.CalGnos(mainP);
                            return true;
                        }, (float e) => true, false));
                        sp.ShowInfoUpdateMes(sp.m_rs.GetScenarioGinaText(1, 18, -1), 45002U, 0, true);
                        gd.baseData.gainExp += 50U;
                    }
                }
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(0.6f, true);
                gd.forwardNext = true;
            };
            __instance.actions[4] = action;
        }

        [HarmonyPatch(typeof(Otome1Scenario), "SetParam")]
        [HarmonyPostfix]
        static void DontVote(Otome1Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[7];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Get options
                var options = ArchipelagoClient.ServerData.SlotData.Options;
                //Base
                List<string> list7;
                if ((gd.actionFlg & 16UL) == 0UL)
                {
                    list7 = Util.Split(sp.m_rs.GetScenarioOtomeText(0, 35, 0), new char[] { '|' });
                    sp.SetNormalSerifu(ad.mainP, 0, 1, list7, true, true, false, true);
                }
                list7 = Util.Split(sp.m_rs.GetScenarioOtomeText(0, 36, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list7, true, false, false, true);
                list7 = Util.Split(sp.m_rs.GetScenarioOtomeText(0, 37, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list7, true, true, false, true);
                list7 = Util.Split(sp.m_rs.GetScenarioOtomeText(0, 38, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list7, true, true, false, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 1.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.WaitSec(0.7f, true);
                int mainP = ad.mainP;
                if (options?.RandomizeNotes ?? true)
                {
                    Plugin.CheckLocationsInScript(1204);
                }
                else
                {
                    //Original
                    sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                    {
                        GameData.character character2 = gd.chara[mainP];
                        character2.allFlg |= 8UL;
                        gd.chara[mainP] = character2;
                        gd.CalGnos(mainP);
                        return true;
                    }, (float e) => true, false));
                    sp.ShowInfoUpdateMes(sp.m_rs.GetScenarioOtomeText(0, 39, -1), 45002U, 0, true);
                }
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(1f, true);
                if (options?.RandomizeSkills ?? true)
                {
                    Plugin.CheckLocationsInScript(7);
                }
                else
                {
                    //Original
                    sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                    {
                        GameData.character character3 = gd.chara[0];
                        character3.allFlg |= 64UL;
                        gd.chara[0] = character3;
                        return true;
                    }, (float e) => true, false));
                    sp.ShowInfoUpdateMes(sp.m_rs.GetScenarioOtomeText(0, 40, -1), 45002U, 1, true);
                }
                gd.baseData.gainExp += 100U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(0.4f, true);
                gd.forwardNext = true;
            };
            __instance.actions[7] = action;
        }

        [HarmonyPatch(typeof(Shamin1Scenario), "SetParam")]
        [HarmonyPostfix]
        static void AceInTheHole(Shamin1Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[5];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Get options
                var options = ArchipelagoClient.ServerData.SlotData.Options;
                //Base
                List<string> list5 = Util.Split(sp.m_rs.GetScenarioShaminText(0, 20, 0), new char[] { '|' });
                sp.SetNormalSerifu(0, ad.targetP, 1, list5, true, false, true, true);
                list5 = Util.Split(sp.m_rs.GetScenarioShaminText(0, 21, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list5, true, false, false, true);
                sp.FadeBgmInScript(-1f, 0f, 3f, true, -1);
                list5 = Util.Split(sp.m_rs.GetScenarioShaminText(0, 22, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list5, true, true, false, true);
                sp.WaitSec(0.05f, false);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.8f, 0, true, true, true);
                sp.UnloadPlace();
                sp.WaitSec(0.2f, true);
                sp.PlaySeInScript("se_fuku_02", 1f);
                sp.WaitSec(0.2f, true);
                sp.PlayBgmInScript("bgm04", 0f, 1f, -1, true);
                list5 = Util.Split(sp.m_rs.GetScenarioShaminText(0, 23, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list5, true, true, true, false);
                sp.FadeBgmInScript(0.4f, 1f, 0.8f, false, -1);
                sp.PlaySeInScript("se_fuku_02", 1f);
                sp.WaitSec(0.2f, true);
                list5 = Util.Split(sp.m_rs.GetScenarioShaminText(0, 24, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list5, true, true, true, false);
                sp.FadeBgmInScript(0.4f, 1f, 0.8f, false, -1);
                sp.PlaySeInScript("se_fuku_02", 1f);
                sp.WaitSec(0.2f, true);
                list5 = Util.Split(sp.m_rs.GetScenarioShaminText(0, 25, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list5, true, true, true, false);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 3.5f, true, -1);
                sp.RemoveScreenInScript(50U);
                sp.WaitSec(0.5f, true);
                if (options?.RandomizeSkills ?? true)
                {
                    Plugin.CheckLocationsInScript(19);
                }
                else
                {
                    //Original
                    sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                    {
                        GameData.character character5 = gd.chara[0];
                        character5.allFlg = gd.chara[0].allFlg | 262144UL;
                        gd.chara[0] = character5;
                        return true;
                    }, (float e) => true, false));
                    sp.ShowInfoUpdateMes(sp.m_rs.GetScenarioShaminText(0, 26, -1), 45002U, 1, true);
                }
                gd.baseData.gainExp += 50U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(0.2f, true);
                if (options?.RandomizeNotes ?? true)
                {
                    if (!ArchipelagoClient.ServerData.CheckedLocations.Contains(1304)) //Changed condition
                    {
                        int targetP = ad.targetP;
                        Plugin.CheckLocationsInScript(1304);
                        sp.WaitSec(0.2f, true);
                    }
                }
                else
                {
                    //Original
                    if ((gd.chara[ad.targetP].allFlg & 8UL) == 0UL)
                    {
                        int targetP = ad.targetP;
                        sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                        {
                            GameData.character character6 = gd.chara[targetP];
                            character6.allFlg |= 8UL;
                            gd.chara[targetP] = character6;
                            gd.CalGnos(targetP);
                            return true;
                        }, (float e) => true, false));
                        sp.ShowInfoUpdateMes(sp.m_rs.GetScenarioShaminText(0, 28, -1), 45002U, 0, true);
                        sp.WaitSec(0.2f, true);
                    }
                }
                gd.forwardNext = true;
            };
            __instance.actions[5] = action;
        }
    }
}
