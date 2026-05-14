using System;
using HarmonyLib;
using UnityEngine;
using System.Reflection;
using gnosia;
using setting;

namespace GnosiaArchipelagoRandomizer.Patches.DeathLink
{
    [HarmonyPatch]
    class DeathLinkPatch
    {
        static MethodBase TargetMethod()
        {
            Type type = AccessTools.TypeByName("application.ResultScreen");
            return AccessTools.Method(type, "InitializeGlm");
        }
        static void Postfix(object __instance)
        {
            //Get gd
            gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
            //Check if result is DeathLink Death
            if ((gd.baseData.sce_all_flg & 1UL) > 0)
            {
                //Reset deathlink flag and display custom message
                gd.baseData.sce_all_flg &= ~1UL;
                Traverse.Create(__instance).Method("SetText", new object[] { "setumei", "You were sent a DeathLink from another world...", true, true }).GetValue();
            }
            else
            {
                //Deathlink stuff
                Setting.Doa doa = gd.chara[gd.personFromId[0]].doa;
                if (doa == Setting.Doa.doa_Kamare || doa == Setting.Doa.doa_Shokei)
                {
                    Plugin.ArchipelagoClient.GetDeathLinkHandler().SendDeathLink();
                }
            }
        }
    }
}