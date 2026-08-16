using application;
using GnosiaArchipelagoRandomizer.Archipelago;
using HarmonyLib;

namespace GnosiaArchipelagoRandomizer.Patches.Core
{
    [HarmonyPatch(typeof(TakashiSetScreen), "InitializeGlm")]
    class DefaultToSlotNamePatch
    {
        [HarmonyPriority(Priority.First)]
        static void Postfix(TakashiSetScreen __instance)
        {
            Traverse.Create(__instance).Method("InputFieldEndEdit", ArchipelagoClient.ServerData.SlotName).GetValue();
        }
    }
}