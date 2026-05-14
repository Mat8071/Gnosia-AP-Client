using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using coreSystem;
using HarmonyLib;
using sce.SampleUtil.Input;
using System.Reflection;

namespace GnosiaArchipelagoRandomizer.Patches.Core
{
    [HarmonyPatch]
    class WWGRequirementsPatch
    {
        static MethodBase TargetMethod()
        {
            Type type = AccessTools.TypeByName("application.MakeLoopScreen");
            return AccessTools.Method(type, "MyUpdate");
        }

        static void Prefix(object __instance, float ellapseSec, ControllerContext controllerContext, bool covered = false)
        {
            //Setup
            Type type = AccessTools.TypeByName("application.MakeLoopScreen");
            Traverse t = Traverse.Create(__instance);
            //Get needed variables
            bool isDicision = t.Field("isDicision").GetValue<bool>();
            int[] yakuNum = t.Field("yakuNum").GetValue<int[]>();
            ScriptParser m_scriptParser = t.Field("m_scriptParser").GetValue<ScriptParser>();
            int state = t.Field("state").GetValue<int>();
            //Do stuff
            if (controllerContext.IsButtonPressed(0) || isDicision)
            {
                //Check if gnosia is set to zero
                if (yakuNum[7] == 0)
                {
                    //Check if the player has all requirements for AWWG
                    if (Plugin.CanAccessWorldWithoutGnosia())
                    {
                        return;
                    }
                    else
                    {
                        state = 1;
                        m_scriptParser.m_sm.PlaySe("se_noiseB", 1);
                        m_scriptParser.m_sb[100U].SetFade(0.1f, 1f, 100, 0f, -1, true);
                        m_scriptParser.SetDialogScreen(200U, "You don't have all the requirements\nto set the number of Gnosia to zero.", 2, false);
                    }
                }
            }
            //Set all changed variables back
            t.Field("state").SetValue(state);
        }
    }
}
