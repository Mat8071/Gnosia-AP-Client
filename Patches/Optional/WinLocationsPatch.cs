using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using gnosia;
using GnosiaArchipelagoRandomizer.Archipelago;
using HarmonyLib;
using setting;
using UnityEngine;

namespace GnosiaArchipelagoRandomizer.Patches.Optional
{
    [HarmonyPatch]
    class WinLocationsPatch
    {
        static MethodBase TargetMethod()
        {
            Type type = AccessTools.TypeByName("application.ResultScreen");
            return AccessTools.Method(type, "InitializeGlm");
        }
        static void Postfix()
        {

            //Get gd
            gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
            int result = Jinro.CheckEnd();
            if (result < 2 || //Player Eliminated or Cold Sleep
                result == 2 && gd.chara[gd.personFromId[0]].i_yaku == Setting.Yakuwari.y_Kyojin || //Player AC on Human Win
                result == 4 || //Alive Player Loses to Gnosia
                result == 6 || //Alive Player Loses to Bug
                result == 8 || //Should be the In The Loop Ending (or similar)
                result == 9 || //Kukrushka The Guard Ending
                result == 10 || //Setsu's Origins Ending (It's not a win)
                result == 12 //Reached day 15 without anyone winning
                )
            {
                return; //Not a win, so no locations unlocked
            }
            //Check location stuff
            ArchipelagoData.OptionsContents options = ArchipelagoClient.ServerData.SlotData.Options;
            Setting.Yakuwari playerRole = gd.chara[gd.personFromId[0]].i_yaku;
            HashSet<int> people = new HashSet<int>();
            HashSet<int> allies = new HashSet<int>();
            HashSet<long> locationIDs = new HashSet<long>();
            for (int id = 1; id < gd.personFromId.Length; id++)
            {
                int person = gd.personFromId[id];
                if (person > 0)
                {
                    Setting.Yakuwari role = gd.chara[person].i_yaku;
                    if (playerRole <= Setting.Yakuwari.y_Murabito && role <= Setting.Yakuwari.y_Murabito)
                    {
                        //Crew Aligned
                        allies.Add(id);
                    }
                    else if (Setting.Yakuwari.y_Murabito < playerRole && playerRole < Setting.Yakuwari.y_Fox && Setting.Yakuwari.y_Murabito < role && role < Setting.Yakuwari.y_Fox)
                    {
                        //Gnosia Aligned
                        allies.Add(id);
                    }
                    people.Add(id);
                }
            }
            if (options?.AddWinAsRoleLocations ?? false)
            {
                locationIDs.Add(1700 + (long)gd.chara[gd.personFromId[0]].i_yaku);
            }
            foreach (int id in people)
            {
                if (allies.Contains(id))
                {
                    if (options?.AddWinWithCharacterLocations ?? false)
                    {
                        locationIDs.Add(1900 + id);
                    }
                }
                else
                {
                    if (options?.AddWinAgainstCharacterLocations ?? false)
                    {
                        locationIDs.Add(2000 + id);
                    }
                    if (options?.AddWinAgainstRoleLocations ?? false)
                    {
                        locationIDs.Add(1800 + (long)gd.chara[gd.personFromId[id]].i_yaku);
                    }
                }
            }
            Plugin.CheckLocationsInScript(locationIDs.ToArray());
        }
    }
}
