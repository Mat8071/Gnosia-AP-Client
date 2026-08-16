using HarmonyLib;
using systemService.trophy;

namespace GnosiaArchipelagoRandomizer.Patches.Core
{
    [HarmonyPatch(typeof(GameLogManager), "CheckTrophy")]
    class NoTrophyPatch
    {
        [HarmonyPriority(Priority.Last)]
        static bool Prefix()
        {
            //Prevent achievements from being awarded while the patch is loaded
            return false;
        }
    }
}