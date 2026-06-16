using System;
using System.Collections.Generic;
using System.Text;
using coreSystem;
using HarmonyLib;
using setting;
using UnityEngine;

namespace GnosiaArchipelagoRandomizer.Patches.Core
{
    [HarmonyPatch(typeof(ScriptParser), "ShowInfoUpdateMes")]
    class SimplifyMessagesPatch
    {
        static bool Prefix(ScriptParser __instance, ref int __result, string mes, uint depth, int type, bool withSound)
        {
            //Get gd
            gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
            //Base function
            if (withSound)
            {
                if (type == 1)
                {
                    __instance.PlaySeInScript("se_jin_25", 1f);
                }
                else
                {
                    __instance.PlaySeInScript("se_hikaruball_03", 1f);
                }
            }
            __instance.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
            {
                __instance.SetScreen(Setting.Screen.s_InfoUpdateMes, depth, true, false, -1);
                __instance.m_sb[depth].SetText("mes", mes, true, true);
                __instance.m_sb[depth].SetFade(0.3f, 1f, 0, 0f, -1, false);
                return true;
            }, (float e) => true, false));
            __instance.SetClipAnim(new List<uint> { depth }, new Vector4(0f, 0f, (float)__instance.m_rs.m_displaySize.width, (float)__instance.m_rs.m_displaySize.height), 0.3f, 0.125f, true, new Vector4?(new Vector4((float)__instance.m_rs.m_displaySize.width * 1f, 0f, (float)__instance.m_rs.m_displaySize.width, (float)__instance.m_rs.m_displaySize.height)), false);
            __instance.WaitSec(1.2f, true);
            __instance.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
            {
                __instance.m_sb[depth].SetFade(0.3f, 0f, 0, 1f, -1, false);
                return true;
            }, (float e) => __instance.m_sb[depth].m_textAreaMap["mes"].visible, false));
            __instance.SetClipAnim(new List<uint> { depth }, new Vector4((float)__instance.m_rs.m_displaySize.width * -1f, 0f, (float)__instance.m_rs.m_displaySize.width, (float)__instance.m_rs.m_displaySize.height), 0.3f, 4f, true, new Vector4?(new Vector4(0f, 0f, (float)__instance.m_rs.m_displaySize.width, (float)__instance.m_rs.m_displaySize.height)), false);
            __instance.RemoveScreenInScript(depth);
            if ((gd.baseData.sce_all_flg & 524288UL) == 0UL && type == 0)
            {
                gd.baseData.sce_all_flg = gd.baseData.sce_all_flg | 524288UL;
                __instance.PlaySeInScript("se_square", 1f);
                __instance.SetDialogScreen(50400U, __instance.m_rs.GetOthersText(3, 7), 2, false);
                __instance.scriptQueue.Enqueue(new ScriptParser.Script((float e) => true, (float e) => __instance.GetSelect(0) >= 0, false));
            }
            //Removed the script that checks trophies
            __result = 1;
            return false;
        }
    }
}
