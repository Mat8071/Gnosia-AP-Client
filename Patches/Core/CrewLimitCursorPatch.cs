using System;
using System.Collections.Generic;
using HarmonyLib;
using System.Reflection;
using baseEffect.graphics;

namespace GnosiaArchipelagoRandomizer.Patches.Core
{
    [HarmonyPatch]
    class CrewLimitCursorPatch
    {
        static MethodBase TargetMethod()
        {
            Type type = AccessTools.TypeByName("application.MakeLoopScreen");
            return AccessTools.Method(type, "SetCursor");
        }

        static bool Prefix(object __instance)
        {
            //Setup
            Type type = AccessTools.TypeByName("application.MakeLoopScreen");
            Traverse t = Traverse.Create(__instance);
            //Get all variables
            Dictionary<uint, Sprite2dEffectArg> m_spriteMap = t.Field("m_spriteMap").GetValue() as Dictionary<uint, Sprite2dEffectArg>;
            int selectTgt = t.Field("selectTgt").GetValue<int>();
            List<int> yakuTgt = t.Field("yakuTgt").GetValue() as List<int>;
            int[] yakuNum = t.Field("yakuNum").GetValue<int[]>();
            int people = t.Field("people").GetValue<int>();
            gnosia.GameData mydata = t.Field("mydata").GetValue() as gnosia.GameData;
            //Base Method (Modified)
            m_spriteMap[200U].SetVisible(false);
            m_spriteMap[300U].SetVisible(false);
            m_spriteMap[400U].SetVisible(false);
            m_spriteMap[500U].SetVisible(false);
            int num;
            if (selectTgt <= 1)
            {
                num = 49 + 44 * selectTgt;
                if ((selectTgt == 0 && people > 5) || (selectTgt == 1 && (yakuNum[7] > 1 || ((mydata.baseData.sce_all_flg & 34359738368UL) > 0UL && yakuNum[7] > 0))))
                {
                    m_spriteMap[200U].SetVisible(true);
                }
                if ((selectTgt == 0 && people < Plugin.crew_max) || (selectTgt == 1 && (yakuNum[7] + 1) * 2 + 2 < people))
                {
                    m_spriteMap[300U].SetVisible(true);
                }
                m_spriteMap[200U].SetDisplayOffsetY((float)(num + 15));
                m_spriteMap[300U].SetDisplayOffsetY((float)(num + 15));
            }
            else if (selectTgt < yakuTgt.Count - 1)
            {
                num = 147 + 40 * (selectTgt - 2);
            }
            else
            {
                m_spriteMap[400U].SetVisible(true);
                m_spriteMap[500U].SetVisible(true);
                num = 447;
            }
            m_spriteMap[2000U].SetDisplayOffsetY((float)num);
            //Set all variables back
            t.Field("m_spriteMap").SetValue(m_spriteMap);
            t.Field("selectTgt").SetValue(selectTgt);
            t.Field("yakuTgt").SetValue(yakuTgt);
            t.Field("yakuNum").SetValue(yakuNum);
            t.Field("people").SetValue(people);
            t.Field("mydata").SetValue(mydata);
            //Prevent original from running
            return false;
        }
    }
}