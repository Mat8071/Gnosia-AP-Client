using GnosiaArchipelagoRandomizer.Archipelago;
using HarmonyLib;

namespace GnosiaArchipelagoRandomizer.Patches.Core
{
    [HarmonyPatch(typeof(gnosia.GameData), "GetFromBaseData")]
    class LoadDataPatch
    {
        //This gets called whenever the player loads a save file or creates a new save file
        static void Postfix(gnosia.GameData __instance)
        {
            //Do stuff here...
            Plugin.UpdateSafeGDItems(__instance);
            if (ArchipelagoClient.ServerData.SlotData.Options?.RandomizeSkills ?? true)
                Plugin.UpdateSkills(__instance);
            while (Plugin.instant_item_queue.Count > 0)
            {
                //Get an item and use it now!
                long item = Plugin.instant_item_queue.Dequeue();
                Plugin.ActivateInstantUseItem(__instance, item);
            }
            Plugin.loadedSavesAtLeastOnce = true;
        }
    }
}