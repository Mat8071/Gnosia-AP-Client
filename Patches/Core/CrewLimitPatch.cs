using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
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
                int people = base_people.GetValue<int>();
                if (people > Plugin.crew_max)
                    people = Plugin.crew_max;
                base_people.SetValue(people);
            }
        }
    }
}
