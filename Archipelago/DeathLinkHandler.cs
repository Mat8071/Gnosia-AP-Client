using System;
using System.Collections.Generic;
using application;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using baseEffect.graphics;
using BepInEx;
using coreSystem;
using gnosia;
using GnosiaArchipelagoRandomizer.Patches.DeathLink;
using GnosiaArchipelagoRandomizer.Utils;
using HarmonyLib;
using setting;
using UnityEngine;
using UnityEngine.UI;

namespace GnosiaArchipelagoRandomizer.Archipelago
{
    public class DeathLinkHandler
    {
        private static bool deathLinkEnabled;
        private string slotName;
        private readonly DeathLinkService service;
        private readonly Queue<DeathLink> deathLinks = new();
        private Harmony harmony = new Harmony(Plugin.PluginGUID + ".deathlink");

        /// <summary>
        /// instantiates our death link handler, sets up the hook for receiving death links, and enables death link if needed
        /// </summary>
        /// <param name="deathLinkService">The new DeathLinkService that our handler will use to send and
        /// receive death links</param>
        /// <param name="enableDeathLink">Whether we should enable death link or not on startup</param>
        public DeathLinkHandler(DeathLinkService deathLinkService, string name, bool enableDeathLink = false)
        {
            service = deathLinkService;
            service.OnDeathLinkReceived += DeathLinkReceived;
            slotName = name;
            deathLinkEnabled = enableDeathLink;

            if (deathLinkEnabled)
            {
                service.EnableDeathLink();
                harmony.CreateClassProcessor(typeof(DeathLinkPatch)).Patch();
                Plugin.BepinLogger.LogInfo("DeathLink Activated!");
            }
        }

        public void Unsubscribe()
        {
            service.OnDeathLinkReceived -= DeathLinkReceived;
        }

        public bool IsDeathLinkEnabled()
        {
            return deathLinkEnabled;
        }

        /// <summary>
        /// enables/disables death link
        /// </summary>
        public void ToggleDeathLink()
        {
            deathLinkEnabled = !deathLinkEnabled;

            if (deathLinkEnabled)
            {
                service.EnableDeathLink();
                harmony.CreateClassProcessor(typeof(DeathLinkPatch)).Patch();
                Plugin.BepinLogger.LogInfo("DeathLink Activated!");
            }
            else
            {
                service.DisableDeathLink();
                harmony.UnpatchSelf();
                Plugin.BepinLogger.LogInfo("DeathLink Deactivated!");
            }
        }

        /// <summary>
        /// what happens when we receive a deathLink
        /// </summary>
        /// <param name="deathLink">Received Death Link object to handle</param>
        private void DeathLinkReceived(DeathLink deathLink)
        {
            deathLinks.Enqueue(deathLink);

            Plugin.BepinLogger.LogDebug(deathLink.Cause.IsNullOrWhiteSpace()
                ? $"Received Death Link from: {deathLink.Source}"
                : deathLink.Cause);

            //For now, just try to kill the player regardless
            KillPlayer();
        }

        /// <summary>
        /// can be called when in a valid state to kill the player, dequeueing and immediately killing the player with a
        /// message if we have a death link in the queue
        /// </summary>
        public void KillPlayer()
        {
            try
            {
                if (deathLinks.Count < 1) return;

                var deathLink = deathLinks.Dequeue();
                var cause = deathLink.Cause.IsNullOrWhiteSpace() ? GetDeathLinkCause(deathLink) : deathLink.Cause;

                KillPlayer(cause);
            }
            catch (Exception e)
            {
                Plugin.BepinLogger.LogError(e);
            }
        }


