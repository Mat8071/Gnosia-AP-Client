using System.Collections.Generic;
using coreSystem;
using setting;
using HarmonyLib;
using UnityEngine;
using gnosia;
using util;

namespace GnosiaArchipelagoRandomizer.Patches.Optional
{
    [HarmonyPatch(typeof(Yuriko6Scenario), "SetParam")]
    class MoreRespecPatch
    {
        static void Postfix(Yuriko6Scenario __instance)
        {
            ScenarioContents.ActionContents myActionContents = default(ScenarioContents.ActionContents);
            myActionContents.Initialize();
            myActionContents.canPlayByState = 1073741824UL;
            myActionContents.canPlayByActionFlg = 2;
            myActionContents.canNotPlayByActionFlg = 18446744073709551613UL;
            myActionContents.changedActionFlg = 16;
            myActionContents.reduceActionFlg = 0;
            myActionContents.forceForward = false;
            myActionContents.name = "I want a new form";
            myActionContents.needSelect = 0;
            myActionContents.SelectUser = delegate(ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                if ((sd.flg & 32768) == 0)
                {
                    return;
                }
                if ((sd.flg & 16384) == 0)
                {
                    return;
                }
                if (gd.chara[sd.mainP].place == gd.baseData.place)
                {
                    ad.canPlayUser = 1;
                    ad.targetP = sd.mainP;
                    ad.power = 0f;
                    return;
                }
            };
            myActionContents.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                List<string> message = Util.Split("You told her that you want to change appearance.|4", new char[] { '|' });
                sp.SetNormalSerifu(0, ad.targetP, 1, message, true, false, true, true);
                message = Util.Split("Hm... Alright.\nI shall help you.|0", new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, message, true, false, false, true);
                message = Util.Split("What exactly were you thinking of?", new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, message, true, false, true, true);
            };
            myActionContents.DoAfter = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
            };
            myActionContents.FinAtHere = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
            };
            __instance.actions.Add(myActionContents);
            ScenarioContents.ActionContents myActionContents2 = default(ScenarioContents.ActionContents);
            myActionContents2.Initialize();
            myActionContents2.canPlayByState = 1073741824UL;
            myActionContents2.canPlayByActionFlg = 16;
            myActionContents2.canNotPlayByActionFlg = 18446744073709551597UL;
            myActionContents2.changedActionFlg = 65535UL;
            myActionContents2.reduceActionFlg = 0;
            myActionContents2.forceForward = false;
            myActionContents2.name = "Male";
            myActionContents2.needSelect = 0;
            myActionContents2.SelectUser = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                if ((sd.flg & 32768) == 0)
                {
                    return;
                }
                if ((sd.flg & 16384) == 0)
                {
                    return;
                }
                if (gd.baseData.takashiSex == 0)
                {
                    return;
                }
                if (gd.chara[sd.mainP].place == gd.baseData.place)
                {
                    ad.canPlayUser = 1;
                    ad.targetP = sd.mainP;
                    ad.power = 0f;
                    return;
                }
            };
            myActionContents2.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get stuff
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                //Initial Dialogue
                List<string> message = Util.Split("I see...\nThen let me help you.", new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, message, false, false, false, true);
                sp.LoadTexture("ive004_1");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.PlaySeInScript("se_ashioto_02", 1f);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.4f, 0, true, true, true);
                sp.WaitSec(0.4f, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ive004_1", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, true));
                sp.StopAllSeInScript();
                sp.FadeBgmInScript(-1f, 1f, 0.8f, false, -1);
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.4f, 0, true, true, true);
                sp.WaitSec(0.2f, true);
                message = Util.Split("Personality is an illusion.\nNothing but a function to control one's own emotional\npatterns.|0", new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, message, true, true, true, false);
                message = Util.Split("For an unstable existence such as yours...\nIt could be changed and reformed infinitely.\nLike this...|0", new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, message, true, true, true, false);
                //Make changes
                gd.baseData.takashiSex = 0;
                gd.GetFromBaseData(ref gd.baseData);
                //Finish dialogue
                sp.UnloadTexture("ive004_1");
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    //Removing duplicate screens
                    sp.RemoveScreen(0);
                    sp.RemoveScreen(20);
                    sp.RemoveScreen(50);
                    //Standard event flow
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Interface, 50U, true, false, -1);
                    sp.SetColorScreen(255U, 50001U, -1);
                    return true;
                }, (float e) => true, false));
                sp.ShowChara(ad.targetP, 4, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(180f, 60f, 600f, 337.5f), 0f, 1f, true, null, true);
                sp.FadeBgmInScript(-1f, 0.6f, 1.2f, false, -1);
                sp.SetFadeScreen(new List<uint> { 50001U }, 50002U, 0.6f, 0, true, true, true);
                message = Util.Split("So...\nYou wanted to change yourself.|4", new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, message, true, true, true, true);
                message = Util.Split("That foolishness... heh.\nIt is almost something to be envied.|3", new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, message, true, true, false, true);
                //End event
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 3f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(0.4f, true);
                gd.forwardNext = true;
            };
            myActionContents2.DoAfter = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScenarioContents.ChangeSceOnFlg(ref gd, ref sd, 8U);
                sd.flg = (ushort)((int)sd.flg & -16385);
                ushort[] sce_ind_flg = gd.baseData.sce_ind_flg;
                int id = sd.id;
                sce_ind_flg[id] |= 128;
                gd.baseData.sce_flg |= 134217728UL;
            };
            myActionContents2.FinAtHere = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                gd.forwardNext = true;
                gd.SetState(21);
            };
            __instance.actions.Add(myActionContents2);
            ScenarioContents.ActionContents myActionContents3 = default(ScenarioContents.ActionContents);
            myActionContents3.Initialize();
            myActionContents3.canPlayByState = 1073741824UL;
            myActionContents3.canPlayByActionFlg = 16;
            myActionContents3.canNotPlayByActionFlg = 18446744073709551597UL;
            myActionContents3.changedActionFlg = 65535UL;
            myActionContents3.reduceActionFlg = 0;
            myActionContents3.forceForward = false;
            myActionContents3.name = "Female";
            myActionContents3.needSelect = 0;
            myActionContents3.SelectUser = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                if ((sd.flg & 32768) == 0)
                {
                    return;
                }
                if ((sd.flg & 16384) == 0)
                {
                    return;
                }
                if (gd.baseData.takashiSex == 1)
                {
                    return;
                }
                if (gd.chara[sd.mainP].place == gd.baseData.place)
                {
                    ad.canPlayUser = 1;
                    ad.targetP = sd.mainP;
                    ad.power = 0f;
                    return;
                }
            };
            myActionContents3.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get stuff
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                //Initial Dialogue
                List<string> message = Util.Split("I see...\nThen let me help you.", new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, message, false, false, false, true);
                sp.LoadTexture("ive004_1");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.PlaySeInScript("se_ashioto_02", 1f);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.4f, 0, true, true, true);
                sp.WaitSec(0.4f, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ive004_1", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, true));
                sp.StopAllSeInScript();
                sp.FadeBgmInScript(-1f, 1f, 0.8f, false, -1);
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.4f, 0, true, true, true);
                sp.WaitSec(0.2f, true);
                message = Util.Split("Personality is an illusion.\nNothing but a function to control one's own emotional\npatterns.|0", new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, message, true, true, true, false);
                message = Util.Split("For an unstable existence such as yours...\nIt could be changed and reformed infinitely.\nLike this...|0", new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, message, true, true, true, false);
                //Make changes
                gd.baseData.takashiSex = 1;
                gd.GetFromBaseData(ref gd.baseData);
                //Finish dialogue
                sp.UnloadTexture("ive004_1");
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    //Removing duplicate screens
                    sp.RemoveScreen(0);
                    sp.RemoveScreen(20);
                    sp.RemoveScreen(50);
                    //Standard event flow
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Interface, 50U, true, false, -1);
                    sp.SetColorScreen(255U, 50001U, -1);
                    return true;
                }, (float e) => true, false));
                sp.ShowChara(ad.targetP, 4, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(180f, 60f, 600f, 337.5f), 0f, 1f, true, null, true);
                sp.FadeBgmInScript(-1f, 0.6f, 1.2f, false, -1);
                sp.SetFadeScreen(new List<uint> { 50001U }, 50002U, 0.6f, 0, true, true, true);
                message = Util.Split("So...\nYou wanted to change yourself.|4", new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, message, true, true, true, true);
                message = Util.Split("That foolishness... heh.\nIt is almost something to be envied.|3", new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, message, true, true, false, true);
                //End event
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 3f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(0.4f, true);
                gd.forwardNext = true;
            };
            myActionContents3.DoAfter = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScenarioContents.ChangeSceOnFlg(ref gd, ref sd, 8U);
                sd.flg = (ushort)((int)sd.flg & -16385);
                ushort[] sce_ind_flg = gd.baseData.sce_ind_flg;
                int id = sd.id;
                sce_ind_flg[id] |= 128;
                gd.baseData.sce_flg |= 134217728UL;
            };
            myActionContents3.FinAtHere = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                gd.forwardNext = true;
                gd.SetState(21);
            };
            __instance.actions.Add(myActionContents3);
            ScenarioContents.ActionContents myActionContents4 = default(ScenarioContents.ActionContents);
            myActionContents4.Initialize();
            myActionContents4.canPlayByState = 1073741824UL;
            myActionContents4.canPlayByActionFlg = 16;
            myActionContents4.canNotPlayByActionFlg = 18446744073709551597UL;
            myActionContents4.changedActionFlg = 65535UL;
            myActionContents4.reduceActionFlg = 0;
            myActionContents4.forceForward = false;
            myActionContents4.name = "N/A";
            myActionContents4.needSelect = 0;
            myActionContents4.SelectUser = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            { 
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                if ((sd.flg & 32768) == 0)
                {
                    return;
                }
                if ((sd.flg & 16384) == 0)
                {
                    return;
                }
                if (gd.baseData.takashiSex == 2)
                {
                    return;
                }
                if (gd.chara[sd.mainP].place == gd.baseData.place)
                {
                    ad.canPlayUser = 1;
                    ad.targetP = sd.mainP;
                    ad.power = 0f;
                    return;
                }
            };
            myActionContents4.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get stuff
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                //Initial Dialogue
                List<string> message = Util.Split("I see...\nThen let me help you.", new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, message, false, false, false, true);
                sp.LoadTexture("ive004_1");
                sp.WaitLoad();
                sp.WaitText(50U, "test", true);
                sp.HideInterface(50U, true);
                sp.PlaySeInScript("se_ashioto_02", 1f);
                sp.SetFadeScreen(new List<uint> { 0U, 20U }, 30U, 0.4f, 0, true, true, true);
                sp.WaitSec(0.4f, true);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_none, 0U, false, false, -1);
                    sp.m_sb[0U].SetTexture(0, sp.m_sb[0U].gameObject.transform, 0U, "ive004_1", null, null);
                    sp.m_sb[0U].m_spriteMap[0U].SetVisible(true);
                    sp.SetColorScreen(255U, 30U, -1);
                    return true;
                }, (float e) => true, true));
                sp.StopAllSeInScript();
                sp.FadeBgmInScript(-1f, 1f, 0.8f, false, -1);
                sp.SetFadeScreen(new List<uint> { 30U }, 31U, 0.4f, 0, true, true, true);
                sp.WaitSec(0.2f, true);
                message = Util.Split("Personality is an illusion.\nNothing but a function to control one's own emotional\npatterns.|0", new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, message, true, true, true, false);
                message = Util.Split("For an unstable existence such as yours...\nIt could be changed and reformed infinitely.\nLike this...|0", new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, message, true, true, true, false);
                //Make changes
                gd.baseData.takashiSex = 2;
                gd.GetFromBaseData(ref gd.baseData);
                //Finish dialogue
                sp.UnloadTexture("ive004_1");
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    //Removing duplicate screens
                    sp.RemoveScreen(0);
                    sp.RemoveScreen(20);
                    sp.RemoveScreen(50);
                    //Standard event flow
                    sp.SetScreen(Setting.Screen.s_BG, 0U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Chara, 20U, true, false, -1);
                    sp.SetScreen(Setting.Screen.s_Interface, 50U, true, false, -1);
                    sp.SetColorScreen(255U, 50001U, -1);
                    return true;
                }, (float e) => true, false));
                sp.ShowChara(ad.targetP, 4, 1, 20U, false);
                sp.SetClipAnim(new List<uint> { 0U, 20U }, new Vector4(180f, 60f, 600f, 337.5f), 0f, 1f, true, null, true);
                sp.FadeBgmInScript(-1f, 0.6f, 1.2f, false, -1);
                sp.SetFadeScreen(new List<uint> { 50001U }, 50002U, 0.6f, 0, true, true, true);
                message = Util.Split("So...\nYou wanted to change yourself.|4", new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, message, true, true, true, true);
                message = Util.Split("That foolishness... heh.\nIt is almost something to be envied.|3", new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, message, true, true, false, true);
                //End event
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 3f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(0.4f, true);
                gd.forwardNext = true;
            };
            myActionContents4.DoAfter = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScenarioContents.ChangeSceOnFlg(ref gd, ref sd, 8U);
                sd.flg = (ushort)((int)sd.flg & -16385);
                ushort[] sce_ind_flg = gd.baseData.sce_ind_flg;
                int id = sd.id;
                sce_ind_flg[id] |= 128;
                gd.baseData.sce_flg |= 134217728UL;
            };
            myActionContents4.FinAtHere = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                gd.forwardNext = true;
                gd.SetState(21);
            };
            __instance.actions.Add(myActionContents4);
            ScenarioContents.ActionContents myActionContents5 = default(ScenarioContents.ActionContents);
            myActionContents5.Initialize();
            myActionContents5.canPlayByState = 1073741824UL;
            myActionContents5.canPlayByActionFlg = 16;
            myActionContents5.canNotPlayByActionFlg = 18446744073709551597UL;
            myActionContents5.changedActionFlg = 65535UL;
            myActionContents5.reduceActionFlg = 0;
            myActionContents5.forceForward = false;
            myActionContents5.name = "Nevermind";
            myActionContents5.needSelect = 0;
            myActionContents5.SelectUser = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                if ((sd.flg & 32768) == 0)
                {
                    return;
                }
                if ((sd.flg & 16384) == 0)
                {
                    return;
                }
                if (gd.chara[sd.mainP].place == gd.baseData.place)
                {
                    ad.canPlayUser = 1;
                    ad.mainP = sd.mainP;
                    ad.targetP = sd.mainP;
                    ad.power = 1f;
                    return;
                }
            };
            myActionContents5.DoIt = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                //Get stuff
                ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                //Dialogue
                List<string> message;
                if (ad.mainP == 0)
                {
                    message = Util.Split("You replied that you changed your mind.|4", new char[] { '|' });
                    sp.SetNormalSerifu(0, ad.targetP, 1, message, true, false, true, true);
                }
                message = Util.Split("Then leave.\nThere is nothing for you here.|2", new char[] { '|' });
                sp.SetNormalSerifu(ad.targetP, 0, 1, message, true, false, false, true);
                //End event
                sp.WaitSec(0.05f, false);
                sp.FadeBgmInScript(-1f, 0f, 3f, true, -1);
                sp.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 40002U, 1f, 0, false, true, true);
                sp.UnloadPlace();
                sp.WaitFade(new List<uint> { 40002U }, true, true);
                sp.WaitSec(0.4f, true);
                gd.forwardNext = true;
            };
            myActionContents5.DoAfter = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                ScenarioContents.ChangeSceOnFlg(ref gd, ref sd, 8U);
                sd.flg = (ushort)((int)sd.flg & -16385);
                ushort[] sce_ind_flg = gd.baseData.sce_ind_flg;
                int id = sd.id;
                sce_ind_flg[id] |= 128;
                gd.baseData.sce_flg |= 134217728UL;
            };
            myActionContents5.FinAtHere = delegate (ref gnosia.GameData.scenarioData sd, ref gnosia.GameData.actionData ad)
            {
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                gd.forwardNext = true;
                gd.SetState(21);
            };
            __instance.actions.Add(myActionContents5);
        }
    }
}