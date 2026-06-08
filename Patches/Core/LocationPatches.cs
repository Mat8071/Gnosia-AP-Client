using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using gnosia;
using JetBrains.Annotations;
using UnityEngine;
using coreSystem;
using util;
using setting;
using UnityEngine.UI;
using Mono.Cecil.Cil;
using System.Reflection;
using systemService.saveData;
using GnosiaArchipelagoRandomizer.Archipelago;
using Newtonsoft.Json.Serialization;
using Steamworks;

namespace GnosiaArchipelagoRandomizer.Patches.Core
{
    [HarmonyPatch]
    class LocationPatches
    {
        static string GetCharaName(Array chara, gnosia.GameData gd, int cid)
        {
            var entry = chara.GetValue((int)gd.chara[cid].id);
            var nameField = AccessTools.Field(entry.GetType(), "name");
            return (string)nameField.GetValue(entry);
        }


        [HarmonyPatch(typeof(Cipi1Scenario), "SetParam")]
        [HarmonyPostfix]
        static void LetsCollaborate(Cipi1Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[1];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base stuff
                gd.pos = 1;
                sp.WaitSec(0.5f, true);
                List<string> list = Util.Split(sp.m_rs.GetScenarioCipiText(0, 2, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list, true, true, true, true);
                sp.PlayBgmInScript("bgm02", 2f, 0.75f, -1, true);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(120f, 65f, 720f, 405f), 0.4f, -2.5f, true, null, true);
                list = Util.Split(sp.m_rs.GetScenarioCipiText(0, 3, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list, true, true, false, true);
                list = Util.Split(sp.m_rs.GetScenarioCipiText(0, 4, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list, true, true, false, true);
                list = Util.Split(sp.m_rs.GetScenarioCipiText(0, 5, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list, true, true, false, true);
                list = Util.Split(sp.m_rs.GetScenarioCipiText(0, 6, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list, true, true, false, true);
                sp.FadeBgmInScript(-1f, 0f, 3f, true, -1);
                sp.WaitSec(0.35f, true);
                list = Util.Split(sp.m_rs.GetScenarioCipiText(0, 7, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list, true, true, false, true);
                sp.WaitSec(0.05f, false);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 30U, 0.8f, 0, false, true, true);
                sp.UnloadPlace();
                sp.UnloadTexture("p06a");
                int mainP = ad.mainP;
                sp.WaitSec(0.25f, false);
                Plugin.CheckLocationsInScript(602, 603);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.PlayBgmInScript("bgm01", 1.2f, 1f, -1, true);
                sp.LoadPlace(5, true);
                sp.WaitLoad();
                sp.WaitSec(1.2f, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Interface, 50U, true, false, -1);
                    sp.SetColorScreen(255U, 50001U, -1);
                    return true;
                }, (float e) => true, true));
                sp.SetFadeScreen(new List<uint> { 50001U }, 50002U, 0.75f, 0, true, true, true);
                gd.forwardNext = true;
            };
            __instance.actions[1] = action;
            action = __instance.actions[6];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Get Other Stuff
                Type dataType = AccessTools.TypeByName("gnosia.Data");
                Array chara = (Array)AccessTools.Field(dataType, "Chara").GetValue(null);
                //Base
                sp.LoadTexture("p06a");
                sp.WaitSec(0.45f, true);
                sp.WaitLoad();
                sp.PlaySeInScript("se_jidoudoa", 1f);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_none, 20U, false, false, -1);
                    sp.SetScreen(Setting.Screen.s_Interface, 50U, true, false, -1);
                    uint num2 = 6U;
                    sp.m_sb[20U].SetPackedTexture(0, sp.m_sb[20U].gameObject.transform, "p06a", "body", 100U * num2, 10U, new Vector2?(new Vector2((float)(18446744073709551416UL + (ulong)(50U * num2)), 0f)), null, null, false);
                    sp.m_sb[20U].m_spriteMap[100U * num2].GetComponent<Image>().material = sp.m_rs.uiCharaDefaultMat;
                    sp.m_sb[20U].m_spriteMap[100U * num2].GetComponent<Image>().material.SetColor("_Color", Color.white);
                    sp.m_sb[20U].m_spriteMap[100U * num2].SetSize(0.7f);
                    sp.m_sb[20U].m_spriteMap[100U * num2].SetDisplayOffsetY((float)sp.m_rs.m_displaySize.height - sp.m_sb[20U].m_spriteMap[100U * num2].GetSizeInDisplay().y * sp.m_sb[20U].m_spriteMap[100U * num2].GetSize() * GraphicsContext.m_textureRatio);
                    return true;
                }, (float e) => true, true));
                sp.ShowChara(ad.mainP, 0, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(120f, 65f, 720f, 405f), 0f, 1f, false, null, true);
                sp.SetFadeScreen(new List<uint> { 50001U, 50002U }, 50003U, 0.3f, 0, true, true, true);
                sp.WaitSec(0.4f, true);
                List<string> list4 = Util.Split(sp.m_rs.GetScenarioCipiText(0, 21, 0), new char[] { '|' });
                string text2 = list4[0];
                Util.Replace(ref text2, "{1}", GetCharaName(chara, gd, ad.targetP));
                list4[0] = text2;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list4, false, false, true, true);
                sp.WaitSec(0.4f, true);
                sp.PlayBgmInScript("bgm03", 0f, 0.75f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list4 = Util.Split(sp.m_rs.GetScenarioCipiText(0, 22, 1), new char[] { '|' });
                text2 = list4[0];
                Util.Replace(ref text2, "{1}", GetCharaName(chara, gd, ad.targetP));
                list4[0] = text2;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list4, true, true, false, true);
                list4 = Util.Split(sp.m_rs.GetScenarioCipiText(0, 23, 3), new char[] { '|' });
                text2 = list4[0];
                Util.Replace(ref text2, "{1}", GetCharaName(chara, gd, ad.targetP));
                list4[0] = text2;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list4, true, true, false, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 1.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadTexture("p06a");
                sp.UnloadPlace();
                sp.WaitSec(0.5f, true);
                Plugin.CheckLocationsInScript(10);
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioCipiText(0, 25, -1), 3, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                gd.baseData.gainExp += 50U;
                gd.forwardNext = true;
            };
            __instance.actions[6] = action;
        }


        [HarmonyPatch(typeof(Cipi2Scenario), "SetParam")]
        [HarmonyPostfix]
        static void Chipie4Comet3(Cipi2Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[4];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                List<string> list4 = Util.Split(sp.m_rs.GetScenarioCipiText(1, 12, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 2, list4, true, false, false, true);
                list4 = Util.Split(sp.m_rs.GetScenarioCipiText(1, 13, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 1, list4, true, false, false, true);
                list4 = Util.Split(sp.m_rs.GetScenarioCipiText(1, 14, 3), new char[] { '|' });
                string text3 = list4[0];
                Util.Replace(ref text3, "{0}", gd.takashiName);
                list4[0] = text3;
                sp.SetNormalSerifu(ad.targetP, 0, 2, list4, true, false, false, true);
                list4 = Util.Split(sp.m_rs.GetScenarioCipiText(1, 15, 7), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 1, list4, true, false, false, true);
                list4 = Util.Split(sp.m_rs.GetScenarioCipiText(1, 16, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 2, list4, true, false, false, true);
                list4 = Util.Split(sp.m_rs.GetScenarioCipiText(1, 17, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list4, true, false, false, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 3.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadTexture("p06b");
                sp.UnloadPlace();
                sp.WaitSec(0.75f, true);
                int mainP = ad.mainP;
                int targetP = ad.targetP;
                Plugin.CheckLocationsInScript(604, 803);
                gd.baseData.gainExp += 100U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(0.4f, true);
                gd.forwardNext = true;
            };
            __instance.actions[4] = action;
        }


        [HarmonyPatch(typeof(Cipi3Scenario), "SetParam")]
        [HarmonyPostfix]
        static void Chipie5(Cipi3Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[7];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 3.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadTexture("p06b");
                sp.UnloadPlace();
                sp.WaitSec(0.75f, true);
                int mainP = ad.mainP;
                Plugin.CheckLocationsInScript(605);
                gd.baseData.gainExp += 50U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(0.8f, true);
                gd.forwardNext = true;
            };
            __instance.actions[7] = action;
        }


        [HarmonyPatch(typeof(Cipi4Scenario), "SetParam")]
        [HarmonyPostfix]
        static void Chipie6Shigemichi2(Cipi4Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[5];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad) 
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                List<string> list5;
                if ((gd.actionFlg & 16UL) == 0UL)
                {
                    list5 = Util.Split(sp.m_rs.GetScenarioCipiText(3, 19, 2), new char[] { '|' });
                    sp.SetNormalSerifu(ad.counterP, -1, 0, list5, true, false, false, true);
                    list5 = Util.Split(sp.m_rs.GetScenarioCipiText(3, 20, 5), new char[] { '|' });
                    sp.SetNormalSerifu(ad.mainP, ad.targetP, 1, list5, true, false, false, true);
                }
                list5 = Util.Split(sp.m_rs.GetScenarioCipiText(3, 21, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 2, list5, true, false, false, true);
                list5 = Util.Split(sp.m_rs.GetScenarioCipiText(3, 22, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, ad.targetP, 0, list5, true, false, false, true);
                list5 = Util.Split(sp.m_rs.GetScenarioCipiText(3, 23, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 2, list5, true, false, false, true);
                list5 = Util.Split(sp.m_rs.GetScenarioCipiText(3, 24, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 2, list5, true, true, false, true);
                list5 = Util.Split(sp.m_rs.GetScenarioCipiText(3, 25, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, ad.targetP, 0, list5, true, false, false, true);
                list5 = Util.Split(sp.m_rs.GetScenarioCipiText(3, 26, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 2, list5, true, false, false, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 3.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadTexture("p11a");
                sp.UnloadPlace();
                sp.WaitSec(0.75f, true);
                int mainP = ad.mainP;
                int targetP = ad.targetP;
                Plugin.CheckLocationsInScript(606, 502);
                gd.baseData.gainExp += 100U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(0.4f, true);
                gd.forwardNext = true;
            };
            __instance.actions[5] = action;
        }


        [HarmonyPatch(typeof(Comet1Scenario), "SetParam")]
        [HarmonyPostfix]
        static void Comet4(Comet1Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[10];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                List<string> list10;
                if ((sd.flg & 8192) > 0)
                {
                    list10 = Util.Split(sp.m_rs.GetScenarioCommetText(0, 40, 2), new char[] { '|' });
                    string text6 = list10[0];
                    Util.Replace(ref text6, "{0}", gd.takashiName);
                    list10[0] = text6;
                }
                else if (((int)gd.peopleFlg[0] & (1 << gd.personFromId[3])) != 0 && sd.targetP != gd.personFromId[3])
                {
                    list10 = Util.Split(sp.m_rs.GetScenarioCommetText(0, 41, 2), new char[] { '|' });
                }
                else
                {
                    list10 = Util.Split(sp.m_rs.GetScenarioCommetText(0, 42, 2), new char[] { '|' });
                }
                sp.SetNormalSerifu(ad.mainP, 0, 1, list10, true, true, false, true);
                list10 = Util.Split(sp.m_rs.GetScenarioCommetText(0, 43, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list10, true, true, false, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 1.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.UnloadTexture("p08a");
                sp.WaitSec(0.5f, true);
                int mainP = ad.mainP;
                Plugin.CheckLocationsInScript(804);
                gd.baseData.gainExp += 50U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                gd.forwardNext = true;
            };
            __instance.actions[10] = action;
        }


        [HarmonyPatch(typeof(Comet2Scenario), "SetParam")]
        [HarmonyPostfix]
        static void Comet5(Comet2Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[8];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                List<string> list8 = Util.Split(sp.m_rs.GetScenarioCommetText(1, 22, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list8, true, true, false, true);
                list8 = Util.Split(sp.m_rs.GetScenarioCommetText(1, 23, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list8, true, true, false, true);
                list8 = Util.Split(sp.m_rs.GetScenarioCommetText(1, 24, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list8, true, true, false, true);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.25f, 0, false, false, true);
                sp.UnvisibleAllChara(20U, -1);
                sp.ShowChara(ad.mainP, 3, 1, 20U, false);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list8 = Util.Split(sp.m_rs.GetScenarioCommetText(1, 25, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list8, false, true, true, true);
                sp.SetNormalClipAnim(-1);
                sp.WaitText(50U, "test", false);
                sp.PlaySeInScript("se_square", 1f);
                string scenarioCommetText = sp.m_rs.GetScenarioCommetText(1, 26, -1);
                Util.Replace(ref scenarioCommetText, "{0}", sp.m_rs.GetButtonName(2));
                int[] array = new int[] { 2, 2, 1 };
                sp.SetDialogScreen(50400U, scenarioCommetText, array[Setting.language], false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 1.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1.4f, 0, false, true, true);
                sp.UnloadSound("G_se_kyu_02");
                sp.UnloadSound("G_se_switch_door_03");
                sp.UnloadPlace();
                sp.UnloadTexture("p08c");
                sp.WaitSec(0.7f, true);
                int mainP = ad.mainP;
                Plugin.CheckLocationsInScript(805);
                gd.baseData.gainExp += 50U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                gd.forwardNext = true;
            };
            __instance.actions[8] = action;
        }


        [HarmonyPatch(typeof(Comet3Scenario), "SetParam")]
        [HarmonyPostfix]
        static void CitizenSlime(Comet3Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[14];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.LoadPlace(9, true);
                sp.WaitLoad();
                sp.LoadTexture("ivep08_01_09");
                sp.WaitSec(3.5f, true);
                sp.WaitLoad();
                sp.StopBgmInScript(-1, false);
                sp.StopAllSeInScript();
                sp.UnloadSound("se_ashioto_multi_01");
                sp.UnloadSound("se_jelly");
                sp.PlaySeInScript("se_gatyan", 0.6f);
                sp.WaitSec(0.1f, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 40U, false, false, -1);
                    sp.m_sb[40U].SetTexture(0, sp.m_sb[40U].gameObject.transform, 0U, "ivep08_01_09", null, null);
                    sp.m_sb[40U].m_spriteMap[0U].SetVisible(true);
                    sp.SetColorScreen(uint.MaxValue, 45U, -1);
                    sp.m_sb[40U].SetFade(0.5f, 1f, 0, 0f, -1, false);
                    sp.m_sb[45U].SetFade(0.8f, 1f, 0, 0f, -1, false);
                    return true;
                }, (float e) => true, true));
                sp.PlaySeInScript("se_pusyu", 1f);
                sp.WaitFade(new List<uint> { 40U }, false, true);
                sp.LoadTexture("ivep08_01_07");
                List<string> list14 = Util.Split(sp.m_rs.GetScenarioCommetText(2, 98, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 0, list14, false, true, true, false);
                sp.WaitFade(new List<uint> { 45U }, false, true);
                sp.RemoveScreenInScript(40U);
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list14 = Util.Split(sp.m_rs.GetScenarioCommetText(2, 99, 3), new char[] { '|' });
                sp.SetNormalSerifu(-3, -1, 1, list14, true, true, true, false);
                int targetP = ad.targetP;
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep08_01_07", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    GameData.character character6 = gd.chara[targetP];
                    character6.doa = Setting.Doa.doa_Seizon;
                    gd.chara[targetP] = character6;
                    gd.RemakePeopleFlg();
                    return true;
                }, (float e) => true, true));
                sp.SetFadeScreen(new List<uint> { 45U }, 46U, 1.4f, 0, false, true, true);
                sp.PlayBgmInScript("bgm09", 2f, 0.8f, -1, true);
                sp.WaitFade(new List<uint> { 46U }, true, true);
                sp.LoadTexture("ivep08_01_08_1");
                sp.WaitLoad();
                sp.LoadTexture("ivep08_01_08_2");
                sp.WaitLoad();
                sp.LoadTexture("ivep08_01_08_3");
                sp.WaitLoad();
                sp.LoadTexture("ivep08_01_08_4");
                sp.WaitLoad();
                sp.LoadTexture("ivep08_01_08");
                sp.WaitSec(0.8f, true);
                sp.WaitLoad();
                list14 = Util.Split(sp.m_rs.GetScenarioCommetText(2, 100, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 0, list14, true, true, true, false);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.4f, 0, false, true, true);
                sp.UnloadTexture("ivep08_01_07");
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep08_01_08", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, true));
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list14 = Util.Split(sp.m_rs.GetScenarioCommetText(2, 101, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 1, list14, true, true, true, false);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.4f, 2, false, true, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_none, 25U, false, false, -1);
                    sp.m_sb[25U].SetTexture(0, sp.m_sb[25U].gameObject.transform, 0U, "ivep08_01_09", null, null);
                    sp.m_sb[25U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, false));
                sp.ShowChara(ad.mainP, 4, 0, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(0f, 80f, 480f, 270f), 0f, 1f, true, null, true);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list14 = Util.Split(sp.m_rs.GetScenarioCommetText(2, 102, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 0, list14, true, true, true, false);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 25U }, 30U, 0.4f, 4, false, true, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep08_01_08", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 10U, "ivep08_01_08_1", new Vector2?(new Vector2(408.75f, 102f)), null);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 20U, "ivep08_01_08_2", new Vector2?(new Vector2(408.75f, 102f)), null);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 30U, "ivep08_01_08_3", new Vector2?(new Vector2(408.75f, 102f)), null);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 40U, "ivep08_01_08_4", new Vector2?(new Vector2(408.75f, 102f)), null);
                    sp.m_sb[0U].m_spriteMap[30U].SetVisible(true);
                    return true;
                }, (float e) => true, true));
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list14 = Util.Split(sp.m_rs.GetScenarioCommetText(2, 103, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 1, list14, true, true, true, false);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.4f, 0, false, false, true);
                sp.SetVisible(0U, 30, false);
                sp.SetVisible(0U, 10, true);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list14 = Util.Split(sp.m_rs.GetScenarioCommetText(2, 104, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 1, list14, true, true, true, false);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.4f, 2, false, true, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_none, 25U, false, false, -1);
                    sp.m_sb[25U].SetTexture(0, sp.m_sb[25U].gameObject.transform, 0U, "ivep08_01_09", null, null);
                    sp.m_sb[25U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, false));
                sp.ShowChara(ad.mainP, 4, 0, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(0f, 80f, 480f, 270f), 0f, 1f, true, null, true);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list14 = Util.Split(sp.m_rs.GetScenarioCommetText(2, 105, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 0, list14, true, true, true, false);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 25U }, 30U, 0.4f, 4, false, true, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep08_01_08", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 10U, "ivep08_01_08_1", new Vector2?(new Vector2(408.75f, 102f)), null);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 20U, "ivep08_01_08_2", new Vector2?(new Vector2(408.75f, 102f)), null);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 30U, "ivep08_01_08_3", new Vector2?(new Vector2(408.75f, 102f)), null);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 40U, "ivep08_01_08_4", new Vector2?(new Vector2(408.75f, 102f)), null);
                    sp.m_sb[0U].m_spriteMap[20U].SetVisible(true);
                    return true;
                }, (float e) => true, true));
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list14 = Util.Split(sp.m_rs.GetScenarioCommetText(2, 106, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 1, list14, true, true, true, false);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.4f, 0, false, false, true);
                sp.SetVisible(0U, 30, false);
                sp.SetVisible(0U, 40, true);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list14 = Util.Split(sp.m_rs.GetScenarioCommetText(2, 107, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 1, list14, true, true, true, false);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.4f, 2, false, true, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_none, 25U, false, false, -1);
                    sp.m_sb[25U].SetTexture(0, sp.m_sb[25U].gameObject.transform, 0U, "ivep08_01_09", null, null);
                    sp.m_sb[25U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, false));
                sp.ShowChara(ad.mainP, 0, 0, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(0f, 80f, 480f, 270f), 0f, 1f, true, null, true);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list14 = Util.Split(sp.m_rs.GetScenarioCommetText(2, 108, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 0, list14, true, true, true, false);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 25U }, 30U, 0.4f, 4, false, true, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep08_01_08", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 10U, "ivep08_01_08_1", new Vector2?(new Vector2(408.75f, 102f)), null);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 20U, "ivep08_01_08_2", new Vector2?(new Vector2(408.75f, 102f)), null);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 30U, "ivep08_01_08_3", new Vector2?(new Vector2(408.75f, 102f)), null);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 40U, "ivep08_01_08_4", new Vector2?(new Vector2(408.75f, 102f)), null);
                    sp.m_sb[0U].m_spriteMap[20U].SetVisible(true);
                    return true;
                }, (float e) => true, true));
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list14 = Util.Split(sp.m_rs.GetScenarioCommetText(2, 109, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 1, list14, true, true, true, false);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.4f, 0, false, false, true);
                sp.SetVisible(0U, 20, false);
                sp.SetVisible(0U, 10, true);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list14 = Util.Split(sp.m_rs.GetScenarioCommetText(2, 110, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 1, list14, true, true, true, false);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.4f, 0, false, false, true);
                sp.SetVisible(0U, 10, false);
                sp.SetVisible(0U, 30, true);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list14 = Util.Split(sp.m_rs.GetScenarioCommetText(2, 111, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 1, list14, true, true, true, false);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.4f, 2, false, true, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_none, 25U, false, false, -1);
                    sp.m_sb[25U].SetTexture(0, sp.m_sb[25U].gameObject.transform, 0U, "ivep08_01_09", null, null);
                    sp.m_sb[25U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, false));
                sp.ShowChara(ad.mainP, 2, 0, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(0f, 80f, 480f, 270f), 0f, 1f, true, null, true);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list14 = Util.Split(sp.m_rs.GetScenarioCommetText(2, 112, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 0, list14, true, true, true, false);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 25U }, 30U, 0.4f, 4, false, true, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep08_01_08", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 10U, "ivep08_01_08_1", new Vector2?(new Vector2(408.75f, 102f)), null);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 20U, "ivep08_01_08_2", new Vector2?(new Vector2(408.75f, 102f)), null);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 30U, "ivep08_01_08_3", new Vector2?(new Vector2(408.75f, 102f)), null);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 40U, "ivep08_01_08_4", new Vector2?(new Vector2(408.75f, 102f)), null);
                    return true;
                }, (float e) => true, true));
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list14 = Util.Split(sp.m_rs.GetScenarioCommetText(2, 113, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 1, list14, false, true, true, false);
                sp.LoadTexture("ivep08_01_10");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.FadeBgmInScript(-1f, 0f, 0.4f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.4f, 2, false, true, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_none, 20U, false, false, -1);
                    sp.m_sb[20U].SetTexture(0, sp.m_sb[20U].gameObject.transform, 10000U, "ivep08_01_10", null, null);
                    sp.m_sb[20U].m_spriteMap[10000U].SetVisible(true);
                    sp.m_sb[20U].m_spriteMap[10000U].SetSize(0.7f);
                    sp.m_sb[20U].m_spriteMap[10000U].SetDisplayOffsetY((float)sp.m_rs.m_displaySize.height - sp.m_sb[20U].m_spriteMap[10000U].GetSizeInDisplay().y * sp.m_sb[20U].m_spriteMap[10000U].GetSize() * GraphicsContext.m_textureRatio);
                    sp.m_sb[20U].m_spriteMap[10000U].SetCenterPosition(new Vector2((float)(sp.m_rs.m_displaySize.width / 4) + sp.m_sb[20U].m_spriteMap[10000U].m_faceCenter * sp.m_sb[20U].m_spriteMap[10000U].GetSize(), sp.m_sb[20U].m_spriteMap[10000U].GetCenterPosition().y));
                    sp.SetScreen(Setting.Screen.s_none, 25U, false, false, -1);
                    sp.m_sb[25U].SetTexture(0, sp.m_sb[25U].gameObject.transform, 0U, "ivep08_01_09", null, null);
                    sp.m_sb[25U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, false));
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(0f, 80f, 480f, 270f), 0f, 1f, true, null, true);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.UnloadTexture("ivep08_01_08_1");
                sp.UnloadTexture("ivep08_01_08_2");
                sp.UnloadTexture("ivep08_01_08_3");
                sp.UnloadTexture("ivep08_01_08_4");
                sp.UnloadTexture("ivep08_01_08");
                list14 = Util.Split(sp.m_rs.GetScenarioCommetText(2, 114, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 0, list14, false, true, true, false);
                sp.PlayBgmInScript("bgm15", 0f, 1f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list14 = Util.Split(sp.m_rs.GetScenarioCommetText(2, 115, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 0, list14, true, true, true, false);
                sp.WaitSec(0.05f, false);
                sp.PlaySeInScript("se_gatyan", 0.6f);
                sp.WaitSec(0.1f, true);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 25U, 50U }, 30U, 0.25f, 0, false, true, true);
                sp.PlaySeInScript("se_pusyu", 1f);
                sp.UnloadTexture("ivep08_01_09");
                sp.UnloadTexture("ivep08_01_10");
                sp.UnloadPlace();
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(2f, true);
                int mainP = ad.mainP;
                int counterP = ad.counterP;
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    GameData.character character7 = gd.chara[mainP];
                    character7.doa = Setting.Doa.doa_Shokei;
                    gd.chara[mainP] = character7;
                    for (int k = 1; k < (int)gd.baseData.totalNum; k++)
                    {
                        if (gd.chara[k].doa == Setting.Doa.doa_Fumei && k != gd.personFromId[4])
                        {
                            character7 = gd.chara[k];
                            character7.doa = Setting.Doa.doa_Kamare;
                            gd.chara[k] = character7;
                        }
                    }
                    gd.RemakePeopleFlg();
                    return true;
                }, (float e) => true, false));
                Plugin.CheckLocationsInScript(806);
                gd.baseData.gainExp += 250U;
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(0.25f, true);
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioCommetText(2, 117, -1), 2, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.FadeBgmInScript(-1f, 0f, 3f, true, -1);
                gd.forwardNext = true;
            };
            __instance.actions[14] = action;
        }


        [HarmonyPatch(typeof(Comet4Scenario), "SetParam")]
        [HarmonyPostfix]
        static void SayYoureHuman(Comet4Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[5];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.FadeBgmInScript(-1f, 0.8f, 1.2f, false, -1);
                List<string> list5 = Util.Split(sp.m_rs.GetScenarioCommetText(3, 24, 0), new char[] { '|' });
                string text2 = list5[0];
                list5[0] = text2;
                sp.SetNormalSerifu(ad.mainP, -1, 0, list5, true, true, false, true);
                list5 = Util.Split(sp.m_rs.GetScenarioCommetText(3, 25, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 0, list5, true, true, false, true);
                list5 = Util.Split(sp.m_rs.GetScenarioCommetText(3, 26, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 2, list5, true, false, false, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 1.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadTexture("p08a");
                sp.UnloadPlace();
                sp.WaitSec(0.5f, true);
                Plugin.CheckLocationsInScript(5);
                gd.baseData.gainExp += 50U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioCommetText(3, 28, -1), 3, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                gd.forwardNext = true;
            };
            __instance.actions[5] = action;
        }


        [HarmonyPatch(typeof(Comet5Scenario), "SetParam")]
        [HarmonyPostfix]
        static void AdventureInAFrozenWorld(Comet5Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[3];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sd.flg |= 4096;
                gd.baseData.sce_ind_flg[sd.id] = (ushort)((ulong)gd.baseData.sce_ind_flg[sd.id] | 256UL);
                sp.LoadTexture("personalRoom2");
                sp.WaitLoad();
                sp.LoadTexture("ive006_0");
                sp.WaitLoad();
                sp.LoadTexture("ive006_0_1");
                sp.WaitLoad();
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.SetScreen(Setting.Screen.s_none, 20U, false, false, -1);
                    sp.SetScreen(Setting.Screen.s_Interface, 50U, true, false, -1);
                    sp.SetColorScreen(255U, 50000U, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ive006_0", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 10U, "ive006_0_1", null, null);
                    sp.m_sb[0U].m_spriteMap[10U].SetVisible(true);
                    sp.m_sb[20U].SetTexture(0, sp.m_sb[20U].gameObject.transform, 0U, "personalRoom2", null, null);
                    sp.m_sb[20U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, false));
                sp.SetFadeScreen(new List<uint> { 50000U }, 50001U, 0.4f, 0, true, true, true);
                sp.WaitSec(1f, true);
                sp.PlaySeInScript("se_timestop_a", 1f);
                sp.SetFadeScreen(new List<uint> { 20U }, 30U, 0.5f, 6, false, true, true);
                sp.UnloadTexture("personalRoom2");
                sp.LoadTexture("p08b");
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(0.6f, true);
                sp.WaitLoad();
                List<string> list2 = Util.Split(sp.m_rs.GetScenarioCommetText(4, 7, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list2, false, true, true, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 20U, false, false, -1);
                    sp.ChangeCharaTexture(8U, "p08b", 10U, 20U, true);
                    return true;
                }, (float e) => true, false));
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.8f, 0, false, false, true);
                sp.ShowChara(ad.mainP, 0, 1, 20U, false);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.PlayBgmInScript("bgm21", 1f, 1f, -1, true);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.4f, 4, false, true, true);
                sp.UnloadTexture("ive006_0_1");
                sp.LoadTexture("ivep08_02_0");
                sp.WaitLoad();
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.SetScreen(Setting.Screen.s_none, 20U, false, false, -1);
                    sp.SetColorScreen(255U, 40U, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ive006_0", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 10U, "ivep08_02_0", null, null);
                    sp.m_sb[0U].m_spriteMap[10U].SetVisible(true);
                    sp.ChangeCharaTexture(8U, "p08b", 10U, 20U, true);
                    return true;
                }, (float e) => true, true));
                sp.ShowChara(ad.mainP, 0, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(120f, 40f, 720f, 405f), 0f, 1f, true, null, true);
                sp.WaitSec(1.4f, true);
                list2 = Util.Split(sp.m_rs.GetScenarioCommetText(4, 8, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list2, false, true, true, true);
                sp.WaitSec(0.6f, true);
                sp.SetFadeScreen(new List<uint> { 40U }, 41U, 0.8f, 0, true, true, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list2 = Util.Split(sp.m_rs.GetScenarioCommetText(4, 9, 7), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list2, true, true, false, true);
                list2 = Util.Split(sp.m_rs.GetScenarioCommetText(4, 10, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list2, true, true, false, true);
                list2 = Util.Split(sp.m_rs.GetScenarioCommetText(4, 11, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list2, true, true, false, true);
                list2 = Util.Split(sp.m_rs.GetScenarioCommetText(4, 12, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list2, true, true, false, true);
                list2 = Util.Split(sp.m_rs.GetScenarioCommetText(4, 13, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list2, true, true, false, true);
                sp.WaitSec(0.05f, false);
                int mainP = ad.mainP;
                Plugin.CheckLocationsInScript(807);
                gd.baseData.gainExp += 50U;
                list2 = Util.Split(sp.m_rs.GetScenarioCommetText(4, 15, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list2, true, true, false, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 3f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 30U, 0.6f, 0, false, true, true);
                sp.UnloadTexture("ive006_0");
                sp.UnloadTexture("ivep08_02_0");
                sp.UnloadTexture("p08b");
                sp.WaitFade(new List<uint> { 30U }, true, true);
                gd.forwardNext = true;
            };
            __instance.actions[3] = action;
        }


        [HarmonyPatch(typeof(Gina1Scenario), "SetParam")]
        [HarmonyPostfix]
        static void Gina3(Gina1Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[15];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                List<string> list14;
                if ((sd.flg & 4096) > 0)
                {
                    list14 = Util.Split(sp.m_rs.GetScenarioGinaText(0, 100, 0), new char[] { '|' });
                    string text3 = list14[0];
                    Util.Replace(ref text3, "{0}", gd.takashiName);
                    list14[0] = text3;
                    sp.SetNormalSerifu(ad.mainP, 0, 1, list14, true, true, false, true);
                }
                sp.SetNormalClipAnim(-1);
                sp.WaitClipAnim(new List<uint> { 0U, 20U }, true);
                list14 = Util.Split(sp.m_rs.GetScenarioGinaText(0, 101, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list14, true, true, false, true);
                sp.WaitSec(0.8f, true);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 30U, 0.1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.StopBgmInScript(-1, false);
                sp.PlaySeInScript("se_gatyan", 1f);
                sp.WaitSec(0.1f, true);
                sp.PlaySeInScript("se_pusyu", 1f);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(3f, true);
                int mainP = ad.mainP;
                Plugin.CheckLocationsInScript(103);
                gd.baseData.gainExp += 50U;
                gd.forwardNext = true;
            };
            __instance.actions[15] = action;
        }


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
                //Base
                List<string> list4 = Util.Split(sp.m_rs.GetScenarioGinaText(1, 11, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list4, true, true, false, true);
                list4 = Util.Split(sp.m_rs.GetScenarioGinaText(1, 12, 5), new char[] { '|' });
                string text4 = list4[0];
                Util.Replace(ref text4, "{1}", GetCharaName(chara, gd, ad.targetP));
                list4[0] = text4;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list4, true, true, false, true);
                list4 = Util.Split(sp.m_rs.GetScenarioGinaText(1, 13, 0), new char[] { '|' });
                text4 = list4[0];
                Util.Replace(ref text4, "{1}", GetCharaName(chara, gd, ad.targetP));
                list4[0] = text4;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list4, true, true, false, true);
                if (!ArchipelagoClient.ServerData.CheckedLocations.Contains(18)) //Changed condition
                {
                    sp.WaitSec(0.1f, false);
                    Plugin.CheckLocationsInScript(18);
                    gd.baseData.gainExp += 50U;
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
                if (!ArchipelagoClient.ServerData.CheckedLocations.Contains(104)) //Changed condition
                {
                    sp.WaitSec(0.75f, true);
                    int mainP = ad.mainP;
                    Plugin.CheckLocationsInScript(104);
                    gd.baseData.gainExp += 50U;
                }
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(0.6f, true);
                gd.forwardNext = true;
            };
            __instance.actions[4] = action;
        }


        [HarmonyPatch(typeof(Gina3Scenario), "SetParam")]
        [HarmonyPostfix]
        static void Allacosia(Gina3Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[1];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                ScenarioContents.ChangeSceOnFlg(ref gd, ref sd, 2U);
                gd.baseData.sce_flg = gd.baseData.sce_flg | 16384UL;
                GameData.character character2 = gd.chara[ad.mainP];
                character2.scenarioFlg |= 1;
                gd.chara[ad.mainP] = character2;
                sd.flg |= 16384;
                sp.LoadTexture("ivep01_01_0");
                sp.WaitLoad();
                sp.LoadTexture("p01a");
                sp.WaitSec(0.35f, true);
                sp.WaitLoad();
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.ChangeCharaTexture(1U, "p01a", 10U, 20U, true);
                    sp.SetScreen(Setting.Screen.s_Interface, 50U, true, false, -1);
                    return true;
                }, (float e) => true, true));
                sp.ShowChara(ad.mainP, 0, 0, 20U, false);
                sp.ShowChara(ad.targetP, 0, 2, 20U, false);
                sp.SetFadeScreen(new List<uint> { 50001U, 50002U }, 50003U, 0.3f, 0, true, true, true);
                sp.WaitSec(0.25f, true);
                List<string> list = Util.Split(sp.m_rs.GetScenarioGinaText(2, 2, 0), new char[] { '|' });
                string text = list[0];
                list[0] = text;
                sp.SetNormalSerifu(0, -1, 1, list, false, true, true, true);
                sp.WaitSec(0.4f, true);
                sp.PlayBgmInScript("bgm03", 0f, 0.8f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list = Util.Split(sp.m_rs.GetScenarioGinaText(2, 3, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 0, list, true, false, true, true);
                list = Util.Split(sp.m_rs.GetScenarioGinaText(2, 4, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 2, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioGinaText(2, 5, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 2, list, true, true, false, true);
                list = Util.Split(sp.m_rs.GetScenarioGinaText(2, 6, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 0, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioGinaText(2, 7, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 2, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioGinaText(2, 8, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 0, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioGinaText(2, 9, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 2, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioGinaText(2, 10, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 0, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioGinaText(2, 11, 6), new char[] { '|' });
                text = list[0];
                Util.Replace(ref text, "{0}", gd.takashiName);
                list[0] = text;
                sp.SetNormalSerifu(ad.targetP, -1, 2, list, true, false, false, true);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.25f, 0, false, false, true);
                sp.UnvisibleAllChara(20U, -1);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetCharaSingleTexture(10000, "ivep01_01_0", 0U, 0f, 20U);
                    return true;
                }, (float e) => true, false));
                sp.SetNormalClipAnim(0);
                sp.WaitClipAnim(new List<uint> { 0U, 20U }, true);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list = Util.Split(sp.m_rs.GetScenarioGinaText(2, 12, 1), new char[] { '|' });
                text = list[0];
                Util.Replace(ref text, "{0}", gd.takashiName);
                list[0] = text;
                sp.SetNormalSerifu(ad.mainP, 0, 0, list, true, true, true, true);
                sp.ShowChara(ad.targetP, 6, 2, 20U, false);
                list = Util.Split(sp.m_rs.GetScenarioGinaText(2, 13, 0), new char[] { '|' });
                text = list[0];
                list[0] = text;
                sp.SetNormalSerifu(0, -1, 1, list, true, false, true, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 2f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadTexture("ivep01_01_0");
                sp.UnloadTexture("p01a");
                sp.UnloadPlace();
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                int mainP = ad.mainP;
                Plugin.CheckLocationsInScript(105);
                gd.baseData.gainExp += 50U;
                sp.WaitSec(0.4f, true);
                gd.forwardNext = true;
            };
            __instance.actions[1] = action;
        }


        [HarmonyPatch(typeof(Gina6Scenario), "SetParam")]
        [HarmonyPostfix]
        static void Gina6(Gina6Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[1];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.WaitSec(0.5f, true);
                sp.PlaySeInScript("se_jidoudoa", 0.7f);
                List<string> list = Util.Split(sp.m_rs.GetScenarioGinaText(5, 2, 1), new char[] { '|' });
                string text = list[0];
                list[0] = text;
                sp.SetNormalSerifu(ad.mainP, 0, 0, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioGinaText(5, 3, 2), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[3], ad.mainP, 1, list, true, false, false, true);
                sp.FadeBgmInScript(0f, 1f, 2f, false, -1);
                sp.PlaySeInScript("se_jin_01", 1f);
                list = Util.Split(sp.m_rs.GetScenarioGinaText(5, 4, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 0, list, false, false, false, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list = Util.Split(sp.m_rs.GetScenarioGinaText(5, 5, 4), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[11], ad.mainP, 1, list, true, false, false, true);
                sp.FadeBgmInScript(0f, 1f, 2f, false, -1);
                sp.PlaySeInScript("se_jin_11", 1f);
                list = Util.Split(sp.m_rs.GetScenarioGinaText(5, 6, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 0, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioGinaText(5, 7, 6), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[5], ad.mainP, 2, list, true, false, false, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.ChangeCharaTexture(1U, "p01", 10U, 20U, true);
                    return true;
                }, (float e) => true, true));
                list = Util.Split(sp.m_rs.GetScenarioGinaText(5, 8, 4), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[13], gd.personFromId[5], 1, list, true, false, false, true);
                sp.UnloadTexture("p01a");
                list = Util.Split(sp.m_rs.GetScenarioGinaText(5, 9, 6), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[4], -1, 2, list, true, false, false, true);
                sp.SetNormalClipAnim(-1);
                sp.WaitSec(0.2f, true);
                if (!ArchipelagoClient.ServerData.CheckedLocations.Contains(106)) //Changed condition
                {
                    int mainP = ad.mainP;
                    sp.StopAllSeInScript();
                    Plugin.CheckLocationsInScript(106);
                    gd.baseData.gainExp += 50U;
                }
                gd.forwardNext = true;
            };
            __instance.actions[1] = action;
        }


        [HarmonyPatch(typeof(Jonas1Scenario), "SetParam")]
        [HarmonyPostfix]
        static void Jonas3(Jonas1Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[5];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                List<string> list5;
                if ((gd.actionFlg & 16UL) == 0UL)
                {
                    list5 = Util.Split(sp.m_rs.GetScenarioJonasText(0, 16, 4), new char[] { '|' });
                    sp.SetNormalSerifu(ad.counterP, ad.mainP, 1, list5, true, false, false, true);
                }
                list5 = Util.Split(sp.m_rs.GetScenarioJonasText(0, 17, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 2, list5, true, false, false, true);
                list5 = Util.Split(sp.m_rs.GetScenarioJonasText(0, 18, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 2, list5, true, true, false, true);
                list5 = Util.Split(sp.m_rs.GetScenarioJonasText(0, 19, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 0, list5, true, false, false, true);
                list5 = Util.Split(sp.m_rs.GetScenarioJonasText(0, 20, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 2, list5, true, false, false, true);
                list5 = Util.Split(sp.m_rs.GetScenarioJonasText(0, 21, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, ad.mainP, 1, list5, true, false, false, true);
                list5 = Util.Split(sp.m_rs.GetScenarioJonasText(0, 22, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.counterP, 2, list5, true, false, false, true);
                sp.WaitSec(0.25f, true);
                list5 = Util.Split(sp.m_rs.GetScenarioJonasText(0, 23, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 2, list5, true, true, false, true);
                sp.FadeBgmInScript(-1f, 0f, 1.5f, true, -1);
                sp.PlaySeInScript("se_ashioto_03", 0.8f);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.4f, 0, false, true, true);
                sp.UnloadTexture("p01a");
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(1f, true);
                sp.StopAllSeInScript();
                sp.PlaySeInScript("se_jidoudoa", 0.6f);
                sp.WaitSec(0.8f, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, false));
                sp.ShowChara(ad.targetP, 5, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(180f, 60f, 600f, 337.5f), 0f, 1f, true, null, true);
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.6f, 0, true, true, true);
                sp.WaitSec(0.4f, true);
                list5 = Util.Split(sp.m_rs.GetScenarioJonasText(0, 24, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 0, list5, false, true, true, true);
                sp.WaitSec(0.4f, true);
                sp.PlayBgmInScript("bgm03", 0.5f, 0.8f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list5 = Util.Split(sp.m_rs.GetScenarioJonasText(0, 25, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, ad.targetP, 2, list5, true, false, false, true);
                list5 = Util.Split(sp.m_rs.GetScenarioJonasText(0, 26, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.counterP, 0, list5, false, false, false, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.m_sb[50U].m_textAreaMap["test"].nowLine >= 1, true));
                sp.FadeBgmInScript(0f, 0.8f, 1f, false, -1);
                sp.PlaySeInScript("se_jin_04", 1f);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list5 = Util.Split(sp.m_rs.GetScenarioJonasText(0, 27, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, ad.targetP, 2, list5, true, false, false, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 3.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.WaitSec(0.7f, true);
                int mainP = ad.mainP;
                Plugin.CheckLocationsInScript(1003);
                gd.baseData.gainExp += 50U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(0.4f, true);
                gd.forwardNext = true;
            };
            __instance.actions[5] = action;
        }


        [HarmonyPatch(typeof(Jonas2Scenario), "SetParam")]
        [HarmonyPostfix]
        static void JonasTheWreck(Jonas2Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[5];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                GameData.character character2 = gd.chara[ad.mainP];
                character2.scenarioFlg |= 1;
                gd.chara[ad.mainP] = character2;
                character2 = gd.chara[ad.targetP];
                character2.scenarioFlg |= 1;
                gd.chara[ad.targetP] = character2;
                if ((gd.baseData.sce_flg & 16384UL) == 0UL)
                {
                    ScenarioContents.ChangeSceOnFlg(ref gd, ref sd, 2U);
                    gd.baseData.sce_flg |= 16384UL;
                }
                sp.LoadTexture("p04a");
                sp.WaitSec(0.45f, true);
                sp.WaitLoad();
                sp.PlaySeInScript("se_jidoudoa", 1f);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.ChangeCharaTexture(4U, "p04a", 10U, 20U, true);
                    sp.SetScreen(Setting.Screen.s_Interface, 50U, true, false, -1);
                    return true;
                }, (float e) => true, true));
                sp.ShowChara(ad.mainP, 7, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(180f, 60f, 600f, 337.5f), 0f, 1f, true, null, true);
                sp.SetFadeScreen(new List<uint> { 50001U, 50002U }, 50003U, 0.3f, 0, true, true, true);
                sp.WaitSec(0.4f, true);
                List<string> list4 = Util.Split(sp.m_rs.GetScenarioJonasText(1, 18, 7), new char[] { '|' });
                string text2 = list4[0];
                Util.Replace(ref text2, "{0}", gd.takashiName);
                list4[0] = text2;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list4, false, true, true, true);
                sp.WaitSec(0.4f, true);
                sp.PlayBgmInScript("bgm21", 1.4f, 0.85f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list4 = Util.Split(sp.m_rs.GetScenarioJonasText(1, 19, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list4, true, true, false, true);
                list4 = Util.Split(sp.m_rs.GetScenarioJonasText(1, 20, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list4, true, true, false, true);
                list4 = Util.Split(sp.m_rs.GetScenarioJonasText(1, 21, 7), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list4, true, true, false, true);
                list4 = Util.Split(sp.m_rs.GetScenarioJonasText(1, 22, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list4, true, true, false, true);
                list4 = Util.Split(sp.m_rs.GetScenarioJonasText(1, 23, 3), new char[] { '|' });
                text2 = list4[0];
                Util.Replace(ref text2, "{0}", gd.takashiName);
                list4[0] = text2;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list4, true, true, false, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 3.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.UnloadTexture("p04a");
                sp.WaitSec(0.7f, true);
                int mainP = ad.mainP;
                Plugin.CheckLocationsInScript(404);
                gd.baseData.gainExp += 50U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(0.4f, true);
                gd.forwardNext = true;
            };
            __instance.actions[5] = action;
            action = __instance.actions[14];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                List<string> list12;
                if ((gd.actionFlg & 128UL) == 0UL)
                {
                    list12 = Util.Split(sp.m_rs.GetScenarioJonasText(1, 50, 1), new char[] { '|' });
                    sp.SetNormalSerifu(ad.mainP, 0, 2, list12, true, true, false, true);
                }
                list12 = Util.Split(sp.m_rs.GetScenarioJonasText(1, 51, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 2, list12, true, true, false, true);
                list12 = Util.Split(sp.m_rs.GetScenarioJonasText(1, 52, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 2, list12, true, true, false, true);
                list12 = Util.Split(sp.m_rs.GetScenarioJonasText(1, 53, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 2, list12, true, true, false, true);
                sp.FadeBgmInScript(0f, 0.55f, 1f, false, -1);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_LeviMes, 35U, true, false, -1);
                    return true;
                }, (float e) => true, true));
                sp.WaitSec(0.4f, false);
                list12 = Util.Split(sp.m_rs.GetScenarioJonasText(1, 54, -1), new char[] { '|' });
                sp.SetNormalSerifu(-2, -1, -1, list12, true, false, true, true);
                list12 = Util.Split(sp.m_rs.GetScenarioJonasText(1, 55, 5), new char[] { '|' });
                string text7 = list12[0];
                Util.Replace(ref text7, "{0}", gd.takashiName);
                list12[0] = text7;
                sp.SetNormalSerifu(ad.mainP, 0, 2, list12, false, false, false, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.m_sb[50U].m_textAreaMap["test"].nowLine >= 1, true));
                sp.RemoveScreenInScript(35U);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.4f, 0, false, false, true);
                sp.UnvisibleAllChara(20U, -1);
                sp.ShowChara(ad.mainP, 0, 2, 20U, false);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.FadeBgmInScript(-1f, 0f, 2f, true, -1);
                sp.WaitSec(0.05f, false);
                sp.PlaySeInScript("se_ashioto_03", 0.85f);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.4f, 4, false, true, true);
                sp.UnloadPlace();
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.LoadPlace(32, false);
                sp.WaitSec(1f, true);
                sp.WaitLoad();
                sp.StopAllSeInScript();
                sp.PlaySeInScript("se_jidoudoa", 1f);
                sp.WaitSec(0.8f, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, false));
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.4f, 0, true, true, true);
                sp.WaitSec(0.6f, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_LeviMes, 35U, true, false, -1);
                    return true;
                }, (float e) => true, true));
                sp.WaitSec(0.4f, true);
                list12 = Util.Split(sp.m_rs.GetScenarioJonasText(1, 56, -1), new char[] { '|' });
                text7 = list12[0];
                Util.Replace(ref text7, "{0}", gd.takashiName);
                list12[0] = text7;
                sp.SetNormalSerifu(-2, 0, -1, list12, true, true, true, true);
                sp.PlayBgmInScript("bgm21", 2f, 0.6f, -1, true);
                list12 = Util.Split(sp.m_rs.GetScenarioJonasText(1, 57, -1), new char[] { '|' });
                sp.SetNormalSerifu(-2, 0, -1, list12, true, true, true, true);
                list12 = Util.Split(sp.m_rs.GetScenarioJonasText(1, 58, -1), new char[] { '|' });
                sp.SetNormalSerifu(-2, 0, -1, list12, true, true, true, true);
                list12 = Util.Split(sp.m_rs.GetScenarioJonasText(1, 59, -1), new char[] { '|' });
                sp.SetNormalSerifu(-2, 0, -1, list12, true, true, true, true);
                sp.WaitSec(0.4f, true);
                list12 = Util.Split(sp.m_rs.GetScenarioJonasText(1, 60, -1), new char[] { '|' });
                sp.SetNormalSerifu(-2, 0, -1, list12, true, true, true, true);
                sp.WaitSec(0.05f, false);
                sp.RemoveScreenInScript(35U);
                sp.FadeBgmInScript(-1f, 0f, 3.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.WaitSec(0.7f, true);
                int mainP = ad.mainP;
                Plugin.CheckLocationsInScript(1002, 1004);
                gd.baseData.gainExp += 50U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(0.4f, true);
                gd.forwardNext = true;
            };
            __instance.actions[14] = action;
        }


        [HarmonyPatch(typeof(Jonas3Scenario), "SetParam")]
        [HarmonyPostfix]
        static void Obfuscate(Jonas3Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[1];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                ScenarioContents.ChangeSceOnFlg(ref gd, ref sd, 2U);
                sd.flg |= 16384;
                sp.WaitSec(0.45f, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Interface, 50U, true, false, -1);
                    sp.m_sb[50001U].SetFade(0.3f, 0f, 0, -1f, -1, false);
                    sp.m_sb[50002U].SetFade(0.3f, 0f, 0, -1f, -1, false);
                    return true;
                }, (float e) => true, false));
                sp.ShowChara(ad.targetP, 1, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(240f, 80f, 480f, 270f), 0f, 1f, true, null, true);
                sp.WaitFade(new List<uint> { 50001U, 50002U }, true, true);
                sp.WaitSec(0.4f, true);
                List<string> list = Util.Split(sp.m_rs.GetScenarioJonasText(2, 2, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 1, list, false, true, true, true);
                sp.PlayBgmInScript("bgm03", 0f, 0.9f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list = Util.Split(sp.m_rs.GetScenarioJonasText(2, 3, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, ad.mainP, 2, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioJonasText(2, 4, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 0, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioJonasText(2, 5, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 0, list, false, false, false, true);
                sp.LoadTexture("ivep07_01_1");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.25f, 0, false, true, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_none, 20U, false, false, -1);
                    sp.SetCharaSingleTexture(10000, "ivep07_01_1", 1U, 0f, 20U);
                    return true;
                }, (float e) => true, false));
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(240f, 80f, 480f, 270f), 0.4f, -2.5f, false, new Vector4?(new Vector4(0f, 80f, 480f, 270f)), true);
                sp.WaitClipAnim(new List<uint> { 0U, 20U }, true);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list = Util.Split(sp.m_rs.GetScenarioJonasText(2, 6, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 1, list, true, true, true, true);
                sp.FadeBgmInScript(-1f, 0.45f, 1.2f, false, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.25f, 0, false, true, true);
                sp.UnloadTexture("ivep07_01_1");
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    return true;
                }, (float e) => true, false));
                sp.ShowChara(ad.mainP, 5, 0, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(0f, 80f, 480f, 270f), 0.4f, -2.5f, false, new Vector4?(new Vector4(240f, 80f, 480f, 270f)), true);
                sp.WaitClipAnim(new List<uint> { 0U, 20U }, true);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list = Util.Split(sp.m_rs.GetScenarioJonasText(2, 7, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 0, list, true, true, true, true);
                list = Util.Split(sp.m_rs.GetScenarioJonasText(2, 8, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, ad.mainP, 2, list, true, false, false, true);
                sp.FadeBgmInScript(-1f, 0f, 1.2f, true, -1);
                list = Util.Split(sp.m_rs.GetScenarioJonasText(2, 9, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 0, list, false, false, false, true);
                sp.WaitText(50U, "test", false);
                sp.PlayBgmInScript("bgm09", 0f, 0.85f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list = Util.Split(sp.m_rs.GetScenarioJonasText(2, 10, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 1, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioJonasText(2, 11, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 0, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioJonasText(2, 12, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, -1, 2, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioJonasText(2, 13, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.counterP, 1, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioJonasText(2, 14, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 0, list, true, false, false, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0.4f, 0.75f, true, -1);
                Plugin.CheckLocationsInScript(14);
                gd.baseData.gainExp += 50U;
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioJonasText(2, 16, -1), 3, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.FadeBgmInScript(-1f, 0f, 1.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(0.7f, true);
                gd.forwardNext = true;
            };
            __instance.actions[1] = action;
        }


        [HarmonyPatch(typeof(Jonas4Scenario), "SetParam")]
        [HarmonyPostfix]
        static void Jonas7Kukrushka6(Jonas4Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[10];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.WaitSec(0.45f, true);
                sp.PlaySeInScript("se_jidoudoa", 1f);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Interface, 50U, true, false, -1);
                    return true;
                }, (float e) => true, true));
                sp.ShowChara(ad.mainP, 0, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(240f, 80f, 480f, 270f), 0f, 1f, false, null, true);
                sp.SetFadeScreen(new List<uint> { 50001U, 50002U }, 50003U, 0.3f, 0, true, true, true);
                sp.WaitSec(0.4f, true);
                List<string> list8 = Util.Split(sp.m_rs.GetScenarioJonasText(3, 82, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list8, false, true, true, true);
                sp.PlayBgmInScript("bgm09", 0.8f, 0.85f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.FadeBgmInScript(-1f, 0.5f, 0.4f, false, -1);
                list8 = Util.Split(sp.m_rs.GetScenarioJonasText(3, 83, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 2, list8, true, false, false, true);
                sp.FadeBgmInScript(-1f, 0.85f, 0.4f, false, -1);
                list8 = Util.Split(sp.m_rs.GetScenarioJonasText(3, 84, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list8, true, false, false, true);
                sp.FadeBgmInScript(-1f, 0.6f, 1.5f, false, -1);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetColorScreen(255U, 40U, -1);
                    sp.m_sb[40U].SetFade(0.6f, 0.33f, 3, 0f, -1, false);
                    return true;
                }, (float e) => true, false));
                list8 = Util.Split(sp.m_rs.GetScenarioJonasText(3, 85, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list8, false, true, false, true);
                sp.WaitFade(new List<uint> { 40U }, false, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.FadeBgmInScript(-1f, 0.3f, 1.5f, false, -1);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.m_sb[40U].SetFade(0.6f, 0.66f, 3, 0.33f, -1, false);
                    return true;
                }, (float e) => true, false));
                list8 = Util.Split(sp.m_rs.GetScenarioJonasText(3, 86, 5), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, 1, list8, false, false, true, true);
                sp.WaitFade(new List<uint> { 40U }, false, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.FadeBgmInScript(-1f, 0f, 1.5f, true, -1);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.m_sb[40U].SetFade(0.6f, 1f, 3, 0.66f, -1, false);
                    return true;
                }, (float e) => true, false));
                list8 = Util.Split(sp.m_rs.GetScenarioJonasText(3, 87, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list8, false, true, false, true);
                sp.WaitFade(new List<uint> { 40U }, false, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.WaitSec(2f, true);
                list8 = Util.Split(sp.m_rs.GetScenarioJonasText(3, 88, 4), new char[] { '|' });
                sp.SetNormalSerifu(-3, 0, 2, list8, true, true, true, false);
                sp.WaitSec(0.2f, true);
                list8 = Util.Split(sp.m_rs.GetScenarioJonasText(3, 89, 4), new char[] { '|' });
                string text6 = list8[0];
                Util.Replace(ref text6, "{0}", gd.takashiName);
                list8[0] = text6;
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(180f, 60f, 600f, 337.5f), 0f, 1f, true, null, true);
                sp.SetNormalSerifu(ad.targetP, 0, 2, list8, true, true, true, false);
                sp.UnvisibleAllChara(20U, -1);
                sp.ShowChara(ad.targetP, 1, 2, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(480f, 80f, 480f, 270f), 0f, 1f, true, null, true);
                sp.SetFadeScreen(new List<uint> { 40U }, 41U, 0.8f, 6, true, true, true);
                sp.PlayBgmInScript("bgm11", 0f, 0.45f, -1, true);
                sp.WaitSec(0.2f, true);
                list8 = Util.Split(sp.m_rs.GetScenarioJonasText(3, 90, 1), new char[] { '|' });
                text6 = list8[0];
                Util.Replace(ref text6, "{0}", gd.takashiName);
                list8[0] = text6;
                sp.SetNormalSerifu(ad.targetP, 0, 2, list8, true, true, true, true);
                sp.FadeBgmInScript(-1f, 0.65f, 0.8f, false, -1);
                list8 = Util.Split(sp.m_rs.GetScenarioJonasText(3, 91, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list8, true, false, false, true);
                list8 = Util.Split(sp.m_rs.GetScenarioJonasText(3, 92, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 2, list8, true, false, false, true);
                list8 = Util.Split(sp.m_rs.GetScenarioJonasText(3, 93, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list8, true, false, false, true);
                list8 = Util.Split(sp.m_rs.GetScenarioJonasText(3, 94, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list8, true, true, false, true);
                list8 = Util.Split(sp.m_rs.GetScenarioJonasText(3, 95, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list8, true, true, false, true);
                list8 = Util.Split(sp.m_rs.GetScenarioJonasText(3, 96, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list8, true, true, false, true);
                list8 = Util.Split(sp.m_rs.GetScenarioJonasText(3, 97, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 2, list8, true, false, false, true);
                list8 = Util.Split(sp.m_rs.GetScenarioJonasText(3, 98, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list8, true, false, false, true);
                list8 = Util.Split(sp.m_rs.GetScenarioJonasText(3, 99, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list8, true, true, false, true);
                list8 = Util.Split(sp.m_rs.GetScenarioJonasText(3, 100, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 2, list8, true, false, false, true);
                sp.FadeBgmInScript(-1f, 1f, 0.4f, false, -1);
                list8 = Util.Split(sp.m_rs.GetScenarioJonasText(3, 101, 7), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list8, true, false, false, true);
                sp.PlayBgmInScript("bgm03", 0.75f, 0.85f, -1, true);
                list8 = Util.Split(sp.m_rs.GetScenarioJonasText(3, 102, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 2, list8, true, false, false, true);
                list8 = Util.Split(sp.m_rs.GetScenarioJonasText(3, 103, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list8, true, false, false, true);
                list8 = Util.Split(sp.m_rs.GetScenarioJonasText(3, 104, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 2, list8, true, false, false, true);
                list8 = Util.Split(sp.m_rs.GetScenarioJonasText(3, 105, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list8, true, false, false, true);
                sp.FadeBgmInScript(-1f, 0.35f, 0.8f, false, -1);
                sp.PlaySeInScript("se_ashioto_02", 1f);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.4f, 4, false, true, true);
                sp.UnloadPlace();
                sp.LoadPlace(31, false);
                sp.WaitLoad();
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.LoadTexture("p11a");
                sp.WaitSec(0.8f, true);
                sp.WaitLoad();
                sp.StopAllSeInScript();
                sp.PlaySeInScript("se_jidoudoa", 1f);
                sp.WaitSec(0.8f, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_none, 20U, false, false, -1);
                    sp.ChangeCharaTexture(11U, "p11a", 10U, 20U, true);
                    sp.SetColorScreen(255U, 40U, -1);
                    return true;
                }, (float e) => true, true));
                sp.ShowChara(ad.targetP, 2, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(120f, 40f, 720f, 405f), 0f, 1f, true, null, true);
                sp.FadeBgmInScript(-1f, 0.85f, 0.8f, false, -1);
                sp.SetFadeScreen(new List<uint> { 40U }, 41U, 0.4f, 0, true, true, true);
                sp.WaitSec(0.4f, true);
                list8 = Util.Split(sp.m_rs.GetScenarioJonasText(3, 106, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list8, false, true, true, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list8 = Util.Split(sp.m_rs.GetScenarioJonasText(3, 107, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list8, true, true, false, true);
                list8 = Util.Split(sp.m_rs.GetScenarioJonasText(3, 108, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list8, true, true, false, true);
                sp.FadeBgmInScript(0f, 0.6f, 1.2f, false, -1);
                sp.WaitSec(0.05f, false);
                int mainP = ad.mainP;
                Plugin.CheckLocationsInScript(1007, 1406);
                gd.baseData.gainExp += 100U;
                sp.WaitSec(0.4f, true);
                list8 = Util.Split(sp.m_rs.GetScenarioJonasText(3, 110, 1), new char[] { '|' });
                text6 = list8[0];
                Util.Replace(ref text6, "{0}", gd.takashiName);
                list8[0] = text6;
                sp.SetNormalSerifu(ad.targetP, 0, 1, list8, true, false, false, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 3.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.UnloadTexture("p11a");
                sp.WaitSec(0.7f, true);
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(0.8f, true);
                gd.forwardNext = true;
            };
            __instance.actions[10] = action;
        }


        [HarmonyPatch(typeof(Kukul1Scenario), "SetParam")]
        [HarmonyPostfix]
        static void Kukrushka2Otome5(Kukul1Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[5];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.ShowChara(ad.mainP, 1, 0, 20U, false);
                sp.SetNormalClipAnim(0);
                sp.WaitClipAnim(new List<uint> { 0U, 20U }, true);
                sp.WaitSec(1f, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 3.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1.4f, 0, false, true, true);
                sp.UnloadPlace();
                sp.WaitSec(1f, true);
                sp.StopAllSeInScript();
                sp.UnloadSound("se_otomeawa_02");
                int mainP = ad.mainP;
                int targetP = ad.targetP;
                Plugin.CheckLocationsInScript(1402, 1205);
                gd.baseData.gainExp += 100U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(0.4f, true);
                gd.forwardNext = true;
            };
            __instance.actions[5] = action;
        }


        [HarmonyPatch(typeof(Kukul2Scenario), "SetParam")]
        [HarmonyPostfix]
        static void TheKukrushkaProblem(Kukul2Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[7];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.StopAllSeInScript();
                sp.UnloadSound("se_ashioto_17");
                sp.FadeBgmInScript(-1f, 0f, 1f, true, -1);
                List<string> list7 = Util.Split(sp.m_rs.GetScenarioKukulText(1, 23, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 2, list7, false, false, false, true);
                sp.WaitText(50U, "test", false);
                sp.PlayBgmInScript("bgm17", 0f, 1f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list7 = Util.Split(sp.m_rs.GetScenarioKukulText(1, 24, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, ad.mainP, 1, list7, true, false, false, true);
                list7 = Util.Split(sp.m_rs.GetScenarioKukulText(1, 25, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 0, list7, true, false, false, true);
                list7 = Util.Split(sp.m_rs.GetScenarioKukulText(1, 26, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, ad.mainP, 1, list7, true, false, false, true);
                list7 = Util.Split(sp.m_rs.GetScenarioKukulText(1, 27, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 0, list7, true, false, false, true);
                list7 = Util.Split(sp.m_rs.GetScenarioKukulText(1, 28, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 2, list7, false, false, false, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.m_sb[50U].m_textAreaMap["test"].nowLine >= 2, true));
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.25f, 0, false, false, true);
                sp.UnvisibleAllChara(20U, -1);
                sp.ShowChara(ad.mainP, 4, 2, 20U, false);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.FadeBgmInScript(-1f, 0.6f, 1f, false, -1);
                list7 = Util.Split(sp.m_rs.GetScenarioKukulText(1, 29, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 0, list7, true, false, false, true);
                list7 = Util.Split(sp.m_rs.GetScenarioKukulText(1, 30, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, ad.mainP, 1, list7, true, false, false, true);
                list7 = Util.Split(sp.m_rs.GetScenarioKukulText(1, 31, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 0, list7, false, false, false, true);
                sp.PlayBgmInScript("bgm09", 0f, 1f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list7 = Util.Split(sp.m_rs.GetScenarioKukulText(1, 32, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, ad.targetP, 1, list7, true, false, false, true);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.25f, 0, false, true, true);
                sp.LoadTexture("ivep14_01_0");
                sp.WaitLoad();
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep14_01_0", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, false));
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.6f, 0, true, true, true);
                list7 = Util.Split(sp.m_rs.GetScenarioKukulText(1, 33, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 0, list7, false, true, true, false);
                sp.LoadTexture("ivep09_01_2");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.PlayBgmInScript("bgm17", 0f, 1f, -1, true);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.25f, 4, false, true, true);
                sp.UnloadTexture("ivep14_01_0");
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep09_01_2", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, false));
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list7 = Util.Split(sp.m_rs.GetScenarioKukulText(1, 34, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 0, list7, true, true, true, false);
                sp.FadeBgmInScript(-1f, 0f, 3f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.5f, 0, false, true, true);
                sp.UnloadTexture("ivep09_01_2");
                sp.WaitSec(0.1f, true);
                sp.PlaySeInScript("se_ashioto_03", 1f);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(1.2f, true);
                sp.StopAllSeInScript();
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, false));
                sp.ShowChara(ad.counterP, 5, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(120f, 40f, 720f, 405f), 0f, 1f, true, null, true);
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.6f, 0, true, true, true);
                list7 = Util.Split(sp.m_rs.GetScenarioKukulText(1, 35, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, -1, 1, list7, true, true, true, true);
                list7 = Util.Split(sp.m_rs.GetScenarioKukulText(1, 36, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, -1, 1, list7, true, true, false, true);
                sp.WaitSec(0.05f, false);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.WaitSec(0.7f, true);
                int targetP = ad.targetP;
                int tuizuiP = (int)ad.tuizuiP;
                Plugin.CheckLocationsInScript(1005, 1403);
                gd.baseData.gainExp += 100U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(0.4f, true);
                gd.forwardNext = true;
            };
            __instance.actions[7] = action;
        }


        [HarmonyPatch(typeof(Kukul3Scenario), "SetParam")]
        [HarmonyPostfix]
        static void KukrushkaTheGuard(Kukul3Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[2];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.LoadPlace(9, true);
                sp.WaitLoad();
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetColorScreen(uint.MaxValue, 50001U, -1);
                    sp.SetScreen(Setting.Screen.s_PlaceName, 50002U, true, false, -1);
                    sp.m_sb[50001U].SetFade(0.3f, 1f, 0, 0f, -1, false);
                    sp.m_sb[50002U].SetFade(0.3f, 1f, 0, 0f, -1, false);
                    return true;
                }, (float e) => true, false));
                sp.WaitFade(new List<uint> { 50001U, 50002U }, false, true);
                sp.LoadTexture("p11a");
                sp.WaitSec(0.6f, false);
                sp.WaitLoad();
                sp.PlaySeInScript("se_jidoudoa", 1f);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.ChangeCharaTexture(11U, "p11a", 10U, 20U, true);
                    sp.SetScreen(Setting.Screen.s_Interface, 50U, true, false, -1);
                    return true;
                }, (float e) => true, true));
                sp.ShowChara(gd.personFromId[11], 7, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(120f, 40f, 720f, 405f), 0f, 1f, true, null, true);
                sp.SetFadeScreen(new List<uint> { 50001U, 50002U }, 50003U, 0.3f, 0, false, true, true);
                sp.WaitFade(new List<uint> { 50003U }, true, true);
                sp.WaitSec(0.4f, true);
                List<string> list2 = Util.Split(sp.m_rs.GetScenarioKukulText(2, 10, -1), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[11], 0, 1, list2, false, true, true, true);
                sp.WaitSec(0.2f, true);
                sp.PlayBgmInScript("bgm18", 0.5f, 0.75f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list2 = Util.Split(sp.m_rs.GetScenarioKukulText(2, 11, 6), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[11], 0, 1, list2, true, true, false, true);
                list2 = Util.Split(sp.m_rs.GetScenarioKukulText(2, 12, 1), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[11], 0, 1, list2, true, true, false, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 2f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 50001U, 0.4f, 0, true, true, true);
                sp.UnloadTexture("p11a");
                sp.WaitFade(new List<uint> { 50001U }, true, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetColorScreen(737023U, 15U, -1);
                    sp.SetColorScreen(255U, 29U, -1);
                    return true;
                }, (float e) => true, false));
                sp.SetFadeScreen(new List<uint> { 29U }, 30U, 0.1f, 0, false, true, true);
                sp.PlaySeInScript("se_gatyan", 1f);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.PlaySeInScript("se_pusyu", 1f);
                sp.SetDialogScreen(200U, sp.m_rs.GetScenarioKukulText(2, 13, -1), 2, false);
                int mainP = ad.mainP;
                int targetP = ad.targetP;
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    for (int i = 0; i < (int)gd.baseData.totalNum; i++)
                    {
                        if (i != mainP && i != targetP)
                        {
                            GameData.character character = gd.chara[i];
                            character.doa = Setting.Doa.doa_Shokei;
                            gd.chara[i] = character;
                        }
                    }
                    gd.RemakePeopleFlg();
                    return true;
                }, (float e) => sp.GetSelect(0) >= 0, false));
                sp.SetFadeScreen(new List<uint> { 15U }, 30U, 0.1f, 5, true, true, true);
                sp.WaitSec(2f, false);
                mainP = ad.mainP;
                targetP = ad.targetP;
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    SaveDataManager.SaveDataFileImage baseData = gd.baseData;
                    baseData.day += 5;
                    for (int j = 1; j < (int)gd.baseData.totalNum; j++)
                    {
                        if (j != mainP && j != targetP)
                        {
                            GameData.character character2 = gd.chara[j];
                            character2.doa = Setting.Doa.doa_Kamare;
                            gd.chara[j] = character2;
                        }
                    }
                    gd.RemakePeopleFlg();
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.SetColorScreen(255U, 40U, -1);
                    sp.SetScreen(Setting.Screen.s_Interface, 50U, true, false, -1);
                    return true;
                }, (float e) => true, true));
                sp.ShowChara(ad.targetP, 4, 1, 20U, false);
                list2 = Util.Split(sp.m_rs.GetScenarioKukulText(2, 14, 1), new char[] { '|' });
                sp.SetNormalSerifu(-3, 0, 1, list2, true, true, true, false);
                sp.WaitSec(0.4f, true);
                list2 = Util.Split(sp.m_rs.GetScenarioKukulText(2, 15, 1), new char[] { '|' });
                sp.SetNormalSerifu(-3, 0, 1, list2, true, true, true, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.m_sb[40U].SetFade(0.8f, 0.5f, 0, -1f, -1, false);
                    return true;
                }, (float e) => true, true));
                sp.WaitFade(new List<uint> { 40U }, false, true);
                sp.PlayBgmInScript("bgm21", 2f, 0.25f, -1, true);
                sp.WaitSec(0.2f, true);
                list2 = Util.Split(sp.m_rs.GetScenarioKukulText(2, 16, 1), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, 1, list2, true, true, true, true);
                sp.FadeBgmInScript(-1f, 0.4f, 1f, false, -1);
                list2 = Util.Split(sp.m_rs.GetScenarioKukulText(2, 17, 4), new char[] { '|' });
                string text2 = list2[0];
                Util.Replace(ref text2, "{0}", gd.takashiName);
                list2[0] = text2;
                sp.SetNormalSerifu(ad.targetP, 0, 1, list2, true, false, false, true);
                sp.FadeBgmInScript(-1f, 0.6f, 1f, false, -1);
                list2 = Util.Split(sp.m_rs.GetScenarioKukulText(2, 18, 3), new char[] { '|' });
                text2 = list2[0];
                Util.Replace(ref text2, "{0}", gd.takashiName);
                list2[0] = text2;
                sp.SetNormalSerifu(ad.targetP, 0, 1, list2, true, true, false, true);
                sp.FadeBgmInScript(-1f, 0.8f, 1f, false, -1);
                list2 = Util.Split(sp.m_rs.GetScenarioKukulText(2, 19, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list2, true, true, false, true);
                list2 = Util.Split(sp.m_rs.GetScenarioKukulText(2, 20, 1), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, 1, list2, true, false, true, true);
                sp.FadeBgmInScript(-1f, 1f, 1f, false, -1);
                list2 = Util.Split(sp.m_rs.GetScenarioKukulText(2, 21, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list2, true, false, false, true);
                sp.StopBgmInScript(-1, false);
                sp.WaitSec(0.05f, false);
                sp.PlaySeInScript("se_jidoudoa", 1f);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetColorScreen(uint.MaxValue, 41U, -1);
                    sp.m_sb[41U].SetFade(0.4f, 1f, 4, 4f, -1, false);
                    return true;
                }, (float e) => true, true));
                sp.LoadTexture("ivep14_02_1");
                sp.WaitLoad();
                sp.WaitFade(new List<uint> { 41U }, false, true);
                sp.RemoveScreenInScript(0U);
                sp.RemoveScreenInScript(20U);
                sp.RemoveScreenInScript(40U);
                sp.UnloadPlace();
                sp.WaitSec(0.2f, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 10U, false, false, -1);
                    sp.m_sb[10U].SetTexture(0, sp.m_sb[10U].gameObject.transform, 0U, "ivep14_02_1", null, null);
                    sp.m_sb[10U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, false));
                sp.SetFadeScreen(new List<uint> { 41U }, 42U, 0.4f, 0, false, true, true);
                sp.LoadTexture("ivep14_02_2");
                sp.WaitLoad();
                sp.WaitFade(new List<uint> { 42U }, true, true);
                list2 = Util.Split(sp.m_rs.GetScenarioKukulText(2, 22, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 1, list2, false, true, true, false);
                sp.LoadSound("se_ashioto_08");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetColorScreen(255U, 42U, -1);
                    sp.m_sb[42U].SetFade(0.4f, 1f, 0, 0f, -1, false);
                    return true;
                }, (float e) => true, true));
                sp.WaitFade(new List<uint> { 42U }, false, true);
                sp.WaitSec(0.2f, true);
                list2 = Util.Split(sp.m_rs.GetScenarioKukulText(2, 23, 4), new char[] { '|' });
                sp.SetNormalSerifu(-3, -1, 1, list2, true, true, true, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep14_02_2", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, false));
                sp.SetFadeScreen(new List<uint> { 10U, 42U }, 11U, 0.4f, 0, false, true, true);
                sp.UnloadTexture("ivep14_02_1");
                sp.WaitFade(new List<uint> { 11U }, true, true);
                sp.WaitSec(0.2f, true);
                list2 = Util.Split(sp.m_rs.GetScenarioKukulText(2, 24, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list2, false, true, true, false);
                sp.LoadTexture("ivep14_02_3");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.StopAllSeInScript();
                sp.UnloadSound("se_ashioto_08");
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.4f, 4, false, true, true);
                sp.UnloadTexture("ivep14_02_2");
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep14_02_3", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, false));
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.PlayBgmInScript("bgm07", 0f, 0.4f, -1, true);
                sp.LoadTexture("ivep14_02_4");
                sp.WaitSec(6f, true);
                sp.WaitLoad();
                sp.FadeBgmInScript(-1f, 0.7f, 0.4f, false, -1);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.4f, 6, false, true, true);
                sp.UnloadTexture("ivep14_02_3");
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep14_02_4", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, false));
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(5.6f, true);
                sp.FadeBgmInScript(-1f, 1f, 2f, false, -1);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    GameData.character character3 = gd.chara[0];
                    character3.doa = Setting.Doa.doa_Kamare;
                    gd.chara[0] = character3;
                    sp.SetColorScreen(1846214911U, 40000U, -1);
                    sp.m_sb[40000U].SetFadeIn(7f, 0);
                    return true;
                }, (float e) => true, false));
                sp.WaitFade(new List<uint> { 40000U }, false, true);
                sp.RemoveScreenInScript(0U);
                sp.RemoveScreenInScript(50U);
                sp.UnloadTexture("ivep14_02_4");
                sp.WaitSec(0.8f, true);
                mainP = ad.mainP;
                Plugin.CheckLocationsInScript(1404);
                gd.baseData.gainExp += 50U;
                sp.WaitSec(1f, true);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioKukulText(2, 26, -1), 2, false);
                sp.FadeBgmInScript(-1f, 0f, 3.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 40000U }, 50001U, 0.8f, 0, true, true, true);
                gd.forwardNext = true;
            };
            __instance.actions[2] = action;
        }


        [HarmonyPatch(typeof(Kukul5Scenario), "SetParam")]
        [HarmonyPostfix]
        static void ToTheHangar(Kukul5Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[13];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                List<string> list11 = Util.Split(sp.m_rs.GetScenarioKukulText(4, 67, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list11, true, false, false, true);
                sp.FadeBgmInScript(-1f, 0.4f, 1f, false, -1);
                if (gd.chara[ad.mainP].i_yaku == Setting.Yakuwari.y_Jinro)
                {
                    list11 = Util.Split(sp.m_rs.GetScenarioKukulText(4, 68, 3), new char[] { '|' });
                }
                else
                {
                    list11 = Util.Split(sp.m_rs.GetScenarioKukulText(4, 69, 3), new char[] { '|' });
                    string text10 = list11[0];
                    Util.Replace(ref text10, "{0}", gd.takashiName);
                    list11[0] = text10;
                }
                sp.SetNormalSerifu(ad.mainP, 0, 1, list11, true, true, false, true);
                list11 = Util.Split(sp.m_rs.GetScenarioKukulText(4, 70, 0), new char[] { '|' });
                string text11 = list11[0];
                Util.Replace(ref text11, "{0}", gd.takashiName);
                list11[0] = text11;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list11, true, true, false, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 3.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.WaitSec(0.75f, true);
                if (!ArchipelagoClient.ServerData.CheckedLocations.Contains(1104)) //Changed condition
                {
                    int mainP = ad.mainP;
                    Plugin.CheckLocationsInScript(1104);
                    gd.baseData.gainExp += 50U;
                }
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(0.4f, true);
                gd.forwardNext = true;
            };
            __instance.actions[13] = action;
            action = __instance.actions[16];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                List<string> list14;
                if ((gd.actionFlg & 8UL) == 0UL)
                {
                    list14 = Util.Split(sp.m_rs.GetScenarioKukulText(4, 80, 7), new char[] { '|' });
                    sp.SetNormalSerifu(ad.mainP, 0, 1, list14, true, true, false, true);
                }
                list14 = Util.Split(sp.m_rs.GetScenarioKukulText(4, 81, 2), new char[] { '|' });
                string text12 = list14[0];
                Util.Replace(ref text12, "{0}", gd.takashiName);
                list14[0] = text12;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list14, true, true, false, true);
                list14 = Util.Split(sp.m_rs.GetScenarioKukulText(4, 82, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list14, true, true, false, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 3.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.UnloadTexture("p11a");
                sp.WaitSec(0.75f, true);
                if (!ArchipelagoClient.ServerData.CheckedLocations.Contains(1104)) //Changed condition
                {
                    int mainP = ad.mainP;
                    Plugin.CheckLocationsInScript(1104);
                    gd.baseData.gainExp += 50U;
                }
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(0.4f, true);
                gd.forwardNext = true;
            };
            __instance.actions[16] = action;
            action = __instance.actions[18];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioKukulText(4, 86, -1), 1, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.WaitSec(0.4f, true);
                sp.LoadPlace(25, true);
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
                sp.WaitSec(0.6f, false);
                sp.StopAllSeInScript();
                sp.PlaySeInScript("se_jidoudoa", 1f);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Interface, 50U, true, false, -1);
                    sp.m_sb[50001U].SetFade(0.3f, 0f, 0, -1f, -1, false);
                    sp.m_sb[50002U].SetFade(0.3f, 0f, 0, -1f, -1, false);
                    return true;
                }, (float e) => true, false));
                sp.ShowChara(ad.mainP, 0, 2, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(360f, 60f, 600f, 337.5f), 0f, 1f, true, null, true);
                sp.WaitFade(new List<uint> { 50001U, 50002U }, true, true);
                sp.WaitSec(0.2f, true);
                List<string> list15 = Util.Split(sp.m_rs.GetScenarioKukulText(4, 87, -1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 2, list15, false, true, true, true);
                sp.PlayBgmInScript("bgm03", 0f, 0.85f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.FadeBgmInScript(-1f, 0.85f, 1f, false, -1);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_LeviMes, 35U, true, false, -1);
                    return true;
                }, (float e) => true, true));
                sp.WaitSec(0.4f, false);
                list15 = Util.Split(sp.m_rs.GetScenarioKukulText(4, 88, -1), new char[] { '|' });
                sp.SetNormalSerifu(-2, ad.mainP, -1, list15, true, false, true, true);
                if (gd.chara[ad.mainP].i_yaku == Setting.Yakuwari.y_Fox || gd.chara[0].i_yaku == Setting.Yakuwari.y_Fox)
                {
                    list15 = Util.Split(sp.m_rs.GetScenarioKukulText(4, 89, 0), new char[] { '|' });
                }
                else
                {
                    list15 = Util.Split(sp.m_rs.GetScenarioKukulText(4, 90, 0), new char[] { '|' });
                }
                string text13 = list15[0];
                Util.Replace(ref text13, "{0}", gd.takashiName);
                list15[0] = text13;
                sp.SetNormalSerifu(ad.mainP, 0, 2, list15, true, false, false, true);
                sp.FadeBgmInScript(-1f, 0.45f, 2.5f, false, -1);
                sp.LoadSound("G_se_switch_door_03");
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(0f, 270f, 480f, 270f), 0.8f, -2.5f, true, null, true);
                sp.WaitLoad();
                sp.WaitSec(0.25f, true);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 35U }, 30U, 0.4f, 0, true, true, true);
                sp.PlaySeInScript("G_se_switch_door_03", 1f);
                sp.LoadSound("se_fuku_03");
                sp.WaitSec(0.4f, true);
                sp.WaitLoad();
                list15 = Util.Split(sp.m_rs.GetScenarioKukulText(4, 91, -1), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, -1, list15, true, true, true, false);
                sp.PlaySeInScript("se_fuku_03", 0.6f);
                sp.WaitSec(0.4f, true);
                list15 = Util.Split(sp.m_rs.GetScenarioKukulText(4, 92, -1), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, -1, list15, true, true, true, false);
                sp.PlaySeInScript("G_se_switch_door_03", 0.4f);
                sp.WaitSec(0.4f, true);
                list15 = Util.Split(sp.m_rs.GetScenarioKukulText(4, 93, -1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, -1, list15, true, true, true, false);
                sp.WaitSec(0.4f, true);
                sp.FadeBgmInScript(-1f, 0f, 4f, true, -1);
                list15 = Util.Split(sp.m_rs.GetScenarioKukulText(4, 94, -1), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, -1, list15, true, true, true, false);
                sp.LoadTexture("ivep14_04_1");
                sp.WaitSec(0.4f, true);
                sp.WaitLoad();
                list15 = Util.Split(sp.m_rs.GetScenarioKukulText(4, 95, -1), new char[] { '|' });
                text13 = list15[0];
                Util.Replace(ref text13, "{0}", gd.takashiName);
                list15[0] = text13;
                sp.SetNormalSerifu(ad.mainP, -1, -1, list15, false, true, true, false);
                sp.WaitText(50U, "test", false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, true, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep14_04_1", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, true));
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.6f, 6, true, true, true);
                sp.PlayBgmInScript("bgm21", 0f, 0.55f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.LoadTexture("ivep14_04_2");
                sp.WaitSec(0.4f, true);
                sp.WaitLoad();
                list15 = Util.Split(sp.m_rs.GetScenarioKukulText(4, 96, -1), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, -1, list15, true, true, true, false);
                sp.WaitSec(0.6f, true);
                list15 = Util.Split(sp.m_rs.GetScenarioKukulText(4, 97, -1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, -1, list15, true, true, true, false);
                sp.PlaySeInScript("G_se_switch_door_03", 0.4f);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.3f, 0, true, true, true);
                sp.UnloadTexture("ivep14_04_1");
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, true, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep14_04_2", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, true));
                sp.FadeBgmInScript(-1f, 1f, 1.2f, false, -1);
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.8f, 0, true, true, true);
                sp.WaitSec(0.6f, true);
                list15 = Util.Split(sp.m_rs.GetScenarioKukulText(4, 98, -1), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, -1, list15, true, true, true, false);
                list15 = Util.Split(sp.m_rs.GetScenarioKukulText(4, 99, -1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, -1, list15, true, true, true, false);
                list15 = Util.Split(sp.m_rs.GetScenarioKukulText(4, 100, -1), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, -1, list15, true, true, true, false);
                sp.FadeBgmInScript(-1f, 0.65f, 2f, false, -1);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.8f, 0, true, true, true);
                sp.UnloadTexture("ivep14_04_2");
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, true));
                sp.ShowChara(ad.mainP, 5, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(180f, 60f, 600f, 337.5f), 0f, 1f, true, null, true);
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.4f, 0, true, true, true);
                sp.WaitSec(0.2f, true);
                list15 = Util.Split(sp.m_rs.GetScenarioKukulText(4, 101, -1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, -1, list15, true, true, true, true);
                list15 = Util.Split(sp.m_rs.GetScenarioKukulText(4, 102, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list15, true, true, false, true);
                sp.FadeBgmInScript(-1f, 0f, 0.4f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.2f, 4, false, true, true);
                sp.UnloadPlace();
                sp.LoadTexture("ivep14_04_3");
                sp.WaitLoad();
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, true, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep14_04_3", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, true));
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.2f, 4, true, true, true);
                sp.WaitSec(1f, true);
                list15 = Util.Split(sp.m_rs.GetScenarioKukulText(4, 103, -1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, -1, list15, true, true, true, false);
                sp.WaitSec(0.05f, false);
                sp.StopAllSeInScript();
                sp.UnloadSound("G_se_switch_door_03");
                sp.UnloadSound("se_fuku_03");
                int mainP = ad.mainP;
                Plugin.CheckLocationsInScript(1405);
                gd.baseData.gainExp += 200U;
                sp.WaitSec(0.4f, true);
                sp.SetFadeScreen(new List<uint> { 0U, 50U }, 30U, 0.8f, 0, true, true, true);
                sp.UnloadTexture("ivep14_04_3");
                sp.WaitSec(0.4f, true);
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioKukulText(4, 105, -1), 1, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.WaitSec(0.2f, true);
                gd.forwardNext = true;
            };
            __instance.actions[18] = action;
        }


        [HarmonyPatch(typeof(Kukul6Scenario), "SetParam")]
        [HarmonyPostfix]
        static void Regret(Kukul6Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[1];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                ScenarioContents.ChangeSceOnFlg(ref gd, ref sd, 2U);
                sd.flg |= 16384;
                sp.LoadSound("se_ashioto_14");
                sp.WaitLoad();
                sp.PlaySeInScript("se_jidoudoa", 1f);
                sp.PlaySeInScript("se_ashioto_14", 1f);
                sp.LoadTexture("p14a");
                sp.WaitSec(0.45f, true);
                sp.WaitLoad();
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.ChangeCharaTexture(14U, "p14a", 10U, 20U, true);
                    sp.SetScreen(Setting.Screen.s_Interface, 50U, true, false, -1);
                    return true;
                }, (float e) => true, false));
                sp.ShowChara(ad.mainP, 3, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(320f, 130f, 320f, 180f), 0f, 1f, true, null, true);
                sp.WaitSec(0.6f, true);
                sp.StopAllSeInScript();
                sp.PlaySeInScript("se_fuku_02", 1f);
                sp.SetFadeScreen(new List<uint> { 50001U, 50002U }, 50003U, 0.25f, 4, true, true, true);
                sp.PlayBgmInScript("bgm21", 1.2f, 0.8f, -1, true);
                sp.WaitSec(0.2f, true);
                List<string> list = Util.Split(sp.m_rs.GetScenarioKukulText(5, 2, 3), new char[] { '|' });
                string text = list[0];
                list[0] = text;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list, true, true, true, true);
                sp.UnloadSound("se_ashioto_14");
                sp.WaitSec(0.4f, true);
                sp.PlayBgmInScript("bgm03", 0.4f, 0.8f, -1, true);
                list = Util.Split(sp.m_rs.GetScenarioKukulText(5, 3, 0), new char[] { '|' });
                text = list[0];
                Util.Replace(ref text, "{0}", gd.takashiName);
                list[0] = text;
                sp.SetNormalSerifu(ad.targetP, 0, 0, list, false, false, false, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list = Util.Split(sp.m_rs.GetScenarioKukulText(5, 4, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioKukulText(5, 5, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 0, list, false, false, false, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.ChangeCharaTexture(14U, "p14", 10U, 20U, true);
                    return true;
                }, (float e) => true, true));
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.FadeBgmInScript(-1f, 0f, 0.2f, false, -1);
                sp.PlaySeInScript("se_se_04", 1f);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.15f, 0, false, false, true);
                sp.UnvisibleAllChara(20U, -1);
                sp.ShowChara(ad.mainP, 3, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(280f, 100f, 400f, 227f), 0f, 1f, false, null, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetColorScreen(2408578911U, 28U, -1);
                    sp.m_sb[0U].SetFade(0.4f, 1f, 102, 0.5f, 0, false);
                    sp.m_sb[20U].SetFade(0.4f, 1f, 102, 0.5f, 1, false);
                    sp.m_sb[28U].SetFade(0.4f, 0f, 0, 1f, -1, false);
                    return true;
                }, (float e) => true, false));
                sp.WaitFade(new List<uint> { 28U, 30U }, true, true);
                sp.WaitFade(new List<uint> { 0U, 20U }, false, true);
                sp.FadeBgmInScript(0f, 1f, 4f, false, -1);
                list = Util.Split(sp.m_rs.GetScenarioKukulText(5, 6, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 1, list, true, true, true, true);
                list = Util.Split(sp.m_rs.GetScenarioKukulText(5, 7, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 0, list, true, false, false, true);
                sp.FadeBgmInScript(-1f, 0f, 1.2f, true, -1);
                list = Util.Split(sp.m_rs.GetScenarioKukulText(5, 8, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list, true, false, false, true);
                sp.PlayBgmInScript("bgm05", 0f, 1f, -1, true);
                list = Util.Split(sp.m_rs.GetScenarioKukulText(5, 9, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 0, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioKukulText(5, 10, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 0, list, false, true, false, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.ChangeCharaTexture(14U, "p14a", 10U, 20U, true);
                    return true;
                }, (float e) => true, true));
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.25f, 0, false, false, true);
                sp.UnvisibleAllChara(20U, -1);
                sp.ShowChara(ad.mainP, 5, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(240f, 80f, 480f, 270f), 0.4f, -2.5f, true, null, true);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(1.5f, true);
                sp.FadeBgmInScript(-1f, 0f, 1.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadTexture("p14a");
                sp.UnloadPlace();
                sp.WaitSec(0.5f, true);
                Plugin.CheckLocationsInScript(16);
                gd.baseData.gainExp += 50U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioKukulText(5, 12, -1), 3, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                gd.forwardNext = true;
            };
            __instance.actions[1] = action;
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
                Plugin.CheckLocationsInScript(1204);
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(1f, true);
                Plugin.CheckLocationsInScript(7);
                gd.baseData.gainExp += 100U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioOtomeText(0, 41, -1), 3, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.WaitSec(0.4f, true);
                gd.forwardNext = true;
            };
            __instance.actions[7] = action;
        }


        [HarmonyPatch(typeof(Otome2Scenario), "SetParam")]
        [HarmonyPostfix]
        static void OtomesResolution(Otome2Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[3];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.LoadPlace(5, true);
                sp.WaitLoad();
                sp.PlaySeInScript("se_square", 1f);
                if (gd.yakuAliveNum[7] > 0)
                {
                    sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioOtomeText(1, 19, -1), 1, false);
                }
                else
                {
                    sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioOtomeText(1, 20, -1), 1, false);
                }
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.WaitSec(0.35f, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Interface, 50U, true, false, -1);
                    sp.SetColorScreen(255U, 50000U, -1);
                    return true;
                }, (float e) => true, true));
                sp.ShowChara(ad.mainP, 0, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(180f, 60f, 600f, 337.5f), 0f, 1f, true, null, true);
                sp.SetFadeScreen(new List<uint> { 50000U }, 50001U, 0.6f, 0, false, true, true);
                sp.WaitFade(new List<uint> { 50001U }, true, true);
                sp.LoadSound("se_otomecar_02");
                sp.WaitSec(0.2f, true);
                sp.WaitLoad();
                List<string> list2 = Util.Split(sp.m_rs.GetScenarioOtomeText(1, 21, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list2, false, true, true, true);
                sp.PlayBgmInScript("bgm18", 2f, 0.8f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list2 = Util.Split(sp.m_rs.GetScenarioOtomeText(1, 22, 6), new char[] { '|' });
                string text2 = list2[0];
                Util.Replace(ref text2, "{0}", gd.takashiName);
                list2[0] = text2;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list2, true, true, false, true);
                sp.FadeBgmInScript(-1f, 0f, 2f, true, -1);
                sp.PlaySeInScript("se_otomecar_02", 0.7f);
                list2 = Util.Split(sp.m_rs.GetScenarioOtomeText(1, 23, 6), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, 1, list2, false, false, false, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.PlaySeInScript("se_ashioto_02", 1f);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.4f, 4, false, true, true);
                sp.UnloadPlace();
                sp.LoadTexture("p12a");
                sp.WaitLoad();
                sp.LoadPlace(9, true);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(1f, true);
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
                sp.WaitSec(0.6f, false);
                sp.StopAllSeInScript();
                sp.UnloadSound("se_otomecar_02");
                sp.PlaySeInScript("se_jidoudoa", 1f);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.ChangeCharaTexture(12U, "p12a", 10U, 20U, true);
                    sp.SetColorScreen(255U, 30U, -1);
                    sp.m_sb[50001U].SetFade(0.3f, 0f, 0, -1f, -1, false);
                    sp.m_sb[50002U].SetFade(0.3f, 0f, 0, -1f, -1, false);
                    return true;
                }, (float e) => true, true));
                sp.ShowChara(ad.mainP, 6, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(120f, 40f, 720f, 405f), 0f, 1f, true, null, true);
                sp.WaitFade(new List<uint> { 50001U, 50002U }, true, true);
                list2 = Util.Split(sp.m_rs.GetScenarioOtomeText(1, 24, -1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list2, true, true, true, false);
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.4f, 0, true, true, true);
                sp.WaitSec(0.25f, true);
                list2 = Util.Split(sp.m_rs.GetScenarioOtomeText(1, 25, 0), new char[] { '|' });
                text2 = list2[0];
                Util.Replace(ref text2, "{0}", gd.takashiName);
                list2[0] = text2;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list2, false, true, false, false);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list2 = Util.Split(sp.m_rs.GetScenarioOtomeText(1, 26, 5), new char[] { '|' });
                sp.SetNormalSerifu(0, ad.mainP, 1, list2, false, false, true, false);
                sp.PlayBgmInScript("bgm14", 2f, 0.6f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.25f, 0, false, false, true);
                sp.UnvisibleAllChara(20U, -1);
                sp.ShowChara(ad.mainP, 1, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(180f, 60f, 600f, 337.5f), 0.4f, -2.5f, true, null, true);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list2 = Util.Split(sp.m_rs.GetScenarioOtomeText(1, 27, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list2, true, true, true, false);
                sp.WaitSec(0.3f, true);
                list2 = Util.Split(sp.m_rs.GetScenarioOtomeText(1, 28, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list2, true, true, false, false);
                list2 = Util.Split(sp.m_rs.GetScenarioOtomeText(1, 29, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list2, true, true, false, false);
                list2 = Util.Split(sp.m_rs.GetScenarioOtomeText(1, 30, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list2, true, true, false, false);
                list2 = Util.Split(sp.m_rs.GetScenarioOtomeText(1, 31, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list2, true, true, false, false);
                sp.WaitSec(0.3f, true);
                list2 = Util.Split(sp.m_rs.GetScenarioOtomeText(1, 32, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list2, true, true, false, false);
                list2 = Util.Split(sp.m_rs.GetScenarioOtomeText(1, 33, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list2, false, true, false, false);
                sp.LoadSound("se_switch_01");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.PlaySeInScript("se_switch_01", 0.6f);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.6f, 0, false, true, true);
                sp.WaitSec(0.3f, true);
                sp.FadeBgmInScript(0.25f, 1f, 4f, false, -1);
                sp.PlaySeInScript("se_pusyu", 1f);
                sp.UnloadPlace();
                sp.UnloadTexture("p12a");
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(1.5f, true);
                list2 = Util.Split(sp.m_rs.GetScenarioOtomeText(1, 34, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list2, true, true, true, false);
                sp.WaitSec(0.4f, true);
                list2 = Util.Split(sp.m_rs.GetScenarioOtomeText(1, 35, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list2, true, true, true, false);
                sp.WaitSec(0.6f, true);
                list2 = Util.Split(sp.m_rs.GetScenarioOtomeText(1, 36, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list2, true, true, true, false);
                sp.WaitSec(0.8f, true);
                list2 = Util.Split(sp.m_rs.GetScenarioOtomeText(1, 37, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list2, true, true, true, false);
                sp.WaitSec(0.6f, true);
                sp.StopAllSeInScript();
                sp.UnloadSound("se_switch_01");
                sp.RemoveScreenInScript(50U);
                sp.FadeBgmInScript(-1f, 0f, 0.05f, true, -1);
                sp.PlaySeInScript("se_gatyan", 1f);
                sp.WaitSec(0.2f, false);
                sp.PlaySeInScript("se_pusyu", 1f);
                sp.WaitSec(2.4f, true);
                int mainP = ad.mainP;
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    GameData.character character3 = gd.chara[mainP];
                    character3.doa = Setting.Doa.doa_Shokei;
                    gd.chara[mainP] = character3;
                    gd.RemakePeopleFlg();
                    Jinro.MakeYakuAliveNum(ref gd);
                    return true;
                }, (float e) => true, false));
                Plugin.CheckLocationsInScript(1206);
                gd.baseData.gainExp += 50U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(1f, true);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioOtomeText(1, 39, -1), 1, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                gd.forwardNext = true;
            };
            __instance.actions[3] = action;
        }


        [HarmonyPatch(typeof(Rakio1Scenario), "SetParam")]
        [HarmonyPostfix]
        static void ShowerRoomRaqio(Rakio1Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[2];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                ScenarioContents.ChangeSceOnFlg(ref gd, ref sd, 2U);
                sp.FadeBgmInScript(-1f, 1f, 1.4f, false, 0);
                sp.LoadTexture("p03c");
                sp.WaitSec(2.4f, true);
                sp.WaitLoad();
                sp.StopBgmInScript(-1, false);
                sp.PlaySeInScript("se_jidoudoa", 1f);
                sp.PlayBgmInScript("bgm98", 0.4f, 0.05f, 1, true);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.3f, 2, false, true, true);
                sp.UnloadPlace();
                sp.LoadPlace(18, false);
                sp.WaitLoad();
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    uint num = 3U;
                    sp.m_sb[20U].RemoveTextureWithChild(100U * num, 10U);
                    sp.m_sb[20U].SetPackedTexture(0, sp.m_sb[20U].gameObject.transform, "p03c", "body", 100U * num, 10U, new Vector2?(new Vector2((float)(18446744073709551416UL + (ulong)(50U * num)), 0f)), null, null, false);
                    sp.m_sb[20U].m_spriteMap[100U * num].GetComponent<Image>().material = sp.m_rs.uiCharaDefaultMat;
                    sp.m_sb[20U].m_spriteMap[100U * num].GetComponent<Image>().material.SetColor("_Color", Color.white);
                    sp.m_sb[20U].m_spriteMap[100U * num].SetSize(0.7f);
                    sp.m_sb[20U].m_spriteMap[100U * num].SetDisplayOffsetY((float)sp.m_rs.m_displaySize.height - sp.m_sb[20U].m_spriteMap[100U * num].GetSizeInDisplay().y * sp.m_sb[20U].m_spriteMap[100U * num].GetSize() * GraphicsContext.m_textureRatio);
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, true));
                sp.ShowChara(ad.targetP, 0, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(180f, 60f, 600f, 337.5f), 0f, 1f, true, null, true);
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.2f, 2, true, true, true);
                List<string> list2 = Util.Split(sp.m_rs.GetScenarioRakioText(0, 4, 0), new char[] { '|' });
                string text2 = list2[0];
                list2[0] = text2;
                sp.SetNormalSerifu(ad.targetP, 0, 1, list2, false, true, true, true);
                sp.WaitText(50U, "test", false);
                sp.PlayBgmInScript("bgm05", 0f, 0.75f, 0, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list2 = Util.Split(sp.m_rs.GetScenarioRakioText(0, 5, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list2, true, true, false, true);
                sp.FadeBgmInScript(-1f, 0.35f, 2.4f, false, 1);
                list2 = Util.Split(sp.m_rs.GetScenarioRakioText(0, 6, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list2, true, true, false, true);
                list2 = Util.Split(sp.m_rs.GetScenarioRakioText(0, 7, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list2, true, true, false, true);
                list2 = Util.Split(sp.m_rs.GetScenarioRakioText(0, 8, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list2, true, false, false, true);
                sp.FadeBgmInScript(-1f, 0f, 1f, true, 0);
                list2 = Util.Split(sp.m_rs.GetScenarioRakioText(0, 9, 1), new char[] { '|' });
                sp.SetNormalSerifu(-3, -1, 1, list2, false, false, true, true);
                sp.LoadSound("G_se_kyu_02");
                sp.WaitLoad();
                sp.LoadSound("G_se_switch_door_03");
                sp.WaitLoad();
                sp.StopBgmInScript(1, false);
                sp.PlaySeInScript("G_se_kyu_02", 0.6f);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.PlaySeInScript("G_se_switch_door_03", 1f);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetColorScreen(255U, 30U, -1);
                    sp.m_sb[30U].SetFade(0.2f, 1f, 0, 0f, -1, false);
                    return true;
                }, (float e) => true, true));
                sp.WaitFade(new List<uint> { 30U }, false, true);
                sp.UnvisibleAllChara(20U, -1);
                sp.ShowChara(ad.counterP, 7, 1, 20U, false);
                sp.WaitSec(0.4f, true);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(240f, 80f, 480f, 270f), 4f, 1f, false, new Vector4?(new Vector4(240f, 270f, 480f, 270f)), true);
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 1.5f, 0, false, true, true);
                sp.WaitSec(0.75f, true);
                sp.PlayBgmInScript("bgm22", 0f, 1f, -1, true);
                sp.WaitFade(new List<uint> { 31U }, true, true);
                sp.WaitClipAnim(new List<uint> { 0U, 20U }, true);
                list2 = Util.Split(sp.m_rs.GetScenarioRakioText(0, 10, 1), new char[] { '|' });
                sp.SetNormalSerifu(-3, -1, 1, list2, true, true, true, true);
                list2 = Util.Split(sp.m_rs.GetScenarioRakioText(0, 11, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 2, list2, true, false, false, true);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.25f, 0, false, false, true);
                sp.UnvisibleAllChara(20U, -1);
                sp.ShowChara(ad.counterP, 2, 1, 20U, false);
                sp.SetNormalClipAnim(1);
                sp.WaitClipAnim(new List<uint> { 0U, 20U }, true);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list2 = Util.Split(sp.m_rs.GetScenarioRakioText(0, 12, 2), new char[] { '|' });
                sp.SetNormalSerifu(-3, -1, 1, list2, true, true, true, true);
                list2 = Util.Split(sp.m_rs.GetScenarioRakioText(0, 13, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 2, list2, true, false, false, true);
                list2 = Util.Split(sp.m_rs.GetScenarioRakioText(0, 14, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, ad.targetP, 1, list2, true, false, false, true);
                list2 = Util.Split(sp.m_rs.GetScenarioRakioText(0, 15, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.counterP, 2, list2, true, false, false, true);
                list2 = Util.Split(sp.m_rs.GetScenarioRakioText(0, 16, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, ad.targetP, 1, list2, true, false, false, true);
                sp.FadeBgmInScript(0f, 1f, 2f, false, -1);
                sp.PlaySeInScript("se_jin_04", 1f);
                list2 = Util.Split(sp.m_rs.GetScenarioRakioText(0, 17, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.counterP, 2, list2, true, false, false, true);
                list2 = Util.Split(sp.m_rs.GetScenarioRakioText(0, 18, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, ad.targetP, 1, list2, true, false, false, true);
                list2 = Util.Split(sp.m_rs.GetScenarioRakioText(0, 19, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 2, list2, true, false, false, true);
                list2 = Util.Split(sp.m_rs.GetScenarioRakioText(0, 20, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, ad.targetP, 1, list2, true, false, false, true);
                list2 = Util.Split(sp.m_rs.GetScenarioRakioText(0, 21, 7), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, -1, 1, list2, true, true, false, true);
                sp.FadeBgmInScript(-1f, 0f, 4f, true, -1);
                sp.PlaySeInScript("se_jidoudoa", 1f);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetColorScreen(255U, 30U, -1);
                    sp.m_sb[30U].SetFade(0.25f, 1f, 4, 0f, -1, false);
                    return true;
                }, (float e) => true, true));
                sp.WaitFade(new List<uint> { 30U }, false, true);
                sp.UnvisibleAllChara(20U, -1);
                sp.ShowChara(ad.targetP, 4, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(120f, 40f, 720f, 405f), 0f, 1f, true, null, true);
                sp.WaitSec(0.6f, true);
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 1f, 0, false, true, true);
                sp.WaitFade(new List<uint> { 31U }, true, true);
                sp.WaitSec(0.8f, true);
                list2 = Util.Split(sp.m_rs.GetScenarioRakioText(0, 22, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 1, list2, true, true, true, true);
                list2 = Util.Split(sp.m_rs.GetScenarioRakioText(0, 23, 0), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, 1, list2, true, true, true, true);
                sp.WaitSec(0.05f, false);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadSound("G_se_kyu_02");
                sp.UnloadSound("G_se_switch_door_03");
                sp.UnloadTexture("p03c");
                sp.UnloadPlace();
                sp.WaitSec(0.6f, true);
                int counterP = ad.counterP;
                Plugin.CheckLocationsInScript(303);
                gd.baseData.gainExp += 50U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(0.4f, true);
                gd.forwardNext = true;
            };
            __instance.actions[2] = action;
        }


        [HarmonyPatch(typeof(Rakio2Scenario), "SetParam")]
        [HarmonyPostfix]
        static void RaqioQuizDefiniteHuman(Rakio2Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[3];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                List<string> list3 = Util.Split(sp.m_rs.GetScenarioRakioText(1, 12, 0), new char[] { '|' });
                sp.SetNormalSerifu(0, ad.targetP, 1, list3, true, false, true, true);
                list3 = Util.Split(sp.m_rs.GetScenarioRakioText(1, 13, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list3, true, false, false, true);
                list3 = Util.Split(sp.m_rs.GetScenarioRakioText(1, 14, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list3, true, true, false, true);
                list3 = Util.Split(sp.m_rs.GetScenarioRakioText(1, 15, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list3, true, true, false, true);
                list3 = Util.Split(sp.m_rs.GetScenarioRakioText(1, 16, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list3, true, true, false, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 1.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.WaitSec(0.5f, true);
                Plugin.CheckLocationsInScript(2, 3, 4);
                gd.baseData.gainExp += 50U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioRakioText(1, 18, -1), 3, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
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
                List<string> list4 = Util.Split(sp.m_rs.GetScenarioRakioText(1, 20, 0), new char[] { '|' });
                sp.SetNormalSerifu(0, ad.targetP, 1, list4, true, false, true, true);
                list4 = Util.Split(sp.m_rs.GetScenarioRakioText(1, 21, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list4, true, false, false, true);
                list4 = Util.Split(sp.m_rs.GetScenarioRakioText(1, 22, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list4, true, true, false, true);
                sp.WaitSec(0.1f, false);
                Plugin.CheckLocationsInScript(2, 3, 4);
                gd.baseData.gainExp += 50U;
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioRakioText(1, 24, -1), 3, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                list4 = Util.Split(sp.m_rs.GetScenarioRakioText(1, 25, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list4, true, true, false, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 1.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                gd.forwardNext = true;
            };
            __instance.actions[4] = action;
        }


        [HarmonyPatch(typeof(Rakio4Scenario), "SetParam")]
        [HarmonyPostfix]
        static void RaqioQuizNote4(Rakio4Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[3];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                List<string> list3 = Util.Split(sp.m_rs.GetScenarioRakioText(3, 13, 0), new char[] { '|' });
                sp.SetNormalSerifu(0, ad.targetP, 1, list3, true, false, true, true);
                list3 = Util.Split(sp.m_rs.GetScenarioRakioText(3, 14, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list3, true, false, false, true);
                list3 = Util.Split(sp.m_rs.GetScenarioRakioText(3, 15, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list3, true, true, false, true);
                list3 = Util.Split(sp.m_rs.GetScenarioRakioText(3, 16, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list3, true, true, false, true);
                list3 = Util.Split(sp.m_rs.GetScenarioRakioText(3, 17, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list3, true, true, false, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 1.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.WaitSec(0.5f, true);
                int targetP = ad.targetP;
                Plugin.CheckLocationsInScript(304);
                gd.baseData.gainExp += 50U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                gd.forwardNext = true;
            };
            __instance.actions[3] = action;
            action = __instance.actions[4];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                List<string> list4 = Util.Split(sp.m_rs.GetScenarioRakioText(3, 20, 0), new char[] { '|' });
                sp.SetNormalSerifu(0, ad.targetP, 1, list4, true, false, true, true);
                list4 = Util.Split(sp.m_rs.GetScenarioRakioText(3, 21, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list4, true, false, false, true);
                list4 = Util.Split(sp.m_rs.GetScenarioRakioText(3, 22, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list4, true, true, false, true);
                list4 = Util.Split(sp.m_rs.GetScenarioRakioText(3, 23, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list4, true, true, false, true);
                list4 = Util.Split(sp.m_rs.GetScenarioRakioText(3, 24, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list4, true, true, false, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 1.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.WaitSec(0.5f, true);
                int targetP = ad.targetP;
                Plugin.CheckLocationsInScript(304);
                gd.baseData.gainExp += 50U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                gd.forwardNext = true;
            };
            __instance.actions[4] = action;
        }


        [HarmonyPatch(typeof(Rakio5Scenario), "SetParam")]
        [HarmonyPostfix]
        static void RaqioQuizNote5(Rakio5Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[7];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                List<string> list7 = Util.Split(sp.m_rs.GetScenarioRakioText(4, 34, 0), new char[] { '|' });
                sp.SetNormalSerifu(0, ad.targetP, 1, list7, true, false, true, true);
                list7 = Util.Split(sp.m_rs.GetScenarioRakioText(4, 35, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list7, true, false, false, true);
                list7 = Util.Split(sp.m_rs.GetScenarioRakioText(4, 36, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list7, true, true, false, true);
                list7 = Util.Split(sp.m_rs.GetScenarioRakioText(4, 37, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list7, true, true, true, true);
                list7 = Util.Split(sp.m_rs.GetScenarioRakioText(4, 38, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list7, true, true, true, true);
                list7 = Util.Split(sp.m_rs.GetScenarioRakioText(4, 39, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list7, true, true, false, true);
                list7 = Util.Split(sp.m_rs.GetScenarioRakioText(4, 40, 7), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list7, true, true, false, true);
                list7 = Util.Split(sp.m_rs.GetScenarioRakioText(4, 41, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list7, true, true, false, true);
                list7 = Util.Split(sp.m_rs.GetScenarioRakioText(4, 42, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list7, true, true, false, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 1.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.UnloadTexture("p03a");
                sp.WaitSec(0.5f, true);
                int targetP = ad.targetP;
                Plugin.CheckLocationsInScript(305);
                gd.baseData.gainExp += 50U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                gd.forwardNext = true;
            };
            __instance.actions[7] = action;
            action = __instance.actions[8];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                List<string> list8 = Util.Split(sp.m_rs.GetScenarioRakioText(4, 45, 0), new char[] { '|' });
                sp.SetNormalSerifu(0, ad.targetP, 1, list8, true, false, true, true);
                list8 = Util.Split(sp.m_rs.GetScenarioRakioText(4, 46, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list8, true, false, false, true);
                list8 = Util.Split(sp.m_rs.GetScenarioRakioText(4, 47, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list8, true, true, false, true);
                list8 = Util.Split(sp.m_rs.GetScenarioRakioText(4, 48, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list8, true, true, true, true);
                list8 = Util.Split(sp.m_rs.GetScenarioRakioText(4, 49, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list8, true, true, true, true);
                list8 = Util.Split(sp.m_rs.GetScenarioRakioText(4, 50, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list8, true, true, false, true);
                list8 = Util.Split(sp.m_rs.GetScenarioRakioText(4, 51, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list8, true, true, false, true);
                list8 = Util.Split(sp.m_rs.GetScenarioRakioText(4, 52, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list8, true, true, true, true);
                list8 = Util.Split(sp.m_rs.GetScenarioRakioText(4, 53, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list8, true, true, false, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 1.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.UnloadTexture("p03a");
                sp.WaitSec(0.5f, true);
                int targetP = ad.targetP;
                Plugin.CheckLocationsInScript(305);
                gd.baseData.gainExp += 50U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                gd.forwardNext = true;
            };
            __instance.actions[8] = action;
            action = __instance.actions[10];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                List<string> list10 = Util.Split(sp.m_rs.GetScenarioRakioText(4, 62, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list10, true, true, false, true);
                list10 = Util.Split(sp.m_rs.GetScenarioRakioText(4, 63, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list10, true, true, false, true);
                list10 = Util.Split(sp.m_rs.GetScenarioRakioText(4, 64, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list10, true, true, true, true);
                list10 = Util.Split(sp.m_rs.GetScenarioRakioText(4, 65, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list10, true, true, true, true);
                list10 = Util.Split(sp.m_rs.GetScenarioRakioText(4, 66, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list10, true, true, false, true);
                list10 = Util.Split(sp.m_rs.GetScenarioRakioText(4, 67, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list10, true, true, false, true);
                list10 = Util.Split(sp.m_rs.GetScenarioRakioText(4, 68, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list10, true, true, true, true);
                list10 = Util.Split(sp.m_rs.GetScenarioRakioText(4, 69, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list10, true, true, false, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 1.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.UnloadTexture("p03a");
                sp.WaitSec(0.5f, true);
                int mainP = ad.mainP;
                Plugin.CheckLocationsInScript(305);
                gd.baseData.gainExp += 50U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                gd.forwardNext = true;
            };
            __instance.actions[10] = action;
        }


        [HarmonyPatch(typeof(Rakio6Scenario), "SetParam")]
        [HarmonyPostfix]
        static void RaqioQuizFreezeAll(Rakio6Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[5];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                List<string> list5 = Util.Split(sp.m_rs.GetScenarioRakioText(5, 23, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list5, true, true, false, true);
                list5 = Util.Split(sp.m_rs.GetScenarioRakioText(5, 24, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list5, true, true, false, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 1.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.WaitSec(0.5f, true);
                Plugin.CheckLocationsInScript(9);
                gd.baseData.gainExp += 50U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioRakioText(5, 26, -1), 3, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                gd.forwardNext = true;
            };
            __instance.actions[5] = action;
        }


        [HarmonyPatch(typeof(Rakio7Scenario), "SetParam")]
        [HarmonyPostfix]
        static void Raqio6(Rakio7Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[16];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.PlayBgmInScript("bgm16", 0.6f, 0.75f, -1, true);
                List<string> list14 = Util.Split(sp.m_rs.GetScenarioRakioText(6, 76, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list14, true, true, false, true);
                list14 = Util.Split(sp.m_rs.GetScenarioRakioText(6, 77, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list14, true, true, false, true);
                list14 = Util.Split(sp.m_rs.GetScenarioRakioText(6, 78, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list14, true, true, false, true);
                list14 = Util.Split(sp.m_rs.GetScenarioRakioText(6, 79, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list14, true, true, false, true);
                sp.FadeBgmInScript(-1f, 0.4f, 2.5f, false, -1);
                list14 = Util.Split(sp.m_rs.GetScenarioRakioText(6, 80, 0), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, 1, list14, true, false, true, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 1f, true, -1);
                int mainP = ad.mainP;
                Plugin.CheckLocationsInScript(306);
                gd.baseData.gainExp += 250U;
                sp.WaitSec(0.4f, true);
                list14 = Util.Split(sp.m_rs.GetScenarioRakioText(6, 82, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list14, false, false, false, true);
                sp.PlayBgmInScript("bgm03", 0.8f, 1f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list14 = Util.Split(sp.m_rs.GetScenarioRakioText(6, 83, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list14, true, false, false, true);
                sp.WaitSec(0.01f, false);
                sp.FadeBgmInScript(-1f, 0.4f, 1.5f, false, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 30U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioRakioText(6, 84, -1), 1, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.FadeBgmInScript(-1f, 0f, 2.5f, true, -1);
                gd.forwardNext = true;
            };
            __instance.actions[16] = action;
        }


        [HarmonyPatch(typeof(Rakio8Scenario), "SetParam")]
        [HarmonyPostfix]
        static void TheFinalProblem(Rakio8Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[3];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.PlayBgmInScript("bgm10", 2f, 0.9f, -1, true);
                List<string> list3;
                if (ad.mainP == 0)
                {
                    list3 = Util.Split(sp.m_rs.GetScenarioRakioText(7, 28, 1), new char[] { '|' });
                    sp.SetNormalSerifu(0, ad.targetP, 1, list3, true, false, true, true);
                    list3 = Util.Split(sp.m_rs.GetScenarioRakioText(7, 29, 6), new char[] { '|' });
                    sp.SetNormalSerifu(ad.targetP, 0, 1, list3, true, false, false, true);
                }
                else
                {
                    list3 = Util.Split(sp.m_rs.GetScenarioRakioText(7, 30, 0), new char[] { '|' });
                    string text3 = list3[0];
                    Util.Replace(ref text3, "{0}", gd.takashiName);
                    list3[0] = text3;
                    sp.SetNormalSerifu(ad.targetP, 0, 1, list3, true, false, false, true);
                    list3 = Util.Split(sp.m_rs.GetScenarioRakioText(7, 31, 6), new char[] { '|' });
                    sp.SetNormalSerifu(ad.targetP, 0, 1, list3, true, true, false, true);
                }
                sp.WaitSec(0.2f, true);
                list3 = Util.Split(sp.m_rs.GetScenarioRakioText(7, 32, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list3, true, true, false, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 3.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.WaitSec(0.7f, true);
                int mainP = ad.mainP;
                int targetP = ad.targetP;
                Plugin.CheckLocationsInScript(906);
                gd.baseData.gainExp += 50U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(0.4f, true);
                gd.forwardNext = true;
            };
            __instance.actions[3] = action;
        }


        [HarmonyPatch(typeof(Remnant1Scenario), "SetParam")]
        [HarmonyPostfix]
        static void InescapablePast(Remnant1Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[9];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.LoadSound("se_fuku_05");
                sp.WaitLoad();
                sp.UnloadPlace();
                sp.LoadTexture("ivep07_00_2");
                sp.WaitSec(0.45f, true);
                sp.WaitLoad();
                sp.PlaySeInScript("se_jidoudoa", 1f);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, true, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep07_00_2", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.SetScreen(Setting.Screen.s_Interface, 50U, true, false, -1);
                    return true;
                }, (float e) => true, true));
                sp.SetFadeScreen(new List<uint> { 50001U, 50002U }, 50003U, 0.3f, 0, true, true, true);
                sp.WaitSec(0.4f, true);
                List<string> list8 = Util.Split(sp.m_rs.GetScenarioRemnantText(0, 50, 0), new char[] { '|' });
                string text3 = list8[0];
                list8[0] = text3;
                sp.SetNormalSerifu(ad.mainP, -1, 1, list8, true, true, true, false);
                sp.WaitSec(0.6f, true);
                list8 = Util.Split(sp.m_rs.GetScenarioRemnantText(0, 51, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list8, false, true, true, false);
                sp.PlayBgmInScript("bgm14", 2f, 1f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.PlaySeInScript("se_ashioto_02", 1f);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.4f, 0, false, true, true);
                sp.UnloadTexture("ivep07_00_2");
                sp.LoadTexture("ivep07_00_3");
                sp.WaitLoad();
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(0.6f, true);
                sp.StopAllSeInScript();
                sp.PlaySeInScript("se_okiagari", 0.6f);
                sp.WaitSec(0.3f, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, true, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep07_00_3", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, true));
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.6f, 0, true, true, true);
                sp.WaitSec(0.6f, true);
                list8 = Util.Split(sp.m_rs.GetScenarioRemnantText(0, 52, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list8, false, true, true, false);
                sp.WaitSec(0.6f, true);
                sp.SetClipAnim(new List<uint> { 0U }, new Vector4(0f, 783.75f, 960f, 540f), 10f, 1f, false, null, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.WaitSec(1f, true);
                list8 = Util.Split(sp.m_rs.GetScenarioRemnantText(0, 53, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list8, false, true, true, false);
                sp.LoadTexture("ivep07_00_4");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.WaitClipAnim(new List<uint> { 0U }, true);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.35f, 0, false, true, true);
                sp.UnloadTexture("ivep07_00_3");
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, true, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep07_00_4", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, true));
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(0.25f, true);
                list8 = Util.Split(sp.m_rs.GetScenarioRemnantText(0, 54, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list8, false, true, true, false);
                sp.LoadTexture("ivep07_00_5");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.FadeBgmInScript(-1f, 0f, 1.2f, true, -1);
                list8 = Util.Split(sp.m_rs.GetScenarioRemnantText(0, 55, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list8, false, true, true, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.m_sb[50U].m_textAreaMap["test"].nowLine >= 2, true));
                sp.PlayBgmInScript("bgm19", 0.1f, 1f, -1, true);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.25f, 3, false, true, true);
                sp.UnloadTexture("ivep07_00_4");
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, true, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep07_00_5", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, true));
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.4f, 0, false, true, true);
                sp.UnloadTexture("ivep07_00_5");
                sp.LoadTexture("ivep07_00_6");
                sp.WaitLoad();
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.PlaySeInScript("se_fuku_05", 1f);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, true, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep07_00_6", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, true));
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.25f, 0, true, true, true);
                list8 = Util.Split(sp.m_rs.GetScenarioRemnantText(0, 56, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list8, false, true, true, false);
                sp.LoadTexture("ivep07_00_7");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.4f, 0, false, true, true);
                sp.UnloadTexture("ivep07_00_6");
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, true, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep07_00_7", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, true));
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list8 = Util.Split(sp.m_rs.GetScenarioRemnantText(0, 57, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list8, true, true, true, false);
                sp.WaitSec(1.2f, true);
                sp.StopAllSeInScript();
                sp.UnloadSound("se_fuku_05");
                list8 = Util.Split(sp.m_rs.GetScenarioRemnantText(0, 58, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list8, true, true, true, false);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 2.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 50U }, 40002U, 2f, 0, false, true, true);
                sp.UnloadTexture("ivep07_00_7");
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                int mainP = ad.mainP;
                int targetP = ad.targetP;
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    gnosia.GameData.character character6 = gd.chara[mainP];
                    character6.doa = Setting.Doa.doa_Fumei;
                    gd.chara[mainP] = character6;
                    gd.RemakePeopleFlg();
                    return true;
                }, (float e) => true, false));
                Plugin.CheckLocationsInScript(704, 203);
                gd.baseData.gainExp += 100U;
                sp.WaitSec(2f, true);
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioRemnantText(0, 60, -1), 1, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.WaitSec(0.4f, true);
                gd.forwardNext = true;
            };
            __instance.actions[9] = action;
        }


        [HarmonyPatch(typeof(Remnant2Scenario), "SetParam")]
        [HarmonyPostfix]
        static void Remnan2(Remnant2Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[1];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                ScenarioContents.ChangeSceOnFlg(ref gd, ref sd, 2U);
                sd.flg |= 16384;
                GameData.character character2 = gd.chara[ad.mainP];
                character2.scenarioFlg |= 1;
                gd.chara[ad.mainP] = character2;
                character2 = gd.chara[ad.targetP];
                character2.scenarioFlg |= 1;
                gd.chara[ad.targetP] = character2;
                sp.LoadTexture("p08a");
                sp.WaitSec(0.45f, true);
                sp.WaitLoad();
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Interface, 50U, true, false, -1);
                    sp.ChangeCharaTexture(8U, "p08a", 10U, 20U, true);
                    sp.m_sb[50001U].SetFade(0.3f, 0f, 0, -1f, -1, false);
                    sp.m_sb[50002U].SetFade(0.3f, 0f, 0, -1f, -1, false);
                    return true;
                }, (float e) => true, false));
                int num = 1;
                sp.ShowChara(ad.counterP, 4, num, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4((float)sp.m_rs.m_displaySize.width * 0.25f * (float)num, 80f, (float)sp.m_rs.m_displaySize.width * 0.5f, (float)sp.m_rs.m_displaySize.height * 0.5f), 0f, 1f, true, null, true);
                sp.WaitFade(new List<uint> { 50001U, 50002U }, true, true);
                sp.PlayBgmInScript("bgm03", 0f, 0.85f, -1, true);
                sp.WaitSec(0.2f, true);
                List<string> list = Util.Split(sp.m_rs.GetScenarioRemnantText(1, 2, 4), new char[] { '|' });
                string text = list[0];
                list[0] = text;
                sp.SetNormalSerifu(ad.counterP, -1, 1, list, false, true, true, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list = Util.Split(sp.m_rs.GetScenarioRemnantText(1, 3, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.counterP, 2, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioRemnantText(1, 4, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, -1, 1, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioRemnantText(1, 5, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.counterP, 2, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioRemnantText(1, 6, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 0, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioRemnantText(1, 7, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 2, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioRemnantText(1, 8, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, ad.targetP, 1, list, true, false, false, true);
                sp.FadeBgmInScript(0.2f, 0.85f, 1f, false, -1);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_LeviMes, 35U, true, false, -1);
                    return true;
                }, (float e) => true, true));
                sp.WaitSec(0.4f, false);
                list = Util.Split(sp.m_rs.GetScenarioRemnantText(1, 9, -1), new char[] { '|' });
                sp.SetNormalSerifu(-2, ad.counterP, -1, list, true, false, true, true);
                list = Util.Split(sp.m_rs.GetScenarioRemnantText(1, 10, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 0, list, true, false, false, true);
                sp.FadeBgmInScript(-1f, 0f, 3f, true, -1);
                list = Util.Split(sp.m_rs.GetScenarioRemnantText(1, 11, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 2, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioRemnantText(1, 12, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 0, list, true, false, false, true);
                sp.PlayBgmInScript("bgm23", 1f, 0.6f, -1, true);
                list = Util.Split(sp.m_rs.GetScenarioRemnantText(1, 13, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 2, list, true, false, false, true);
                sp.RemoveScreenInScript(35U);
                list = Util.Split(sp.m_rs.GetScenarioRemnantText(1, 14, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 0, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioRemnantText(1, 15, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 0, list, true, true, false, true);
                list = Util.Split(sp.m_rs.GetScenarioRemnantText(1, 16, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 2, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioRemnantText(1, 17, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, -1, 1, list, false, false, false, true);
                sp.PlayBgmInScript("bgm05", 0f, 0.85f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list = Util.Split(sp.m_rs.GetScenarioRemnantText(1, 18, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.counterP, 0, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioRemnantText(1, 19, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, ad.mainP, 1, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioRemnantText(1, 20, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.counterP, 2, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioRemnantText(1, 21, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.counterP, 0, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioRemnantText(1, 22, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, -1, 1, list, true, false, false, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_LeviMes, 35U, true, false, -1);
                    return true;
                }, (float e) => true, true));
                sp.WaitSec(0.4f, false);
                list = Util.Split(sp.m_rs.GetScenarioRemnantText(1, 23, -1), new char[] { '|' });
                sp.SetNormalSerifu(-2, ad.counterP, -1, list, true, false, true, true);
                list = Util.Split(sp.m_rs.GetScenarioRemnantText(1, 24, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, -1, 1, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioRemnantText(1, 25, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 2, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioRemnantText(1, 26, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 0, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioRemnantText(1, 27, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.counterP, 0, list, true, true, false, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 3.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 35U, 50U }, 40002U, 1.4f, 0, false, true, true);
                sp.UnloadTexture("p08a");
                sp.UnloadPlace();
                sp.WaitSec(1f, true);
                int mainP = ad.mainP;
                Plugin.CheckLocationsInScript(702);
                gd.baseData.gainExp += 50U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(0.4f, true);
                gd.forwardNext = true;
            };
            __instance.actions[1] = action;
        }


        [HarmonyPatch(typeof(Remnant3Scenario), "SetParam")]
        [HarmonyPostfix]
        static void HopeForTheFuture(Remnant3Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[14];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                List<string> list13 = Util.Split(sp.m_rs.GetScenarioRemnantText(2, 51, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list13, false, true, false, true);
                sp.LoadTexture("ivep07_01_2");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list13 = Util.Split(sp.m_rs.GetScenarioRemnantText(2, 52, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list13, true, true, true, true);
                sp.WaitSec(0.2f, true);
                list13 = Util.Split(sp.m_rs.GetScenarioRemnantText(2, 53, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list13, true, true, false, true);
                if ((sd.flg & 4096) > 0)
                {
                    sp.FadeBgmInScript(-1f, 1f, 1f, false, -1);
                    sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.4f, 0, false, false, true);
                    sp.UnvisibleAllChara(20U, -1);
                    sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                    {
                        sp.SetCharaSingleTexture(10200, "ivep07_01_2", 1U, 0f, 20U);
                        return true;
                    }, (float e) => true, false));
                    sp.WaitFade(new List<uint> { 30U }, true, true);
                    list13 = Util.Split(sp.m_rs.GetScenarioRemnantText(2, 54, 6), new char[] { '|' });
                    sp.SetNormalSerifu(ad.mainP, 0, 1, list13, true, true, true, true);
                }
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 1.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.UnloadTexture("ivep07_01_0");
                sp.UnloadTexture("ivep07_01_1");
                sp.UnloadTexture("ivep07_01_2");
                if (!ArchipelagoClient.ServerData.CheckedLocations.Contains(705)) //Changed condition
                {
                    sp.WaitSec(0.7f, true);
                    int mainP = ad.mainP;
                    Plugin.CheckLocationsInScript(705);
                    gd.baseData.gainExp += 50U;
                }
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(0.4f, true);
                gd.forwardNext = true;
            };
            __instance.actions[14] = action;
        }


        [HarmonyPatch(typeof(Setsu1Scenario), "SetParam")]
        [HarmonyPostfix]
        static void Exaggerate(Setsu1Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[4];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.FadeBgmInScript(-1f, 0f, 1f, true, -1);
                List<string> list2 = Util.Split(sp.m_rs.GetScenarioSetsuText(0, 11, 0), new char[] { '|' });
                string text2 = list2[0];
                Util.Replace(ref text2, "{0}", gd.takashiName);
                list2[0] = text2;
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 2, list2, false, false, false, true);
                sp.WaitText(50U, "test", false);
                sp.PlayBgmInScript("bgm03", 0f, 1f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list2 = Util.Split(sp.m_rs.GetScenarioSetsuText(0, 12, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 1, list2, false, false, false, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.m_sb[50U].m_textAreaMap["test"].nowLine >= 1, true));
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.25f, 0, false, false, true);
                sp.UnvisibleAllChara(20U, -1);
                sp.ShowChara(ad.targetP, 6, 1, 20U, false);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list2 = Util.Split(sp.m_rs.GetScenarioSetsuText(0, 13, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 2, list2, true, false, false, true);
                list2 = Util.Split(sp.m_rs.GetScenarioSetsuText(0, 14, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, ad.targetP, 0, list2, true, false, false, true);
                list2 = Util.Split(sp.m_rs.GetScenarioSetsuText(0, 15, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 1, list2, true, false, false, true);
                list2 = Util.Split(sp.m_rs.GetScenarioSetsuText(0, 16, 2), new char[] { '|' });
                text2 = list2[0];
                Util.Replace(ref text2, "{0}", gd.takashiName);
                list2[0] = text2;
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 2, list2, true, false, false, true);
                list2 = Util.Split(sp.m_rs.GetScenarioSetsuText(0, 17, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 1, list2, true, false, false, true);
                sp.WaitSec(0.05f, false);
                sp.PlaySeInScript("se_ashioto_02", 1f);
                sp.FadeBgmInScript(-1f, 0.4f, 1.5f, false, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 30U, 0.4f, 0, false, true, true);
                sp.UnloadPlace();
                sp.UnloadTexture("p05a");
                sp.LoadPlace(31, false);
                sp.WaitLoad();
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(1.5f, true);
                sp.StopAllSeInScript();
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Interface, 50U, true, false, -1);
                    sp.SetColorScreen(255U, 50000U, -1);
                    return true;
                }, (float e) => true, true));
                sp.ShowChara(ad.mainP, 0, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(120f, 40f, 720f, 405f), 0f, 1f, true, null, true);
                sp.SetFadeScreen(new List<uint> { 50000U }, 50001U, 0.4f, 0, true, true, true);
                sp.WaitSec(0.2f, true);
                list2 = Util.Split(sp.m_rs.GetScenarioSetsuText(0, 18, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list2, false, true, true, true);
                sp.WaitSec(0.4f, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list2 = Util.Split(sp.m_rs.GetScenarioSetsuText(0, 19, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list2, true, true, false, true);
                list2 = Util.Split(sp.m_rs.GetScenarioSetsuText(0, 20, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list2, false, true, false, true);
                sp.WaitText(50U, "test", true);
                sp.FadeBgmInScript(0f, 0.4f, 2f, false, -1);
                Plugin.CheckLocationsInScript(13);
                gd.baseData.gainExp += 50U;
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioSetsuText(0, 22, -1), 3, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.HideInterface(50U, true);
                sp.FadeBgmInScript(-1f, 0.8f, 1.5f, false, -1);
                sp.WaitSec(0.4f, true);
                list2 = Util.Split(sp.m_rs.GetScenarioSetsuText(0, 23, 3), new char[] { '|' });
                text2 = list2[0];
                Util.Replace(ref text2, "{0}", gd.takashiName);
                list2[0] = text2;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list2, true, false, false, true);
                list2 = Util.Split(sp.m_rs.GetScenarioSetsuText(0, 24, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list2, true, true, false, true);
                list2 = Util.Split(sp.m_rs.GetScenarioSetsuText(0, 25, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list2, true, true, false, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 1.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 0.8f, 0, false, true, true);
                sp.UnloadPlace();
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                gd.forwardNext = true;
            };
            __instance.actions[4] = action;
        }


        [HarmonyPatch(typeof(Setsu2Scenario), "SetParam")]
        [HarmonyPostfix]
        static void Setsu2(Setsu2Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[4];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                List<string> list4 = Util.Split(sp.m_rs.GetScenarioSetsuText(1, 13, 7), new char[] { '|' });
                string text4 = list4[0];
                Util.Replace(ref text4, "{0}", gd.takashiName);
                list4[0] = text4;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list4, true, (gd.actionFlg & 4UL) != 0UL, false, true);
                list4 = Util.Split(sp.m_rs.GetScenarioSetsuText(1, 14, 0), new char[] { '|' });
                text4 = list4[0];
                Util.Replace(ref text4, "{0}", gd.takashiName);
                list4[0] = text4;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list4, true, true, false, true);
                if (gd.baseData.loop == 61)
                {
                    list4 = Util.Split(sp.m_rs.GetScenarioSetsuText(1, 15, 7), new char[] { '|' });
                    text4 = list4[0];
                    Util.Replace(ref text4, "{0}", gd.takashiName);
                    list4[0] = text4;
                    text4 = list4[0];
                    Util.Replace(ref text4, "{1}", gd.baseData.loop.ToString());
                    list4[0] = text4;
                    sp.SetNormalSerifu(ad.mainP, 0, 1, list4, true, true, false, true);
                    list4 = Util.Split(sp.m_rs.GetScenarioSetsuText(1, 16, 4), new char[] { '|' });
                    text4 = list4[0];
                    Util.Replace(ref text4, "{0}", gd.takashiName);
                    list4[0] = text4;
                    sp.SetNormalSerifu(ad.mainP, 0, 1, list4, true, true, false, true);
                    list4 = Util.Split(sp.m_rs.GetScenarioSetsuText(1, 17, 7), new char[] { '|' });
                    text4 = list4[0];
                    Util.Replace(ref text4, "{0}", gd.takashiName);
                    list4[0] = text4;
                    sp.SetNormalSerifu(ad.mainP, 0, 1, list4, true, true, false, true);
                }
                else
                {
                    list4 = Util.Split(sp.m_rs.GetScenarioSetsuText(1, 18, 2), new char[] { '|' });
                    text4 = list4[0];
                    Util.Replace(ref text4, "{0}", gd.takashiName);
                    list4[0] = text4;
                    text4 = list4[0];
                    Util.Replace(ref text4, "{1}", gd.baseData.loop.ToString());
                    list4[0] = text4;
                    sp.SetNormalSerifu(ad.mainP, 0, 1, list4, true, true, false, true);
                    list4 = Util.Split(sp.m_rs.GetScenarioSetsuText(1, 19, 7), new char[] { '|' });
                    text4 = list4[0];
                    Util.Replace(ref text4, "{0}", gd.takashiName);
                    list4[0] = text4;
                    sp.SetNormalSerifu(ad.mainP, 0, 1, list4, true, true, false, true);
                }
                list4 = Util.Split(sp.m_rs.GetScenarioSetsuText(1, 20, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list4, true, true, false, true);
                list4 = Util.Split(sp.m_rs.GetScenarioSetsuText(1, 21, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list4, true, true, false, true);
                list4 = Util.Split(sp.m_rs.GetScenarioSetsuText(1, 22, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list4, true, true, true, true);
                list4 = Util.Split(sp.m_rs.GetScenarioSetsuText(1, 23, 7), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list4, true, true, false, true);
                sp.WaitSec(0.4f, true);
                list4 = Util.Split(sp.m_rs.GetScenarioSetsuText(1, 24, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list4, true, true, false, true);
                list4 = Util.Split(sp.m_rs.GetScenarioSetsuText(1, 25, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list4, true, true, false, true);
                sp.FadeBgmInScript(-1f, 0.4f, 2f, false, -1);
                sp.WaitSec(0.05f, false);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.UnloadTexture("p11a");
                sp.WaitSec(0.7f, true);
                int mainP = ad.mainP;
                Plugin.CheckLocationsInScript(1102);
                gd.baseData.gainExp += 50U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(0.8f, true);
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioSetsuText(1, 27, -1), 1, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.FadeBgmInScript(-1f, 0f, 2f, true, -1);
                sp.WaitSec(0.5f, true);
                gd.forwardNext = true;
            };
            __instance.actions[4] = action;
        }


        [HarmonyPatch(typeof(Setsu6Scenario), "SetParam")]
        [HarmonyPostfix]
        static void LetsPlay(Setsu6Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[11];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                List<string> list11;
                if ((gd.actionFlg & 16UL) > 0UL)
                {
                    list11 = Util.Split(sp.m_rs.GetScenarioSetsuText(4, 102, 6), new char[] { '|' });
                    sp.SetNormalSerifu(ad.mainP, -1, 1, list11, true, true, false, true);
                    list11 = Util.Split(sp.m_rs.GetScenarioSetsuText(4, 103, 1), new char[] { '|' });
                    sp.SetNormalSerifu(ad.mainP, 0, 1, list11, true, true, false, true);
                }
                else
                {
                    list11 = Util.Split(sp.m_rs.GetScenarioSetsuText(4, 104, 6), new char[] { '|' });
                    sp.SetNormalSerifu(ad.mainP, -1, 1, list11, true, true, false, true);
                    list11 = Util.Split(sp.m_rs.GetScenarioSetsuText(4, 105, 7), new char[] { '|' });
                    sp.SetNormalSerifu(ad.mainP, 0, 1, list11, true, true, false, true);
                    list11 = Util.Split(sp.m_rs.GetScenarioSetsuText(4, 106, 1), new char[] { '|' });
                    sp.SetNormalSerifu(ad.mainP, 0, 1, list11, true, true, false, true);
                }
                sp.FadeBgmInScript(0.25f, 0.9f, 1.2f, false, -1);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_LeviMes, 35U, true, false, -1);
                    return true;
                }, (float e) => true, true));
                sp.WaitSec(0.4f, true);
                list11 = Util.Split(sp.m_rs.GetScenarioSetsuText(4, 107, -1), new char[] { '|' });
                sp.SetNormalSerifu(-2, -1, -1, list11, true, false, true, true);
                list11 = Util.Split(sp.m_rs.GetScenarioSetsuText(4, 108, 6), new char[] { '|' });
                string text6 = list11[0];
                Util.Replace(ref text6, "{0}", gd.takashiName);
                list11[0] = text6;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list11, true, true, false, true);
                list11 = Util.Split(sp.m_rs.GetScenarioSetsuText(4, 109, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list11, true, true, false, true);
                sp.SetFadeScreen(new List<uint> { 0U, 35U, 20U }, 30U, 0.2f, 0, false, true, true);
                sp.LoadTexture("ivep11_00_5");
                sp.WaitLoad();
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.FadeBgmInScript(-1f, 1f, 1f, false, -1);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, true, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep11_00_5", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, false));
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.4f, 0, false, true, true);
                sp.WaitFade(new List<uint> { 31U }, true, true);
                sp.WaitSec(0.2f, true);
                list11 = Util.Split(sp.m_rs.GetScenarioSetsuText(4, 110, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list11, true, true, true, false);
                sp.WaitSec(0.4f, true);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.2f, 0, true, true, true);
                sp.UnloadTexture("ivep11_00_5");
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.ChangeCharaTexture(11U, "p11a", 10U, 20U, true);
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, false));
                sp.ShowChara(ad.mainP, 1, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(180f, 60f, 600f, 337.5f), 0f, 1f, true, null, true);
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.4f, 0, false, true, true);
                sp.WaitFade(new List<uint> { 31U }, true, true);
                list11 = Util.Split(sp.m_rs.GetScenarioSetsuText(4, 111, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list11, true, true, true, true);
                sp.WaitSec(0.4f, true);
                list11 = Util.Split(sp.m_rs.GetScenarioSetsuText(4, 112, 7), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list11, true, true, false, true);
                sp.WaitSec(0.6f, true);
                sp.StopBgmInScript(-1, false);
                sp.PlaySeInScript("se_gatyan", 0.6f);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 30U, 0.1f, 0, false, true, true);
                sp.WaitSec(0.1f, true);
                sp.PlaySeInScript("se_pusyu", 1f);
                sp.UnloadPlace();
                sp.UnloadTexture("p11a");
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(2f, true);
                int mainP = ad.mainP;
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    GameData.character character = gd.chara[0];
                    character.doa = Setting.Doa.doa_Shokei;
                    gd.chara[0] = character;
                    character = gd.chara[mainP];
                    character.doa = Setting.Doa.doa_Shokei;
                    gd.chara[mainP] = character;
                    gd.RemakePeopleFlg();
                    return true;
                }, (float e) => true, false));
                Plugin.CheckLocationsInScript(1105);
                gd.baseData.gainExp += 250U;
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(0.25f, true);
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioSetsuText(4, 114, -1), 1, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                gd.forwardNext = true;
            };
            __instance.actions[11] = action;
        }


        [HarmonyPatch(typeof(Setsu8Scenario), "SetParam")]
        [HarmonyPostfix]
        static void SetsusOrigins(Setsu8Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[1];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.WaitSec(0.2f, true);
                List<string> list = Util.Split(sp.m_rs.GetScenarioSetsuText(7, 2, 5), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, 1, list, false, true, true, true);
                sp.PlayBgmInScript("bgm03", 0f, 0.85f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.PlaySeInScript("se_ashioto_02", 0.7f);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.4f, 4, true, true, true);
                sp.WaitSec(0.6f, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, true));
                sp.ShowChara(sd.mainP, 0, 2, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(360f, 60f, 600f, 337.5f), 0f, 1f, true, null, true);
                sp.StopAllSeInScript();
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.4f, 4, true, true, true);
                sp.WaitSec(0.2f, true);
                list = Util.Split(sp.m_rs.GetScenarioSetsuText(7, 3, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 2, list, true, true, true, true);
                sp.WaitSec(0.4f, true);
                sp.FadeBgmInScript(-1f, 0f, 1.4f, true, -1);
                list = Util.Split(sp.m_rs.GetScenarioSetsuText(7, 4, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 2, list, false, true, false, true);
                sp.LoadTexture("base_bg");
                sp.WaitLoad();
                sp.LoadTexture("loop");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.WaitSec(0.05f, false);
                sp.PlaySeInScript("se_noiseA", 1f);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetColorScreen(255U, 50000U, -1);
                    sp.SetScreen(Setting.Screen.s_Loop, 60U, true, false, -1);
                    return true;
                }, (float e) => true, true));
                sp.SetFadeScreen(new List<uint> { 50000U }, 50001U, 0.1f, 7, true, true, true);
                sp.RemoveScreenInScript(0U);
                sp.RemoveScreenInScript(20U);
                sp.UnloadPlace();
                sp.LoadPlace(5, true);
                sp.WaitSec(1.4f, true);
                sp.WaitLoad();
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    return true;
                }, (float e) => true, false));
                sp.PlaySeInScript("se_noiseB", 1f);
                sp.SetFadeScreen(new List<uint> { 60U }, 50001U, 0.2f, 7, true, true, true);
                sp.UnloadTexture("base_bg");
                sp.UnloadTexture("loop");
                sp.PlayBgmInScript("bgm01", 1f, 0.7f, -1, true);
                sp.WaitSec(0.4f, true);
                list = Util.Split(sp.m_rs.GetScenarioSetsuText(7, 5, 5), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, 1, list, true, true, true, true);
                list = Util.Split(sp.m_rs.GetScenarioSetsuText(7, 6, 0), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[13], -1, 2, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioSetsuText(7, 7, 2), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[4], -1, 0, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioSetsuText(7, 8, 4), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[5], -1, 1, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioSetsuText(7, 9, 5), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, 1, list, true, false, true, true);
                list = Util.Split(sp.m_rs.GetScenarioSetsuText(7, 10, 4), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[3], -1, 2, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioSetsuText(7, 11, 4), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[9], ad.mainP, 0, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioSetsuText(7, 12, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, gd.personFromId[9], 1, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioSetsuText(7, 13, 5), new char[] { '|' });
                sp.SetNormalSerifu(gd.personFromId[3], -1, 2, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioSetsuText(7, 14, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioSetsuText(7, 15, 5), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, 1, list, false, false, true, true);
                if (!ArchipelagoClient.ServerData.CheckedLocations.Contains(1104))
                {
                    sp.WaitText(50U, "test", true);
                    sp.HideInterface(50U, true);
                    sp.WaitSec(0.01f, false);
                    int mainP = ad.mainP;
                    Plugin.CheckLocationsInScript(1104);
                    sp.ShowInfoUpdateMes(sp.m_rs.GetScenarioSetsuText(7, 16, -1), 45002U, 0, true);
                    gd.baseData.gainExp += 50U;
                    gd.forwardNext = true;
                }
            };
            __instance.actions[1] = action;
            action = __instance.actions[17];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.FadeBgmInScript(-1f, 0f, 3f, true, -1);
                sp.PlaySeInScript("se_hikaruball", 0.5f);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetColorScreen(uint.MaxValue, 30U, -1);
                    sp.m_sb[30U].SetFadeIn(0.25f, 0);
                    return true;
                }, (float e) => true, true));
                sp.LoadTexture("ivep11_02_05");
                sp.WaitLoad();
                sp.WaitFade(new List<uint> { 30U }, false, true);
                sp.RemoveScreenInScript(0U);
                sp.UnloadTexture("ivep11_02_02_3");
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, true, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep11_02_05", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, true));
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.8f, 0, true, true, true);
                List<string> list12 = Util.Split(sp.m_rs.GetScenarioSetsuText(7, 117, 1), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, 1, list12, false, true, true, false);
                sp.LoadTexture("ivep11_02_05_1");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.SetCopyScreen(new List<uint> { 0U }, 30U, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, true, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep11_02_05", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 10U, "ivep11_02_05_1", new Vector2?(new Vector2(242.25f, 12f)), null);
                    sp.m_sb[0U].m_spriteMap[10U].SetVisible(true);
                    return true;
                }, (float e) => true, true));
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.4f, 0, true, true, true);
                list12 = Util.Split(sp.m_rs.GetScenarioSetsuText(7, 118, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 1, list12, true, true, true, false);
                sp.PlayBgmInScript("bgm11", 0f, 0.6f, -1, true);
                sp.WaitSec(0.2f, true);
                list12 = Util.Split(sp.m_rs.GetScenarioSetsuText(7, 119, 1), new char[] { '|' });
                sp.SetNormalSerifu(0, ad.targetP, 1, list12, false, true, true, false);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.PlaySeInScript("se_hikaruball", 0.35f);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetColorScreen(uint.MaxValue, 47U, -1);
                    sp.m_sb[47U].SetFade(1f, 0.25f, 0, 0f, -1, false);
                    sp.SetScreen(Setting.Screen.s_LightBall, 45U, false, false, -1);
                    return true;
                }, (float e) => true, true));
                sp.WaitFade(new List<uint> { 47U }, false, true);
                list12 = Util.Split(sp.m_rs.GetScenarioSetsuText(7, 120, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 1, list12, true, true, true, false);
                sp.FadeBgmInScript(-1f, 1f, 2f, false, -1);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.m_sb[47U].SetFade(2f, 1f, 0, 0.25f, -1, false);
                    return true;
                }, (float e) => true, true));
                sp.PlaySeInScript("se_hikaruball", 1f);
                sp.WaitSec(0.4f, true);
                list12 = Util.Split(sp.m_rs.GetScenarioSetsuText(7, 121, 1), new char[] { '|' });
                sp.SetNormalSerifu(0, ad.targetP, 1, list12, false, true, true, false);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.WaitFade(new List<uint> { 47U }, false, true);
                sp.WaitSec(2f, true);
                sp.RemoveScreenInScript(0U);
                sp.WaitSec(0.01f, false);
                sp.RemoveScreenInScript(45U);
                sp.WaitSec(0.01f, false);
                sp.UnloadTexture("ivep11_02_05");
                sp.UnloadTexture("ivep11_02_05_1");
                sp.WaitSec(0.01f, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, true, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep11_02_02", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, true));
                sp.SetFadeScreen(new List<uint> { 47U }, 48U, 0.8f, 0, true, true, true);
                sp.WaitSec(0.4f, true);
                list12 = Util.Split(sp.m_rs.GetScenarioSetsuText(7, 122, 1), new char[] { '|' });
                sp.SetNormalSerifu(0, ad.targetP, 1, list12, false, true, true, false);
                sp.LoadTexture("ivep11_02_07");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list12 = Util.Split(sp.m_rs.GetScenarioSetsuText(7, 123, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 1, list12, true, true, true, false);
                sp.SetCopyScreen(new List<uint> { 0U }, 30U, true);
                sp.UnloadTexture("ivep11_02_02");
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, true, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep11_02_07", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, true));
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.8f, 0, true, true, true);
                sp.WaitSec(0.4f, true);
                list12 = Util.Split(sp.m_rs.GetScenarioSetsuText(7, 124, 1), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, 1, list12, true, true, true, false);
                sp.FadeBgmInScript(-1f, 0f, 0.6f, true, -1);
                sp.WaitSec(0.4f, true);
                sp.PlaySeInScript("se_okiagari", 1f);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.25f, 0, true, true, true);
                sp.UnloadTexture("ivep11_02_07");
                sp.WaitSec(1f, true);
                list12 = Util.Split(sp.m_rs.GetScenarioSetsuText(7, 125, 1), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, 1, list12, true, true, true, false);
                sp.WaitSec(0.05f, false);
                sp.RemoveScreenInScript(50U);
                sp.WaitSec(2f, true);
                int targetP = ad.targetP;
                Plugin.CheckLocationsInScript(1106);
                gd.baseData.gainExp += 250U;
                sp.WaitSec(0.8f, true);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioSetsuText(7, 127, -1), 1, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.WaitSec(0.4f, true);
                gd.forwardNext = true;
            };
            __instance.actions[17] = action;
        }


        [HarmonyPatch(typeof(Setsu9Scenario), "SetParam")]
        [HarmonyPostfix]
        static void Setsu3(Setsu9Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[1];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.PlayBgmInScript("bgm01", 0f, 0.4f, -1, true);
                sp.WaitSec(0.4f, true);
                List<string> list = Util.Split(sp.m_rs.GetScenarioSetsuText(8, 2, 5), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, 1, list, true, true, true, true);
                if ((2 & gd.baseData.sce_ind_flg[sd.id]) == 0)
                {
                    list = Util.Split(sp.m_rs.GetScenarioSetsuText(8, 3, 6), new char[] { '|' });
                }
                else
                {
                    list = Util.Split(sp.m_rs.GetScenarioSetsuText(8, 4, 6), new char[] { '|' });
                }
                sp.SetNormalSerifu(ad.mainP, 0, 2, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioSetsuText(8, 5, 5), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, 1, list, true, false, true, true);
                sp.WaitSec(0.05f, false);
                if (!ArchipelagoClient.ServerData.CheckedLocations.Contains(1103)) //Changed condition
                {
                    int mainP = ad.mainP;
                    Plugin.CheckLocationsInScript(1103);
                    gd.baseData.gainExp += 50U;
                    sp.WaitSec(0.4f, true);
                }
                sp.FadeBgmInScript(-1f, 1f, 1.4f, false, -1);
                gd.forwardNext = true;
            };
            __instance.actions[1] = action;
        }


        [HarmonyPatch(typeof(Shamin1Scenario), "SetParam")]
        [HarmonyPostfix]
        static void AceInTheHole(Shamin1Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[3];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                List<string> list3 = Util.Split(sp.m_rs.GetScenarioShaminText(0, 12, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list3, true, true, false, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 1.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                if (!ArchipelagoClient.ServerData.CheckedLocations.Contains(1304)) //Changed condition
                {
                    sp.WaitSec(0.5f, true);
                    int mainP = ad.mainP;
                    Plugin.CheckLocationsInScript(1304);
                    gd.baseData.gainExp += 50U;
                }
                sp.WaitFade(new List<uint> { 40002U }, true, true);
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
                List<string> list4 = Util.Split(sp.m_rs.GetScenarioShaminText(0, 15, 0), new char[] { '|' });
                sp.SetNormalSerifu(0, ad.targetP, 1, list4, true, false, true, true);
                list4 = Util.Split(sp.m_rs.GetScenarioShaminText(0, 16, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list4, true, false, false, true);
                list4 = Util.Split(sp.m_rs.GetScenarioShaminText(0, 17, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list4, true, true, false, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 1.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                if (!ArchipelagoClient.ServerData.CheckedLocations.Contains(1304)) //Changed condition
                {
                    sp.WaitSec(0.5f, true);
                    int targetP = ad.targetP;
                    Plugin.CheckLocationsInScript(1304);
                }
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                gd.forwardNext = true;
            };
            __instance.actions[4] = action;
            action = __instance.actions[5];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
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
                Plugin.CheckLocationsInScript(19);
                gd.baseData.gainExp += 50U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioShaminText(0, 27, -1), 3, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.WaitSec(0.2f, true);
                if (!ArchipelagoClient.ServerData.CheckedLocations.Contains(1304)) //Changed condition
                {
                    int targetP = ad.targetP;
                    Plugin.CheckLocationsInScript(1304);
                    sp.WaitSec(0.2f, true);
                }
                gd.forwardNext = true;
            };
            __instance.actions[5] = action;
        }


        [HarmonyPatch(typeof(Shamin2Scenario), "SetParam")]
        [HarmonyPostfix]
        static void Shaming2Otome3(Shamin2Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[1];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                ScenarioContents.ChangeSceOnFlg(ref gd, ref sd, 2U);
                sd.flg |= 16384;
                sp.WaitSec(0.45f, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Interface, 50U, true, false, -1);
                    sp.m_sb[50001U].SetFade(0.3f, 0f, 0, -1f, -1, false);
                    sp.m_sb[50002U].SetFade(0.3f, 0f, 0, -1f, -1, false);
                    return true;
                }, (float e) => true, false));
                int num = 1;
                sp.ShowChara(ad.targetP, 5, num, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4((float)sp.m_rs.m_displaySize.width * 0.25f * (float)num, 80f, (float)sp.m_rs.m_displaySize.width * 0.5f, (float)sp.m_rs.m_displaySize.height * 0.5f), 0f, 1f, true, null, true);
                sp.WaitFade(new List<uint> { 50001U, 50002U }, true, true);
                sp.WaitSec(0.2f, true);
                List<string> list = Util.Split(sp.m_rs.GetScenarioShaminText(1, 2, 0), new char[] { '|' });
                string text = list[0];
                list[0] = text;
                sp.SetNormalSerifu(ad.targetP, -1, num, list, false, true, true, true);
                sp.WaitSec(0.2f, true);
                sp.PlayBgmInScript("bgm02", 0f, 0.75f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list = Util.Split(sp.m_rs.GetScenarioShaminText(1, 3, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, ad.targetP, 2, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioShaminText(1, 4, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 0, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioShaminText(1, 5, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 1, list, true, false, false, true);
                sp.FadeBgmInScript(-1f, 0f, 2.5f, true, -1);
                list = Util.Split(sp.m_rs.GetScenarioShaminText(1, 6, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 0, list, false, false, false, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.PlayBgmInScript("bgm17", 0f, 0.6f, -1, true);
                list = Util.Split(sp.m_rs.GetScenarioShaminText(1, 7, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, ad.mainP, 2, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioShaminText(1, 8, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 1, list, true, false, false, true);
                sp.FadeBgmInScript(-1f, 0.8f, 1.2f, false, -1);
                list = Util.Split(sp.m_rs.GetScenarioShaminText(1, 9, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 0, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioShaminText(1, 10, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 1, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioShaminText(1, 11, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, ad.mainP, 2, list, true, false, false, true);
                sp.FadeBgmInScript(-1f, 1f, 1.2f, false, -1);
                list = Util.Split(sp.m_rs.GetScenarioShaminText(1, 12, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 0, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioShaminText(1, 13, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 0, list, true, true, false, true);
                list = Util.Split(sp.m_rs.GetScenarioShaminText(1, 14, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 1, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioShaminText(1, 15, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 0, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioShaminText(1, 16, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, ad.mainP, 2, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioShaminText(1, 17, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.counterP, 0, list, true, false, false, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 1f, true, -1);
                sp.PlaySeInScript("se_ashioto_03", 1f);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetColorScreen(255U, 30U, -1);
                    sp.m_sb[30U].SetFade(0.5f, 1f, 0, 0f, -1, false);
                    return true;
                }, (float e) => true, false));
                sp.WaitFade(new List<uint> { 30U }, false, true);
                sp.WaitSec(0.7f, true);
                sp.UnvisibleAllChara(20U, -1);
                sp.ShowChara(ad.targetP, 5, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(180f, 60f, 600f, 337.5f), 0f, 1f, true, null, true);
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.6f, 0, true, true, true);
                sp.WaitSec(0.6f, true);
                list = Util.Split(sp.m_rs.GetScenarioShaminText(1, 18, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, ad.targetP, 2, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioShaminText(1, 19, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.counterP, 1, list, false, false, false, true);
                sp.WaitSec(0.4f, true);
                sp.PlayBgmInScript("bgm18", 1.5f, 0.85f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list = Util.Split(sp.m_rs.GetScenarioShaminText(1, 20, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.counterP, 1, list, true, true, false, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 3.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1.4f, 0, false, true, true);
                sp.UnloadPlace();
                sp.WaitSec(1f, true);
                int mainP = ad.mainP;
                int targetP = ad.targetP;
                Plugin.CheckLocationsInScript(1302, 1203);
                gd.baseData.gainExp += 100U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(0.4f, true);
                gd.forwardNext = true;
            };
            __instance.actions[1] = action;
        }


        [HarmonyPatch(typeof(Shamin3Scenario), "SetParam")]
        [HarmonyPostfix]
        static void SmallTalk(Shamin3Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[7];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                List<string> list7;
                if ((gd.actionFlg & 4UL) == 0UL)
                {
                    list7 = Util.Split(sp.m_rs.GetScenarioShaminText(2, 18, 6), new char[] { '|' });
                    sp.SetNormalSerifu(ad.mainP, 0, 1, list7, true, false, false, true);
                }
                list7 = Util.Split(sp.m_rs.GetScenarioShaminText(2, 19, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list7, true, false, false, true);
                list7 = Util.Split(sp.m_rs.GetScenarioShaminText(2, 20, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list7, true, true, false, true);
                list7 = Util.Split(sp.m_rs.GetScenarioShaminText(2, 21, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list7, true, true, false, true);
                sp.FadeBgmInScript(-1f, 0.6f, 0.4f, false, -1);
                sp.WaitSec(0.05f, false);
                Plugin.CheckLocationsInScript(8);
                gd.baseData.gainExp += 50U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioShaminText(2, 23, -1), 3, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.WaitSec(0.2f, true);
                sp.FadeBgmInScript(-1f, 0f, 1.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                gd.forwardNext = true;
            };
            __instance.actions[7] = action;
        }


        [HarmonyPatch(typeof(Shamin4Scenario), "SetParam")]
        [HarmonyPostfix]
        static void ShamingsPromise(Shamin4Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[18];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.LoadPlace(6, true);
                sp.WaitLoad();
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioShaminText(3, 56, -1), 2, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.WaitSec(0.4f, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Interface, 50U, true, false, -1);
                    sp.SetColorScreen(255U, 40U, -1);
                    return true;
                }, (float e) => true, true));
                sp.ShowChara(ad.mainP, 0, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(120f, 40f, 720f, 405f), 0f, 1f, true, null, true);
                sp.SetFadeScreen(new List<uint> { 40U }, 41U, 0.4f, 0, true, true, true);
                sp.WaitSec(0.25f, true);
                List<string> list13 = Util.Split(sp.m_rs.GetScenarioShaminText(3, 57, 0), new char[] { '|' });
                string text10 = list13[0];
                list13[0] = text10;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list13, false, true, true, true);
                sp.WaitText(50U, "test", false);
                sp.PlayBgmInScript("bgm15", 0f, 0.75f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list13 = Util.Split(sp.m_rs.GetScenarioShaminText(3, 58, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list13, true, true, false, true);
                sp.FadeBgmInScript(-1f, 0.5f, 1.2f, false, -1);
                list13 = Util.Split(sp.m_rs.GetScenarioShaminText(3, 59, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list13, true, false, false, true);
                sp.FadeBgmInScript(-1f, 0f, 2.5f, true, -1);
                list13 = Util.Split(sp.m_rs.GetScenarioShaminText(3, 60, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list13, true, true, false, true);
                sp.LoadTexture("ivep13_01_4");
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetColorScreen(255U, 40U, -1);
                    sp.m_sb[40U].SetFade(1.4f, 1f, 0, 0f, -1, false);
                    return true;
                }, (float e) => true, false));
                list13 = Util.Split(sp.m_rs.GetScenarioShaminText(3, 61, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list13, false, true, false, true);
                sp.WaitFade(new List<uint> { 40U }, false, true);
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.RemoveScreenInScript(0U);
                sp.RemoveScreenInScript(20U);
                sp.WaitSec(0.6f, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep13_01_4", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, true));
                list13 = Util.Split(sp.m_rs.GetScenarioShaminText(3, 62, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list13, false, true, true, false);
                sp.PlayBgmInScript("bgm23", 2f, 0.8f, -1, true);
                sp.WaitSec(1f, true);
                sp.SetFadeScreen(new List<uint> { 40U }, 41U, 1.4f, 6, false, true, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.WaitFade(new List<uint> { 41U }, true, true);
                list13 = Util.Split(sp.m_rs.GetScenarioShaminText(3, 63, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list13, true, true, true, false);
                list13 = Util.Split(sp.m_rs.GetScenarioShaminText(3, 64, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list13, true, true, true, false);
                list13 = Util.Split(sp.m_rs.GetScenarioShaminText(3, 65, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list13, true, true, true, false);
                sp.SetCopyScreen(new List<uint> { 0U }, 40U, true);
                sp.UnloadTexture("ivep13_01_4");
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    return true;
                }, (float e) => true, false));
                sp.ShowChara(ad.mainP, 7, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(180f, 60f, 600f, 337.5f), 0f, 1f, true, null, true);
                sp.FadeBgmInScript(-1f, 0.45f, 2f, false, -1);
                sp.SetFadeScreen(new List<uint> { 40U }, 41U, 1f, 0, true, true, true);
                sp.WaitSec(0.25f, true);
                list13 = Util.Split(sp.m_rs.GetScenarioShaminText(3, 66, 7), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list13, true, true, true, true);
                list13 = Util.Split(sp.m_rs.GetScenarioShaminText(3, 67, 0), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, 1, list13, true, false, true, true);
                sp.FadeBgmInScript(-1f, 0f, 4f, true, -1);
                list13 = Util.Split(sp.m_rs.GetScenarioShaminText(3, 68, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list13, true, true, false, true);
                list13 = Util.Split(sp.m_rs.GetScenarioShaminText(3, 69, 0), new char[] { '|' });
                sp.SetNormalSerifu(0, ad.mainP, 1, list13, true, true, true, true);
                list13 = Util.Split(sp.m_rs.GetScenarioShaminText(3, 70, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list13, true, true, true, true);
                list13 = Util.Split(sp.m_rs.GetScenarioShaminText(3, 71, 0), new char[] { '|' });
                sp.SetNormalSerifu(0, ad.mainP, 1, list13, true, true, true, true);
                list13 = Util.Split(sp.m_rs.GetScenarioShaminText(3, 72, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list13, false, false, false, true);
                sp.PlayBgmInScript("bgm15", 0f, 1f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list13 = Util.Split(sp.m_rs.GetScenarioShaminText(3, 73, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list13, true, true, false, true);
                list13 = Util.Split(sp.m_rs.GetScenarioShaminText(3, 74, 0), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, 1, list13, true, false, true, true);
                list13 = Util.Split(sp.m_rs.GetScenarioShaminText(3, 75, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list13, true, false, false, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0.35f, 2f, false, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 30U, 1.4f, 0, false, true, true);
                sp.UnloadPlace();
                sp.WaitSec(0.6f, true);
                int mainP = ad.mainP;
                Plugin.CheckLocationsInScript(1303);
                gd.baseData.gainExp += 150U;
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(0.25f, true);
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioShaminText(3, 77, -1), 1, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.FadeBgmInScript(-1f, 0f, 3f, true, -1);
                gd.forwardNext = true;
            };
            __instance.actions[18] = action;
        }


        [HarmonyPatch(typeof(Sige1Scenario), "SetParam")]
        [HarmonyPostfix]
        static void SeekAgreement(Sige1Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[1];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                ScenarioContents.ChangeSceOnFlg(ref gd, ref sd, 2U);
                sd.flg |= 16384;
                int num = gd.personFromId[7];
                sp.WaitSec(0.45f, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Interface, 50U, true, false, -1);
                    sp.m_sb[50001U].SetFade(0.3f, 0f, 0, -1f, -1, false);
                    sp.m_sb[50002U].SetFade(0.3f, 0f, 0, -1f, -1, false);
                    return true;
                }, (float e) => true, false));
                sp.ShowChara(ad.targetP, 0, 0, 20U, false);
                sp.ShowChara(ad.mainP, 0, 2, 20U, false);
                sp.WaitFade(new List<uint> { 50001U, 50002U }, true, true);
                sp.WaitSec(0.2f, true);
                List<string> list = Util.Split(sp.m_rs.GetScenarioShigeText(0, 2, 4), new char[] { '|' });
                string text = list[0];
                list[0] = text;
                sp.SetNormalSerifu(0, -1, 0, list, true, true, true, true);
                list = Util.Split(sp.m_rs.GetScenarioShigeText(0, 3, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 0, list, false, false, false, true);
                sp.PlayBgmInScript("bgm02", 0f, 0.8f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list = Util.Split(sp.m_rs.GetScenarioShigeText(0, 4, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 0, list, true, true, false, true);
                list = Util.Split(sp.m_rs.GetScenarioShigeText(0, 5, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, ad.targetP, 1, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioShigeText(0, 6, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 2, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioShigeText(0, 7, 0), new char[] { '|' });
                sp.SetNormalSerifu(num, ad.mainP, 1, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioShigeText(0, 8, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 0, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioShigeText(0, 9, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, -1, 1, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioShigeText(0, 10, 5), new char[] { '|' });
                sp.SetNormalSerifu(num, ad.counterP, 0, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioShigeText(0, 11, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 2, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioShigeText(0, 12, 6), new char[] { '|' });
                sp.SetNormalSerifu(num, -1, 1, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioShigeText(0, 13, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 0, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioShigeText(0, 14, 1), new char[] { '|' });
                sp.SetNormalSerifu(num, -1, 1, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioShigeText(0, 15, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, -1, 0, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioShigeText(0, 16, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 2, list, true, false, false, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 1.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.WaitSec(0.7f, true);
                Plugin.CheckLocationsInScript(11);
                gd.baseData.gainExp += 50U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioShigeText(0, 18, -1), 3, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                gd.forwardNext = true;
            };
            __instance.actions[1] = action;
        }


        [HarmonyPatch(typeof(Sige2Scenario), "SetParam")]
        [HarmonyPostfix]
        static void GameSermon(Sige2Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[3];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Get Other Stuff
                Type dataType = AccessTools.TypeByName("gnosia.Data");
                Array chara = (Array)AccessTools.Field(dataType, "Chara").GetValue(null);
                //Base
                sp.WaitSec(0.8f, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_Interface, 50U, true, false, -1);
                    return true;
                }, (float e) => true, true));
                List<string> list3 = Util.Split(sp.m_rs.GetScenarioShigeText(1, 21, -1), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, -1, list3, true, true, true, true);
                sp.PlaySeInScript("se_ashioto_02", 0.7f);
                sp.LoadPlace(16, true);
                sp.WaitFade(new List<uint> { 30U }, true, true);
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
                sp.WaitSec(0.6f, false);
                sp.StopAllSeInScript();
                sp.PlaySeInScript("se_jidoudoa", 1f);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.m_sb[50001U].SetFade(0.3f, 0f, 0, -1f, -1, false);
                    sp.m_sb[50002U].SetFade(0.3f, 0f, 0, -1f, -1, false);
                    return true;
                }, (float e) => true, true));
                sp.ShowChara(ad.counterP, 2, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(120f, 40f, 720f, 405f), 0f, 1f, true, null, true);
                sp.WaitFade(new List<uint> { 50001U, 50002U }, true, true);
                sp.PlayBgmInScript("bgm04", 1f, 0.5f, -1, true);
                list3 = Util.Split(sp.m_rs.GetScenarioShigeText(1, 22, -1), new char[] { '|' });
                string text2 = list3[0];
                Util.Replace(ref text2, "{0}", gd.takashiName);
                list3[0] = text2;
                sp.SetNormalSerifu(ad.counterP, 0, 1, list3, true, true, true, true);
                sp.FadeBgmInScript(-1f, 0.8f, 2f, false, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.25f, 0, false, true, true);
                sp.UnloadPlace();
                sp.LoadTexture("ivep05_00_0");
                sp.WaitLoad();
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, true, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep05_00_0", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, true));
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.6f, 0, true, true, true);
                sp.LoadSound("se_syu_02");
                sp.WaitSec(0.2f, true);
                sp.WaitLoad();
                list3 = Util.Split(sp.m_rs.GetScenarioShigeText(1, 23, -1), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, -1, list3, false, true, true, false);
                sp.LoadTexture("ivep05_00_1");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.PlaySeInScript("se_syu_02", 0.6f);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.25f, 2, false, true, true);
                sp.UnloadTexture("ivep05_00_0");
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, true, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep05_00_1", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, true));
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(0.2f, true);
                list3 = Util.Split(sp.m_rs.GetScenarioShigeText(1, 24, -1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 1, list3, false, true, true, false);
                sp.LoadTexture("ivep05_00_2");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.PlaySeInScript("se_syu_02", 0.6f);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.25f, 4, false, true, true);
                sp.UnloadTexture("ivep05_00_1");
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, true, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep05_00_2", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, true));
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(0.2f, true);
                list3 = Util.Split(sp.m_rs.GetScenarioShigeText(1, 25, -1), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, -1, 1, list3, false, true, true, false);
                sp.LoadTexture("ivep05_00_3");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.PlaySeInScript("se_syu_02", 0.6f);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.25f, 4, false, true, true);
                sp.UnloadTexture("ivep05_00_2");
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, true, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep05_00_3", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, true));
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(0.2f, true);
                list3 = Util.Split(sp.m_rs.GetScenarioShigeText(1, 26, -1), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 1, list3, false, true, true, false);
                sp.LoadTexture("ivep05_00_5");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                bool flag = false;
                if ((ad.tuizuiP & 16) > 0 && (gd.chara[gd.personFromId[4]].allFlg & 8UL) > 0UL)
                {
                    flag = true;
                    sp.FadeBgmInScript(-1f, 0.5f, 0.4f, false, -1);
                    sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.25f, 0, false, true, true);
                    sp.UnloadTexture("ivep05_00_3");
                    sp.LoadTexture("ivep05_00_4");
                    sp.WaitLoad();
                    sp.WaitFade(new List<uint> { 30U }, true, true);
                    sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                    {
                        sp.SetScreen(Setting.Screen.s_none, 0U, true, false, -1);
                        sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep05_00_4", null, null);
                        sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                        sp.SetColorScreen(255U, 30U, -1);
                        return true;
                    }, (float e) => true, true));
                    sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.45f, 0, true, true, true);
                    sp.WaitSec(0.2f, true);
                    list3 = Util.Split(sp.m_rs.GetScenarioShigeText(1, 27, -1), new char[] { '|' });
                    sp.SetNormalSerifu(gd.personFromId[4], -1, 1, list3, true, true, true, false);
                }
                sp.FadeBgmInScript(-1f, 1f, 1f, false, -1);
                sp.PlaySeInScript("se_syu_02", 0.6f);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.25f, 2, false, true, true);
                if (flag)
                {
                    sp.UnloadTexture("ivep05_00_4");
                }
                else
                {
                    sp.UnloadTexture("ivep05_00_3");
                }
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, true, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep05_00_5", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, true));
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(0.2f, true);
                list3 = Util.Split(sp.m_rs.GetScenarioShigeText(1, 28, -1), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, -1, 1, list3, false, true, true, false);
                sp.LoadTexture("ivep05_00_6");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.PlaySeInScript("se_syu_02", 0.6f);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.25f, 2, false, true, true);
                sp.UnloadTexture("ivep05_00_5");
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, true, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep05_00_6", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, true));
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(0.2f, true);
                list3 = Util.Split(sp.m_rs.GetScenarioShigeText(1, 29, -1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list3, false, true, true, false);
                sp.PlaySeInScript("se_Kinzoku_02", 1f);
                sp.LoadTexture("ivep05_00_7");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.PlaySeInScript("se_syu_02", 0.6f);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.25f, 4, false, true, true);
                sp.UnloadTexture("ivep05_00_6");
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, true, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep05_00_7", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, true));
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(0.2f, true);
                list3 = Util.Split(sp.m_rs.GetScenarioShigeText(1, 30, -1), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, -1, 1, list3, true, true, true, false);
                sp.StopAllSeInScript();
                sp.PlaySeInScript("se_hikaruball", 1f);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetColorScreen(uint.MaxValue, 50000U, -1);
                    sp.m_sb[50000U].SetFade(1.6f, 1f, 0, 0f, -1, false);
                    return true;
                }, (float e) => true, true));
                sp.LoadPlace(16, true);
                sp.WaitLoad();
                sp.WaitFade(new List<uint> { 50000U }, false, true);
                sp.RemoveScreenInScript(0U);
                sp.UnloadTexture("ivep05_00_7");
                sp.FadeBgmInScript(-1f, 0f, 2f, true, -1);
                sp.WaitSec(1.8f, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    return true;
                }, (float e) => true, true));
                gd.pos = 2;
                int i = 0;
                if (gd.chara[ad.targetP].i_yaku == Setting.Yakuwari.y_Jinro)
                {
                    sp.ShowChara(ad.mainP, 2, 1, 20U, false);
                    sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(180f, 60f, 600f, 337.5f), 0f, 1f, true, null, true);
                    sp.PlayBgmInScript("bgm03", 1f, 0.8f, -1, true);
                    sp.SetFadeScreen(new List<uint> { 50000U }, 50001U, 1f, 0, true, true, true);
                    list3 = Util.Split(sp.m_rs.GetScenarioShigeText(1, 31, -1), new char[] { '|' });
                    sp.SetNormalSerifu(ad.mainP, -1, 1, list3, true, true, true, true);
                    list3 = Util.Split(sp.m_rs.GetScenarioShigeText(1, 32, 1), new char[] { '|' });
                    sp.SetNormalSerifu(ad.targetP, ad.mainP, 2, list3, true, false, false, true);
                    sp.FadeBgmInScript(-1f, 0f, 0.4f, true, -1);
                    if ((ad.tuizuiP & 2048) > 0)
                    {
                        list3 = Util.Split(sp.m_rs.GetScenarioShigeText(1, 33, 4), new char[] { '|' });
                        sp.SetNormalSerifu(gd.personFromId[11], -1, gd.GetNextPos(), list3, true, false, false, true);
                    }
                    if ((ad.tuizuiP & 256) > 0)
                    {
                        list3 = Util.Split(sp.m_rs.GetScenarioShigeText(1, 34, 4), new char[] { '|' });
                        sp.SetNormalSerifu(gd.personFromId[8], -1, gd.GetNextPos(), list3, true, false, false, true);
                    }
                    if ((ad.tuizuiP & 16) > 0)
                    {
                        list3 = Util.Split(sp.m_rs.GetScenarioShigeText(1, 35, 4), new char[] { '|' });
                        sp.SetNormalSerifu(gd.personFromId[4], -1, gd.GetNextPos(), list3, true, false, false, true);
                    }
                    list3 = Util.Split(sp.m_rs.GetScenarioShigeText(1, 36, 5), new char[] { '|' });
                    sp.SetNormalSerifu(ad.counterP, -1, gd.GetNextPos(), list3, true, false, false, true);
                    list3 = Util.Split(sp.m_rs.GetScenarioShigeText(1, 37, 1), new char[] { '|' });
                    sp.SetNormalSerifu(ad.targetP, -1, gd.GetNextPos(), list3, true, false, false, true);
                    i = ad.targetP;
                }
                else
                {
                    sp.ShowChara(ad.targetP, 3, 1, 20U, false);
                    sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(180f, 60f, 600f, 337.5f), 0f, 1f, true, null, true);
                    sp.PlayBgmInScript("bgm03", 1f, 0.8f, -1, true);
                    sp.SetFadeScreen(new List<uint> { 50000U }, 50001U, 1f, 0, true, true, true);
                    list3 = Util.Split(sp.m_rs.GetScenarioShigeText(1, 38, -1), new char[] { '|' });
                    sp.SetNormalSerifu(ad.targetP, -1, 1, list3, true, true, true, true);
                    list3 = Util.Split(sp.m_rs.GetScenarioShigeText(1, 39, 7), new char[] { '|' });
                    sp.SetNormalSerifu(ad.mainP, -1, 2, list3, true, false, false, true);
                    sp.FadeBgmInScript(-1f, 0f, 0.4f, true, -1);
                    if ((ad.tuizuiP & 2048) > 0)
                    {
                        list3 = Util.Split(sp.m_rs.GetScenarioShigeText(1, 40, 4), new char[] { '|' });
                        sp.SetNormalSerifu(gd.personFromId[11], -1, gd.GetNextPos(), list3, true, false, false, true);
                    }
                    if ((ad.tuizuiP & 256) > 0)
                    {
                        list3 = Util.Split(sp.m_rs.GetScenarioShigeText(1, 41, 4), new char[] { '|' });
                        sp.SetNormalSerifu(gd.personFromId[8], -1, gd.GetNextPos(), list3, true, false, false, true);
                    }
                    if ((ad.tuizuiP & 16) > 0)
                    {
                        list3 = Util.Split(sp.m_rs.GetScenarioShigeText(1, 42, 4), new char[] { '|' });
                        sp.SetNormalSerifu(gd.personFromId[4], -1, gd.GetNextPos(), list3, true, false, false, true);
                    }
                    list3 = Util.Split(sp.m_rs.GetScenarioShigeText(1, 43, 5), new char[] { '|' });
                    sp.SetNormalSerifu(ad.counterP, -1, gd.GetNextPos(), list3, true, false, false, true);
                    list3 = Util.Split(sp.m_rs.GetScenarioShigeText(1, 44, 0), new char[] { '|' });
                    sp.SetNormalSerifu(ad.mainP, -1, gd.GetNextPos(), list3, true, false, false, true);
                    i = ad.mainP;
                }
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 30U, 0.1f, 0, true, true, true);
                sp.UnloadPlace();
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    gd.chara[0].knowledge[i] = Setting.Yakuwari.y_Jinro;
                    Jinro.IsY(Setting.Yakuwari.y_Jinro, i, ref gd.knowTable);
                    gd.peopleFlg[9] = (ushort)((int)gd.peopleFlg[9] | (1 << i));
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    uint num4 = ((i == 0) ? 737023U : 0U);
                    sp.SetColorScreen(num4, 15U, -1);
                    sp.m_sb[20U].SetColorCoeff(new Vector4(0.208f, 0.31f, 0.6f, 1f));
                    sp.SetColorScreen(255U, 29U, -1);
                    return true;
                }, (float e) => true, false));
                if (i > 0)
                {
                    sp.ShowChara(i, 3, 2, 20U, false);
                }
                sp.SetClipAnim(new List<uint> { 20U }, new Vector4(250f, 120f, 540f, 306f), 0f, 1f, true, null, true);
                sp.SetFadeScreen(new List<uint> { 29U }, 30U, 0.1f, 0, false, true, true);
                sp.StopAllSeInScript();
                sp.UnloadSound("se_syu_02");
                sp.PlaySeInScript("se_gatyan", 1f);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.PlaySeInScript("se_pusyu", 1f);
                sp.SetDialogScreen(200U, GetCharaName(chara, gd, i) + sp.m_rs.GetScenarioShigeText(1, 45, -1), 1, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.SetFadeScreen(new List<uint> { 15U, 20U }, 30U, 0.1f, 5, true, true, true);
                sp.WaitSec(0.5f, true);
                int mainP = ad.mainP;
                int targetP = ad.targetP;
                Plugin.CheckLocationsInScript(505, 1006);
                gd.baseData.gainExp += 100U;
                sp.WaitSec(0.4f, true);
                ad.ctuizuiP = (ushort)i;
                gd.forwardNext = true;
            };
            __instance.actions[3] = action;
        }


        [HarmonyPatch(typeof(Sige3Scenario), "SetParam")]
        [HarmonyPostfix]
        static void ShowerRoomShigemichi(Sige3Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[5];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 3.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadTexture("p05a");
                sp.UnloadTexture("ivep05_01_0");
                sp.UnloadPlace();
                sp.WaitSec(0.7f, true);
                int mainP = ad.mainP;
                Plugin.CheckLocationsInScript(503);
                gd.baseData.gainExp += 50U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(0.4f, true);
                gd.forwardNext = true;
            };
            __instance.actions[5] = action;
        }


        [HarmonyPatch(typeof(Sige4Scenario), "SetParam")]
        [HarmonyPostfix]
        static void Shigemichi4(Sige4Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[6];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                List<string> list6 = Util.Split(sp.m_rs.GetScenarioShigeText(3, 19, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, -1, 2, list6, true, false, false, true);
                list6 = Util.Split(sp.m_rs.GetScenarioShigeText(3, 20, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 0, list6, true, false, false, true);
                list6 = Util.Split(sp.m_rs.GetScenarioShigeText(3, 21, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list6, true, false, false, true);
                sp.FadeBgmInScript(-1f, 0f, 1.6f, true, -1);
                list6 = Util.Split(sp.m_rs.GetScenarioShigeText(3, 22, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list6, false, true, false, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.m_sb[50U].m_textAreaMap["test"].nowLine >= 2, true));
                sp.PlaySeInScript("se_se_03", 1f);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.1f, 0, false, false, true);
                sp.UnvisibleAllChara(20U, -1);
                sp.ShowChara(ad.mainP, 5, 1, 20U, false);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.PlayBgmInScript("bgm22", 0f, 1f, -1, true);
                list6 = Util.Split(sp.m_rs.GetScenarioShigeText(3, 23, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, ad.mainP, 2, list6, true, false, false, true);
                list6 = Util.Split(sp.m_rs.GetScenarioShigeText(3, 24, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list6, true, false, false, true);
                list6 = Util.Split(sp.m_rs.GetScenarioShigeText(3, 25, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list6, true, true, false, true);
                list6 = Util.Split(sp.m_rs.GetScenarioShigeText(3, 26, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list6, false, true, true, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.m_sb[50U].m_textAreaMap["test"].nowLine >= 1, true));
                sp.FadeBgmInScript(0.2f, 1f, 1f, false, -1);
                sp.PlaySeInScript("se_se_03", 1f);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.1f, 0, false, false, true);
                sp.UnvisibleAllChara(20U, -1);
                sp.ShowChara(ad.mainP, 5, 1, 20U, false);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list6 = Util.Split(sp.m_rs.GetScenarioShigeText(3, 27, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 0, list6, true, false, false, true);
                list6 = Util.Split(sp.m_rs.GetScenarioShigeText(3, 28, 0), new char[] { '|' });
                string text3 = list6[0];
                Util.Replace(ref text3, "{0}", gd.takashiName);
                list6[0] = text3;
                sp.SetNormalSerifu(ad.counterP, 0, 2, list6, true, false, false, true);
                list6 = Util.Split(sp.m_rs.GetScenarioShigeText(3, 29, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list6, false, false, false, true);
                sp.PlaySeInScript("se_se_03", 1f);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 3.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadTexture("p05a");
                sp.UnloadPlace();
                sp.WaitSec(0.7f, true);
                int mainP = ad.mainP;
                Plugin.CheckLocationsInScript(504);
                gd.baseData.gainExp += 50U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(0.4f, true);
                gd.forwardNext = true;
            };
            __instance.actions[6] = action;
        }


        [HarmonyPatch(typeof(Sige5Scenario), "SetParam")]
        [HarmonyPostfix]
        static void Shigemichi6(Sige5Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[5];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                List<string> list5 = Util.Split(sp.m_rs.GetScenarioShigeText(4, 24, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 0, list5, true, false, false, true);
                list5 = Util.Split(sp.m_rs.GetScenarioShigeText(4, 25, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 2, list5, true, false, false, true);
                list5 = Util.Split(sp.m_rs.GetScenarioShigeText(4, 26, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 0, list5, true, false, false, true);
                list5 = Util.Split(sp.m_rs.GetScenarioShigeText(4, 27, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 2, list5, true, false, false, true);
                list5 = Util.Split(sp.m_rs.GetScenarioShigeText(4, 28, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 0, list5, true, false, false, true);
                list5 = Util.Split(sp.m_rs.GetScenarioShigeText(4, 29, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 2, list5, false, false, false, true);
                sp.LoadSound("se_ashioto_08");
                sp.FadeBgmInScript(0f, 0.8f, 1.5f, false, -1);
                sp.PlaySeInScript("se_jin_11", 1f);
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list5 = Util.Split(sp.m_rs.GetScenarioShigeText(4, 30, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 0, list5, true, false, false, true);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.4f, 0, false, true, true);
                sp.FadeBgmInScript(-1f, 0f, 2.5f, true, -1);
                sp.PlaySeInScript("se_ashioto_08", 0.6f);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(1f, true);
                sp.StopAllSeInScript();
                sp.UnloadSound("se_ashioto_08");
                sp.PlaySeInScript("se_jidoudoa", 1f);
                sp.WaitSec(1f, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, false));
                sp.ShowChara(ad.mainP, 5, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(120f, 40f, 720f, 405f), 0f, 1f, true, null, true);
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.4f, 0, true, true, true);
                sp.WaitSec(0.4f, true);
                list5 = Util.Split(sp.m_rs.GetScenarioShigeText(4, 31, 5), new char[] { '|' });
                string text3 = list5[0];
                Util.Replace(ref text3, "{0}", gd.takashiName);
                list5[0] = text3;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list5, true, true, true, true);
                sp.PlaySeInScript("se_jin_09", 1f);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetColorScreen(4068727236U, 15U, -1);
                    sp.m_sb[15U].SetFade(0.6f, 1f, 3, 0f, -1, false);
                    return true;
                }, (float e) => true, false));
                list5 = Util.Split(sp.m_rs.GetScenarioShigeText(4, 32, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list5, true, false, false, true);
                sp.WaitSec(0.05f, false);
                sp.SetFadeScreen(new List<uint> { 0U, 15U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.WaitSec(0.7f, true);
                int mainP = ad.mainP;
                Plugin.CheckLocationsInScript(506);
                gd.baseData.gainExp += 50U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(0.8f, true);
                gd.forwardNext = true;
            };
            __instance.actions[5] = action;
        }


        [HarmonyPatch(typeof(SQ1Scenario), "SetParam")]
        [HarmonyPostfix]
        static void FoolAndBeFooled(SQ1Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[8];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                List<string> list8 = Util.Split(sp.m_rs.GetScenarioSQText(0, 30, 1), new char[] { '|' });
                string text7 = list8[0];
                Util.Replace(ref text7, "{0}", gd.takashiName);
                list8[0] = text7;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list8, true, true, false, true);
                list8 = Util.Split(sp.m_rs.GetScenarioSQText(0, 31, 0), new char[] { '|' });
                text7 = list8[0];
                Util.Replace(ref text7, "{0}", gd.takashiName);
                list8[0] = text7;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list8, true, true, false, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 1.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.UnloadTexture("p02a");
                sp.WaitSec(0.7f, true);
                if ((gd.baseData.sce_ind_flg[sd.id] & 256) == 0)
                {
                    Plugin.CheckLocationsInScript(17);
                }
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                if ((gd.baseData.sce_ind_flg[sd.id] & 256) == 0)
                {
                    gd.baseData.gainExp += 50U;
                    sp.PlaySeInScript("se_square", 1f);
                    sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioSQText(0, 33, -1), 3, false);
                    sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                }
                sp.WaitSec(0.4f, true);
                gd.forwardNext = true;
            };
            __instance.actions[8] = action;
            action = __instance.actions[18];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                List<string> list17;
                if (ad.mainP == 0)
                {
                    gd.ChangeLove(ad.targetP, 0, -0.2f);
                    list17 = Util.Split(sp.m_rs.GetScenarioSQText(0, 70, 1), new char[] { '|' });
                    sp.SetNormalSerifu(0, ad.targetP, 1, list17, true, false, true, true);
                    list17 = Util.Split(sp.m_rs.GetScenarioSQText(0, 71, 6), new char[] { '|' });
                    sp.SetNormalSerifu(ad.targetP, 0, 1, list17, true, false, false, true);
                }
                sp.SetCopyScreen(new List<uint> { 0U, 20U }, 29U, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_none, 20U, false, false, -1);
                    sp.ChangeCharaTexture(2U, "p02c", 10U, 20U, true);
                    return true;
                }, (float e) => true, true));
                sp.SetFadeScreen(new List<uint> { 29U }, 30U, 0.4f, 0, false, true, true);
                sp.ShowChara(ad.targetP, 0, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(240f, 80f, 480f, 270f), 0f, 1f, true, null, true);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list17 = Util.Split(sp.m_rs.GetScenarioSQText(0, 72, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list17, true, true, true, true);
                sp.WaitSec(0.2f, true);
                sp.FadeBgmInScript(-1f, 1f, 0.85f, false, -1);
                sp.SetCopyScreen(new List<uint> { 0U, 20U }, 29U, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_none, 20U, false, false, -1);
                    sp.ChangeCharaTexture(2U, "p02b", 10U, 20U, true);
                    return true;
                }, (float e) => true, true));
                sp.ShowChara(ad.targetP, 5, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(240f, 80f, 480f, 270f), 0f, 1f, true, null, true);
                sp.SetFadeScreen(new List<uint> { 29U }, 30U, 0.25f, 0, false, true, true);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list17 = Util.Split(sp.m_rs.GetScenarioSQText(0, 73, 1), new char[] { '|' });
                string text12 = list17[0];
                Util.Replace(ref text12, "{0}", gd.takashiName);
                list17[0] = text12;
                sp.SetNormalSerifu(ad.targetP, 0, 1, list17, true, true, true, true);
                sp.WaitSec(0.2f, true);
                list17 = Util.Split(sp.m_rs.GetScenarioSQText(0, 74, 2), new char[] { '|' });
                text12 = list17[0];
                Util.Replace(ref text12, "{0}", gd.takashiName);
                list17[0] = text12;
                sp.SetNormalSerifu(ad.targetP, 0, 1, list17, true, true, false, true);
                sp.SetCopyScreen(new List<uint> { 0U, 20U }, 29U, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_none, 20U, false, false, -1);
                    sp.ChangeCharaTexture(2U, "p02c", 10U, 20U, true);
                    return true;
                }, (float e) => true, true));
                sp.ShowChara(ad.targetP, 2, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(240f, 80f, 480f, 270f), 0f, 1f, true, null, true);
                sp.SetFadeScreen(new List<uint> { 29U }, 30U, 0.4f, 0, false, true, true);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(0.2f, true);
                list17 = Util.Split(sp.m_rs.GetScenarioSQText(0, 75, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list17, true, true, true, true);
                sp.WaitSec(0.05f, false);
                sp.WaitSec(0.2f, true);
                sp.FadeBgmInScript(-1f, 0.45f, 2f, false, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1.5f, 0, false, true, true);
                sp.UnloadPlace();
                sp.UnloadTexture("p02b");
                sp.UnloadTexture("p02c");
                sp.WaitSec(1.2f, true);
                int targetP = ad.targetP;
                Plugin.CheckLocationsInScript(204);
                gd.baseData.gainExp += 250U;
                sp.WaitSec(1f, true);
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioSQText(0, 77, -1), 2, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.FadeBgmInScript(-1f, 0f, 3.5f, true, -1);
                sp.WaitSec(0.4f, true);
                gd.forwardNext = true;
            };
            __instance.actions[18] = action;
        }


        [HarmonyPatch(typeof(SQ2Scenario), "SetParam")]
        [HarmonyPostfix]
        static void Retaliate(SQ2Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[4];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.FadeBgmInScript(-1f, 0f, 1f, true, -1);
                List<string> list2 = Util.Split(sp.m_rs.GetScenarioSQText(1, 11, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 2, list2, false, false, false, true);
                sp.WaitText(50U, "test", false);
                sp.PlayBgmInScript("bgm03", 0f, 1f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list2 = Util.Split(sp.m_rs.GetScenarioSQText(1, 12, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 1, list2, true, false, false, true);
                list2 = Util.Split(sp.m_rs.GetScenarioSQText(1, 13, 5), new char[] { '|' });
                string text2 = list2[0];
                Util.Replace(ref text2, "{0}", gd.takashiName);
                list2[0] = text2;
                sp.SetNormalSerifu(ad.mainP, -1, 2, list2, false, false, false, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.m_sb[50U].m_textAreaMap["test"].nowLine >= 1 && sp.m_sb[50U].m_textAreaMap["test"].strNowList[1] >= 5, true));
                sp.FadeBgmInScript(-1f, 0f, 0.25f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.25f, 0, false, false, true);
                sp.UnvisibleAllChara(20U, -1);
                sp.ShowChara(ad.mainP, 1, 2, 20U, false);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.PlayBgmInScript("bgm05", 0f, 1f, -1, true);
                list2 = Util.Split(sp.m_rs.GetScenarioSQText(1, 14, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 1, list2, true, false, false, true);
                list2 = Util.Split(sp.m_rs.GetScenarioSQText(1, 15, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 2, list2, true, false, false, true);
                list2 = Util.Split(sp.m_rs.GetScenarioSQText(1, 16, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, ad.mainP, 0, list2, true, false, false, true);
                list2 = Util.Split(sp.m_rs.GetScenarioSQText(1, 17, 0), new char[] { '|' });
                text2 = list2[0];
                Util.Replace(ref text2, "{0}", gd.takashiName);
                list2[0] = text2;
                sp.SetNormalSerifu(ad.mainP, -1, 2, list2, true, false, false, true);
                list2 = Util.Split(sp.m_rs.GetScenarioSQText(1, 18, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 1, list2, true, false, false, true);
                list2 = Util.Split(sp.m_rs.GetScenarioSQText(1, 19, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, ad.mainP, 0, list2, true, false, false, true);
                sp.WaitSec(0.05f, false);
                sp.PlaySeInScript("se_ashioto_02", 1f);
                sp.FadeBgmInScript(-1f, 0f, 1.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.4f, 0, false, true, true);
                sp.UnloadPlace();
                sp.UnloadTexture("p05a");
                sp.LoadPlace(31, false);
                sp.WaitLoad();
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.LoadTexture("p11a");
                sp.WaitSec(1.5f, true);
                sp.WaitLoad();
                sp.StopAllSeInScript();
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.ChangeCharaTexture(11U, "p11a", 10U, 20U, true);
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, true));
                sp.ShowChara(ad.mainP, 1, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(120f, 40f, 720f, 405f), 0f, 1f, true, null, true);
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.4f, 0, true, true, true);
                sp.WaitSec(0.2f, true);
                list2 = Util.Split(sp.m_rs.GetScenarioSQText(1, 20, 1), new char[] { '|' });
                text2 = list2[0];
                list2[0] = text2;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list2, false, true, true, true);
                sp.WaitSec(0.4f, true);
                sp.PlayBgmInScript("bgm03", 0f, 0.8f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list2 = Util.Split(sp.m_rs.GetScenarioSQText(1, 21, 7), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list2, true, true, false, true);
                list2 = Util.Split(sp.m_rs.GetScenarioSQText(1, 22, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list2, false, true, false, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.m_sb[50U].m_textAreaMap["test"].nowLine >= 1, false));
                sp.FadeBgmInScript(-1f, 0f, 1.2f, false, -1);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.WaitSec(0.05f, false);
                Plugin.CheckLocationsInScript(15);
                gd.baseData.gainExp += 50U;
                sp.FadeBgmInScript(0f, 0.4f, 2f, false, -1);
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioSQText(1, 24, -1), 3, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.WaitSec(0.4f, true);
                sp.FadeBgmInScript(-1f, 0f, 1.2f, true, -1);
                list2 = Util.Split(sp.m_rs.GetScenarioSQText(1, 25, 2), new char[] { '|' });
                text2 = list2[0];
                Util.Replace(ref text2, "{0}", gd.takashiName);
                list2[0] = text2;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list2, true, false, false, true);
                sp.PlayBgmInScript("bgm18", 2f, 0.85f, -1, true);
                list2 = Util.Split(sp.m_rs.GetScenarioSQText(1, 26, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list2, true, true, false, true);
                list2 = Util.Split(sp.m_rs.GetScenarioSQText(1, 27, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list2, true, true, false, true);
                list2 = Util.Split(sp.m_rs.GetScenarioSQText(1, 28, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list2, false, true, false, true);
            };
            __instance.actions[4] = action;
        }


        [HarmonyPatch(typeof(SQ3Scenario), "SetParam")]
        [HarmonyPostfix]
        static void TearsOfSQ(SQ3Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[28];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.FadeBgmInScript(-1f, 0.4f, 1.2f, false, -1);
                List<string> list23 = Util.Split(sp.m_rs.GetScenarioSQText(2, 96, 0), new char[] { '|' });
                sp.SetNormalSerifu(0, ad.targetP, 1, list23, true, false, true, true);
                list23 = Util.Split(sp.m_rs.GetScenarioSQText(2, 97, 0), new char[] { '|' });
                string text19 = list23[0];
                Util.Replace(ref text19, "{0}", gd.takashiName);
                list23[0] = text19;
                sp.SetNormalSerifu(ad.targetP, 0, 1, list23, true, false, false, true);
                sp.FadeBgmInScript(-1f, 0f, 1.3f, true, -1);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(0f, 0f, 960f, 540f), 1.6f, 1f, true, null, true);
                list23 = Util.Split(sp.m_rs.GetScenarioSQText(2, 98, 4), new char[] { '|' });
                text19 = list23[0];
                Util.Replace(ref text19, "{0}", gd.takashiName);
                list23[0] = text19;
                sp.SetNormalSerifu(ad.targetP, 0, 1, list23, true, false, false, true);
                list23 = Util.Split(sp.m_rs.GetScenarioSQText(2, 99, 6), new char[] { '|' });
                text19 = list23[0];
                Util.Replace(ref text19, "{0}", gd.takashiName);
                list23[0] = text19;
                sp.SetNormalSerifu(ad.targetP, 0, 1, list23, true, true, false, true);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.6f, 0, true, true, true);
                sp.PlayBgmInScript("bgm23", 2f, 0.5f, -1, true);
                sp.WaitSec(0.4f, true);
                list23 = Util.Split(sp.m_rs.GetScenarioSQText(2, 100, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 1, list23, true, true, true, false);
                sp.WaitSec(0.2f, true);
                list23 = Util.Split(sp.m_rs.GetScenarioSQText(2, 101, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 1, list23, true, true, true, false);
                sp.WaitSec(0.2f, true);
                list23 = Util.Split(sp.m_rs.GetScenarioSQText(2, 102, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 1, list23, true, true, true, false);
                sp.WaitSec(0.5f, true);
                list23 = Util.Split(sp.m_rs.GetScenarioSQText(2, 103, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 1, list23, true, true, true, false);
                sp.WaitSec(0.4f, true);
                list23 = Util.Split(sp.m_rs.GetScenarioSQText(2, 104, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 1, list23, true, true, true, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, true));
                sp.ShowChara(ad.targetP, 1, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(180f, 60f, 600f, 337.5f), 0f, 1f, true, null, true);
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.7f, 0, true, true, true);
                list23 = Util.Split(sp.m_rs.GetScenarioSQText(2, 105, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list23, true, true, true, true);
                list23 = Util.Split(sp.m_rs.GetScenarioSQText(2, 106, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list23, true, true, false, true);
                sp.WaitSec(0.4f, true);
                sp.FadeBgmInScript(-1f, 0.75f, 0.6f, false, -1);
                if (gd.aliveNum == 2 && (sd.flg & 256) > 0)
                {
                    list23 = Util.Split(sp.m_rs.GetScenarioSQText(2, 107, 2), new char[] { '|' });
                }
                else
                {
                    list23 = Util.Split(sp.m_rs.GetScenarioSQText(2, 108, 2), new char[] { '|' });
                }
                text19 = list23[0];
                Util.Replace(ref text19, "{0}", gd.takashiName);
                list23[0] = text19;
                sp.SetNormalSerifu(ad.targetP, 0, 1, list23, true, true, false, true);
                list23 = Util.Split(sp.m_rs.GetScenarioSQText(2, 109, 0), new char[] { '|' });
                text19 = list23[0];
                Util.Replace(ref text19, "{0}", gd.takashiName);
                list23[0] = text19;
                sp.SetNormalSerifu(ad.targetP, 0, 1, list23, true, true, false, true);
                list23 = Util.Split(sp.m_rs.GetScenarioSQText(2, 110, 0), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, 1, list23, true, false, true, true);
                list23 = Util.Split(sp.m_rs.GetScenarioSQText(2, 111, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list23, false, false, false, true);
                sp.LoadTexture("ivep02_03_0");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.4f, 0, true, true, true);
                sp.WaitSec(0.4f, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep02_03_0", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, true));
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.6f, 0, true, true, true);
                list23 = Util.Split(sp.m_rs.GetScenarioSQText(2, 112, 0), new char[] { '|' });
                text19 = list23[0];
                Util.Replace(ref text19, "{0}", gd.takashiName);
                list23[0] = text19;
                sp.SetNormalSerifu(ad.targetP, 0, 1, list23, true, true, true, false);
                sp.WaitSec(0.05f, false);
                sp.WaitSec(0.55f, true);
                sp.FadeBgmInScript(-1f, 0f, 4.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 50U }, 40002U, 2f, 0, true, true, true);
                sp.UnloadPlace();
                sp.UnloadTexture("ivep02_03_0");
                int targetP = ad.targetP;
                Plugin.CheckLocationsInScript(205);
                gd.baseData.gainExp += 50U;
                sp.WaitSec(1.4f, true);
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioSQText(2, 114, -1), 1, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.WaitSec(0.6f, true);
                gd.forwardNext = true;
            };
            __instance.actions[28] = action;
        }


        [HarmonyPatch(typeof(SQ4Scenario), "SetParam")]
        [HarmonyPostfix]
        static void SQ2GnosiaIntro(SQ4Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[1];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.WaitSec(0.5f, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.m_sb[50002U].NotifyFinish(2f, 0, true);
                    return true;
                }, (float e) => true, false));
                sp.WaitSec(2f, true);
                sp.RemoveScreenInScript(50002U);
                List<string> list = Util.Split(sp.m_rs.GetScenarioSQText(3, 2, 1), new char[] { '|' });
                string text = list[0];
                Util.Replace(ref text, "{0}", gd.takashiName);
                list[0] = text;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list, false, true, false, true);
                sp.WaitSec(0.4f, true);
                sp.PlayBgmInScript("bgm17", 0f, 0.5f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list = Util.Split(sp.m_rs.GetScenarioSQText(3, 3, 0), new char[] { '|' });
                text = list[0];
                Util.Replace(ref text, "{0}", gd.takashiName);
                list[0] = text;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list, true, true, false, true);
                sp.FadeBgmInScript(-1f, 1f, 1.2f, false, -1);
                list = Util.Split(sp.m_rs.GetScenarioSQText(3, 4, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list, true, false, false, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 1.2f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 30U, 0.4f, 4, false, true, true);
                sp.UnloadPlace();
                sp.UnloadTexture("p02a");
                sp.LoadPlace(5, true);
                sp.WaitLoad();
                sp.WaitFade(new List<uint> { 30U }, true, true);
                if (!ArchipelagoClient.ServerData.CheckedLocations.Contains(202)) //Changed condition
                {
                    int mainP = ad.mainP;
                    int targetP = ad.targetP;
                    sp.StopAllSeInScript();
                    Plugin.CheckLocationsInScript(202, 703);
                    gd.baseData.gainExp += 100U;
                    sp.WaitSec(0.75f, true);
                }
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Interface, 50U, true, false, -1);
                    sp.SetColorScreen(255U, 50001U, -1);
                    return true;
                }, (float e) => true, true));
                sp.ShowChara(ad.targetP, 4, 0, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(0f, 80f, 480f, 270f), 0f, 1f, true, null, true);
                sp.SetFadeScreen(new List<uint> { 50001U }, 50002U, 0.4f, 4, true, true, true);
                list = Util.Split(sp.m_rs.GetScenarioSQText(3, 6, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 0, list, true, true, true, true);
                list = Util.Split(sp.m_rs.GetScenarioSQText(3, 7, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, ad.targetP, 1, list, false, false, false, true);
                sp.PlayBgmInScript("bgm01", 1f, 1f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list = Util.Split(sp.m_rs.GetScenarioSQText(3, 8, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.counterP, 0, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioSQText(3, 9, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 2, list, false, false, false, true);
            };
            __instance.actions[1] = action;
        }


        [HarmonyPatch(typeof(Stella1Scenario), "SetParam")]
        [HarmonyPostfix]
        static void ShigemichiInLove(Stella1Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[14];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                List<string> list14;
                if ((gd.actionFlg & 32UL) == 0UL)
                {
                    list14 = Util.Split(sp.m_rs.GetScenarioStellaText(0, 45, 1), new char[] { '|' });
                    sp.SetNormalSerifu(ad.mainP, 0, 1, list14, true, true, false, true);
                }
                sp.PlaySeInScript("se_Kinzoku_02", 1f);
                sp.FadeBgmInScript(0f, 0.85f, 1f, false, -1);
                list14 = Util.Split(sp.m_rs.GetScenarioStellaText(0, 46, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list14, true, true, false, true);
                list14 = Util.Split(sp.m_rs.GetScenarioStellaText(0, 47, 5), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, 1, list14, true, false, true, true);
                list14 = Util.Split(sp.m_rs.GetScenarioStellaText(0, 48, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list14, true, false, false, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 3.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.UnloadTexture("p05a");
                sp.WaitSec(0.7f, true);
                sp.StopAllSeInScript();
                int mainP = ad.mainP;
                Plugin.CheckLocationsInScript(507);
                gd.baseData.gainExp += 50U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(0.4f, true);
                gd.forwardNext = true;
            };
            __instance.actions[14] = action;
            action = __instance.actions[16];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.LoadTexture("p04a");
                sp.WaitSec(0.45f, true);
                sp.WaitLoad();
                sp.PlaySeInScript("se_jidoudoa", 1f);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.ChangeCharaTexture(4U, "p04a", 10U, 20U, true);
                    sp.SetScreen(Setting.Screen.s_Interface, 50U, true, false, -1);
                    return true;
                }, (float e) => true, true));
                sp.ShowChara(ad.mainP, 0, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(120f, 65f, 720f, 405f), 0f, 1f, true, null, true);
                sp.SetFadeScreen(new List<uint> { 50001U, 50002U }, 50003U, 0.3f, 0, false, true, true);
                sp.WaitFade(new List<uint> { 50003U }, true, true);
                sp.WaitSec(0.2f, true);
                List<string> list15 = Util.Split(sp.m_rs.GetScenarioStellaText(0, 52, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list15, false, false, true, true);
                sp.PlayBgmInScript("bgm18", 0f, 0.85f, -1, true);
                sp.LoadSound("se_syu_02");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list15 = Util.Split(sp.m_rs.GetScenarioStellaText(0, 53, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list15, false, true, false, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.WaitSec(0.05f, false);
                int mainP = ad.mainP;
                Plugin.CheckLocationsInScript(402);
                gd.baseData.gainExp += 50U;
                sp.LoadTexture("p05a");
                sp.WaitLoad();
                sp.SetCopyScreen(new List<uint> { 0U, 20U }, 30U, true);
                sp.UnloadTexture("p04a");
                sp.UnloadPlace();
                sp.LoadPlace(8, false);
                sp.WaitLoad();
                sp.StopBgmInScript(-1, false);
                sp.PlaySeInScript("se_syu_02", 1f);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.ChangeCharaTexture(5U, "p05a", 10U, 20U, true);
                    return true;
                }, (float e) => true, false));
                sp.ShowChara(ad.targetP, 4, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(240f, 80f, 480f, 270f), 0f, 1f, true, null, true);
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.25f, 4, true, true, true);
                list15 = Util.Split(sp.m_rs.GetScenarioStellaText(0, 55, 4), new char[] { '|' });
                string text2 = list15[0];
                Util.Replace(ref text2, "{0}", gd.takashiName);
                list15[0] = text2;
                sp.SetNormalSerifu(ad.targetP, 0, 1, list15, false, true, true, true);
                sp.WaitText(50U, "test", false);
                sp.PlayBgmInScript("bgm02", 0f, 0.85f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list15 = Util.Split(sp.m_rs.GetScenarioStellaText(0, 56, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 1, list15, true, true, false, true);
                list15 = Util.Split(sp.m_rs.GetScenarioStellaText(0, 57, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list15, true, true, false, true);
                list15 = Util.Split(sp.m_rs.GetScenarioStellaText(0, 58, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 1, list15, true, true, false, true);
                sp.FadeBgmInScript(-1f, 0f, 2f, false, -1);
                list15 = Util.Split(sp.m_rs.GetScenarioStellaText(0, 59, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 1, list15, false, true, false, true);
                sp.LoadTexture("ivep05_01_0");
                sp.WaitLoad();
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.m_sb[50U].m_textAreaMap["test"].nowLine >= 1, true));
                sp.PlaySeInScript("se_Kinzoku_02", 1f);
                sp.FadeBgmInScript(0f, 0.85f, 1.6f, false, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.25f, 0, false, false, true);
                sp.UnvisibleAllChara(20U, -1);
                sp.ShowChara(ad.targetP, 5, 1, 20U, false);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.4f, 0, false, false, true);
                sp.UnvisibleAllChara(20U, -1);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, delegate (float e)
                {
                    sp.SetCharaSingleTexture(10000, "ivep05_01_0", 1U, -40f, 20U);
                    return true;
                }, true));
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list15 = Util.Split(sp.m_rs.GetScenarioStellaText(0, 60, 0), new char[] { '|' });
                text2 = list15[0];
                Util.Replace(ref text2, "{0}", gd.takashiName);
                list15[0] = text2;
                sp.SetNormalSerifu(ad.targetP, -1, 1, list15, true, true, true, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 2.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.UnloadTexture("p05a");
                sp.UnloadTexture("ivep05_01_0");
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(0.4f, true);
                sp.StopAllSeInScript();
                sp.UnloadSound("se_syu_02");
                gd.forwardNext = true;
            };
            __instance.actions[16] = action;
        }


        [HarmonyPatch(typeof(Stella2Scenario), "SetParam")]
        [HarmonyPostfix]
        static void Flowers(Stella2Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[5];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 3.5f, true, -1);
                sp.WaitSec(0.2f, true);
                sp.SetFadeScreen(new List<uint> { 6U, 50U }, 40002U, 1.5f, 0, false, true, true);
                sp.UnloadTexture("ivep04_01_4");
                sp.UnloadTexture("ivep04_01_4_1");
                sp.UnloadTexture("ivep04_01_4_2");
                sp.WaitSec(1.2f, true);
                int mainP = ad.mainP;
                Plugin.CheckLocationsInScript(403);
                gd.baseData.gainExp += 50U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(0.8f, true);
                gd.forwardNext = true;
            };
            __instance.actions[5] = action;
        }


        [HarmonyPatch(typeof(Stella3Scenario), "SetParam")]
        [HarmonyPostfix]
        static void TearsGoBy(Stella3Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[2];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Get Other Stuff
                Type dataType = AccessTools.TypeByName("gnosia.Data");
                Array chara = (Array)AccessTools.Field(dataType, "Chara").GetValue(null);
                //Base
                sd.flg |= 16384;
                ScenarioContents.ChangeSceOnFlg(ref gd, ref sd, 2U);
                sp.WaitSec(0.45f, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Interface, 50U, true, false, -1);
                    return true;
                }, (float e) => true, true));
                sp.ShowChara(ad.targetP, 6, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(240f, 80f, 480f, 270f), 0f, 1f, true, null, true);
                sp.SetFadeScreen(new List<uint> { 50001U, 50002U }, 50003U, 0.3f, 0, true, true, true);
                List<string> list3 = Util.Split(sp.m_rs.GetScenarioStellaText(2, 8, 6), new char[] { '|' });
                string text2 = list3[0];
                Util.Replace(ref text2, "{1}", GetCharaName(chara, gd, ad.counterP));
                list3[0] = text2;
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 1, list3, false, true, true, true);
                sp.WaitSec(0.4f, true);
                sp.PlayBgmInScript("bgm15", 0f, 1f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list3 = Util.Split(sp.m_rs.GetScenarioStellaText(2, 9, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 2, list3, true, false, false, true);
                if (gd.chara[ad.mainP].p_rate[ad.counterP] > 0f)
                {
                    list3 = Util.Split(sp.m_rs.GetScenarioStellaText(2, 10, 4), new char[] { '|' });
                }
                else
                {
                    list3 = Util.Split(sp.m_rs.GetScenarioStellaText(2, 11, 4), new char[] { '|' });
                }
                text2 = list3[0];
                Util.Replace(ref text2, "{1}", GetCharaName(chara, gd, ad.counterP));
                list3[0] = text2;
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 1, list3, true, false, false, true);
                list3 = Util.Split(sp.m_rs.GetScenarioStellaText(2, 12, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 2, list3, true, false, false, true);
                list3 = Util.Split(sp.m_rs.GetScenarioStellaText(2, 13, 1), new char[] { '|' });
                text2 = list3[0];
                Util.Replace(ref text2, "{1}", GetCharaName(chara, gd, ad.counterP));
                list3[0] = text2;
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 1, list3, true, false, false, true);
                list3 = Util.Split(sp.m_rs.GetScenarioStellaText(2, 14, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 2, list3, true, false, false, true);
                list3 = Util.Split(sp.m_rs.GetScenarioStellaText(2, 15, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 1, list3, true, false, false, true);
                list3 = Util.Split(sp.m_rs.GetScenarioStellaText(2, 16, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 1, list3, false, true, false, true);
                sp.LoadTexture("ivep04_02_0");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.25f, 0, false, false, true);
                sp.UnvisibleAllChara(20U, -1);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetCharaSingleTexture(10000, "ivep04_02_0", 2U, 0f, 20U);
                    return true;
                }, (float e) => true, false));
                sp.SetNormalClipAnim(2);
                sp.WaitClipAnim(new List<uint> { 0U, 20U }, true);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list3 = Util.Split(sp.m_rs.GetScenarioStellaText(2, 17, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 2, list3, true, true, true, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 1.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.UnloadTexture("ivep04_02_0");
                sp.WaitSec(0.7f, true);
                Plugin.CheckLocationsInScript(6);
                gd.baseData.gainExp += 50U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioStellaText(2, 19, -1), 3, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.WaitSec(0.4f, true);
                gd.forwardNext = true;
            };
            __instance.actions[2] = action;
        }


        [HarmonyPatch(typeof(Stella4Scenario), "SetParam")]
        [HarmonyPostfix]
        static void JonasTheWreck(Stella4Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[7];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                List<string> list6 = Util.Split(sp.m_rs.GetScenarioStellaText(3, 45, 0), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, 1, list6, true, true, true, true);
                list6 = Util.Split(sp.m_rs.GetScenarioStellaText(3, 46, 1), new char[] { '|' });
                string text4 = list6[0];
                Util.Replace(ref text4, "{0}", gd.takashiName);
                list6[0] = text4;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list6, true, false, false, true);
                list6 = Util.Split(sp.m_rs.GetScenarioStellaText(3, 47, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, ad.mainP, 0, list6, false, false, false, true);
                sp.PlayBgmInScript("bgm10", 0.2f, 0.85f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list6 = Util.Split(sp.m_rs.GetScenarioStellaText(3, 48, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.counterP, 1, list6, true, false, false, true);
                list6 = Util.Split(sp.m_rs.GetScenarioStellaText(3, 49, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, ad.mainP, 0, list6, true, false, false, true);
                sp.FadeBgmInScript(-1f, 0f, 0.35f, true, -1);
                list6 = Util.Split(sp.m_rs.GetScenarioStellaText(3, 50, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.counterP, 1, list6, true, false, false, true);
                sp.PlayBgmInScript("bgm00", 1.2f, 0.85f, -1, true);
                if ((ad.tuizuiP & 2048) > 0)
                {
                    list6 = Util.Split(sp.m_rs.GetScenarioStellaText(3, 51, 4), new char[] { '|' });
                    sp.SetNormalSerifu(gd.personFromId[11], ad.mainP, 2, list6, true, false, false, true);
                }
                list6 = Util.Split(sp.m_rs.GetScenarioStellaText(3, 52, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 0, list6, true, false, false, true);
                list6 = Util.Split(sp.m_rs.GetScenarioStellaText(3, 53, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list6, false, false, false, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list6 = Util.Split(sp.m_rs.GetScenarioStellaText(3, 54, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 0, list6, false, false, false, true);
                sp.LoadTexture("ivep04_03_02");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.25f, 0, false, false, true);
                sp.UnvisibleAllChara(20U, -1);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetCharaSingleTexture(10000, "ivep04_03_02", 1U, 0f, 20U);
                    return true;
                }, (float e) => true, false));
                sp.SetNormalClipAnim(1);
                sp.WaitClipAnim(new List<uint> { 0U, 20U }, true);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.FadeBgmInScript(0f, 0.35f, 1.4f, false, -1);
                sp.PlaySeInScript("se_se_03", 1f);
                list6 = Util.Split(sp.m_rs.GetScenarioStellaText(3, 55, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 1, list6, false, true, true, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.25f, 0, false, false, true);
                sp.SetVisible(20U, 10000, false);
                sp.ShowChara(ad.targetP, 3, 0, 20U, false);
                sp.SetNormalClipAnim(0);
                sp.LoadTexture("ivep04_03_03");
                sp.WaitLoad();
                sp.WaitClipAnim(new List<uint> { 0U, 20U }, true);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list6 = Util.Split(sp.m_rs.GetScenarioStellaText(3, 56, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 0, list6, true, true, true, true);
                sp.FadeBgmInScript(-1f, 0f, 1.2f, true, -1);
                sp.SetNormalClipAnim(-1);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_LeviMes, 35U, true, false, -1);
                    return true;
                }, (float e) => true, true));
                sp.WaitClipAnim(new List<uint> { 0U, 20U }, true);
                sp.WaitSec(0.25f, true);
                sp.PlaySeInScript("se_square", 1f);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 40U, false, false, -1);
                    sp.m_sb[40U].SetTexture(0, sp.m_sb[40U].gameObject.transform, 0U, "ivep04_03_03", null, null);
                    sp.m_sb[40U].m_spriteMap[0U].SetVisible(true);
                    sp.m_sb[40U].SetAlphaCoeff(0f);
                    sp.m_sb[40U].SetFade(0.35f, 1f, 0, 0f, -1, false);
                    return true;
                }, (float e) => true, false));
                sp.SetClipAnim(new List<uint> { 40U }, new Vector4(0f, 0f, (float)sp.m_rs.m_displaySize.width, (float)sp.m_rs.m_displaySize.height), 0.35f, -4f, true, new Vector4?(new Vector4(0f, -30000f, (float)sp.m_rs.m_displaySize.width, (float)(sp.m_rs.m_displaySize.height + 60000))), false);
                sp.WaitFade(new List<uint> { 40U }, false, true);
                sp.RemoveScreenInScript(20U);
                sp.RemoveScreenInScript(0U);
                sp.UnloadTexture("ivep04_03_02");
                sp.WaitSec(1f, true);
                sp.PlayBgmInScript("bgm23", 0f, 0.8f, -1, true);
                list6 = Util.Split(sp.m_rs.GetScenarioStellaText(3, 57, 0), new char[] { '|' });
                sp.SetNormalSerifu(-2, -1, 1, list6, true, true, true, false);
                list6 = Util.Split(sp.m_rs.GetScenarioStellaText(3, 58, 0), new char[] { '|' });
                sp.SetNormalSerifu(-2, -1, 1, list6, true, true, true, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    return true;
                }, (float e) => true, false));
                sp.ShowChara(ad.counterP, 5, 0, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(0f, 80f, 480f, 270f), 0f, 1f, true, null, true);
                sp.SetFadeScreen(new List<uint> { 35U, 40U }, 41U, 0.1f, 0, true, true, true);
                sp.UnloadTexture("ivep04_03_03");
                list6 = Util.Split(sp.m_rs.GetScenarioStellaText(3, 59, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, ad.mainP, 0, list6, true, true, true, true);
                if ((ad.tuizuiP & 4) > 0)
                {
                    list6 = Util.Split(sp.m_rs.GetScenarioStellaText(3, 60, 4), new char[] { '|' });
                    sp.SetNormalSerifu(gd.personFromId[2], ad.counterP, 2, list6, true, false, false, true);
                }
                list6 = Util.Split(sp.m_rs.GetScenarioStellaText(3, 61, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list6, true, false, false, true);
                if ((ad.tuizuiP & 8) > 0)
                {
                    list6 = Util.Split(sp.m_rs.GetScenarioStellaText(3, 62, 5), new char[] { '|' });
                    sp.SetNormalSerifu(gd.personFromId[3], ad.mainP, 2, list6, true, false, false, true);
                }
                else if ((ad.tuizuiP & 128) > 0)
                {
                    list6 = Util.Split(sp.m_rs.GetScenarioStellaText(3, 63, 4), new char[] { '|' });
                    sp.SetNormalSerifu(gd.personFromId[7], ad.mainP, 2, list6, true, false, false, true);
                }
                list6 = Util.Split(sp.m_rs.GetScenarioStellaText(3, 64, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list6, true, false, false, true);
                sp.PlayBgmInScript("bgm03", 0.4f, 1f, -1, true);
                list6 = Util.Split(sp.m_rs.GetScenarioStellaText(3, 65, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list6, true, true, false, true);
                list6 = Util.Split(sp.m_rs.GetScenarioStellaText(3, 66, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, ad.mainP, 0, list6, true, false, false, true);
                list6 = Util.Split(sp.m_rs.GetScenarioStellaText(3, 67, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list6, true, false, false, true);
                list6 = Util.Split(sp.m_rs.GetScenarioStellaText(3, 68, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.counterP, ad.mainP, 0, list6, true, false, false, true);
                sp.FadeBgmInScript(-1f, 0f, 1.8f, true, -1);
                sp.SetNormalClipAnim(-1);
                int mainP = ad.mainP;
                Plugin.CheckLocationsInScript(405);
                gd.baseData.gainExp += 50U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(0.4f, true);
                gd.forwardNext = true;
            };
            __instance.actions[7] = action;
        }


        [HarmonyPatch(typeof(Yuriko1Scenario), "SetParam")]
        [HarmonyPostfix]
        static void Chaos(Yuriko1Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[2];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.LoadTexture("ivep09_00_2");
                sp.WaitLoad();
                sp.FadeBgmInScript(-1f, 1f, 0.4f, false, -1);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.4f, 0, false, true, true);
                sp.UnloadTexture("ivep09_00_1");
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, true, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep09_00_2", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, false));
                sp.WaitFade(new List<uint> { 30U }, true, true);
                List<string> list2 = Util.Split(sp.m_rs.GetScenarioYurikoText(0, 9, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list2, true, true, true, false);
                sp.FadeBgmInScript(-1f, 0.6f, 0.8f, false, -1);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.4f, 0, false, true, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, true, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep09_00_0", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, false));
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list2 = Util.Split(sp.m_rs.GetScenarioYurikoText(0, 10, 4), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, 1, list2, true, true, true, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 3f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadTexture("ivep09_00_0");
                sp.UnloadTexture("ivep09_00_2");
                sp.WaitSec(0.7f, false);
                Plugin.CheckLocationsInScript(12);
                gd.baseData.gainExp += 50U;
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioYurikoText(0, 12, -1), 3, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.WaitSec(0.2f, true);
                gd.forwardNext = true;
            };
            __instance.actions[2] = action;
            action = __instance.actions[6];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.WaitSec(1f, true);
                sp.FadeBgmInScript(-1f, 0f, 0.25f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.25f, 0, false, true, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, true, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep09_00_2", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, false));
                sp.WaitFade(new List<uint> { 30U }, true, true);
                List<string> list6 = Util.Split(sp.m_rs.GetScenarioYurikoText(0, 20, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list6, false, true, true, false);
                sp.WaitText(50U, "test", true);
                Plugin.CheckLocationsInScript(12);
                gd.baseData.gainExp += 50U;
                sp.WaitSec(0.2f, true);
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioYurikoText(0, 22, -1), 3, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.HideInterface(50U, true);
                sp.WaitSec(0.4f, true);
                sp.SetFadeScreen(new List<uint> { 0U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadTexture("ivep09_00_0");
                sp.UnloadTexture("ivep09_00_2");
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(0.4f, true);
                gd.forwardNext = true;
            };
            __instance.actions[6] = action;
        }


        [HarmonyPatch(typeof(Yuriko2Scenario), "SetParam")]
        [HarmonyPostfix]
        static void StarshipOracle(Yuriko2Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[11];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                List<string> list10 = Util.Split(sp.m_rs.GetScenarioYurikoText(1, 68, 2), new char[] { '|' });
                string text6 = list10[0];
                list10[0] = text6;
                sp.SetNormalSerifu(ad.targetP, -1, 1, list10, true, false, false, true);
                sp.FadeBgmInScript(-1f, 0.6f, 2f, false, -1);
                list10 = Util.Split(sp.m_rs.GetScenarioYurikoText(1, 69, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 1, list10, true, true, false, true);
                list10 = Util.Split(sp.m_rs.GetScenarioYurikoText(1, 70, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 0, list10, true, false, false, true);
                list10 = Util.Split(sp.m_rs.GetScenarioYurikoText(1, 71, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 1, list10, true, false, false, true);
                list10 = Util.Split(sp.m_rs.GetScenarioYurikoText(1, 72, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 0, list10, true, false, false, true);
                sp.FadeBgmInScript(-1f, 0f, 3f, true, -1);
                list10 = Util.Split(sp.m_rs.GetScenarioYurikoText(1, 73, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 1, list10, true, false, false, true);
                list10 = Util.Split(sp.m_rs.GetScenarioYurikoText(1, 74, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 0, list10, true, false, false, true);
                sp.PlayBgmInScript("bgm16", 0.6f, 0.5f, -1, true);
                list10 = Util.Split(sp.m_rs.GetScenarioYurikoText(1, 75, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 1, list10, true, false, false, true);
                list10 = Util.Split(sp.m_rs.GetScenarioYurikoText(1, 76, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 1, list10, true, true, false, true);
                list10 = Util.Split(sp.m_rs.GetScenarioYurikoText(1, 77, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 0, list10, true, false, false, true);
                list10 = Util.Split(sp.m_rs.GetScenarioYurikoText(1, 78, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 1, list10, true, false, false, true);
                list10 = Util.Split(sp.m_rs.GetScenarioYurikoText(1, 79, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 1, list10, true, true, false, true);
                sp.FadeBgmInScript(-1f, 0.2f, 0.4f, false, -1);
                sp.WaitSec(0.05f, false);
                int targetP = ad.targetP;
                Plugin.CheckLocationsInScript(902, 903);
                gd.baseData.gainExp += 100U;
                sp.WaitSec(0.4f, true);
                list10 = Util.Split(sp.m_rs.GetScenarioYurikoText(1, 81, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 1, list10, true, true, false, true);
                list10 = Util.Split(sp.m_rs.GetScenarioYurikoText(1, 82, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 0, list10, true, false, false, true);
                sp.FadeBgmInScript(-1f, 1f, 0.6f, false, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.25f, 0, false, true, true);
                sp.UnloadTexture("p01a");
                sp.UnloadPlace();
                sp.LoadTexture("ivep09_01_2");
                sp.WaitLoad();
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, true, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep09_01_2", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.SetColorScreen(255U, 30U, -1);
                    sp.m_sb[30U].SetFade(0.35f, 0f, 0, -1f, -1, false);
                    return true;
                }, (float e) => true, true));
                sp.SetClipAnim(new List<uint> { 0U }, new Vector4(0f, 0f, 960f, 540f), 4f, 1f, false, new Vector4?(new Vector4(0f, 236f, 960f, 540f)), true);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(2.5f, false);
                list10 = Util.Split(sp.m_rs.GetScenarioYurikoText(1, 83, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 1, list10, false, true, true, false);
                sp.WaitClipAnim(new List<uint> { 0U }, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.WaitSec(0.05f, false);
                sp.StopBgmInScript(-1, false);
                sp.PlaySeInScript("se_noiseB", 1f);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetColorScreen(4294967167U, 40U, -1);
                    return true;
                }, (float e) => true, false));
                sp.SetFadeScreen(new List<uint> { 0U, 40U, 50U }, 50000U, 0.2f, 7, true, true, true);
                sp.UnloadTexture("ivep09_01_2");
                sp.WaitSec(1.6f, true);
                gd.forwardNext = true;
            };
            __instance.actions[11] = action;
        }


        [HarmonyPatch(typeof(Yuriko3Scenario), "SetParam")]
        [HarmonyPostfix]
        static void Confrontation(Yuriko3Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[22];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                List<string> list21 = Util.Split(sp.m_rs.GetScenarioYurikoText(2, 102, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list21, false, false, false, true);
                sp.FadeBgmInScript(-1f, 0.65f, 1.2f, false, -1);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list21 = Util.Split(sp.m_rs.GetScenarioYurikoText(2, 103, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list21, true, true, false, true);
                list21 = Util.Split(sp.m_rs.GetScenarioYurikoText(2, 104, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 2, list21, true, false, false, true);
                list21 = Util.Split(sp.m_rs.GetScenarioYurikoText(2, 105, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 2, list21, true, true, false, true);
                list21 = Util.Split(sp.m_rs.GetScenarioYurikoText(2, 106, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 1, list21, true, false, false, true);
                sp.FadeBgmInScript(-1f, 0f, 2f, true, -1);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_LeviMes, 35U, true, false, -1);
                    return true;
                }, (float e) => true, true));
                list21 = Util.Split(sp.m_rs.GetScenarioYurikoText(2, 107, -1), new char[] { '|' });
                sp.SetNormalSerifu(-2, -1, -1, list21, true, false, true, true);
                sp.PlayBgmInScript("bgm10", 2f, 0.6f, -1, true);
                list21 = Util.Split(sp.m_rs.GetScenarioYurikoText(2, 108, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list21, true, false, false, true);
                list21 = Util.Split(sp.m_rs.GetScenarioYurikoText(2, 109, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 2, list21, true, false, false, true);
                sp.RemoveScreenInScript(35U);
                list21 = Util.Split(sp.m_rs.GetScenarioYurikoText(2, 110, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list21, true, false, false, true);
                list21 = Util.Split(sp.m_rs.GetScenarioYurikoText(2, 111, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 2, list21, true, false, false, true);
                list21 = Util.Split(sp.m_rs.GetScenarioYurikoText(2, 112, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list21, true, false, false, true);
                list21 = Util.Split(sp.m_rs.GetScenarioYurikoText(2, 113, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list21, true, true, false, true);
                list21 = Util.Split(sp.m_rs.GetScenarioYurikoText(2, 114, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 2, list21, true, false, false, true);
                list21 = Util.Split(sp.m_rs.GetScenarioYurikoText(2, 115, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list21, true, false, false, true);
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 0.8f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 50001U, 0.1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.WaitFade(new List<uint> { 50001U }, true, false);
                sp.ShowColdDialogue();
                if (gd.baseData.shokeiList[ad.targetP] != gd.baseData.day - 1 && gd.baseData.shokeiList[0] != gd.baseData.day - 1)
                {
                    sp.LoadPlace(32, true);
                    sp.WaitLoad();
                    sp.LoadTexture("p11a");
                    sp.WaitLoad();
                    sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                    {
                        sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                        sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                        sp.ChangeCharaTexture(11U, "p11a", 10U, 20U, true);
                        sp.SetScreen(Setting.Screen.s_Interface, 50U, true, false, -1);
                        sp.SetColorScreen(255U, 50000U, -1);
                        return true;
                    }, (float e) => true, true));
                    sp.ShowChara(ad.targetP, 2, 1, 20U, false);
                    sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(120f, 40f, 720f, 405f), 0f, 1f, true, null, true);
                    sp.SetFadeScreen(new List<uint> { 50000U }, 50001U, 0.4f, 0, true, true, true);
                    sp.WaitSec(0.4f, true);
                    list21 = Util.Split(sp.m_rs.GetScenarioYurikoText(2, 116, 2), new char[] { '|' });
                    sp.SetNormalSerifu(ad.targetP, -1, 1, list21, true, true, true, true);
                    list21 = Util.Split(sp.m_rs.GetScenarioYurikoText(2, 117, 7), new char[] { '|' });
                    sp.SetNormalSerifu(ad.targetP, 0, 1, list21, false, false, false, true);
                    sp.PlayBgmInScript("bgm03", 0f, 0.8f, -1, true);
                    sp.WaitText(50U, "test", true);
                    sp.HideInterface(50U, true);
                    list21 = Util.Split(sp.m_rs.GetScenarioYurikoText(2, 118, 7), new char[] { '|' });
                    string text11 = list21[0];
                    Util.Replace(ref text11, "{0}", gd.takashiName);
                    list21[0] = text11;
                    sp.SetNormalSerifu(ad.targetP, 0, 1, list21, true, true, true, true);
                    sp.WaitSec(0.05f, false);
                    sp.FadeBgmInScript(-1f, 0f, 2.5f, true, -1);
                    sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 30U, 1f, 0, false, true, true);
                    sp.UnloadTexture("p11a");
                    sp.UnloadPlace();
                }
                sp.WaitSec(0.7f, true);
                int mainP = ad.mainP;
                Plugin.CheckLocationsInScript(904);
                gd.baseData.gainExp += 400U;
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(0.8f, true);
                gd.forwardNext = true;
            };
            __instance.actions[22] = action;
        }


        [HarmonyPatch(typeof(Yuriko4Scenario), "SetParam")]
        [HarmonyPostfix]
        static void TheAlienGnos(Yuriko4Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[14];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                List<string> list12 = Util.Split(sp.m_rs.GetScenarioYurikoText(3, 59, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list12, true, false, false, true);
                list12 = Util.Split(sp.m_rs.GetScenarioYurikoText(3, 60, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list12, true, true, false, true);
                list12 = Util.Split(sp.m_rs.GetScenarioYurikoText(3, 61, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list12, true, true, false, true);
                sp.FadeBgmInScript(-1f, 0.15f, 3.5f, false, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.6f, 0, false, true, true);
                sp.UnloadPlace();
                sp.LoadTexture("ivep09_03_0");
                sp.WaitLoad();
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list12 = Util.Split(sp.m_rs.GetScenarioYurikoText(3, 62, 0), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, 1, list12, false, true, true, true);
                sp.LoadTexture("ivep09_02_0_1");
                sp.WaitLoad();
                sp.LoadTexture("ivep09_02_0_2");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.WaitSec(0.05f, false);
                int mainP = ad.mainP;
                Plugin.CheckLocationsInScript(905);
                gd.baseData.gainExp += 200U;
                sp.WaitSec(0.6f, true);
                sp.FadeBgmInScript(-1f, 0.6f, 2f, false, -1);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, true, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep09_03_0", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 10U, "ivep09_02_0_1", new Vector2?(new Vector2(361.5f, 49.5f)), null);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 20U, "ivep09_02_0_2", new Vector2?(new Vector2(361.5f, 49.5f)), null);
                    sp.SetColorScreen(255U, 40U, -1);
                    return true;
                }, (float e) => true, true));
                sp.SetFadeScreen(new List<uint> { 40U }, 41U, 0.4f, 0, true, true, true);
                sp.WaitSec(0.2f, true);
                list12 = Util.Split(sp.m_rs.GetScenarioYurikoText(3, 64, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list12, true, true, true, false);
                sp.WaitSec(0.2f, true);
                list12 = Util.Split(sp.m_rs.GetScenarioYurikoText(3, 65, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list12, true, true, true, false);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.25f, 0, false, false, true);
                sp.SetVisible(0U, 20, true);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(0.2f, true);
                list12 = Util.Split(sp.m_rs.GetScenarioYurikoText(3, 66, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list12, true, true, true, false);
                list12 = Util.Split(sp.m_rs.GetScenarioYurikoText(3, 67, 0), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, 1, list12, true, true, true, false);
                sp.FadeBgmInScript(-1f, 0f, 1.2f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.25f, 0, false, false, true);
                sp.SetVisible(0U, 10, true);
                sp.SetVisible(0U, 20, false);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list12 = Util.Split(sp.m_rs.GetScenarioYurikoText(3, 68, 4), new char[] { '|' });
                string text8 = list12[0];
                Util.Replace(ref text8, "{0}", gd.takashiName);
                list12[0] = text8;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list12, true, true, true, false);
                sp.PlayBgmInScript("bgm19", 2f, 0.5f, -1, true);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.25f, 0, false, false, true);
                sp.SetClipAnim(new List<uint> { 0U }, new Vector4(240f, 0f, 480f, 270f), 0f, 1f, true, null, true);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(0.2f, true);
                list12 = Util.Split(sp.m_rs.GetScenarioYurikoText(3, 69, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list12, false, true, true, false);
                sp.LoadTexture("ivep09_01_2");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.FadeBgmInScript(-1f, 1f, 0.4f, false, -1);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.25f, 0, false, true, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, true, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep09_01_2", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, false));
                sp.SetClipAnim(new List<uint> { 0U }, new Vector4(390f, 0f, 480f, 270f), 0f, 1f, true, null, true);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(0.2f, true);
                list12 = Util.Split(sp.m_rs.GetScenarioYurikoText(3, 70, 4), new char[] { '|' });
                text8 = list12[0];
                Util.Replace(ref text8, "{0}", gd.takashiName);
                list12[0] = text8;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list12, true, true, true, false);
                sp.FadeBgmInScript(-1f, 0f, 0.1f, true, -1);
                sp.WaitSec(0.01f, false);
                sp.PlaySeInScript("se_noiseB", 1f);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetColorScreen(4294967167U, 40U, -1);
                    return true;
                }, (float e) => true, false));
                sp.SetFadeScreen(new List<uint> { 0U, 40U, 50U }, 50000U, 0.2f, 7, true, true, true);
                sp.UnloadTexture("ivep09_01_2");
                sp.UnloadTexture("ivep09_03_0");
                sp.UnloadTexture("ivep09_02_0_1");
                sp.UnloadTexture("ivep09_02_0_2");
                sp.WaitSec(1.5f, true);
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioYurikoText(3, 71, -1), 1, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                gd.forwardNext = true;
            };
            __instance.actions[14] = action;
        }


        [HarmonyPatch(typeof(RECipi0Scenario), "SetParam")]
        [HarmonyPostfix]
        static void Chipie2Gnosia(RECipi0Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[1];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioEndingText(0, 2, -1), 1, false);
                sp.LoadPlace(7, true);
                sp.WaitLoad();
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Interface, 50U, true, false, -1);
                    sp.SetColorScreen(255U, 50000U, -1);
                    sp.m_sb[50000U].SetFade(0.7f, 0f, 0, -1f, -1, false);
                    return true;
                }, (float e) => true, false));
                sp.ShowChara(ad.mainP, 5, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(120f, 40f, 720f, 405f), 0f, 1f, true, null, true);
                sp.PlayBgmInScript("bgm03", 0f, 0.6f, -1, true);
                sp.WaitFade(new List<uint> { 50000U }, true, true);
                List<string> list = Util.Split(sp.m_rs.GetScenarioEndingText(0, 3, 5), new char[] { '|' });
                string text = list[0];
                list[0] = text;
                sp.SetNormalSerifu(ad.mainP, -1, 1, list, true, true, true, true);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(0, 4, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list, true, true, false, true);
                sp.FadeBgmInScript(-1f, 0f, 1.5f, true, -1);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(0, 5, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list, true, true, false, true);
                sp.StopBgmInScript(-1, false);
                sp.PlaySeInScript("se_noiseB", 1f);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetColorScreen(uint.MaxValue, 40U, -1);
                    return true;
                }, (float e) => true, false));
                sp.SetFadeScreen(new List<uint> { 40U }, 45U, 0.2f, 7, false, true, true);
                sp.PlayBgmInScript("bgm19", 0.4f, 0.3f, -1, true);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(0, 6, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list, true, false, false, true);
                sp.WaitFade(new List<uint> { 45U }, true, true);
                sp.PlaySeInScript("se_noiseB", 1f);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetColorScreen(uint.MaxValue, 40U, -1);
                    return true;
                }, (float e) => true, false));
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4((float)sp.m_rs.m_displaySize.width * 0.25f * 1f, 160f, (float)sp.m_rs.m_displaySize.width * 0.5f, (float)sp.m_rs.m_displaySize.height * 0.5f), 0f, 1f, true, null, true);
                sp.SetFadeScreen(new List<uint> { 40U }, 45U, 0.2f, 7, false, true, true);
                sp.FadeBgmInScript(-1f, 1f, 0.4f, false, -1);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(0, 7, 7), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list, true, true, false, true);
                sp.WaitFade(new List<uint> { 45U }, true, true);
                sp.FadeBgmInScript(-1f, 0f, 3.5f, true, -1);
                sp.WaitSec(0.05f, true);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 30U, 1f, 5, false, true, true);
                sp.UnloadPlace();
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(0.4f, true);
                if (!ArchipelagoClient.ServerData.CheckedLocations.Contains(602))
                {
                    int mainP = ad.mainP;
                    Plugin.CheckLocationsInScript(602);
                    gd.baseData.gainExp += 50U;
                    sp.WaitSec(0.75f, true);
                }
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioEndingText(0, 9, -1), 2, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                gd.forwardNext = true;
            };
            __instance.actions[1] = action;
        }


        [HarmonyPatch(typeof(REComet1Scenario), "SetParam")]
        [HarmonyPostfix]
        static void Comet2(REComet1Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[1];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioEndingText(3, 2, -1), 1, false);
                sp.LoadPlace(15, true);
                sp.WaitLoad();
                sp.LoadTexture("p08a");
                sp.WaitLoad();
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_none, 20U, false, false, -1);
                    sp.SetScreen(Setting.Screen.s_Interface, 50U, true, false, -1);
                    sp.SetColorScreen(255U, 50000U, -1);
                    sp.m_sb[50000U].SetFade(0.4f, 0f, 0, -1f, -1, false);
                    uint num = 8U;
                    sp.m_sb[20U].SetPackedTexture(0, sp.m_sb[20U].gameObject.transform, "p08a", "body", 100U * num, 10U, new Vector2?(new Vector2((float)(18446744073709551416UL + (ulong)(50U * num)), 0f)), null, null, false);
                    sp.m_sb[20U].m_spriteMap[100U * num].GetComponent<Image>().material = sp.m_rs.uiCharaDefaultMat;
                    sp.m_sb[20U].m_spriteMap[100U * num].GetComponent<Image>().material.SetColor("_Color", Color.white);
                    sp.m_sb[20U].m_spriteMap[100U * num].SetSize(0.7f);
                    sp.m_sb[20U].m_spriteMap[100U * num].SetDisplayOffsetY((float)sp.m_rs.m_displaySize.height - sp.m_sb[20U].m_spriteMap[100U * num].GetSizeInDisplay().y * sp.m_sb[20U].m_spriteMap[100U * num].GetSize() * GraphicsContext.m_textureRatio);
                    return true;
                }, (float e) => true, true));
                sp.ShowChara(ad.mainP, 0, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(120f, 40f, 720f, 405f), 0f, 1f, true, null, true);
                sp.WaitFade(new List<uint> { 50000U }, true, true);
                List<string> list = Util.Split(sp.m_rs.GetScenarioEndingText(3, 3, 0), new char[] { '|' });
                string text = list[0];
                list[0] = text;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list, false, true, true, true);
                sp.WaitSec(0.3f, true);
                sp.PlayBgmInScript("bgm15", 0f, 0.6f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(3, 4, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list, true, true, true, true);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(3, 5, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list, true, true, false, true);
                sp.WaitSec(0.4f, true);
                if (gd.GetFriend(ad.mainP, 0) >= 0.15f)
                {
                    list = Util.Split(sp.m_rs.GetScenarioEndingText(3, 6, 3), new char[] { '|' });
                }
                else
                {
                    list = Util.Split(sp.m_rs.GetScenarioEndingText(3, 7, 3), new char[] { '|' });
                }
                text = list[0];
                Util.Replace(ref text, "{0}", gd.takashiName);
                list[0] = text;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list, true, true, false, true);
                sp.FadeBgmInScript(-1f, 0.85f, 0.3f, false, -1);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(3, 8, 2), new char[] { '|' });
                text = list[0];
                Util.Replace(ref text, "{0}", gd.takashiName);
                list[0] = text;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list, true, true, false, true);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(3, 9, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list, true, true, false, true);
                sp.FadeBgmInScript(-1f, 0.4f, 1f, false, -1);
                sp.WaitSec(0.01f, false);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 30U, 0.8f, 0, false, true, true);
                sp.UnloadPlace();
                sp.UnloadTexture("p08a");
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(0.2f, true);
                if (!ArchipelagoClient.ServerData.CheckedLocations.Contains(802)) //Changed condition
                {
                    int mainP = ad.mainP;
                    Plugin.CheckLocationsInScript(802);
                    gd.baseData.gainExp += 50U;
                    sp.WaitSec(0.4f, true);
                }
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioEndingText(3, 11, -1), 2, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.FadeBgmInScript(-1f, 0f, 2.5f, true, -1);
                sp.WaitSec(0.5f, true);
                gd.forwardNext = true;
            };
            __instance.actions[1] = action;
        }


        [HarmonyPatch(typeof(REGina1Scenario), "SetParam")]
        [HarmonyPostfix]
        static void Gina2(REGina1Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[1];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioEndingText(5, 2, -1), 1, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.LoadTexture("ive001_0");
                sp.WaitLoad();
                sp.LoadTexture("ive001_0_1");
                sp.WaitLoad();
                sp.LoadTexture("ive001_0_2");
                sp.WaitLoad();
                sp.WaitSec(0.5f, true);
                sp.PlayBgmInScript("bgm06", 0f, 0.75f, -1, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ive001_0", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 10U, "ive001_0_1", new Vector2?(new Vector2(198.75f, 54.75f)), null);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 20U, "ive001_0_2", new Vector2?(new Vector2(198.75f, 54.75f)), null);
                    sp.SetScreen(Setting.Screen.s_Interface, 50U, true, false, -1);
                    sp.SetColorScreen(255U, 50000U, -1);
                    sp.m_sb[50000U].SetFade(0.8f, 0f, 0, -1f, -1, false);
                    return true;
                }, (float e) => true, false));
                sp.WaitFade(new List<uint> { 50000U }, true, true);
                sp.WaitSec(0.65f, true);
                List<string> list = Util.Split(sp.m_rs.GetScenarioEndingText(5, 3, 0), new char[] { '|' });
                string text = list[0];
                list[0] = text;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list, false, true, true, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.WaitSec(0.01f, false);
                sp.SetFadeScreen(new List<uint> { 0U }, 10U, 0.4f, 0, false, false, true);
                sp.SetVisible(0U, 10, true);
                sp.WaitFade(new List<uint> { 10U }, true, true);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(5, 4, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list, true, true, true, true);
                sp.WaitSec(0.01f, false);
                sp.SetFadeScreen(new List<uint> { 0U }, 10U, 0.4f, 0, false, false, true);
                sp.SetVisible(0U, 10, false);
                sp.SetVisible(0U, 20, true);
                sp.WaitFade(new List<uint> { 10U }, true, true);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(5, 5, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list, true, true, true, true);
                sp.FadeBgmInScript(-1f, 1f, 0.8f, false, -1);
                sp.WaitSec(0.01f, false);
                sp.SetFadeScreen(new List<uint> { 0U }, 10U, 0.25f, 0, false, true, true);
                sp.UnloadTexture("ive001_0");
                sp.UnloadTexture("ive001_0_1");
                sp.UnloadTexture("ive001_0_2");
                sp.LoadTexture("ive001_1");
                sp.WaitLoad();
                sp.LoadTexture("ive001_1_4");
                sp.WaitLoad();
                sp.LoadTexture("ive001_1_5");
                sp.WaitLoad();
                sp.WaitFade(new List<uint> { 10U }, true, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ive001_1", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 40U, "ive001_1_4", new Vector2?(new Vector2(195.75f, 0f)), null);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 50U, "ive001_1_5", new Vector2?(new Vector2(195.75f, 0f)), null);
                    sp.m_sb[0U].m_spriteMap[40U].SetVisible(true);
                    sp.SetColorScreen(255U, 10U, -1);
                    sp.m_sb[10U].SetFade(0.35f, 0f, 0, -1f, -1, false);
                    return true;
                }, (float e) => true, false));
                sp.WaitFade(new List<uint> { 10U }, true, true);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(5, 6, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list, true, true, true, true);
                sp.WaitSec(0.01f, false);
                sp.SetFadeScreen(new List<uint> { 0U }, 10U, 0.4f, 0, false, false, true);
                sp.SetVisible(0U, 40, false);
                sp.SetVisible(0U, 50, true);
                sp.WaitFade(new List<uint> { 10U }, true, true);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(5, 7, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list, true, true, true, true);
                sp.WaitSec(0.01f, false);
                sp.SetFadeScreen(new List<uint> { 0U }, 10U, 0.4f, 0, false, false, true);
                sp.SetVisible(0U, 50, false);
                sp.WaitFade(new List<uint> { 10U }, true, true);
                sp.WaitSec(0.6f, true);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(5, 8, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list, true, true, true, true);
                sp.WaitSec(0.01f, false);
                sp.FadeBgmInScript(-1f, 0f, 1.2f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 50U }, 30U, 1f, 0, false, true, true);
                sp.UnloadTexture("ive001_1");
                sp.UnloadTexture("ive001_1_4");
                sp.UnloadTexture("ive001_1_5");
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(0.4f, true);
                int mainP = ad.mainP;
                Plugin.CheckLocationsInScript(102);
                gd.baseData.gainExp += 50U;
                sp.WaitSec(0.4f, true);
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioEndingText(5, 10, -1), 1, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.WaitSec(0.5f, true);
                gd.forwardNext = true;
            };
            __instance.actions[1] = action;
        }


        [HarmonyPatch(typeof(REJonas1Scenario), "SetParam")]
        [HarmonyPostfix]
        static void Jonas2(REJonas1Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[1];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioEndingText(7, 2, -1), 1, false);
                sp.LoadPlace(15, true);
                sp.WaitLoad();
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Interface, 50U, true, false, -1);
                    sp.SetColorScreen(255U, 50000U, -1);
                    sp.m_sb[50000U].SetFade(0.7f, 0f, 0, -1f, -1, false);
                    return true;
                }, (float e) => true, false));
                sp.ShowChara(ad.targetP, 1, 0, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(0f, 80f, 480f, 270f), 0f, 1f, false, null, true);
                sp.WaitFade(new List<uint> { 50000U }, true, true);
                sp.PlayBgmInScript("bgm18", 0f, 0.75f, -1, true);
                List<string> list = Util.Split(sp.m_rs.GetScenarioEndingText(7, 3, 1), new char[] { '|' });
                string text = list[0];
                list[0] = text;
                sp.SetNormalSerifu(ad.targetP, -1, 0, list, true, true, true, true);
                if (ad.counterP != 0)
                {
                    list = Util.Split(sp.m_rs.GetScenarioEndingText(7, 4, 1), new char[] { '|' });
                    sp.SetNormalSerifu(ad.counterP, ad.targetP, 1, list, true, false, false, true);
                }
                list = Util.Split(sp.m_rs.GetScenarioEndingText(7, 5, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 2, list, true, false, false, true);
                if (ad.counterP != 0)
                {
                    list = Util.Split(sp.m_rs.GetScenarioEndingText(7, 6, 0), new char[] { '|' });
                    sp.SetNormalSerifu(ad.counterP, ad.mainP, 1, list, true, false, false, true);
                    list = Util.Split(sp.m_rs.GetScenarioEndingText(7, 7, 1), new char[] { '|' });
                    sp.SetNormalSerifu(ad.targetP, ad.mainP, 0, list, true, false, false, true);
                }
                else
                {
                    list = Util.Split(sp.m_rs.GetScenarioEndingText(7, 8, 1), new char[] { '|' });
                    sp.SetNormalSerifu(ad.targetP, ad.mainP, 0, list, true, false, false, true);
                }
                list = Util.Split(sp.m_rs.GetScenarioEndingText(7, 9, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 2, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(7, 10, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 2, list, true, true, false, true);
                sp.FadeBgmInScript(-1f, 0.4f, 1f, false, -1);
                sp.WaitSec(0.01f, false);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 30U, 0.8f, 0, false, true, true);
                sp.UnloadPlace();
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(0.2f, true);
                if (!ArchipelagoClient.ServerData.CheckedLocations.Contains(1002)) //Changed condition
                {
                    int mainP = ad.mainP;
                    Plugin.CheckLocationsInScript(1002);
                    sp.WaitSec(0.4f, true);
                }
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioEndingText(7, 12, -1), 1, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.FadeBgmInScript(-1f, 0f, 2.5f, true, -1);
                sp.WaitSec(0.5f, true);
                gd.forwardNext = true;
            };
            __instance.actions[1] = action;
        }


        [HarmonyPatch(typeof(REOtome1Scenario), "SetParam")]
        [HarmonyPostfix]
        static void Otome2(REOtome1Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[1];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioEndingText(11, 2, -1), 1, false);
                sp.LoadPlace(22, true);
                sp.WaitLoad();
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Interface, 50U, true, false, -1);
                    sp.SetColorScreen(255U, 50000U, -1);
                    sp.m_sb[50000U].SetFade(0.7f, 0f, 0, -1f, -1, false);
                    return true;
                }, (float e) => true, false));
                sp.ShowChara(ad.mainP, 6, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(120f, 40f, 720f, 405f), 0f, 1f, true, null, true);
                sp.WaitFade(new List<uint> { 50000U }, true, true);
                sp.PlayBgmInScript("bgm18", 0f, 0.75f, -1, true);
                List<string> list = Util.Split(sp.m_rs.GetScenarioEndingText(11, 3, 6), new char[] { '|' });
                string text = list[0];
                list[0] = text;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list, false, true, true, true);
                sp.WaitSec(0.4f, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(11, 4, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list, true, true, false, true);
                sp.FadeBgmInScript(-1f, 0.45f, 1f, false, -1);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetColorScreen(255U, 30U, -1);
                    sp.m_sb[30U].SetFade(0.8f, 1f, 0, 0f, -1, false);
                    return true;
                }, (float e) => true, false));
                sp.WaitFade(new List<uint> { 30U }, false, true);
                sp.WaitSec(0.3f, true);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(11, 5, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list, true, true, true, false);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(11, 6, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list, true, true, true, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(240f, 80f, 480f, 270f), 0f, 1f, false, null, true);
                sp.FadeBgmInScript(-1f, 0.8f, 0.4f, false, -1);
                sp.SetFadeScreen(new List<uint> { 30U }, 35U, 0.4f, 0, true, true, true);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(11, 7, 1), new char[] { '|' });
                text = list[0];
                Util.Replace(ref text, "{0}", gd.takashiName);
                list[0] = text;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list, true, true, false, true);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(11, 8, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list, true, true, false, true);
                sp.WaitSec(0.01f, false);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 30U, 1.2f, 0, false, true, true);
                sp.UnloadPlace();
                sp.WaitFade(new List<uint> { 30U }, true, true);
                int mainP = ad.mainP;
                Plugin.CheckLocationsInScript(1202);
                sp.WaitSec(0.6f, true);
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioEndingText(11, 10, -1), 1, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.FadeBgmInScript(-1f, 0f, 2.5f, true, -1);
                sp.WaitSec(0.5f, true);
                gd.forwardNext = true;
            };
            __instance.actions[1] = action;
        }


        [HarmonyPatch(typeof(RERakio1Scenario), "SetParam")]
        [HarmonyPostfix]
        static void Raqio2(RERakio1Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[1];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioEndingText(13, 2, -1), 1, false);
                sp.LoadPlace(5, true);
                sp.WaitLoad();
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Interface, 50U, true, false, -1);
                    sp.SetColorScreen(255U, 50000U, -1);
                    sp.m_sb[50000U].SetFade(0.7f, 0f, 0, -1f, -1, false);
                    return true;
                }, (float e) => true, false));
                sp.ShowChara(ad.mainP, 0, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(120f, 40f, 720f, 405f), 0f, 1f, true, null, true);
                sp.PlayBgmInScript("bgm18", 1f, 0.4f, -1, true);
                sp.WaitFade(new List<uint> { 50000U }, true, true);
                List<string> list = Util.Split(sp.m_rs.GetScenarioEndingText(13, 3, 0), new char[] { '|' });
                string text = list[0];
                list[0] = text;
                sp.SetNormalSerifu(ad.mainP, -1, 1, list, true, true, true, true);
                sp.FadeBgmInScript(-1f, 0.4f, 0.6f, false, -1);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(13, 4, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list, true, true, false, true);
                sp.PlayBgmInScript("bgm03", 0.4f, 1f, -1, true);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(13, 5, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list, true, false, false, true);
                sp.FadeBgmInScript(-1f, 0.6f, 0.75f, false, -1);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(13, 6, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list, true, true, false, true);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(13, 7, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list, true, true, false, true);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(13, 8, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list, true, true, false, true);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(13, 9, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, 0, 1, list, true, true, false, true);
                sp.FadeBgmInScript(-1f, 0.4f, 1.5f, false, -1);
                sp.WaitSec(0.05f, true);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 30U, 0.7f, 0, false, true, true);
                sp.UnloadPlace();
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(0.4f, true);
                int mainP = ad.mainP;
                Plugin.CheckLocationsInScript(302);
                sp.WaitSec(0.4f, true);
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioEndingText(13, 11, -1), 2, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.FadeBgmInScript(-1f, 0f, 1.5f, true, -1);
                gd.forwardNext = true;
            };
            __instance.actions[1] = action;
        }


        [HarmonyPatch(typeof(RESQ0Scenario), "SetParam")]
        [HarmonyPostfix]
        static void SQ2ResultEvent(RESQ0Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[1];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.LoadTexture("personalRoom2");
                sp.WaitLoad();
                sp.LoadTexture("personalRoom2b");
                sp.WaitLoad();
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioEndingText(21, 2, -1), 1, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.LoadSound("se_hirate_04");
                sp.WaitLoad();
                sp.LoadSound("se_ashioto_08");
                sp.WaitLoad();
                sp.WaitSec(0.6f, true);
                sp.PlaySeInScript("se_ashioto_09", 1f);
                sp.LoadTexture("p02a");
                sp.WaitLoad();
                sp.WaitSec(1.2f, true);
                sp.StopAllSeInScript();
                sp.PlaySeInScript("se_jidoudoa", 1f);
                sp.WaitSec(0.6f, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.SetScreen(Setting.Screen.s_none, 20U, false, false, -1);
                    sp.SetScreen(Setting.Screen.s_Interface, 50U, true, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "personalRoom2", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 10U, "personalRoom2b", new Vector2?(new Vector2(295.5f, 73.5f)), null);
                    sp.m_sb[0U].m_spriteMap[10U].SetVisible(true);
                    uint num = 2U;
                    sp.m_sb[20U].SetPackedTexture(0, sp.m_sb[20U].gameObject.transform, "p02a", "body", 100U * num, 10U, new Vector2?(new Vector2((float)(18446744073709551416UL + (ulong)(50U * num)), 0f)), null, null, false);
                    sp.m_sb[20U].m_spriteMap[100U * num].GetComponent<Image>().material = sp.m_rs.uiCharaDefaultMat;
                    sp.m_sb[20U].m_spriteMap[100U * num].GetComponent<Image>().material.SetColor("_Color", Color.white);
                    sp.m_sb[20U].m_spriteMap[100U * num].SetSize(0.7f);
                    sp.m_sb[20U].m_spriteMap[100U * num].SetDisplayOffsetY((float)sp.m_rs.m_displaySize.height - sp.m_sb[20U].m_spriteMap[100U * num].GetSizeInDisplay().y * sp.m_sb[20U].m_spriteMap[100U * num].GetSize() * GraphicsContext.m_textureRatio);
                    num = 7U;
                    sp.m_sb[20U].SetPackedTexture(0, sp.m_sb[20U].gameObject.transform, "p07", "body", 100U * num, 10U, new Vector2?(new Vector2((float)(18446744073709551416UL + (ulong)(50U * num)), 0f)), null, null, false);
                    sp.m_sb[20U].m_spriteMap[100U * num].GetComponent<Image>().material = sp.m_rs.uiCharaDefaultMat;
                    sp.m_sb[20U].m_spriteMap[100U * num].GetComponent<Image>().material.SetColor("_Color", Color.white);
                    sp.m_sb[20U].m_spriteMap[100U * num].SetSize(0.7f);
                    sp.m_sb[20U].m_spriteMap[100U * num].SetDisplayOffsetY((float)sp.m_rs.m_displaySize.height - sp.m_sb[20U].m_spriteMap[100U * num].GetSizeInDisplay().y * sp.m_sb[20U].m_spriteMap[100U * num].GetSize() * GraphicsContext.m_textureRatio);
                    sp.SetColorScreen(255U, 50000U, -1);
                    sp.m_sb[50000U].SetFade(0.6f, 0f, 0, -1f, -1, false);
                    return true;
                }, (float e) => true, false));
                sp.ShowChara(ad.targetP, 3, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(120f, 40f, 720f, 405f), 0f, 1f, true, null, true);
                sp.WaitFade(new List<uint> { 50000U }, true, true);
                List<string> list = Util.Split(sp.m_rs.GetScenarioEndingText(21, 3, 3), new char[] { '|' });
                string text = list[0];
                list[0] = text;
                sp.SetNormalSerifu(ad.targetP, 0, 1, list, false, true, true, true);
                sp.PlayBgmInScript("bgm04", 0f, 0.8f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(21, 4, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, -1, list, false, true, true, false);
                sp.SetNormalClipAnim(-1);
                sp.WaitClipAnim(new List<uint> { 0U, 20U }, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(21, 5, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list, true, false, false, true);
                sp.FadeBgmInScript(-1f, 0f, 2.5f, true, -1);
                sp.PlaySeInScript("se_fukuwonugu", 1f);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetColorScreen(255U, 30U, -1);
                    sp.m_sb[30U].SetFade(0.4f, 1f, 0, 0f, -1, false);
                    return true;
                }, (float e) => true, false));
                sp.WaitFade(new List<uint> { 30U }, false, true);
                sp.WaitSec(1.2f, true);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(21, 6, 1), new char[] { '|' });
                text = list[0];
                Util.Replace(ref text, "{0}", gd.takashiName);
                list[0] = text;
                sp.SetNormalSerifu(ad.mainP, 0, -1, list, true, true, true, true);
                sp.PlaySeInScript("se_jidoudoa", 1f);
                sp.WaitSec(0.3f, true);
                sp.UnvisibleAllChara(20U, -1);
                sp.ShowChara(ad.mainP, 0, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(120f, 40f, 720f, 405f), 0f, 1f, true, null, true);
                sp.SetFadeScreen(new List<uint> { 30U }, 35U, 0.4f, 0, false, true, true);
                sp.WaitFade(new List<uint> { 35U }, true, true);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(21, 7, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list, false, true, true, true);
                sp.WaitSec(0.8f, true);
                sp.PlayBgmInScript("bgm17", 0f, 0.55f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(21, 8, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list, true, true, false, true);
                sp.PlaySeInScript("se_ashioto_08", 1f);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetColorScreen(255U, 35U, -1);
                    sp.m_sb[35U].SetFade(0.4f, 1f, 0, 0f, -1, false);
                    return true;
                }, (float e) => true, false));
                sp.WaitFade(new List<uint> { 35U }, false, true);
                sp.RemoveScreenInScript(0U);
                sp.UnloadTexture("personalRoom2");
                sp.UnloadTexture("personalRoom2b");
                sp.LoadPlace(6, true);
                sp.WaitLoad();
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    return true;
                }, (float e) => true, false));
                sp.LoadSound("se_Zugaikotsu_02");
                sp.WaitSec(1f, true);
                sp.WaitLoad();
                sp.StopAllSeInScript();
                list = Util.Split(sp.m_rs.GetScenarioEndingText(21, 9, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 1, list, true, true, true, false);
                sp.UnloadSound("se_ashioto_08");
                sp.PlaySeInScript("se_okiagari", 1f);
                sp.UnvisibleAllChara(20U, -1);
                sp.ShowChara(ad.targetP, 4, 0, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(0f, 80f, 480f, 270f), 0f, 1f, false, null, true);
                sp.FadeBgmInScript(-1f, 1f, 0.2f, false, -1);
                sp.SetFadeScreen(new List<uint> { 35U }, 40U, 0.2f, 0, true, true, true);
                sp.WaitSec(0.2f, true);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(21, 10, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 2, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(21, 11, 4), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 1, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(21, 12, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 2, list, true, false, false, true);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(21, 13, 0), new char[] { '|' });
                text = list[0];
                Util.Replace(ref text, "{0}", gd.takashiName);
                list[0] = text;
                sp.SetNormalSerifu(ad.mainP, 0, 2, list, true, false, false, true);
                sp.FadeBgmInScript(0f, 0.2f, 1f, false, -1);
                sp.PlaySeInScript("se_Zugaikotsu_02", 1f);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetColorScreen(1846214911U, 40U, -1);
                    return true;
                }, (float e) => true, false));
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 40U }, 45U, 0.3f, 5, false, true, true);
                sp.UnloadTexture("p02a");
                sp.WaitFade(new List<uint> { 45U }, true, true);
                sp.WaitSec(2f, true);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(21, 14, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 2, list, true, true, true, false);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(21, 15, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 2, list, true, true, true, false);
                sp.PlaySeInScript("se_hirate_04", 1f);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(21, 16, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 2, list, true, true, true, false);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(21, 17, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, -1, 2, list, true, true, true, false);
                sp.WaitSec(0.75f, true);
                list = Util.Split(sp.m_rs.GetScenarioEndingText(21, 18, 5), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, 2, list, true, true, true, false);
                sp.RemoveScreenInScript(50U);
                sp.FadeBgmInScript(-1f, 0f, 5f, true, -1);
                sp.WaitSec(1f, true);
                if (!ArchipelagoClient.ServerData.CheckedLocations.Contains(202)) //Changed condition
                {
                    int mainP = ad.mainP;
                    int targetP = ad.targetP;
                    sp.StopAllSeInScript();
                    Plugin.CheckLocationsInScript(202, 703);
                    gd.baseData.gainExp += 100U;
                    sp.WaitSec(0.75f, true);
                }
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioEndingText(21, 20, -1), 2, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.UnloadSound("se_hirate_04");
                sp.UnloadSound("se_Zugaikotsu_02");
                sp.UnloadPlace();
                gd.forwardNext = true;
            };
            __instance.actions[1] = action;
        }


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
                Util.Replace(ref text4, "{0}", GetCharaName(chara, gd, ad.targetP));
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
                Util.Replace(ref text4, "{0}", GetCharaName(chara, gd, ad.targetP));
                list13[0] = text4;
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 1, list13, true, false, false, true);
                list13 = Util.Split(sp.m_rs.GetScenarioTutorialText(1, 51, -1), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, -1, -1, list13, true, false, true, true);
                sp.FadeBgmInScript(-1f, 0.6f, 1f, false, -1);
                list13 = Util.Split(sp.m_rs.GetScenarioTutorialText(1, 52, 2), new char[] { '|' });
                sp.SetNormalSerifu(sd.mainP, ad.mainP, 2, list13, true, false, false, true);
                list13 = Util.Split(sp.m_rs.GetScenarioTutorialText(1, 53, 2), new char[] { '|' });
                text4 = list13[0];
                Util.Replace(ref text4, "{0}", GetCharaName(chara, gd, ad.targetP));
                list13[0] = text4;
                sp.SetNormalSerifu(sd.mainP, ad.mainP, 2, list13, true, true, true, true);
                list13 = Util.Split(sp.m_rs.GetScenarioTutorialText(1, 54, 0), new char[] { '|' });
                text4 = list13[0];
                Util.Replace(ref text4, "{0}", GetCharaName(chara, gd, ad.targetP));
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
            ScenarioContents.ActionContents action = __instance.actions[37];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
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
                Util.Replace(ref text18, "{0}", GetCharaName(chara, gd, ad.targetP));
                list35[0] = text18;
                sp.SetNormalSerifu(ad.mainP, -1, gd.pos, list35, true, true, true, true);
                sp.FadeBgmInScript(-1f, 0f, 0.1f, false, -1);
                sp.PlaySeInScript("se_jin_05", 1f);
                list35 = Util.Split(sp.m_rs.GetScenarioTutorialText(2, 153, 5), new char[] { '|' });
                text18 = list35[0];
                Util.Replace(ref text18, "{0}", GetCharaName(chara, gd, ad.targetP));
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
            ScenarioContents.ActionContents action = __instance.actions[18];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
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
            ScenarioContents.ActionContents action = __instance.actions[3];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
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
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
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
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Get Other Stuff
                Type dataType = AccessTools.TypeByName("gnosia.Data");
                Array chara = (Array)AccessTools.Field(dataType, "Chara").GetValue(null);
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
                Util.Replace(ref text2, "{0}", GetCharaName(chara, gd, ad.targetP));
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
                Plugin.CheckLocationsInScript(1502);
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
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
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
                Plugin.CheckLocationsInScript(1301);
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
                Plugin.CheckLocationsInScript(901);
                gd.forwardNext = true;
            };
            __instance.actions[3] = action;
            action = __instance.actions[4];
            action.DoIt = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
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
                Plugin.CheckLocationsInScript(1504);
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
                Plugin.CheckLocationsInScript(1201, 701);
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
                Plugin.CheckLocationsInScript(1201, 701);
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


        [HarmonyPatch(typeof(TutorialNanoriScenario), "SetParam")]
        [HarmonyPostfix]
        static void StepForward(TutorialNanoriScenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[6];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                List<string> list6 = Util.Split(sp.m_rs.GetScenarioTutorialText(15, 21, 7), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 1, list6, true, true, false, false);
                list6 = Util.Split(sp.m_rs.GetScenarioTutorialText(15, 22, 3), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 1, list6, true, true, false, false);
                list6 = Util.Split(sp.m_rs.GetScenarioTutorialText(15, 23, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 1, list6, true, true, false, false);
                sp.WaitSec(0.05f, false);
                Plugin.CheckLocationsInScript(1);
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioTutorialText(15, 26, -1), 2, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                sp.PlaySeInScript("se_square", 1f);
                sp.SetDialogScreen(50400U, sp.m_rs.GetScenarioTutorialText(15, 27, -1), (Setting.language == 1) ? 2 : 3, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => sp.GetSelect(0) >= 0, false));
                list6 = Util.Split(sp.m_rs.GetScenarioTutorialText(15, 28, 6), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 1, list6, true, true, false, false);
                list6 = Util.Split(sp.m_rs.GetScenarioTutorialText(15, 29, 1), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 1, list6, false, true, false, false);
            };
            __instance.actions[6] = action;
        }
    }
}