        public void KillPlayer(string cause)
        {
            //Show the deathlink cause in the console (regardless of whether it works)
            ArchipelagoConsole.LogMessage(cause);
            //Get gd and sp
            gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
            ScriptParser sp = GameObject.Find("Application").GetComponent<ScriptParser>();
            //Check if another deathlink is already happening
            if ((gd.baseData.sce_all_flg & 1UL) != 0)
                return;
            //Check if state is ok for killing player
            if (gd.personFromId[0] >= 0 && gd.baseData.state > 4 && gd.baseData.state < 32)
            {
                //Don't kill the player during tutorial loops or things could break
                if (gd.baseData.loop < 14)
                    return;
                //Mark deathlink death with an unused flag
                gd.baseData.sce_all_flg |= 1UL;
                //Insert killing the player (+ fx) in the script queue
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    //Set player status to "Eliminated"
                    gnosia.GameData.character player = gd.chara[gd.personFromId[0]];
                    player.doa = setting.Setting.Doa.doa_Kamare;
                    gd.chara[gd.personFromId[0]] = player;
                    gd.RemakePeopleFlg();
                    //Play sfx and music and prepare for vfx
                    sp.m_sm.PlaySe("se_jin_06", 1f);
                    sp.m_sm.FadeBgm(-1f, 0f, 3.5f, true, -1);
                    return true;
                }, (float e) => true, false));
                //Do a screen animation (safely)
                uint _to = 50000U;
                float fadeTime = 1.6f;
                int fadeType = 100;
                bool waitFinish = true;
                bool deleteOldScreen = true;
                bool canSkip = false;
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    //Try catch just in case so the game does not freeze
                    try
                    {
                        //Getting the list directly in the script queue
                        List<uint> _from = new List<uint>(sp.m_sb.Keys);
                        //Base method
                        sp.isFading = true;
                        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(sp.m_rs.baseScreenPrefab, sp.m_rs.ParentObj[0].transform);
                        gameObject.GetComponent<Canvas>().sortingOrder = Traverse.Create(sp).Method("GetDepth", new object[] { (int)_to }).GetValue<int>();
                        gameObject.GetComponent<Canvas>().worldCamera = sp.m_rs.mainCamera;
                        int fadeScreenEnableId = sp.m_rs.GetFadeScreenEnableId();
                        sp.m_rs.SetFadeScreenEnable(fadeScreenEnableId, Traverse.Create(sp).Method("GetDepth", new object[] { (int)_to }).GetValue<int>());
                        gameObject.name = "FadeScreenCanvas" + _to;
                        FadeScreen fadeScreen = gameObject.AddComponent<FadeScreen>();
                        fadeScreen.screenId = fadeScreenEnableId;
                        gameObject.layer = ((fadeScreenEnableId == 0) ? fadeScreen.CopyLayer : fadeScreen.Copy2Layer);
                        sp.SetScreen(fadeScreen, _to, false);
                        foreach (uint num in _from)
                        {
                            foreach (object obj in sp.m_sb[num].gameObject.transform)
                            {
                                Transform transform = (Transform)obj;
                                if (!transform.gameObject.name.Contains("threshold") && !transform.gameObject.name.Contains("bugFadeImg"))
                                {
                                    GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(transform.gameObject, fadeScreen.gameObject.transform);
                                    Vector2 vector = sp.m_sb[num].GetComponent<RectTransform>().localScale;
                                    float num2 = gameObject2.transform.localScale.x * vector.x;
                                    float num3 = gameObject2.transform.localScale.y * vector.y;
                                    gameObject2.transform.localScale = new Vector3(num2, num3, 1f);
                                    Vector3 anchoredPosition3D = gameObject2.GetComponent<RectTransform>().anchoredPosition3D;
                                    Vector3 anchoredPosition3D2 = sp.m_sb[num].GetComponent<RectTransform>().anchoredPosition3D;
                                    float num4 = anchoredPosition3D2.x - -640f;
                                    float num5 = anchoredPosition3D2.y - 360f;
                                    num4 = anchoredPosition3D.x * vector.x + num4;
                                    num5 = anchoredPosition3D.y * vector.y + num5;
                                    gameObject2.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(num4, num5, 0f);
                                    if (gameObject2.GetComponent<Image>() != null)
                                    {
                                        gameObject2.GetComponent<Image>().material = sp.m_rs.uiDefaultMat;
                                    }
                                }
                            }
                        }
                        fadeScreen.SetTexture(0, fadeScreen.gameObject.transform, 0U, fadeScreen.textureName, null, null);
                        fadeScreen.m_spriteMap[0U].m_type = Sprite2dEffectArg.SpriteType.k_SpriteTypeCopy;
                        fadeScreen.SetFadeMaterial(fadeScreenEnableId);
                        fadeScreen.NotifyFinish(fadeTime, fadeType, false);
                        if (deleteOldScreen)
                        {
                            foreach (uint num6 in _from)
                            {
                                sp.RemoveScreen(num6);
                            }
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Plugin.BepinLogger.LogError($"Error during deathlink animation!\n\n{ex}");
                        return true;
                    }
                }, (float e) => true, true));
                if (waitFinish)
                {
                    sp.WaitFade(new List<uint> { _to }, true, canSkip);
                }
                //Show message with cause
                sp.SetDialogScreen(50400U, cause, 1, false);
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    //For SetState to work properly, first we need to set seState to ss_init
                    //This is because of how ScenarioEngineObj works
                    gd.seState = GameData.ScenarioState.ss_init;
                    //Set state to game finished
                    gd.SetState(35);
                    gd.forwardNext = true;
                    return true;
                }, (float e) => true, false));
                //End Loop
                gd.stopScenario = true;
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetColorScreen(255U, 50000U, -1);
                    return true;
                }, (float e) => true, false));
                sp.LoadTexture("result");
                sp.WaitLoad();
                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                {
                    sp.SetScreen(Setting.Screen.s_Result, 100U, true, false, -1);
                    return true;
                }, (float e) => true, true));
                sp.SetFadeScreen(new List<uint> { 50000U }, 50001U, 0.25f, 0, true, true, true);
                //Log
                Plugin.BepinLogger.LogMessage(cause);
            }
            else
                Plugin.BepinLogger.LogInfo("DeathLink skipped due to invalid state");
        }


        /// <summary>
        /// returns message for the player to see when a death link is received without a cause
        /// </summary>
        /// <param name="deathLink">death link object to get relevant info from</param>
        /// <returns></returns>
        private string GetDeathLinkCause(DeathLink deathLink)
        {
            return $"Received death from {deathLink.Source}";
        }

        /// <summary>
        /// called to send a death link to the multiworld
        /// </summary>
        public void SendDeathLink()
        {
            try
            {
                if (!deathLinkEnabled) return;

                Plugin.BepinLogger.LogMessage("sharing your death...");

                //Get gd
                gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                // add the cause here
                var cause = "";
                if (gd.personFromId[0] >= 0)
                {
                    gnosia.GameData.character player = gd.chara[gd.personFromId[0]];
                    if (player.doa == setting.Setting.Doa.doa_Kamare)
                        if (Jinro.CheckEnd() == 9)
                            cause = $"Kukrushka has... destroyed everything (including {slotName})";
                        else if (player.i_yaku != setting.Setting.Yakuwari.y_Fox)
                            cause = $"{slotName} has been... eliminated by the Gnosia";
                        else
                            cause = $"The engineer has discovered that {slotName} is a Bug and has eliminated them";
                    else if (player.doa == setting.Setting.Doa.doa_Shokei)
                        cause = $"The crew has decided to put {slotName} into cold sleep";
                }

                var linkToSend = new DeathLink(slotName, cause);

                service.SendDeathLink(linkToSend);
                //Show the deathlink message on the console as well
                ArchipelagoConsole.LogMessage(cause);
            }
            catch (Exception e)
            {
                Plugin.BepinLogger.LogError(e);
            }
        }
    }
}