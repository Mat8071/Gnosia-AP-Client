using gnosia;
using HarmonyLib;
using UnityEngine;

namespace GnosiaArchipelagoRandomizer.Patches.Optional
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