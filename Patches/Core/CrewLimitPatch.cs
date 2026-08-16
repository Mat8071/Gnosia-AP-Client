using System;
using System.Reflection;
using HarmonyLib;

namespace GnosiaArchipelagoRandomizer.Patches.Core
{
    [HarmonyPatch]
    class CrewLimitPatch
    {
        static MethodBase TargetMethod()
        {
            Type type = AccessTools.TypeByName("application.MakeLoopScreen");
            return AccessTools.Method(type, "ChangeNum");
        }
        static void Prefix(object __instance)
        {
            //Get gd
            gnosia.GameData gd = Traverse.Create(__instance).Field("mydata").GetValue<gnosia.GameData>();
            if (gd.baseData.loop >= 14)
            {
                //Limit people variable to crew max
                Traverse base_people = Traverse.Create(__instance).Field("people");
                Traverse base_gnosia = Traverse.Create(__instance).Field("yakuNum");
                int people = base_people.GetValue<int>();
                int[] yakuNum = base_gnosia.GetValue<int[]>();
                if (people > Plugin.crew_max)
                    people = Plugin.crew_max;
                else if (people < 5)
                    people = 5;
                while (yakuNum[7] * 2 + 2 >= people)
                    yakuNum[7]--;
                base_people.SetValue(people);
                base_gnosia.SetValue(yakuNum);
            }
        }
    }
}
