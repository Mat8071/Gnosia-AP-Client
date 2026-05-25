using System.Collections.Generic;
using coreSystem;
using setting;
using HarmonyLib;
using UnityEngine;
using gnosia;
using util;

namespace GnosiaArchipelagoRandomizer.Patches.Optional
{
    [HarmonyPatch(typeof(Yuriko6Scenario), "SetParam")]
    class MoreRespecPatch
    {
        static void Postfix(Yuriko6Scenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[2];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //Base
                List<string> list3 = Util.Split(sp.m_rs.GetScenarioYurikoText(5, 12, 4), new char[] { '|' });
                sp.SetNormalSerifu(0, ad.targetP, 1, list3, true, false, true, true);
                list3 = Util.Split(sp.m_rs.GetScenarioYurikoText(5, 13, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list3, false, false, false, true);
                sp.LoadTexture("ive004_1");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.PlaySeInScript("se_ashioto_02", 1f);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.4f, 0, true, true, true);
                sp.WaitSec(0.4f, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ive004_1", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, true));
                sp.StopAllSeInScript();
                sp.FadeBgmInScript(-1f, 1f, 0.8f, false, -1);
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.4f, 0, true, true, true);
                sp.WaitSec(0.2f, true);
                list3 = Util.Split(sp.m_rs.GetScenarioYurikoText(5, 14, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list3, true, true, true, false);
                list3 = Util.Split(sp.m_rs.GetScenarioYurikoText(5, 15, 0), new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, list3, false, true, true, false);
                sp.LoadTexture("entry");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.RemoveScreenInScript(50U);
                sp.FadeBgmInScript(-1f, 0.3f, 0.15f, false, -1);
                sp.PlaySeInScript("se_noiseB", 1f);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetColorScreen(sp.m_rs.defColor_blueScr - 159U, 20U, -1);
                    sp.SetScreen(Setting.Screen.s_TakashiSet, 50U, true, false, -1);
                    sp.SetColorScreen(uint.MaxValue, 60U, -1);
                    return true;
                }, (float e) => true, false));
                sp.SetFadeScreen(new List<uint> { 60U }, 50001U, 0.2f, 7, true, true, true);
            };
            __instance.actions[2] = action;
        }
    }
}