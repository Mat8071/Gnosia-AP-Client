using GnosiaArchipelagoRandomizer.Archipelago;
using HarmonyLib;

namespace GnosiaArchipelagoRandomizer.Patches.Core
{
    [HarmonyPatch(typeof(application.Application), "Start")]
    class MustConnectBeforeTitlePatch
    {
        static bool Prefix(application.Application __instance)
        {
            if (ArchipelagoClient.Authenticated)
                return true;
            Plugin.Application = __instance;
            return false;
        }
    }
}