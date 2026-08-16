using System.Collections.Generic;
using coreSystem;
using gnosia;
using HarmonyLib;
using setting;
using UnityEngine;
using util;

namespace GnosiaArchipelagoRandomizer.Patches.Optional
{
    [HarmonyPatch(typeof(MainWithoutGlastScenario), "SetParam")]
    class NormalEndingGoal
    {
        static void Postfix(MainWithoutGlastScenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[2];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                sp.WaitSec(0.4f, true);
                List<string> list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 47, 0), new char[] { '|' });
                string text2 = list2[0];
                Util.Replace(ref text2, "{0}", gd.takashiName);
                list2[0] = text2;
                sp.SetNormalSerifu(sd.mainP, -1, 1, list2, false, true, true, true);
                sp.LoadTexture("ivep00_03_03");
                sp.WaitLoad();
                sp.WaitText(50U, "test", false);
                sp.PlayBgmInScript("bgm21", 1.5f, 0.5f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 48, 0), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, 1, list2, true, true, true, true);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.2f, 0, false, true, true);
                sp.UnloadTexture("ivep00_00_5");
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, true, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep00_03_03", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, true));
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.4f, 0, true, true, true);
                list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 49, 0), new char[] { '|' });
                text2 = list2[0];
                Util.Replace(ref text2, "{0}", gd.takashiName);
                list2[0] = text2;
                sp.SetNormalSerifu(sd.mainP, 0, 1, list2, false, true, true, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 50, 0), new char[] { '|' });
                sp.SetNormalSerifu(0, sd.mainP, 1, list2, false, true, true, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.2f, 0, false, true, true);
                sp.UnloadTexture("ivep00_03_03");
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.PlaySeInScript("se_hikaruball", 1f);
                sp.FadeBgmInScript(-1f, 0.8f, 2.5f, false, -1);
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
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ive00_1", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 1U, "ive008_0_1", new Vector2?(new Vector2(429f, 53.25f)), null);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 2U, "ive008_0_2", new Vector2?(new Vector2(429f, 53.25f)), null);
                    return true;
                }, (float e) => true, true));
                sp.SetFadeScreen(new List<uint> { 35U }, 36U, 0.6f, 0, true, true, true);
                list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 51, 0), new char[] { '|' });
                sp.SetNormalSerifu(sd.mainP, 0, 1, list2, true, true, true, true);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.4f, 0, false, false, true);
                sp.SetVisible(0U, 2, true);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 52, 0), new char[] { '|' });
                sp.SetNormalSerifu(sd.mainP, 0, 1, list2, true, true, true, true);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.4f, 0, false, false, true);
                sp.SetVisible(0U, 1, true);
                sp.SetVisible(0U, 2, false);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 53, 0), new char[] { '|' });
                text2 = list2[0];
                Util.Replace(ref text2, "{0}", gd.takashiName);
                list2[0] = text2;
                sp.SetNormalSerifu(sd.mainP, 0, 1, list2, true, true, true, true);
                list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 54, 0), new char[] { '|' });
                sp.SetNormalSerifu(sd.mainP, 0, 1, list2, true, true, true, true);
                list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 55, 0), new char[] { '|' });
                sp.SetNormalSerifu(0, sd.mainP, 1, list2, false, true, true, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.4f, 0, false, true, true);
                sp.UnloadTexture("ive00_1");
                sp.UnloadTexture("ive008_0_1");
                sp.UnloadTexture("ive008_0_2");
                sp.LoadTexture("ivep00_00_8");
                sp.WaitLoad();
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep00_00_8", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, true));
                sp.ShowChara(sd.mainP, 0, 0, 20U, false);
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.4f, 0, true, true, true);
                sp.FadeBgmInScript(-1f, 0f, 2f, true, -1);
                list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 56, 0), new char[] { '|' });
                sp.SetNormalSerifu(sd.mainP, 0, 2, list2, true, true, true, true);
                list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 57, 0), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, 1, list2, false, true, true, true);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.4f, 0, false, true, true);
                sp.UnloadTexture("ivep00_00_8");
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.WaitSec(0.6f, true);
                sp.LoadTexture("ivep00_03_04");
                sp.WaitLoad();
                sp.LoadSound("se_se_10");
                sp.WaitLoad();
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep00_03_04", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, true));
                sp.SetClipAnim(new List<uint> { 0U }, new Vector4(360f, 196f, 240f, 135f), 0f, 1f, true, null, true);
                sp.WaitSec(0.01f, false);
                sp.PlaySeInScript("se_se_10", 1f);
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.2f, 0, true, true, true);
                sp.SetClipAnim(new List<uint> { 0U }, new Vector4(0f, 0f, 960f, 540f), 0.4f, -2.5f, true, null, true);
                sp.WaitSec(0.6f, true);
                list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 58, 0), new char[] { '|' });
                sp.SetNormalSerifu(sd.mainP, -1, 1, list2, true, true, true, true);
                sp.PlayBgmInScript("bgm12", 0f, 0.6f, -1, true);
                sp.WaitSec(0.2f, true);
                list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 59, 0), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, 1, list2, true, true, true, true);
                sp.WaitSec(0.4f, true);
                sp.StopAllSeInScript();
                sp.UnloadSound("se_se_10");
                list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 60, 0), new char[] { '|' });
                sp.SetNormalSerifu(sd.mainP, 0, 1, list2, false, true, true, true);
                sp.LoadTexture("ivep00_03_05");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.25f, 2, true, true, true);
                sp.UnloadTexture("ivep00_03_04");
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep00_03_05", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, true));
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.25f, 2, true, true, true);
                list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 61, 0), new char[] { '|' });
                sp.SetNormalSerifu(sd.mainP, 0, 1, list2, false, true, true, true);
                sp.LoadTexture("ivep00_03_06");
                sp.WaitLoad();
                sp.WaitText(50U, "test", false);
                sp.FadeBgmInScript(-1f, 0f, 1.2f, true, -1);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 62, 0), new char[] { '|' });
                sp.SetNormalSerifu(0, sd.mainP, 1, list2, true, true, true, true);
                sp.SetCopyScreen(new List<uint> { 0U }, 30U, true);
                sp.UnloadTexture("ivep00_03_05");
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep00_03_06", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, true));
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.4f, 0, true, true, true);
                sp.WaitSec(0.2f, true);
                list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 63, 0), new char[] { '|' });
                sp.SetNormalSerifu(sd.mainP, 0, 1, list2, false, true, true, true);
                sp.LoadTexture("ivep00_03_06_1");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.4f, 0, false, false, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 5U, false, false, -1);
                    sp.m_sb[5U].SetTexture(0, sp.m_sb[5U].gameObject.transform, 0U, "ivep00_03_06_1", new Vector2?(new Vector2(186f, 0f)), null);
                    sp.m_sb[5U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, false));
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 64, 0), new char[] { '|' });
                sp.SetNormalSerifu(sd.mainP, 0, 1, list2, false, true, true, true);
                sp.LoadTexture("ivep00_03_06_2");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.SetFadeScreen(new List<uint> { 0U, 5U }, 30U, 0.4f, 0, false, false, true);
                sp.RemoveScreenInScript(5U);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 5U, false, false, -1);
                    sp.m_sb[5U].SetTexture(0, sp.m_sb[5U].gameObject.transform, 0U, "ivep00_03_06_2", new Vector2?(new Vector2(186f, 0f)), null);
                    sp.m_sb[5U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, true));
                sp.UnloadTexture("ivep00_03_06_1");
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 65, 0), new char[] { '|' });
                sp.SetNormalSerifu(sd.mainP, 0, 1, list2, false, true, true, true);
                sp.LoadTexture("ivep00_03_06_3");
                sp.WaitLoad();
                sp.WaitText(50U, "test", false);
                sp.PlayBgmInScript("bgm11", 0f, 0.65f, -1, true);
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.SetFadeScreen(new List<uint> { 0U, 5U }, 30U, 0.4f, 0, false, false, true);
                sp.RemoveScreenInScript(5U);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 5U, false, false, -1);
                    sp.m_sb[5U].SetTexture(0, sp.m_sb[5U].gameObject.transform, 0U, "ivep00_03_06_3", new Vector2?(new Vector2(186f, 0f)), null);
                    sp.m_sb[5U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, true));
                sp.UnloadTexture("ivep00_03_06_2");
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 66, 0), new char[] { '|' });
                text2 = list2[0];
                Util.Replace(ref text2, "{0}", gd.takashiName);
                list2[0] = text2;
                sp.SetNormalSerifu(sd.mainP, 0, 1, list2, false, true, true, true);
                sp.LoadTexture("ivep00_03_06_4");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.SetFadeScreen(new List<uint> { 0U, 5U }, 30U, 0.4f, 0, false, false, true);
                sp.RemoveScreenInScript(5U);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 5U, false, false, -1);
                    sp.m_sb[5U].SetTexture(0, sp.m_sb[5U].gameObject.transform, 0U, "ivep00_03_06_4", new Vector2?(new Vector2(186f, 0f)), null);
                    sp.m_sb[5U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, true));
                sp.UnloadTexture("ivep00_03_06_3");
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 67, 0), new char[] { '|' });
                sp.SetNormalSerifu(sd.mainP, 0, 1, list2, false, true, true, true);
                sp.LoadTexture("ivep00_03_07");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.SetFadeScreen(new List<uint> { 0U, 5U }, 30U, 0.2f, 0, true, true, true);
                sp.UnloadTexture("ivep00_03_06");
                sp.UnloadTexture("ivep00_03_06_4");
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep00_03_07", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, true));
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.4f, 0, true, true, true);
                list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 68, 0), new char[] { '|' });
                sp.SetNormalSerifu(sd.mainP, 0, 1, list2, false, true, true, true);
                sp.LoadTexture("ivep00_03_05");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.SetCopyScreen(new List<uint> { 0U }, 30U, true);
                sp.UnloadTexture("ivep00_03_07");
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep00_03_05", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, true));
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.6f, 0, true, true, true);
                list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 69, 0), new char[] { '|' });
                sp.SetNormalSerifu(sd.mainP, 0, 1, list2, false, true, true, true);
                sp.LoadTexture("ivep00_03_08");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.PlaySeInScript("se_ashioto_02", 1f);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.25f, 4, false, true, true);
                sp.UnloadTexture("ivep00_03_05");
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep00_03_08", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, true));
                sp.WaitSec(0.4f, true);
                sp.StopAllSeInScript();
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.25f, 4, true, true, true);
                list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 70, 0), new char[] { '|' });
                sp.SetNormalSerifu(sd.mainP, 0, 1, list2, false, true, true, true);
                sp.LoadTexture("ivep00_03_09");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 71, 0), new char[] { '|' });
                sp.SetNormalSerifu(0, -1, -1, list2, true, true, true, true);
                sp.SetCopyScreen(new List<uint> { 0U }, 30U, true);
                sp.UnloadTexture("ivep00_03_08");
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep00_03_09", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, true));
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.6f, 0, true, true, true);
                sp.FadeBgmInScript(-1f, 0.75f, 0.4f, false, -1);
                list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 72, 0), new char[] { '|' });
                sp.SetNormalSerifu(sd.mainP, 0, 1, list2, true, true, true, true);
                list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 73, 0), new char[] { '|' });
                sp.SetNormalSerifu(sd.mainP, 0, 1, list2, true, true, true, true);
                list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 74, 0), new char[] { '|' });
                sp.SetNormalSerifu(0, sd.mainP, -1, list2, false, true, true, true);
                sp.LoadTexture("ivep00_03_09_1");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.4f, 0, false, false, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 5U, "ivep00_03_09_1", new Vector2?(new Vector2(323.25f, 0f)), null);
                    sp.m_sb[0U].m_spriteMap[5U].SetVisible(true);
                    return true;
                }, (float e) => true, true));
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(0.2f, true);
                list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 75, 0), new char[] { '|' });
                sp.SetNormalSerifu(sd.mainP, 0, 1, list2, true, true, true, true);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.25f, 0, false, false, true);
                sp.SetVisible(0U, 5, false);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 76, 0), new char[] { '|' });
                sp.SetNormalSerifu(sd.mainP, 0, 1, list2, false, true, true, true);
                sp.LoadTexture("ivep00_03_10");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.SetCopyScreen(new List<uint> { 0U }, 30U, true);
                sp.UnloadTexture("ivep00_03_09");
                sp.UnloadTexture("ivep00_03_09_1");
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep00_03_10", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, true));
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.6f, 0, true, true, true);
                list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 77, 0), new char[] { '|' });
                sp.SetNormalSerifu(sd.mainP, 0, 1, list2, false, true, true, true);
                sp.LoadTexture("ivep00_03_11");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.PlaySeInScript("se_ashioto_02", 1f);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.3f, 0, false, true, true);
                sp.UnloadTexture("ivep00_03_10");
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(0.3f, true);
                sp.StopAllSeInScript();
                sp.FadeBgmInScript(-1f, 0.4f, 1f, false, -1);
                list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 78, 0), new char[] { '|' });
                text2 = list2[0];
                Util.Replace(ref text2, "{0}", gd.takashiName);
                list2[0] = text2;
                sp.SetNormalSerifu(sd.mainP, 0, 1, list2, true, true, true, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep00_03_11", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, true));
                sp.FadeBgmInScript(-1f, 0f, 0.4f, false, -1);
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.25f, 0, true, true, true);
                sp.WaitSec(0.2f, true);
                list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 79, 0), new char[] { '|' });
                sp.SetNormalSerifu(sd.mainP, 0, 1, list2, false, true, true, true);
                sp.LoadTexture("ivep00_03_12");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.FadeBgmInScript(-1f, 0.75f, 15f, false, -1);
                sp.WaitSec(0.25f, true);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.55f, 0, false, true, true);
                sp.UnloadTexture("ivep00_03_11");
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(0.75f, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep00_03_12", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, true));
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.6f, 0, true, true, true);
                list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 80, 0), new char[] { '|' });
                text2 = list2[0];
                Util.Replace(ref text2, "{0}", gd.takashiName);
                list2[0] = text2;
                sp.SetNormalSerifu(sd.mainP, 0, 1, list2, false, true, true, true);
                sp.LoadTexture("ivep00_03_13");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.SetCopyScreen(new List<uint> { 0U }, 30U, true);
                sp.UnloadTexture("ivep00_03_12");
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep00_03_13", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, true));
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.6f, 0, true, true, true);
                sp.WaitSec(0.2f, true);
                list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 81, 0), new char[] { '|' });
                sp.SetNormalSerifu(sd.mainP, 0, 1, list2, false, true, true, true);
                sp.LoadTexture("ivep00_03_14");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.SetCopyScreen(new List<uint> { 0U }, 30U, true);
                sp.UnloadTexture("ivep00_03_13");
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep00_03_14", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    return true;
                }, (float e) => true, true));
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.6f, 0, false, true, true);
                sp.LoadSound("se_akeru_02");
                sp.WaitLoad();
                sp.WaitFade(new List<uint> { 31U }, true, true);
                sp.WaitSec(0.2f, true);
                list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 82, 0), new char[] { '|' });
                sp.SetNormalSerifu(sd.mainP, 0, 1, list2, false, true, true, true);
                sp.LoadTexture("ivep00_03_15");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.FadeBgmInScript(-1f, 0f, 0.6f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.4f, 0, false, true, true);
                sp.UnloadTexture("ivep00_03_14");
                sp.WaitSec(1.2f, true);
                sp.PlaySeInScript("se_akeru_02", 1f);
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.PlaySeInScript("se_pusyu", 1f);
                sp.WaitSec(1.2f, true);
                sp.PlaySeInScript("se_ashioto_02", 0.55f);
                sp.WaitSec(1.4f, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep00_03_15", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, true));
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.6f, 0, true, true, true);
                sp.StopAllSeInScript();
                sp.UnloadSound("se_akeru_02");
                sp.WaitSec(0.2f, true);
                list2 = Util.Split(sp.m_rs.GetScenarioWithoutText(3, 83, 0), new char[] { '|' });
                text2 = list2[0];
                Util.Replace(ref text2, "{0}", gd.takashiName);
                list2[0] = text2;
                sp.SetNormalSerifu(sd.mainP, 0, 1, list2, false, true, true, true);
                sp.LoadTexture("ivep00_03_17");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.4f, 0, false, true, true);
                sp.UnloadTexture("ivep00_03_15");
                sp.WaitFade(new List<uint> { 30U }, true, true);
                sp.WaitSec(1.6f, true);
                sp.StopAllSeInScript();
                sp.RemoveScreenInScript(50U);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep00_03_17", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, true));
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.65f, 0, true, true, true);
                sp.WaitSec(2.6f, true);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.4f, 0, true, true, true);
                sp.UnloadTexture("ivep00_03_17");
                sp.LoadTexture("ivep00_03_16");
                sp.WaitSec(1.2f, true);
                sp.WaitLoad();
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ivep00_03_16", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, true));
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.65f, 0, true, true, true);
                sp.WaitSec(3f, true);
                sp.SetFadeScreen(new List<uint> { 0U }, 30U, 0.4f, 0, true, true, true);
                sp.UnloadTexture("ivep00_03_16");
                sp.WaitSec(0.4f, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    TextArea textArea = UnityEngine.Object.Instantiate<TextArea>(sp.m_rs.textAreaPrefab, sp.m_sb[0U].gameObject.transform);
                    textArea.name = "thxTextArea";
                    int[] array = new int[] { 20, 40, 20 };
                    float[] array2 = new float[] { 177f, 252f, 201f };
                    sp.m_sb[0U].SetTextArea(textArea, "thx", array[Setting.language], 1, 40, new Vector2(array2[Setting.language], 253f), 70, 0, sp.m_rs.m_defaultFont, TextAlign.k_text_Left, null);
                    string scenarioWithoutText = sp.m_rs.GetScenarioWithoutText(3, 84, -1);
                    Util.Replace(ref scenarioWithoutText, "{0}", gd.takashiName);
                    sp.m_sb[0U].SetText("thx", scenarioWithoutText, false, true);
                    Plugin.CompleteGoal();
                    return true;
                }, (float e) => true, true));
                sp.WaitSec(4.8f, false);
                gd.forwardNext = true;
            };
            __instance.actions[2] = action;
        }
    }
}
