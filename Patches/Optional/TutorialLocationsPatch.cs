using System;
using System.Collections.Generic;
using coreSystem;
using gnosia;
using GnosiaArchipelagoRandomizer.Archipelago;
using GnosiaArchipelagoRandomizer.Utils;
using HarmonyLib;
using setting;
using UnityEngine;
using util;

namespace GnosiaArchipelagoRandomizer.Patches.Optional
{
    [HarmonyPatch]
    class TutorialLocationsPatch
    {
        [HarmonyPatch(typeof(TutorialLoop1Scenario), "SetParam")]
        [HarmonyPostfix]
        static void TutorialLoop1(TutorialLoop1Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[2];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                gd.peopleFlg[6] = (ushort)((int)gd.peopleFlg[6] | (1 << ad.mainP));
                List<string> list = Util.Split(sp.m_rs.GetScenarioTutorialText(1, 3, -1), new char[] { '|' });
                sp.SetInterface(50U, ad.mainP, -1, true, true);
                sp.SetText(list[0], false, 50U, "test");
                sp.WaitText(50U, "test", true);
                Plugin.CheckLocationsInScript(1100);
                sp.PlayBgmInScript("bgm14", 1.5f, 1f, -1, true);
                sp.PlaySeInScript("se_square", 1f);
                string scenarioTutorialText = sp.m_rs.GetScenarioTutorialText(1, 5, -1);
                Util.Replace(ref scenarioTutorialText, "{0}", sp.m_rs.GetButtonName(0));
                Util.Replace(ref scenarioTutorialText, "{1}", sp.m_rs.GetButtonName(1));
                sp.SetDialogScreen(50400U, scenarioTutorialText, (Setting.language == 1) ? 3 : 2, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
            };
            __instance.actions[2] = action;
            action = __instance.actions[14];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Get Other Stuff
                Type dataType = AccessTools.TypeByName("gnosia.Data");
                Array chara = (Array)AccessTools.Field(dataType, "Chara").GetValue(null);
                //Base
                gd.pos = 1;
                sp.StopAllSeInScript();
                sp.PlaySeInScript("se_jidoudoa", 1f);
                sp.WaitSec(0.6f, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Interface, 50U, true, false, -1);
                    sp.m_sb[50001U].SetFade(0.3f, 0f, 0, -1f, -1, false);
                    sp.m_sb[50002U].SetFade(0.3f, 0f, 0, -1f, -1, false);
                    return true;
                }, (float e) => true, false));
                sp.ShowChara(ad.mainP, 0, gd.pos, 20U, false);
                sp.WaitFade(new List<uint> { 50001U, 50002U }, true, true);
                sp.WaitSec(0.35f, true);
                List<string> list13 = Util.Split(sp.m_rs.GetScenarioTutorialText(1, 45, 1), new char[] { '|' });
                sp.PlayBgmInScript("bgm00", 0.5f, 0.4f, -1, true);
                sp.SetNormalSerifu(ad.mainP, -1, gd.pos, list13, false, false, false, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                Plugin.CheckLocationsInScript(300);
                list13 = Util.Split(sp.m_rs.GetScenarioTutorialText(1, 46, 0), new char[] { '|' });
                string text4 = list13[0];
                Util.Replace(ref text4, "{0}", MyUtils.GetCharaName(chara, gd, ad.targetP));
                list13[0] = text4;
                sp.SetNormalSerifu(gd.personFromId[1], sd.mainP, 0, list13, true, false, false, true);
                Plugin.CheckLocationsInScript(100);
                list13 = Util.Split(sp.m_rs.GetScenarioTutorialText(1, 47, 5), new char[] { '|' });
                sp.SetNormalSerifu(sd.mainP, gd.personFromId[1], 2, list13, true, false, false, true);
                list13 = Util.Split(sp.m_rs.GetScenarioTutorialText(1, 48, 5), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[2], sd.mainP, 0, list13, true, false, false, true);
                Plugin.CheckLocationsInScript(200);
                list13 = Util.Split(sp.m_rs.GetScenarioTutorialText(1, 49, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list13, true, false, false, true);
                sp.FadeBgmInScript(-1f, 1f, 0.8f, false, -1);
                list13 = Util.Split(sp.m_rs.GetScenarioTutorialText(1, 50, 2), new char[] { '|' });
                text4 = list13[0];
                Util.Replace(ref text4, "{0}", MyUtils.GetCharaName(chara, gd, ad.targetP));
                list13[0] = text4;
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 1, list13, true, false, false, true);
                list13 = Util.Split(sp.m_rs.GetScenarioTutorialText(1, 51, -1), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, -1, list13, true, false, true, true);
                sp.FadeBgmInScript(-1f, 0.6f, 1f, false, -1);
                list13 = Util.Split(sp.m_rs.GetScenarioTutorialText(1, 52, 2), new char[] { '|' });
                sp.SetNormalSerifu(sd.mainP, ad.mainP, 2, list13, true, false, false, true);
                list13 = Util.Split(sp.m_rs.GetScenarioTutorialText(1, 53, 2), new char[] { '|' });
                text4 = list13[0];
                Util.Replace(ref text4, "{0}", MyUtils.GetCharaName(chara, gd, ad.targetP));
                list13[0] = text4;
                sp.SetNormalSerifu(sd.mainP, ad.mainP, 2, list13, true, true, true, true);
                list13 = Util.Split(sp.m_rs.GetScenarioTutorialText(1, 54, 0), new char[] { '|' });
                text4 = list13[0];
                Util.Replace(ref text4, "{0}", MyUtils.GetCharaName(chara, gd, ad.targetP));
                list13[0] = text4;
                sp.SetNormalSerifu(gd.personFromId[1], -1, 0, list13, true, false, false, true);
                list13 = Util.Split(sp.m_rs.GetScenarioTutorialText(1, 55, 1), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[2], -1, 1, list13, false, false, false, true);
            };
            __instance.actions[14] = action;
        }


        [HarmonyPatch(typeof(TutorialLoop2Scenario), "SetParam")]
        [HarmonyPostfix]
        static void TutorialLoop2(TutorialLoop2Scenario __instance)
        {
            var options = ArchipelagoClient.ServerData.SlotData.Options;
            if (!(options?.RandomizeRoleUnlocks ?? true))
                return;
            ScenarioContents.ActionContents action = __instance.actions[37];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Get Other Stuff
                Type dataType = AccessTools.TypeByName("gnosia.Data");
                Array chara = (Array)AccessTools.Field(dataType, "Chara").GetValue(null);
                //Base
                if (ad.mainP == 0)
                {
                    ad.mainP = gd.personFromId[1];
                }
                GameData.character character = gd.chara[ad.mainP];
                Jinro.ClearTable(ref character.p_knowTable);
                Jinro.IsY((Setting.Yakuwari)ad.type, ad.mainP, ref character.p_knowTable);
                for (int num7 = 1; num7 <= 5; num7++)
                {
                    if (num7 != ad.type)
                    {
                        Jinro.IsNotY((Setting.Yakuwari)num7, ad.mainP, ref gd.knowTable);
                    }
                }
                character.p_yaku = (Setting.Yakuwari)ad.type;
                Jinro.MakeYakuAliveNum(ref gd);
                if (character.i_yaku == Setting.Yakuwari.y_Uranai)
                {
                    Jinro.MakeFakeRireki(ad.mainP, (Setting.Yakuwari)ad.type, ref gd);
                }
                else
                {
                    Jinro.IsY(Setting.Yakuwari.y_Jinro, ad.targetP, ref character.p_knowTable);
                    character.yaku_rireki[(int)(gd.baseData.day - 2)].Add((byte)(144L | (long)ad.targetP));
                }
                gd.chara[ad.mainP] = character;
                ad.ctuizuiP = (ushort)(1 << ad.targetP);
                gd.peopleFlg[6] = (ushort)((int)gd.peopleFlg[6] | (1 << ad.mainP));
                gd.makeRate = true;
                gd.GainHate(ad.mainP, 0.2f);
                gd.pos = 1;
                sp.FadeBgmInScript(-1f, 0.5f, 0.8f, false, -1);
                List<string> list35 = Util.Split(sp.m_rs.GetScenarioTutorialText(2, 150, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, gd.pos, list35, true, false, false, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    gd.baseData.sce_all_flg = gd.baseData.sce_all_flg | 2UL;
                    return true;
                }, (float e) => true, false));
                sp.FadeBgmInScript(0f, 0.5f, 2f, false, -1);
                sp.PlaySeInScript("se_jin_01", 1f);
                list35 = Util.Split(sp.m_rs.GetScenarioTutorialText(2, 151, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, gd.pos, list35, true, true, true, true);
                Plugin.CheckLocationsInScript(1501);
                list35 = Util.Split(sp.m_rs.GetScenarioTutorialText(2, 152, 0), new char[] { '|' });
                string text18 = list35[0];
                Util.Replace(ref text18, "{0}", MyUtils.GetCharaName(chara, gd, ad.targetP));
                list35[0] = text18;
                sp.SetNormalSerifu(ad.mainP, -1, gd.pos, list35, true, true, true, true);
                sp.FadeBgmInScript(-1f, 0f, 0.1f, false, -1);
                sp.PlaySeInScript("se_jin_05", 1f);
                list35 = Util.Split(sp.m_rs.GetScenarioTutorialText(2, 153, 5), new char[] { '|' });
                text18 = list35[0];
                Util.Replace(ref text18, "{0}", MyUtils.GetCharaName(chara, gd, ad.targetP));
                list35[0] = text18;
                sp.SetNormalSerifu(ad.mainP, ad.targetP, gd.pos, list35, false, true, false, true);
                sp.WaitText(50U, "test", true);
                sp.PlaySeInScript("se_square", 1f);
                sp.FadeBgmInScript(-1f, 0.5f, 1.2f, false, -1);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioTutorialText(2, 154, -1), 3, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.HideInterface(50U, true);
                gd.forwardNext = true;
            };
            __instance.actions[37] = action;
        }


        [HarmonyPatch(typeof(TutorialLoop3Scenario), "SetParam")]
        [HarmonyPostfix]
        static void TutorialLoop3(TutorialLoop3Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[2];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.WaitSec(0.5f, true);
                List<string> list = Util.Split(sp.m_rs.GetScenarioTutorialText(3, 3, 0), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[11], -1, 2, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(3, 4, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 0, list, false, false, false, true);
                sp.PlayBgmInScript("bgm02", 0f, 1f, -1, true);
                sp.WaitText(50U, "test", true);
                Plugin.CheckLocationsInScript(500);
                sp.HideInterface(50U, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(3, 5, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 2, list, true, false, false, true);
                Plugin.CheckLocationsInScript(400);
                sp.ShowChara(ad.targetP, 5, 0, 20U, false);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(3, 6, -1), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, -1, list, true, false, true, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(3, 7, 1), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[3], ad.targetP, 1, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(3, 8, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, gd.personFromId[3], 0, list, false, false, false, true);
            };
            __instance.actions[2] = action;
        }


        [HarmonyPatch(typeof(TutorialLoop4Scenario), "SetParam")]
        [HarmonyPostfix]
        static void TutorialLoop4(TutorialLoop4Scenario __instance)
        {
            var options = ArchipelagoClient.ServerData.SlotData.Options;
            if (!(options?.RandomizeRoleUnlocks ?? true))
                return;
            ScenarioContents.ActionContents action = __instance.actions[18];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                gd.GainHate(ad.mainP, 0.2f);
                List<string> list16;
                if (gd.chara[gd.personFromId[5]].doa == Setting.Doa.doa_Seizon)
                {
                    list16 = Util.Split(sp.m_rs.GetScenarioTutorialText(4, 72, 6), new char[] { '|' });
                    sp.SetNormalSerifu(gd.personFromId[5], -1, 1, list16, true, false, false, true);
                }
                else
                {
                    list16 = Util.Split(sp.m_rs.GetScenarioTutorialText(4, 73, 1), new char[] { '|' });
                    sp.SetNormalSerifu(gd.personFromId[2], -1, 1, list16, true, false, false, true);
                }
                if (gd.chara[gd.personFromId[1]].doa == Setting.Doa.doa_Seizon)
                {
                    list16 = Util.Split(sp.m_rs.GetScenarioTutorialText(4, 74, 5), new char[] { '|' });
                    sp.SetNormalSerifu(gd.personFromId[1], -1, 2, list16, true, false, false, true);
                }
                else
                {
                    list16 = Util.Split(sp.m_rs.GetScenarioTutorialText(4, 75, 0), new char[] { '|' });
                    sp.SetNormalSerifu(gd.personFromId[3], -1, 2, list16, true, false, false, true);
                }
                list16 = Util.Split(sp.m_rs.GetScenarioTutorialText(4, 76, 1), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[4], -1, 0, list16, true, false, false, true);
                Plugin.CheckLocationsInScript(1503);
                if (gd.chara[gd.personFromId[2]].doa == Setting.Doa.doa_Seizon)
                {
                    list16 = Util.Split(sp.m_rs.GetScenarioTutorialText(4, 77, 4), new char[] { '|' });
                    sp.SetNormalSerifu(gd.personFromId[2], gd.personFromId[4], 1, list16, true, false, false, true);
                }
                else
                {
                    list16 = Util.Split(sp.m_rs.GetScenarioTutorialText(4, 78, 2), new char[] { '|' });
                    sp.SetNormalSerifu(gd.personFromId[3], gd.personFromId[4], 2, list16, true, false, false, true);
                }
                list16 = Util.Split(sp.m_rs.GetScenarioTutorialText(4, 79, 1), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[4], -1, 0, list16, true, false, false, true);
                int num5 = gd.personFromId[1];
                if (gd.chara[gd.personFromId[3]].doa == Setting.Doa.doa_Seizon)
                {
                    list16 = Util.Split(sp.m_rs.GetScenarioTutorialText(4, 80, 5), new char[] { '|' });
                    sp.SetNormalSerifu(gd.personFromId[3], gd.personFromId[4], 2, list16, true, false, false, true);
                    num5 = gd.personFromId[3];
                }
                else
                {
                    list16 = Util.Split(sp.m_rs.GetScenarioTutorialText(4, 81, 0), new char[] { '|' });
                    sp.SetNormalSerifu(gd.personFromId[1], gd.personFromId[4], 2, list16, true, false, false, true);
                }
                list16 = Util.Split(sp.m_rs.GetScenarioTutorialText(4, 82, 6), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[4], num5, 0, list16, true, false, false, true);
                if (gd.chara[gd.personFromId[2]].doa == Setting.Doa.doa_Seizon)
                {
                    list16 = Util.Split(sp.m_rs.GetScenarioTutorialText(4, 83, 5), new char[] { '|' });
                    sp.SetNormalSerifu(gd.personFromId[2], gd.personFromId[4], 1, list16, true, false, false, true);
                    num5 = gd.personFromId[2];
                }
                else
                {
                    list16 = Util.Split(sp.m_rs.GetScenarioTutorialText(4, 84, 0), new char[] { '|' });
                    sp.SetNormalSerifu(gd.personFromId[5], gd.personFromId[4], 1, list16, true, false, false, true);
                    num5 = gd.personFromId[5];
                }
                list16 = Util.Split(sp.m_rs.GetScenarioTutorialText(4, 85, 5), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[4], num5, 0, list16, false, false, false, true);
                sp.WaitText(50U, "test", true);
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioTutorialText(4, 86, -1), 3, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    gd.baseData.sce_all_flg = gd.baseData.sce_all_flg | 8UL;
                    return true;
                }, (float e) => sp.GetSelect(0) >= 0, false));
                sp.HideInterface(50U, true);
                list16 = Util.Split(sp.m_rs.GetScenarioTutorialText(4, 87, 0), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[4], num5, 0, list16, true, true, false, true);
                if (gd.chara[gd.personFromId[11]].doa == Setting.Doa.doa_Seizon)
                {
                    list16 = Util.Split(sp.m_rs.GetScenarioTutorialText(4, 88, 0), new char[] { '|' });
                    sp.SetNormalSerifu(gd.personFromId[11], -1, 1, list16, false, false, false, true);
                }
                else
                {
                    list16 = Util.Split(sp.m_rs.GetScenarioTutorialText(4, 89, 0), new char[] { '|' });
                    sp.SetNormalSerifu(gd.personFromId[2], -1, 1, list16, false, false, false, true);
                }
                sp.WaitText(50U, "test", true);
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioTutorialText(4, 90, -1), 2, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.HideInterface(50U, true);
                gd.pos = 1;
                gd.forwardNext = true;
                string text12 = list16[0];
                list16[0] = text12;
            };
            __instance.actions[18] = action;
        }


        [HarmonyPatch(typeof(TutorialLoop5Scenario), "SetParam")]
        [HarmonyPostfix]
        static void TutorialLoop5(TutorialLoop5Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[2];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                List<string> list = Util.Split(sp.m_rs.GetScenarioTutorialText(5, 3, 0), new char[] { '|' });
                string text = list[0];
                Util.Replace(ref text, "{0}", gd.takashiName);
                list[0] = text;
                sp.SetNormalSerifu(-3, 0, 1, list, true, true, true, true);
                Plugin.CheckLocationsInScript(900);
                sp.PlayBgmInScript("bgm00", 1f, 0.75f, -1, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(5, 4, 0), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, 1, list, true, false, true, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(5, 5, 4), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[4], ad.mainP, 2, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(5, 6, 4), new char[] { '|' });
                text = list[0];
                Util.Replace(ref text, "{0}", gd.takashiName);
                list[0] = text;
                sp.SetNormalSerifu(ad.mainP, -1, 1, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(5, 7, 0), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[11], ad.mainP, 0, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(5, 8, 1), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[3], -1, 2, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(5, 9, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list, true, false, false, true);
                sp.PlayBgmInScript("bgm01", 2.5f, 1f, -1, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(5, 10, 5), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, 1, list, true, false, true, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(5, 11, 5), new char[] { '|' });
                text = list[0];
                Util.Replace(ref text, "{0}", gd.takashiName);
                list[0] = text;
                sp.SetNormalSerifu(gd.personFromId[11], 0, 0, list, true, true, true, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(5, 12, 5), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[11], 0, 0, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(5, 13, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, gd.personFromId[11], 1, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(5, 14, 0), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[11], ad.mainP, 0, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(5, 15, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, gd.personFromId[11], 1, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(5, 16, 5), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[11], -1, 0, list, false, false, false, true);
            };
            __instance.actions[2] = action;
        }


        [HarmonyPatch(typeof(TutorialLoop7Scenario), "SetParam")]
        [HarmonyPostfix]
        static void TutorialLoop7(TutorialLoop7Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[7];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.UnloadTexture("ive005_0");
                sp.UnloadTexture("ive005_0_1");
                Plugin.CheckLocationsInScript(600);
                List<string> list6 = Util.Split(sp.m_rs.GetScenarioTutorialText(7, 16, 0), new char[] { '|' });
                string text3 = list6[0];
                Util.Replace(ref text3, "{0}", gd.takashiName);
                list6[0] = text3;
                sp.SetNormalSerifu(ad.targetP, 0, 2, list6, true, false, false, true);
                list6 = Util.Split(sp.m_rs.GetScenarioTutorialText(7, 17, 0), new char[] { '|' });
                text3 = list6[0];
                Util.Replace(ref text3, "{0}", gd.takashiName);
                list6[0] = text3;
                sp.SetNormalSerifu(ad.mainP, -1, 0, list6, false, true, true, true);
                sp.WaitSec(0.4f, true);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.25f, 0, false, false, true);
                sp.UnvisibleAllChara(20U, -1);
                sp.ShowChara(ad.mainP, 4, 0, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(0f, 60f, 600f, 337.5f), 0.4f, -2.5f, false, null, true);
                sp.WaitClipAnim(new List<uint> { 0U, 20U }, true);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list6 = Util.Split(sp.m_rs.GetScenarioTutorialText(7, 18, 0), new char[] { '|' });
                Plugin.CheckLocationsInScript(800);
                sp.SetNormalSerifu(ad.mainP, -1, 0, list6, true, true, false, true);
                list6 = Util.Split(sp.m_rs.GetScenarioTutorialText(7, 19, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 2, list6, true, false, false, true);
                list6 = Util.Split(sp.m_rs.GetScenarioTutorialText(7, 20, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 0, list6, true, false, false, true);
                list6 = Util.Split(sp.m_rs.GetScenarioTutorialText(7, 21, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 2, list6, true, false, false, true);
                sp.ShowChara(ad.mainP, 6, 0, 20U, false);
                sp.SetNormalClipAnim(-1);
                sp.WaitClipAnim(new List<uint> { 0U, 20U }, true);
                list6 = Util.Split(sp.m_rs.GetScenarioTutorialText(7, 22, -1), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, 0, list6, true, true, true, true);
                list6 = Util.Split(sp.m_rs.GetScenarioTutorialText(7, 23, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 2, list6, true, false, false, true);
                list6 = Util.Split(sp.m_rs.GetScenarioTutorialText(7, 24, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 0, list6, true, false, false, true);
                list6 = Util.Split(sp.m_rs.GetScenarioTutorialText(7, 25, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 0, list6, true, false, false, true);
                list6 = Util.Split(sp.m_rs.GetScenarioTutorialText(7, 26, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 2, list6, true, false, false, true);
                sp.WaitSec(0.05f, false);
                sp.PlaySeInScript("se_ashioto_02", 0.7f);
                sp.FadeBgmInScript(-1f, 0f, 1.2f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 30U, 0.75f, 0, true, true, true);
                sp.UnloadPlace();
                sp.LoadPlace(5, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetColorScreen(255U, 50003U, -1);
                    sp.m_sb[50003U].SetFade(0.75f, 0f, 0, -1f, -1, false);
                    return true;
                }, (float e) => true, false));
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetColorScreen(uint.MaxValue, 50001U, -1);
                    sp.SetScreen(Setting.Screen.s_PlaceName, 50002U, true, false, -1);
                    return true;
                }, (float e) => true, false));
                sp.WaitLoad();
                sp.WaitFade(new List<uint> { 50003U }, true, false);
                gd.forwardNext = true;
            };
            __instance.actions[7] = action;
        }


        [HarmonyPatch(typeof(TutorialLoop9Scenario), "SetParam")]
        [HarmonyPostfix]
        static void TutorialLoop9(TutorialLoop9Scenario __instance)
        {
            var options = ArchipelagoClient.ServerData.SlotData.Options;
            if (!(options?.RandomizeRoleUnlocks ?? true))
                return;
            ScenarioContents.ActionContents action = __instance.actions[3];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                ad.mainP = gd.personFromId[4];
                gd.GainHate(ad.mainP, 0.2f);
                gd.GainHate(ad.targetP, 0.2f);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    gd.baseData.sce_all_flg = gd.baseData.sce_all_flg | 64UL;
                    return true;
                }, (float e) => true, false));
                sp.FadeBgmInScript(0f, 0.6f, 2f, false, -1);
                sp.PlaySeInScript("se_jin_05", 1f);
                List<string> list2 = Util.Split(sp.m_rs.GetScenarioTutorialText(9, 18, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 1, list2, false, false, false, true);
                sp.WaitText(50, "test", true);
                sp.HideInterface(50, true);
                Plugin.CheckLocationsInScript(1506);
            };
            __instance.actions[3] = action;
        }


        [HarmonyPatch(typeof(TutorialLoop10Scenario), "SetParam")]
        [HarmonyPostfix]
        static void TutorialLoop10(TutorialLoop10Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[2];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                gd.pos = 1;
                List<string> list = Util.Split(sp.m_rs.GetScenarioTutorialText(10, 3, -1), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, -1, list, true, true, true, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(10, 4, -1), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, -1, list, true, true, true, true);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.4f, 0, false, true, true);
                sp.UnloadTexture("ive007_0");
                sp.LoadTexture("ive007_1");
                sp.WaitLoad();
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ive007_1", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.SetColorScreen(255U, 30U, -1);
                    sp.m_sb[30U].SetFade(0.4f, 0f, 0, 1f, -1, false);
                    return true;
                }, (float e) => true, true));
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(10, 5, -1), new char[] { '|' });
                sp.SetNormalSerifu(-3, -1, -1, list, true, true, true, true);
                Plugin.CheckLocationsInScript(1400);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(10, 6, -1), new char[] { '|' });
                sp.SetNormalSerifu(-3, -1, -1, list, true, true, true, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(10, 7, -1), new char[] { '|' });
                sp.SetNormalSerifu(-3, -1, -1, list, true, true, true, true);
                sp.FadeBgmInScript(-1f, 0.4f, 1f, false, -1);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(10, 8, -1), new char[] { '|' });
                sp.SetNormalSerifu(-3, -1, -1, list, true, true, true, true);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.4f, 4, false, true, true);
                sp.UnloadTexture("ive007_1");
                sp.LoadTexture("ive007_2");
                sp.WaitLoad();
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, true));
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.4f, 4, false, true, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.m_sb[31U].SetAlphaCoeff(1f);
                    return true;
                }, (float e) => true, true));
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ive007_2", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    return true;
                }, (float e) => true, true));
                sp.ShowChara(sd.mainP, 0, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(120f, 40f, 720f, 405f), 0f, 1f, true, null, true);
                sp.WaitFade(new List<uint> { 31U }, true, true);
                sp.WaitSec(0.25f, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(10, 9, -1), new char[] { '|' });
                string text = list[0];
                Util.Replace(ref text, "{0}", gd.takashiName);
                list[0] = text;
                sp.SetNormalSerifu(sd.mainP, 0, 1, list, true, true, true, true);
                Plugin.CheckLocationsInScript(1000);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(10, 10, -1), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, -1, list, true, false, true, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(10, 11, 6), new char[] { '|' });
                text = list[0];
                Util.Replace(ref text, "{0}", gd.takashiName);
                list[0] = text;
                sp.SetNormalSerifu(sd.mainP, 0, 1, list, true, false, false, true);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.4f, 2, false, true, true);
                sp.LoadTexture("ive007_2_1");
                sp.WaitLoad();
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, true));
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.4f, 2, false, true, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 10U, false, false, -1);
                    sp.m_sb[10U].SetTexture(0, sp.m_sb[10U].gameObject.transform, 0U, "ive007_2", null, null);
                    sp.m_sb[10U].m_spriteMap[0U].SetVisible(true);
                    sp.SetScreen(Setting.Screen.s_none, 20U, false, false, -1);
                    sp.m_sb[10U].SetTexture(0, sp.m_sb[10U].gameObject.transform, 1U, "ive007_2_1", new Vector2?(new Vector2(267.75f, 0f)), null);
                    sp.m_sb[10U].m_spriteMap[1U].SetVisible(true);
                    return true;
                }, (float e) => true, true));
                sp.FadeBgmInScript(-1f, 0.75f, 0.4f, false, -1);
                sp.LoadTexture("ive007_3");
                sp.WaitLoad();
                sp.WaitFade(new List<uint> { 31U }, true, true);
                sp.WaitSec(0.25f, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(10, 12, -1), new char[] { '|' });
                sp.SetNormalSerifu(sd.targetP, -1, 1, list, true, true, true, true);
                sp.FadeBgmInScript(-1f, 1f, 1.2f, false, -1);
                sp.SetFadeScreen(new List<uint> { 10U, 20U }, 30U, 0.4f, 6, false, true, true);
                sp.UnloadTexture("ive007_2");
                sp.UnloadTexture("ive007_2_1");
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ive007_3", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, true));
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(10, 13, -1), new char[] { '|' });
                sp.SetNormalSerifu(sd.targetP, -1, 1, list, true, true, true, true);
                sp.WaitSec(0.6f, true);
                sp.FadeBgmInScript(0.4f, 0f, 0.4f, true, -1);
                sp.PlaySeInScript("se_gatyan", 1f);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.1f, 0, false, true, true);
                sp.UnloadTexture("ive007_3");
                sp.LoadTexture("p14a");
                sp.WaitLoad();
                sp.LoadPlace(12, true);
                sp.WaitLoad();
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_none, 18U, false, false, -1);
                    uint num3 = 14U;
                    sp.m_sb[18U].SetPackedTexture(0, sp.m_sb[18U].gameObject.transform, "p14a", "body", 100U * num3, 10U, new Vector2?(new Vector2((float)(18446744073709551416UL + (ulong)(50U * num3)), 0f)), null, null, false);
                    sp.m_sb[18U].m_spriteMap[100U * num3].SetSize(0.7f);
                    sp.m_sb[18U].m_spriteMap[100U * num3].SetDisplayOffsetY((float)sp.m_rs.m_displaySize.height - sp.m_sb[18U].m_spriteMap[100U * num3].GetSizeInDisplay().y * sp.m_sb[18U].m_spriteMap[100U * num3].GetSize() * GraphicsContext.m_textureRatio);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, true));
                sp.ShowChara(sd.targetP, 4, 1, 18U, false);
                sp.WaitSec(0.8f, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_LeviMes, 35U, true, false, -1);
                    return true;
                }, (float e) => true, true));
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.8f, 0, false, true, true);
                sp.WaitFade(new List<uint> { 31U }, true, true);
                sp.WaitSec(0.2f, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(10, 14, -1), new char[] { '|' });
                sp.SetNormalSerifu(-2, -1, -1, list, true, true, true, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(10, 15, -1), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, -1, list, true, true, true, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(10, 16, -1), new char[] { '|' });
                sp.SetFadeScreen(new List<uint> { 0U, 18U }, 30U, 0.25f, 0, false, false, true);
                sp.UnvisibleAllChara(18U, -1);
                sp.ShowChara(sd.mainP, 3, 2, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 18U, 20U }, new Vector4((float)sp.m_rs.m_displaySize.width * 0.25f * 2f, 80f, (float)sp.m_rs.m_displaySize.width * 0.5f, (float)sp.m_rs.m_displaySize.height * 0.5f), 0.4f, -2.5f, false, null, true);
                sp.WaitClipAnim(new List<uint> { 0U, 18U, 20U }, true);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.SetInterface(50U, sd.mainP, 0, true, true);
                sp.SetText(list[0], false, 50U, "test");
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.25f, 0, false, false, true);
                sp.UnvisibleAllChara(20U, -1);
                sp.ShowChara(sd.targetP, (gd.chara[sd.targetP].i_yaku == Setting.Yakuwari.y_Jinro) ? 1 : 0, 1, 18U, false);
                sp.SetClipAnim(new List<uint> { 0U, 18U }, new Vector4((float)sp.m_rs.m_displaySize.width * 0.25f * 1f, 80f, (float)sp.m_rs.m_displaySize.width * 0.5f, (float)sp.m_rs.m_displaySize.height * 0.5f), 0.4f, -2.5f, false, null, true);
                sp.WaitClipAnim(new List<uint> { 0U, 18U }, true);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(0.6f, true);
                sp.RemoveScreenInScript(35U);
                sp.SetFadeScreen(new List<uint> { 0U, 18U, 20U }, 30U, 1.2f, 0, false, true, true);
                sp.WaitSec(0.6f, true);
                sp.PlaySeInScript("se_ashioto_02", 0.7f);
                sp.UnloadPlace();
                sp.UnloadTexture("p14a");
                sp.WaitFade(new List<uint> { 30U }, true, true);
                gd.forwardNext = true;
            };
            __instance.actions[2] = action;
            action = __instance.actions[9];
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
                int pos = gd.pos;
                if (ad.mainP == 0)
                {
                    ad.mainP = sd.mainP;
                }
                GameData.character character2 = gd.chara[ad.mainP];
                Jinro.ClearTable(ref character2.p_knowTable);
                Jinro.IsY((Setting.Yakuwari)ad.type, ad.mainP, ref character2.p_knowTable);
                for (int j = 1; j <= 5; j++)
                {
                    if (j != ad.type)
                    {
                        Jinro.IsNotY((Setting.Yakuwari)j, ad.mainP, ref gd.knowTable);
                    }
                }
                character2.p_yaku = (Setting.Yakuwari)ad.type;
                Jinro.MakeYakuAliveNum(ref gd);
                gd.chara[ad.mainP] = character2;
                ad.ctuizuiP = Jinro.MakeFakeRireki(ad.mainP, (Setting.Yakuwari)ad.type, ref gd);
                gd.peopleFlg[6] = (ushort)((int)gd.peopleFlg[6] | (1 << ad.mainP));
                gd.makeRate = true;
                gd.GainHate(ad.mainP, 0.2f);
                sp.FadeBgmInScript(-1f, 0.5f, 0.8f, false, -1);
                List<string> list6 = Util.Split(sp.m_rs.GetScenarioTutorialText(10, 35, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, gd.pos, list6, true, false, false, true);
                list6 = Util.Split(sp.m_rs.GetScenarioTutorialText(10, 36, 0), new char[] { '|' });
                string text2 = list6[0];
                Util.Replace(ref text2, "{0}", MyUtils.GetCharaName(chara, gd, ad.targetP));
                list6[0] = text2;
                sp.SetNormalSerifu(ad.mainP, -1, gd.pos, list6, true, true, true, true);
                int num5 = -1;
                if (((long)(gd.peopleFlg[0] & ~gd.peopleFlg[6]) & (long)(1UL << (gd.personFromId[2] & 31))) > 0L)
                {
                    num5 = 2;
                    list6 = Util.Split(sp.m_rs.GetScenarioTutorialText(10, 37, 5), new char[] { '|' });
                }
                else if (((long)(gd.peopleFlg[0] & ~gd.peopleFlg[6]) & (long)(1UL << (gd.personFromId[3] & 31))) > 0L)
                {
                    num5 = 3;
                    list6 = Util.Split(sp.m_rs.GetScenarioTutorialText(10, 38, 0), new char[] { '|' });
                }
                else if (((long)(gd.peopleFlg[0] & ~gd.peopleFlg[6]) & (long)(1UL << (gd.personFromId[5] & 31))) > 0L)
                {
                    num5 = 5;
                    list6 = Util.Split(sp.m_rs.GetScenarioTutorialText(10, 39, 6), new char[] { '|' });
                }
                else if (((long)(gd.peopleFlg[0] & ~gd.peopleFlg[6]) & (long)(1UL << (gd.personFromId[8] & 31))) > 0L)
                {
                    num5 = 8;
                    list6 = Util.Split(sp.m_rs.GetScenarioTutorialText(10, 40, 5), new char[] { '|' });
                }
                if (num5 > 0)
                {
                    gd.peopleFlg[6] = (ushort)((int)gd.peopleFlg[6] | (1 << gd.personFromId[num5]));
                    sp.SetNormalSerifu(gd.personFromId[num5], ad.mainP, gd.GetNextPos(), list6, true, false, false, true);
                }
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    gd.baseData.sce_all_flg = gd.baseData.sce_all_flg | 4UL;
                    return true;
                }, (float e) => true, false));
                sp.FadeBgmInScript(0f, 0.5f, 2f, false, -1);
                sp.PlaySeInScript("se_jin_01", 1f);
                list6 = Util.Split(sp.m_rs.GetScenarioTutorialText(10, 41, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, gd.personFromId[num5], pos, list6, false, false, false, true);
                gd.pos = pos;
                sp.WaitText(50U, "test", true);
                if (options?.RandomizeRoleUnlocks ?? true)
                {
                    Plugin.CheckLocationsInScript(1502);
                }
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioTutorialText(10, 42, -1), 3, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.HideInterface(50U, true);
                gd.forwardNext = true;
            };
            __instance.actions[9] = action;
        }


        [HarmonyPatch(typeof(TutorialLoop11Scenario), "SetParam")]
        [HarmonyPostfix]
        static void TutorialLoop11(TutorialLoop11Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[4];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                var options = ArchipelagoClient.ServerData.SlotData.Options;
                if (!(options?.RandomizeNotes ?? true))
                    return;
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                List<string> list3;
                if (ad.mainP == 0)
                {
                    list3 = Util.Split(sp.m_rs.GetScenarioTutorialText(11, 9, -1), new char[] { '|' });
                    sp.SetNormalSerifu(0, ad.targetP, 1, list3, true, false, true, true);
                }
                list3 = Util.Split(sp.m_rs.GetScenarioTutorialText(11, 10, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list3, true, false, false, true);
                list3 = Util.Split(sp.m_rs.GetScenarioTutorialText(11, 11, 1), new char[] { '|' });
                string text3 = list3[0];
                Util.Replace(ref text3, "{0}", gd.takashiName);
                list3[0] = text3;
                sp.SetNormalSerifu(ad.targetP, 0, 1, list3, true, true, false, true);
                list3 = Util.Split(sp.m_rs.GetScenarioTutorialText(11, 12, 0), new char[] { '|' });
                sp.SetNormalSerifu(0, ad.targetP, 1, list3, false, false, false, true);
                sp.LoadTexture("ivep03_01_0");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetColorScreen(uint.MaxValue, 50001U, -1);
                    sp.m_sb[50001U].SetFadeIn(0.3f, 0);
                    sp.SetScreen(Setting.Screen.s_LightBall, 45U, false, false, -1);
                    return true;
                }, (float e) => true, true));
                sp.PlaySeInScript("se_hikaruball", 1f);
                sp.FadeBgmInScript(-1f, 0.7f, 1f, false, -1);
                sp.WaitFade(new List<uint> { 50001U }, false, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 32U, false, false, -1);
                    sp.m_sb[32U].SetTexture(0, sp.m_sb[32U].gameObject.transform, 0U, "ivep03_01_0", null, null);
                    sp.m_sb[32U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, true));
                sp.SetFadeScreen(new List<uint> { 50001U }, 50002U, 1f, 0, true, true, true);
                list3 = Util.Split(sp.m_rs.GetScenarioTutorialText(11, 13, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list3, true, true, false, true);
                sp.PlaySeInScript("se_hikaruball", 1f);
                sp.FadeBgmInScript(-1f, 1f, 1f, false, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.4f, 0, false, true, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetColorScreen(uint.MaxValue, 35U, -1);
                    sp.m_sb[35U].SetFadeIn(0.4f, 0);
                    return true;
                }, (float e) => true, false));
                sp.UnloadPlace();
                sp.LoadTexture("ive00_1");
                sp.WaitLoad();
                sp.LoadTexture("ive008_0_1");
                sp.WaitLoad();
                sp.LoadTexture("ive008_0_2");
                sp.WaitLoad();
                sp.WaitFade(new List<uint> { 35U }, false, true);
                sp.RemoveScreenInScript(32U);
                sp.UnloadTexture("ivep03_01_0");
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ive00_1", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, false));
                sp.SetFadeScreen(new List<uint> { 35U }, 36U, 1.2f, 0, true, true, true);
                list3 = Util.Split(sp.m_rs.GetScenarioTutorialText(11, 14, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list3, true, true, true, true);
                sp.PlaySeInScript("se_square", 1f);
                sp.FadeBgmInScript(-1f, 1f, 1f, false, -1);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetColorScreen(sp.m_rs.defColor_blueScr, 41U, -1);
                    sp.m_sb[41U].SetAlphaCoeff(0f);
                    sp.m_sb[41U].SetFade(0.35f, 0.4f, 0, 0f, -1, false);
                    sp.SetScreen(Setting.Screen.s_none, 42U, false, false, -1);
                    TextArea textArea = UnityEngine.Object.Instantiate<TextArea>(sp.m_rs.textAreaPrefab, sp.m_sb[42U].gameObject.transform);
                    textArea.name = "listTextArea";
                    sp.m_sb[42U].SetTextArea(textArea, "list", 38, 20, 28, new Vector2(480f, 0f), 5, 0, sp.m_rs.m_defaultFont, TextAlign.k_text_Center, null);
                    sp.m_sb[42U].m_textAreaMap["list"].SetSize(0.5f);
                    return true;
                }, (float e) => true, false));
                sp.SetClipAnim(new List<uint> { 41U, 42U }, new Vector4(0f, 0f, (float)sp.m_rs.m_displaySize.width, (float)sp.m_rs.m_displaySize.height), 0.35f, -4f, false, new Vector4?(new Vector4(0f, -30000f, (float)sp.m_rs.m_displaySize.width, (float)(sp.m_rs.m_displaySize.height + 60000))), false);
                sp.SetText("RL:E nm:11 g:2 E D A K\nst,gn,sq,jn,kk,nn,sg,sm,yr,lv,rc\nD1:CO_E st gn jn,CS rc,DEL kk\nD2:CO_D sm,WH yr,CS gn,DEL sg\nD3:CS lv,DEL st\n---------------Loop158---------------\nRL:N nm:6 g:1 E\nst,gn,sq,lv,sg,rc\nD1:CS rc,DEL gn\nD2:CO_E sg,WH st,CS sq\n---------------Loop159---------------\nRL:E nm:15 g:2 E D A K W\nst,gn,sq,jn,kk,nn,sg,sm,yr,lv,rc,cp,rn\nD1:CO_E yr sq,CO_W sg jn,CS sm,DEL sg\nD2:CS rn,DEL jn\nD3:CO_D nn,BL sq,CS sq,DEL nn\nD4:CS lv,DEL st\n---------------Loop160---------------\nRL:G nm:11 g:2 E D A K\nst,gn,sq,jn,kk,nn,sg,lv,rc,cp,cm\nD1:\n", false, 42U, "list");
                sp.WaitClipAnim(new List<uint> { 41U, 42U }, false);
                sp.WaitSec(0.6f, true);
                list3 = Util.Split(sp.m_rs.GetScenarioTutorialText(11, 16, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list3, false, true, true, true);
                sp.WaitText(50U, "test", false);
                List<long> locationIds = new List<long>();
                for (int j = 1; j < (int)gd.baseData.totalNum; j++)
                {
                    int id = gd.chara[j].id;
                    locationIds.Add(id * 100 + 1);
                }
                Plugin.CheckLocationsInScript(locationIds.ToArray());
            };
            __instance.actions[4] = action;
        }


        [HarmonyPatch(typeof(TutorialLoop14Scenario), "SetParam")]
        [HarmonyPostfix]
        static void TutorialLoop12(TutorialLoop14Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[13];
            action.FinAtHere = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Get options
                var options = ArchipelagoClient.ServerData.SlotData.Options;
                //Base
                sp.WaitSec(0.1f, false);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 60U, 0.8f, 0, false, true, true);
                sp.UnloadPlace();
                sp.UnloadTexture("p11a");
                sp.WaitSec(0.2f, true);
                sp.PlaySeInScript("se_ashioto_03", 0.7f);
                sp.WaitFade(new List<uint> { 60U }, true, true);
                sp.WaitSec(1f, true);
                int targetP = ad.targetP;
                if (options?.RandomizeNotes ?? true)
                {
                    Plugin.CheckLocationsInScript(1301);
                }
                sp.WaitSec(0.4f, true);
                gd.SetState(28, ref sd);
                gd.forwardNext = true;
            };
            __instance.actions[13] = action;
            action = __instance.actions[3];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.WaitSec(0.5f, true);
                gd.pos = 1;
                sp.PlayBgmInScript("bgm01", 0f, 1f, -1, true);
                List<string> list = Util.Split(sp.m_rs.GetScenarioTutorialText(12, 4, 0), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[10], -1, 1, list, true, true, true, true);
                sp.FadeBgmInScript(-1f, 0f, 2f, true, -1);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(12, 5, 0), new char[] { '|' });
                sp.SetNormalSerifu(-3, gd.personFromId[10], 1, list, true, false, true, true);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.25f, 0, false, false, true);
                sp.UnvisibleAllChara(20U, -1);
                sp.ShowChara(sd.counterP, 5, 2, 20U, false);
                sp.SetNormalClipAnim(2);
                sp.WaitClipAnim(new List<uint> { 0U, 20U }, true);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(12, 6, 5), new char[] { '|' });
                sp.SetNormalSerifu(-3, gd.personFromId[10], 2, list, false, true, true, true);
                sp.WaitSec(0.2f, true);
                sp.PlayBgmInScript("bgm02", 0f, 1f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                Plugin.CheckLocationsInScript(1200);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(12, 7, 4), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[8], sd.counterP, 0, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(12, 8, 0), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[3], sd.counterP, 1, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(12, 9, 3), new char[] { '|' });
                sp.SetNormalSerifu(sd.counterP, gd.personFromId[3], 2, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(12, 10, 5), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[11], -1, 0, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(12, 11, 5), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[4], -1, 1, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(12, 12, 0), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[1], -1, 0, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(12, 13, 6), new char[] { '|' });
                sp.SetNormalSerifu(sd.counterP, gd.personFromId[1], 2, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(12, 14, 3), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[10], -1, 0, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(12, 15, 4), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[3], -1, 1, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(12, 16, 4), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[2], gd.personFromId[5], 2, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(12, 17, 2), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[5], -1, 1, list, true, false, false, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_LeviMes, 35U, true, false, -1);
                    return true;
                }, (float e) => true, true));
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(12, 18, -1), new char[] { '|' });
                sp.SetNormalSerifu(-2, -1, -1, list, true, false, true, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(12, 19, 5), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[6], -1, 0, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(12, 20, 0), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[11], -1, 1, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(12, 21, -1), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, -1, list, true, false, true, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(12, 22, 6), new char[] { '|' });
                sp.SetNormalSerifu(sd.counterP, -1, 2, list, false, false, false, true);
            };
            __instance.actions[3] = action;
            action = __instance.actions[10];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.WaitSec(0.05f, false);
                sp.PlaySeInScript("se_ashioto_02", 0.7f);
                sp.FadeBgmInScript(-1f, 0f, 1.2f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 30U, 0.75f, 4, true, true, true);
                sp.UnloadPlace();
                sp.UnloadTexture("p11a");
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetColorScreen(255U, 50003U, -1);
                    sp.m_sb[50003U].SetFade(0.75f, 0f, 0, -1f, -1, false);
                    return true;
                }, (float e) => true, false));
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    gd.baseData.place = 16;
                    sp.SetColorScreen(uint.MaxValue, 50001U, -1);
                    sp.SetScreen(Setting.Screen.s_PlaceName, 50002U, true, false, -1);
                    return true;
                }, (float e) => true, false));
                sp.WaitLoad();
                sp.WaitFade(new List<uint> { 50003U }, true, false);
                sp.StopAllSeInScript();
                sp.PlaySeInScript("se_jidoudoa", 1f);
                sp.LoadTexture("ive009_1_0");
                sp.WaitLoad();
                sp.LoadTexture("ive009_1_1");
                sp.WaitLoad();
                sp.LoadTexture("ive009_1_2");
                sp.WaitLoad();
                sp.LoadTexture("ive009_0");
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_Interface, 50U, true, false, -1);
                    sp.m_sb[50001U].SetFade(0.3f, 0f, 0, -1f, -1, false);
                    sp.m_sb[50002U].SetFade(0.3f, 0f, 0, -1f, -1, false);
                    return true;
                }, (float e) => true, true));
                sp.WaitFade(new List<uint> { 50001U, 50002U }, true, true);
                List<string> list7 = Util.Split(sp.m_rs.GetScenarioTutorialText(12, 39, -1), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, -1, list7, true, true, true, true);
                sp.WaitLoad();
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ive009_0", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.SetColorScreen(255U, 40U, -1);
                    return true;
                }, (float e) => true, false));
                sp.SetClipAnim(new List<uint> { 0U }, new Vector4(0f, 0f, 960f, 540f), 3.2f, 1f, false, new Vector4?(new Vector4(0f, 422f, 960f, 540f)), true);
                sp.SetFadeScreen(new List<uint> { 40U }, 41U, 0.6f, 0, false, true, true);
                sp.WaitFade(new List<uint> { 41U }, true, true);
                sp.PlayBgmInScript("bgm15", 0f, 1f, -1, true);
                sp.WaitSec(1f, true);
                list7 = Util.Split(sp.m_rs.GetScenarioTutorialText(12, 40, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 1, list7, false, true, true, false);
                sp.WaitSec(0.4f, false);
                sp.WaitText(50U, "test", true);
                Plugin.CheckLocationsInScript(1300);
                sp.HideInterface(50U, true);
                sp.FadeBgmInScript(-1f, 0.7f, 1.5f, false, -1);
                list7 = Util.Split(sp.m_rs.GetScenarioTutorialText(12, 41, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 2, list7, true, true, true, false);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.4f, 6, false, true, true);
                sp.UnloadTexture("ive009_0");
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ive009_1_0", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 1U, "ive009_1_1", new Vector2?(new Vector2(419.25f, 65.25f)), null);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 2U, "ive009_1_2", new Vector2?(new Vector2(419.25f, 65.25f)), null);
                    return true;
                }, (float e) => true, false));
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list7 = Util.Split(sp.m_rs.GetScenarioTutorialText(12, 42, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 1, list7, false, true, true, false);
            };
            __instance.actions[10] = action;
        }


        [HarmonyPatch(typeof(TutorialLoop17Scenario), "SetParam")]
        [HarmonyPostfix]
        static void TutorialLoop13(TutorialLoop17Scenario __instance)
        {
            //Get options
            var options = ArchipelagoClient.ServerData.SlotData.Options;
            ScenarioContents.ActionContents action = __instance.actions[3];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.PlayBgmInScript("bgm10", 0f, 1f, -1, true);
                sp.WaitSec(0.5f, true);
                gd.pos = 1;
                List<string> list = Util.Split(sp.m_rs.GetScenarioTutorialText(13, 4, 0), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[9], -1, 1, list, true, true, true, true);
                sp.FadeBgmInScript(-1f, 0.5f, 0.6f, false, -1);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(13, 5, -1), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, -1, list, true, false, true, true);
                sp.FadeBgmInScript(-1f, 0.75f, 0.6f, false, -1);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(13, 6, 6), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[9], -1, 1, list, true, false, false, true);
                sp.PlayBgmInScript("bgm02", 0.75f, 1f, -1, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(13, 7, 1), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[13], gd.personFromId[9], 2, list, true, false, false, true);
                sp.PlayBgmInScript("bgm10", 0.4f, 1f, -1, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(13, 8, 5), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[9], gd.personFromId[13], 1, list, true, false, false, true);
                sp.FadeBgmInScript(-1f, 0.5f, 0.6f, false, -1);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(13, 9, 3), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[14], -1, 0, list, true, false, false, true);
                sp.PlayBgmInScript("bgm03", 1.4f, 0.85f, -1, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(13, 10, 3), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[10], -1, 1, list, true, false, false, true);
                sp.FadeBgmInScript(-1f, 0.25f, 0.8f, false, -1);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(13, 11, 4), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[10], -1, 1, list, true, true, false, true);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.25f, 0, false, false, true);
                sp.UnvisibleAllChara(20U, -1);
                sp.ShowChara(sd.targetP, 5, 2, 20U, false);
                sp.SetNormalClipAnim(2);
                sp.WaitClipAnim(new List<uint> { 0U, 20U }, true);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(13, 12, 5), new char[] { '|' });
                sp.SetNormalSerifu(-3, gd.personFromId[10], 2, list, true, true, true, true);
                Plugin.CheckLocationsInScript(700);
                sp.FadeBgmInScript(-1f, 0.85f, 1.5f, false, -1);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(13, 13, 0), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[4], gd.personFromId[10], 0, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(13, 14, 0), new char[] { '|' });
                sp.SetNormalSerifu(sd.targetP, gd.personFromId[4], 2, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(13, 15, 3), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[3], sd.targetP, 1, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(13, 16, 6), new char[] { '|' });
                sp.SetNormalSerifu(sd.targetP, gd.personFromId[3], 2, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(13, 17, 3), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[1], sd.targetP, 0, list, true, false, false, true);
                sp.SetNormalClipAnim(-1);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(13, 18, -1), new char[] { '|' });
                string text = list[0];
                Util.Replace(ref text, "{0}", gd.takashiName);
                list[0] = text;
                sp.WaitClipAnim(new List<uint> { 0U, 20U }, true);
                sp.SetNormalSerifu(gd.personFromId[11], 0, -1, list, true, true, true, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(13, 19, 5), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[11], 0, 2, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(13, 20, 0), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[11], 0, 2, list, true, true, false, true);
                sp.PlayBgmInScript("bgm01", 3.5f, 1f, -1, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(13, 21, 0), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[6], -1, 0, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioTutorialText(13, 22, 0), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[9], -1, 1, list, true, false, false, true);
                sp.WaitSec(0.05f, false);
                sp.SetNormalClipAnim(-1);
                sp.WaitClipAnim(new List<uint> { 0U, 20U }, true);
                if (options?.RandomizeNotes ?? true)
                {
                    Plugin.CheckLocationsInScript(901);
                }
                gd.forwardNext = true;
            };
            __instance.actions[3] = action;
            action = __instance.actions[4];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                ad.mainP = gd.personFromId[9];
                GameData.character character3 = gd.chara[ad.mainP];
                Jinro.ClearTable(ref character3.p_knowTable);
                Jinro.IsY((Setting.Yakuwari)ad.type, ad.mainP, ref character3.p_knowTable);
                for (int k = 1; k <= 5; k++)
                {
                    if (k != ad.type)
                    {
                        Jinro.IsNotY((Setting.Yakuwari)k, ad.mainP, ref gd.knowTable);
                    }
                }
                character3.p_yaku = (Setting.Yakuwari)ad.type;
                gd.chara[ad.mainP] = character3;
                Jinro.MakeYakuAliveNum(ref gd);
                gd.GainHate(ad.mainP, 0.1f);
                sp.FadeBgmInScript(-1f, 0.55f, 1.2f, false, -1);
                List<string> list2 = Util.Split(sp.m_rs.GetScenarioTutorialText(13, 25, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list2, true, false, false, true);
                list2 = Util.Split(sp.m_rs.GetScenarioTutorialText(13, 26, 2), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[3], ad.mainP, 2, list2, true, false, false, true);
                list2 = Util.Split(sp.m_rs.GetScenarioTutorialText(13, 27, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list2, true, false, false, true);
                if (gd.chara[gd.personFromId[2]].i_yaku == Setting.Yakuwari.y_Jinro)
                {
                    list2 = Util.Split(sp.m_rs.GetScenarioTutorialText(13, 28, 6), new char[] { '|' });
                }
                else
                {
                    list2 = Util.Split(sp.m_rs.GetScenarioTutorialText(13, 29, 6), new char[] { '|' });
                }
                sp.SetNormalSerifu(gd.personFromId[2], ad.mainP, 0, list2, true, false, false, true);
                list2 = Util.Split(sp.m_rs.GetScenarioTutorialText(13, 30, 5), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[4], -1, 2, list2, true, false, false, true);
                list2 = Util.Split(sp.m_rs.GetScenarioTutorialText(13, 31, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list2, true, false, false, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    gd.baseData.sce_all_flg = gd.baseData.sce_all_flg | 16UL;
                    return true;
                }, (float e) => true, false));
                sp.FadeBgmInScript(0f, 0.55f, 2f, false, -1);
                sp.PlaySeInScript("se_jin_01", 1f);
                list2 = Util.Split(sp.m_rs.GetScenarioTutorialText(13, 32, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list2, true, false, false, true);
                if (options?.RandomizeRoleUnlocks ?? true)
                {
                    Plugin.CheckLocationsInScript(1504);
                }
                list2 = Util.Split(sp.m_rs.GetScenarioTutorialText(13, 33, 1), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[10], ad.mainP, 0, list2, true, false, false, true);
                list2 = Util.Split(sp.m_rs.GetScenarioTutorialText(13, 34, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, gd.personFromId[7], 1, list2, false, false, false, true);
            };
            __instance.actions[4] = action;
            action = __instance.actions[11];
            action.FinAtHere = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.FadeBgmInScript(-1f, 0f, 1.5f, true, -1);
                sp.WaitSec(0.05f, false);
                sp.SetFadeScreen(new List<uint> { 50U }, 60U, 0.25f, 0, false, true, true);
                sp.UnloadPlace();
                sp.WaitFade(new List<uint> { 60U }, true, true);
                int mainP = sd.mainP;
                int targetP = sd.targetP;
                if (options?.RandomizeNotes ?? true)
                {
                    Plugin.CheckLocationsInScript(1201, 701);
                }
                sp.WaitSec(0.75f, true);
                gd.SetState(21, ref sd);
                gd.forwardNext = true;
            };
            __instance.actions[11] = action;
            action = __instance.actions[12];
            action.FinAtHere = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.FadeBgmInScript(-1f, 0f, 1.5f, true, -1);
                sp.WaitSec(0.05f, false);
                sp.SetFadeScreen(new List<uint> { 50U }, 60U, 0.25f, 0, false, true, true);
                sp.UnloadPlace();
                sp.WaitFade(new List<uint> { 60U }, true, true);
                int mainP = sd.mainP;
                int targetP = sd.targetP;
                if (options?.RandomizeNotes ?? true)
                {
                    Plugin.CheckLocationsInScript(1201, 701);
                }
                sp.WaitSec(0.75f, true);
                gd.SetState(21, ref sd);
                gd.forwardNext = true;
            };
            __instance.actions[12] = action;
        }

        [HarmonyPatch(typeof(TutorialLoop19Scenario), "SetParam")]
        [HarmonyPostfix]
        static void BugLoop(TutorialLoop19Scenario __instance)
        {
            var options = ArchipelagoClient.ServerData.SlotData.Options;
            if (!(options?.RandomizeRoleUnlocks ?? true))
                return;
            ScenarioContents.ActionContents action = __instance.actions[7];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.LoadPlace(5, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetColorScreen(uint.MaxValue, 50001U, -1);
                    sp.SetScreen(Setting.Screen.s_PlaceName, 50002U, true, false, -1);
                    sp.m_sb[50001U].SetFade(0.3f, 1f, 0, 0f, -1, false);
                    sp.m_sb[50002U].SetFade(0.3f, 1f, 0, 0f, -1, false);
                    return true;
                }, (float e) => true, false));
                sp.WaitFade(new List<uint> { 50001U, 50002U }, false, true);
                sp.WaitLoad();
                sp.PlayBgmInScript("bgm01", 1.2f, 1f, -1, true);
                sp.WaitSec(0.6f, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Interface, 50U, true, false, -1);
                    sp.m_sb[50001U].SetFade(1f, 0f, 0, -1f, -1, false);
                    sp.m_sb[50002U].SetFade(1f, 0f, 0, -1f, -1, false);
                    return true;
                }, (float e) => true, true));
                sp.WaitFade(new List<uint> { 50001U, 50002U }, true, true);
                List<string> list3 = Util.Split(sp.m_rs.GetScenarioTutorialText(14, 21, -1), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, 1, list3, true, true, true, true);
                sp.PlayBgmInScript("bgm19", 0f, 0.6f, -1, true);
                sp.FadeBgmInScript(-1f, 1f, 3f, false, -1);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.m_sb[0U].SetFade(10f, 0f, 101, 1f, 0, false);
                    return true;
                }, (float e) => true, true));
                list3 = Util.Split(sp.m_rs.GetScenarioTutorialText(14, 22, -1), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, 1, list3, true, true, true, true);
                list3 = Util.Split(sp.m_rs.GetScenarioTutorialText(14, 23, -1), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, 1, list3, true, true, true, true);
                sp.WaitFade(new List<uint> { 0U }, true, true);
                sp.RemoveScreenInScript(50U);
                sp.UnloadPlace();
                sp.FadeBgmInScript(-1f, 0.3f, 1.5f, false, -1);
                sp.WaitSec(0.5f, true);
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioTutorialText(14, 24, -1), 2, false);
                Plugin.CheckLocationsInScript(1508);
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioTutorialText(14, 25, -1), 3, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.WaitSec(0.5f, true);
                sp.FadeBgmInScript(-1f, 1f, 0.4f, false, -1);
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioTutorialText(14, 26, -1), 1, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                gd.forwardNext = true;
            };
            __instance.actions[7] = action;
        }
    }
}
