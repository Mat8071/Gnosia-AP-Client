using System.Collections.Generic;
using coreSystem;
using setting;
using HarmonyLib;
using UnityEngine;
using gnosia;

namespace GnosiaArchipelagoRandomizer.Patches.Core
{
    [HarmonyPatch(typeof(MakeLoopScenario), "SetParam")]
    class NextLoopPatch
    {
        static void Postfix(MakeLoopScenario __instance)
        {
            ScenarioContents.ActionContents action = __instance.actions[0];
            action.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get gd and sp
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                //My stuff
                if (gd.baseData.loop >= 14)
                {
                    Plugin.ResetRoleFlags(gd);
                }
                Plugin.ResetSetupSettings(gd);
                gd.personFromId[0] = -1;
                //Base thing
                sd.flg |= 2048;
                if (ad.type >= 0)
                {
                    sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                    {
                        sp.SetColorScreen(255U, 50000U, -1);
                        return true;
                    }, (float e) => true, false));
                    sp.LoadTexture("base_bg");
                    sp.WaitLoad();
                    sp.LoadTexture("setup");
                    sp.WaitLoad();
                    sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                    {
                        sp.SetScreen(Setting.Screen.s_MakeLoop, 100U, true, false, -1);
                        sp.m_sm.PlayBgm("bgm12", 0f, 1f, -1, true);
                        return true;
                    }, (float e) => true, true));
                    sp.SetFadeScreen(new List<uint> { 50000U }, 50001U, 0.5f, 0, true, true, true);
                    return;
                }
                gd.forwardNext = true;
            };
            __instance.actions[0] = action;
        }
    }
}