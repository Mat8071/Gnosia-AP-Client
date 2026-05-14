using GnosiaArchipelagoRandomizer.Archipelago;
using HarmonyLib;
using application;

namespace GnosiaArchipelagoRandomizer.Patches.Core
{
    [HarmonyPatch(typeof(TakashiSetScreen), "InitializeGlm")]
    class DefaultToSlotNamePatch
    {
        static void Postfix(TakashiSetScreen __instance)
        {
            Traverse.Create(__instance).Method("InputFieldEndEdit", ArchipelagoClient.ServerData.SlotName).GetValue();
        }
    }
}