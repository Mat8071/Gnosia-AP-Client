using HarmonyLib;
using UnityEngine;
using gnosia;

namespace GnosiaArchipelagoRandomizer.Patches.Core
{
    [HarmonyPatch(typeof(Jinro), "NextDay")]
    class NextDayPatch
    {
        static bool Prefix()
        {
            //Get gd
            gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
            //Do stuff...
            Plugin.UpdateSkills(gd);
            return true;
        }
    }
}