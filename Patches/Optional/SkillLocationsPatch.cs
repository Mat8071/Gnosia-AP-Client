using System;
using System.Collections.Generic;
using coreSystem;
using gnosia;
using GnosiaArchipelagoRandomizer.Utils;
using HarmonyLib;
using setting;
using UnityEngine;
using UnityEngine.UI;
using util;

namespace GnosiaArchipelagoRandomizer.Patches.Optional
{
    [HarmonyPatch]
    class SkillLocationsPatch
    {
        [HarmonyPatch(typeof(Cipi1Scenario), "SetParam")]
        [HarmonyPostfix]
        static void LetsCollaborate(Cipi1Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[6];
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
                Util.Replace(ref text2, "{1}", MyUtils.GetCharaName(chara, gd, ad.targetP));
                list4[0] = text2;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list4, false, false, true, true);
                sp.WaitSec(0.4f, true);
                sp.PlayBgmInScript("bgm03", 0f, 0.75f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list4 = Util.Split(sp.m_rs.GetScenarioCipiText(0, 22, 1), new char[] { '|' });
                text2 = list4[0];
                Util.Replace(ref text2, "{1}", MyUtils.GetCharaName(chara, gd, ad.targetP));
                list4[0] = text2;
                sp.SetNormalSerifu(ad.mainP, 0, 1, list4, true, true, false, true);
                list4 = Util.Split(sp.m_rs.GetScenarioCipiText(0, 23, 3), new char[] { '|' });
                text2 = list4[0];
                Util.Replace(ref text2, "{1}", MyUtils.GetCharaName(chara, gd, ad.targetP));
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
                gd.baseData.gainExp += 50U;
                gd.forwardNext = true;
            };
            __instance.actions[6] = action;
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
                gd.forwardNext = true;
            };
            __instance.actions[5] = action;
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
                sp.FadeBgmInScript(-1f, 0f, 1.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(0.7f, true);
                gd.forwardNext = true;
            };
            __instance.actions[1] = action;
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
                gd.forwardNext = true;
            };
            __instance.actions[1] = action;
        }

        [HarmonyPatch(typeof(Rakio2Scenario), "SetParam")]
        [HarmonyPostfix]
        static void RaqioQuizDefiniteHuman(Rakio2Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[3];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
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
                gd.forwardNext = true;
            };
            __instance.actions[5] = action;
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

        [HarmonyPatch(typeof(Shamin3Scenario), "SetParam")]
        [HarmonyPostfix]
        static void SmallTalk(Shamin3Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[7];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
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
                sp.WaitSec(0.2f, true);
                sp.FadeBgmInScript(-1f, 0f, 1.5f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                gd.forwardNext = true;
            };
            __instance.actions[7] = action;
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
                gd.forwardNext = true;
            };
            __instance.actions[1] = action;
        }

        [HarmonyPatch(typeof(SQ1Scenario), "SetParam")]
        [HarmonyPostfix]
        static void FoolAndBeFooled(SQ1Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[8];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
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
                }
                sp.WaitSec(0.4f, true);
                gd.forwardNext = true;
            };
            __instance.actions[8] = action;
        }

        [HarmonyPatch(typeof(SQ2Scenario), "SetParam")]
        [HarmonyPostfix]
        static void Retaliate(SQ2Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[4];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
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
                Util.Replace(ref text2, "{1}", MyUtils.GetCharaName(chara, gd, ad.counterP));
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
                Util.Replace(ref text2, "{1}", MyUtils.GetCharaName(chara, gd, ad.counterP));
                list3[0] = text2;
                sp.SetNormalSerifu(ad.targetP, ad.mainP, 1, list3, true, false, false, true);
                list3 = Util.Split(sp.m_rs.GetScenarioStellaText(2, 12, 2), new char[] { '|' });
                sp.SetNormalSerifu(ad.mainP, ad.targetP, 2, list3, true, false, false, true);
                list3 = Util.Split(sp.m_rs.GetScenarioStellaText(2, 13, 1), new char[] { '|' });
                text2 = list3[0];
                Util.Replace(ref text2, "{1}", MyUtils.GetCharaName(chara, gd, ad.counterP));
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
                sp.WaitSec(0.4f, true);
                gd.forwardNext = true;
            };
            __instance.actions[2] = action;
        }

        [HarmonyPatch(typeof(Yuriko1Scenario), "SetParam")]
        [HarmonyPostfix]
        static void Chaos(Yuriko1Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[2];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
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
