using HarmonyLib;

namespace GnosiaArchipelagoRandomizer.Patches.Optional
{
    [HarmonyPatch(typeof(gnosia.GameData), "MakeLoop")]
    class AfterBugAllRolesPatch
    {
        static void Prefix(gnosia.GameData __instance)
        {
            //Check if this is the Tutorial After Bug Loop
            foreach (gnosia.GameData.scenarioData scenario in __instance.sceOn)
            {
                if (scenario.id == 30)
                {
                    //This is the TutorialAfterBug Scenario (Loop)
                    //Set all role flags to true (skip the first cause it's the deathlink flag)
                    for (int i = 1; i < Plugin.found_roles.Length; i++)
                    {
                        __instance.baseData.sce_all_flg |= (1UL << i);
                    }
                    //The flags should reset when the player gets to the setup screen again
                }
            }
        }
    }
}
