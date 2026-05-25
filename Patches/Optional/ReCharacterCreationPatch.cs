using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using application;
using coreSystem;
using HarmonyLib;
using sce.SampleUtil.Input;
using UnityEngine.Rendering;
using setting;
using Rewired;
using JetBrains.Annotations;
using util;
using resource;
using UnityEngine;
using System.Reflection;
using TMPro;

namespace GnosiaArchipelagoRandomizer.Patches.Optional
{
    [HarmonyPatch]
    class ReCharacterCreationPatch
    {
        [HarmonyReversePatch]
        [HarmonyPatch(typeof(application.Screen), "MyUpdate")]
        static int BaseMyUpdate(application.Screen __instance, float ellapseSec, ControllerContext controllerContext, bool covered = false)
        {
            throw new NotImplementedException();
        }

        [HarmonyPatch(typeof(TakashiSetScreen), "InitializeGlm")]
        [HarmonyPostfix]
        static void SetCorrectValues(TakashiSetScreen __instance)
        {
            Traverse t = Traverse.Create(__instance);
            gnosia.GameData gd = t.Field("mydata").GetValue<gnosia.GameData>();
            if (gd.baseData.loop > 0)
            {
                //Replace default values with ones from current playthrough
                t.Field("abilLeft").SetValue(0);
                __instance.SetText("abilleft", t.Field("abilLeft").GetValue<int>().ToString(), false, true);
                t.Method("InputFieldEndEdit", gd.takashiName).GetValue();
                //Reset Sex/Color modifiers
                switch (gd.baseData.takashiColor)
                {
                    case 0:
                        gd.baseData.s_loveList[0].data[2] -= 3276U;
                        gd.baseData.s_loveList[0].data[3] -= 3276U;
                        gd.baseData.s_loveList[0].data[8] -= 3276U;
                        gd.baseData.s_loveList[0].data[4] += 2457U;
                        gd.baseData.s_loveList[0].data[7] += 2457U;
                        break;
                    case 1:
                        gd.baseData.s_loveList[0].data[5] -= 3276U;
                        gd.baseData.s_loveList[0].data[6] -= 3276U;
                        gd.baseData.s_loveList[0].data[10] -= 3276U;
                        gd.baseData.s_loveList[0].data[13] += 2457U;
                        gd.baseData.s_loveList[0].data[1] += 2457U;
                        gd.baseData.s_loveList[0].data[12] += 2457U;
                        gd.baseData.s_loveList[0].data[7] += 2457U;
                        break;
                    case 2:
                        gd.baseData.s_loveList[0].data[2] -= 3276U;
                        gd.baseData.s_loveList[0].data[3] -= 3276U;
                        gd.baseData.s_loveList[0].data[8] -= 3276U;
                        gd.baseData.s_loveList[0].data[11] += 2457U;
                        gd.baseData.s_loveList[0].data[13] += 2457U;
                        gd.baseData.s_loveList[0].data[12] += 2457U;
                        break;
                    case 3:
                        gd.baseData.s_loveList[0].data[5] -= 3276U;
                        gd.baseData.s_loveList[0].data[6] -= 3276U;
                        gd.baseData.s_loveList[0].data[10] -= 3276U;
                        gd.baseData.s_loveList[0].data[1] += 2457U;
                        break;
                    case 4:
                        gd.baseData.s_loveList[0].data[1] -= 3276U;
                        gd.baseData.s_loveList[0].data[4] -= 3276U;
                        gd.baseData.s_loveList[0].data[13] -= 3276U;
                        gd.baseData.s_loveList[0].data[2] += 2457U;
                        gd.baseData.s_loveList[0].data[10] += 2457U;
                        break;
                    case 5:
                        gd.baseData.s_loveList[0].data[11] -= 3276U;
                        gd.baseData.s_loveList[0].data[12] -= 3276U;
                        gd.baseData.s_loveList[0].data[7] -= 3276U;
                        gd.baseData.s_loveList[0].data[10] += 2457U;
                        gd.baseData.s_loveList[0].data[8] += 2457U;
                        break;
                    case 6:
                        gd.baseData.s_loveList[0].data[2] -= 3276U;
                        gd.baseData.s_loveList[0].data[3] -= 3276U;
                        gd.baseData.s_loveList[0].data[8] -= 3276U;
                        gd.baseData.s_loveList[0].data[1] += 2457U;
                        break;
                    case 7:
                        gd.baseData.s_loveList[0].data[1] -= 3276U;
                        gd.baseData.s_loveList[0].data[4] -= 3276U;
                        gd.baseData.s_loveList[0].data[13] -= 3276U;
                        gd.baseData.s_loveList[0].data[5] += 2457U;
                        gd.baseData.s_loveList[0].data[6] += 2457U;
                        gd.baseData.s_loveList[0].data[7] += 2457U;
                        break;
                    case 8:
                        gd.baseData.s_loveList[0].data[11] -= 3276U;
                        gd.baseData.s_loveList[0].data[12] -= 3276U;
                        gd.baseData.s_loveList[0].data[7] -= 3276U;
                        gd.baseData.s_loveList[0].data[1] -= 3276U;
                        gd.baseData.s_loveList[0].data[2] += 2457U;
                        gd.baseData.s_loveList[0].data[6] += 2457U;
                        gd.baseData.s_loveList[0].data[8] += 2457U;
                        break;
                    case 9:
                        gd.baseData.s_loveList[0].data[5] -= 3276U;
                        gd.baseData.s_loveList[0].data[6] -= 3276U;
                        gd.baseData.s_loveList[0].data[10] -= 3276U;
                        gd.baseData.s_loveList[0].data[11] += 2457U;
                        gd.baseData.s_loveList[0].data[7] += 2457U;
                        gd.baseData.s_loveList[0].data[4] += 2457U;
                        break;
                    case 10:
                        gd.baseData.s_loveList[0].data[1] -= 3276U;
                        gd.baseData.s_loveList[0].data[4] -= 3276U;
                        gd.baseData.s_loveList[0].data[13] -= 3276U;
                        gd.baseData.s_loveList[0].data[6] += 2457U;
                        gd.baseData.s_loveList[0].data[8] += 2457U;
                        break;
                    case 11:
                        gd.baseData.s_loveList[0].data[11] -= 3276U;
                        gd.baseData.s_loveList[0].data[12] -= 3276U;
                        gd.baseData.s_loveList[0].data[7] -= 3276U;
                        gd.baseData.s_loveList[0].data[1] += 2457U;
                        gd.baseData.s_loveList[0].data[6] += 2457U;
                        gd.baseData.s_loveList[0].data[2] += 2457U;
                        gd.baseData.s_loveList[0].data[10] += 2457U;
                        break;
                }
                if (gd.baseData.takashiSex == 0)
                {
                    gd.baseData.s_loveList[0].data[4] -= 6553U;
                }
                else if (gd.baseData.takashiSex == 1)
                {
                    gd.baseData.s_loveList[0].data[13] -= 6553U;
                }
                else
                {
                    gd.baseData.s_loveList[0].data[3] -= 6553U;
                    gd.baseData.s_loveList[0].data[13] -= 3276U;
                }
            }
        }

        [HarmonyPatch(typeof(TakashiSetScreen), "MyUpdate")]
        [HarmonyPrefix]
        static bool StopIntroCutscene(TakashiSetScreen __instance, ref int __result, float ellapseSec, ControllerContext controllerContext, bool covered = false)
        {
            Traverse t = Traverse.Create(__instance);
            gnosia.GameData gd = t.Field("mydata").GetValue<gnosia.GameData>();
            ScriptParser sp = t.Field("m_scriptParser").GetValue<ScriptParser>();
            if (gd.baseData.loop == 0)
            {
                return true;
            }
            //Some more reflection setup
            Type InputFieldStateType = typeof(application.TakashiSetScreen).GetNestedType("InputFieldState", BindingFlags.NonPublic);
            Type DataType = AccessTools.TypeByName("gnosia.Data");
            Array chara = (Array)AccessTools.Field(DataType, "Chara").GetValue(null);
            object mainChara = chara.GetValue(0);
            Traverse mcTraverse = Traverse.Create(mainChara);
            //Get private stuff
            int selectState = t.Field("selectState").GetValue<int>();
            int selectTgt = t.Field("selectTgt").GetValue<int>();
            int abilLeft = t.Field("abilLeft").GetValue<int>();
            bool isEnter = t.Field("isEnter").GetValue<bool>();
            bool isStart = t.Field("isStart").GetValue<bool>();
            bool isOnPoint = t.Field("isOnPoint").GetValue<bool>();
            bool isEnterOnPointer = t.Field("isEnterOnPointer").GetValue<bool>();
            bool isDownUpDownKey = t.Field("isDownUpDownKey").GetValue<bool>();
            bool isMouseEnable = t.Field("isMouseEnable").GetValue<bool>();
            bool isScreenEnd = t.Field("isScreenEnd").GetValue<bool>();
            ResourceManager m_resourceManager = t.Field("m_resourceManager").GetValue<ResourceManager>();
            object inputFieldState = t.Field("inputFieldState").GetValue();
            TMP_InputField inputField = t.Field("inputField").GetValue<TMP_InputField>();
            List<float> mcAbil = mcTraverse.Field("abil").GetValue<List<float>>();
            //Base implementation (Modified)
            BaseMyUpdate((application.Screen)__instance, ellapseSec, controllerContext, covered);
            __instance.m_spriteMap[1U].SetVisible(false);
            __instance.m_spriteMap[1010U].SetVisible(false);
            __instance.m_spriteMap[1020U].SetVisible(false);
            __instance.m_spriteMap[1030U].SetVisible(false);
            __instance.m_spriteMap[1200U].SetVisible(false);
            __instance.m_spriteMap[5000U].SetVisible(false);
            __instance.m_spriteMap[5001U].SetVisible(false);
            __instance.m_spriteMap[5002U].SetVisible(false);
            __instance.m_spriteMap[580U].SetVisible(false);
            if (!covered && sp.scriptQueue.Count == 0)
            {
                if (selectState == 0)
                {
                    bool flag = false;
                    int num = selectTgt;
                    if (!inputFieldState.Equals(Enum.Parse(InputFieldStateType, "active")))
                    {
                        selectTgt = (selectTgt + 80 + Util.IsMenuUpDown(controllerContext, 30, 8) * 10) % 80;
                    }
                    if (__instance.m_textAreaMap["namae"].Finished() && __instance.m_textAreaMap["namae"].strList.Count != 0 && abilLeft == 0)
                    {
                        __instance.m_spriteMap[1U].SetVisible(true);
                    }
                    int num2 = ((!inputFieldState.Equals(Enum.Parse(InputFieldStateType, "active"))) ? Util.IsMenuLeftRight(controllerContext, 30, 8) : 0);
                    if (selectTgt < 10)
                    {
                        if (__instance.m_spriteMap[1U].GetVisible())
                        {
                            selectTgt = (selectTgt + 3 + num2) % 3;
                        }
                        else
                        {
                            selectTgt = (selectTgt + 2 + num2) % 2;
                        }
                    }
                    else if (selectTgt >= 10 && selectTgt < 70)
                    {
                        if (num2 == 0)
                        {
                            if (controllerContext.IsMouseWheelUPDown())
                            {
                                num2 = 1;
                            }
                            else if (controllerContext.IsMouseWheelDownDown())
                            {
                                num2 = -1;
                            }
                        }
                        if (num != selectTgt)
                        {
                            selectTgt = selectTgt / 10 * 10;
                        }
                        if (num2 != 0)
                        {
                            if (selectTgt % 10 > 0)
                            {
                                selectTgt = selectTgt / 10 * 10;
                            }
                            else if (__instance.m_spriteMap[1U].GetVisible() && ((gd.baseData.takashiAbil[selectTgt / 10 - 1] == 1 && num2 == -1) || num2 == 1))
                            {
                                selectTgt++;
                            }
                            else
                            {
                                if (gd.baseData.takashiAbil[selectTgt / 10 - 1] > 1 && num2 == -1)
                                {
                                    byte[] takashiAbil = gd.baseData.takashiAbil;
                                    int num3 = selectTgt / 10 - 1;
                                    takashiAbil[num3] -= 1;
                                    abilLeft++;
                                    flag = true;
                                }
                                else if (abilLeft > 0 && num2 == 1)
                                {
                                    byte[] takashiAbil2 = gd.baseData.takashiAbil;
                                    int num4 = selectTgt / 10 - 1;
                                    takashiAbil2[num4] += 1;
                                    abilLeft--;
                                    flag = true;
                                }
                                if (flag)
                                {
                                    string text = string.Concat(new object[]
                                    {
                                        gd.baseData.takashiAbil[0],
                                        "\n",
                                        gd.baseData.takashiAbil[1],
                                        "\n",
                                        gd.baseData.takashiAbil[2],
                                        "\n",
                                        gd.baseData.takashiAbil[3],
                                        "\n",
                                        gd.baseData.takashiAbil[4],
                                        "\n",
                                        gd.baseData.takashiAbil[5]
                                    });
                                    __instance.SetText("abil", text, true, true);
                                    __instance.SetText("abilleft", abilLeft.ToString(), true, true);
                                    float num5 = (float)gd.baseData.takashiAbil[selectTgt / 10 - 1] / 50f;
                                    __instance.m_spriteMap[(uint)(1500 + selectTgt / 10 - 1)].SetVisible(true);
                                    __instance.m_spriteMap[(uint)(1500 + selectTgt / 10 - 1)].SetImageGauge(num5);
                                }
                            }
                        }
                    }
                    else if (__instance.m_spriteMap[1U].GetVisible())
                    {
                        selectTgt = (selectTgt % 10 + 2 + num2) % 2 + selectTgt / 10 * 10;
                    }
                    else
                    {
                        selectTgt = selectTgt / 10 * 10;
                    }
                    if ((selectTgt == 2 || (selectTgt >= 10 && selectTgt % 10 > 0)) && !isEnterOnPointer)
                    {
                        __instance.m_spriteMap[580U].SetVisible(true);
                    }
                    if (num != selectTgt || flag)
                    {
                        sp.m_sm.PlaySe("se_cursormove", 1f);
                        if (num >= 10 && num < 70 && num % 10 == 0)
                        {
                            __instance.m_spriteMap[510U].SetVisible(false);
                            __instance.m_spriteMap[1900U].SetVisible(false);
                            __instance.m_spriteMap[1901U].SetVisible(false);
                        }
                        else if (num == 2 || (num >= 10 && num % 10 > 0))
                        {
                            __instance.m_spriteMap[580U].SetVisible(false);
                        }
                        else
                        {
                            __instance.m_spriteMap[(uint)(500 + num)].SetVisible(false);
                        }
                        if (selectTgt >= 10 && selectTgt < 70 && selectTgt % 10 == 0)
                        {
                            __instance.m_spriteMap[510U].SetVisible(true);
                            if (gd.baseData.takashiAbil[selectTgt / 10 - 1] > 1)
                            {
                                __instance.m_spriteMap[1900U].SetVisible(true);
                            }
                            if (abilLeft > 0)
                            {
                                __instance.m_spriteMap[1901U].SetVisible(true);
                            }
                            __instance.m_spriteMap[510U].SetDisplayOffsetY(194f + 28f * (float)(selectTgt / 10 - 1));
                            __instance.m_spriteMap[1900U].SetDisplayOffsetY(202f + 28f * (float)(selectTgt / 10 - 1));
                            __instance.m_spriteMap[1901U].SetDisplayOffsetY(202f + 28f * (float)(selectTgt / 10 - 1));
                            isDownUpDownKey = true;
                        }
                        else if (selectTgt < 2 || selectTgt == 70)
                        {
                            __instance.m_spriteMap[(uint)(500 + selectTgt)].SetVisible(true);
                        }
                    }
                    if (selectTgt == 0)
                    {
                        __instance.m_spriteMap[5000U].SetVisible(true);
                    }
                    else if (selectTgt == 1)
                    {
                        __instance.m_spriteMap[5001U].SetVisible(true);
                    }
                    else if (selectTgt >= 10 && selectTgt < 70 && !__instance.m_spriteMap[1U].GetVisible())
                    {
                        __instance.m_spriteMap[1010U].SetVisible(true);
                    }
                    else if (selectTgt == 70)
                    {
                        __instance.m_spriteMap[5001U].SetVisible(true);
                    }
                    else if (selectTgt == 2 || (selectTgt >= 10 && selectTgt % 10 > 0))
                    {
                        __instance.m_spriteMap[5002U].SetVisible(true);
                    }
                    if (__instance.m_spriteMap[1U].GetVisible())
                    {
                        __instance.m_spriteMap[1030U].SetVisible(true);
                    }
                    else if (selectTgt < 10 || selectTgt >= 70)
                    {
                        __instance.m_spriteMap[1020U].SetVisible(true);
                    }
                    if (!inputFieldState.Equals(Enum.Parse(InputFieldStateType, "active")) && (controllerContext.IsButtonPressed(0) || isEnter || (!isOnPoint && controllerContext.IsMouseEnterPressed())))
                    {
                        isEnter = false;
                        if (selectTgt == 0)
                        {
                            sp.m_sm.PlaySe("se_select", 1f);
                            if (inputFieldState.Equals(Enum.Parse(InputFieldStateType, "finished")))
                            {
                                inputFieldState = Enum.Parse(InputFieldStateType, "inactive");
                            }
                            else
                            {
                                inputField.gameObject.SetActive(true);
                                inputField.Select();
                                selectState = 1;
                                isScreenEnd = true;
                            }
                        }
                        else if (selectTgt == 1)
                        {
                            sp.m_sm.PlaySe("se_select", 1f);
                            selectState = 2;
                            isScreenEnd = true;
                            sp.WaitSec(0.01f, false);
                            sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                            {
                                sp.SetScreen(Setting.Screen.s_SexSelect, 200U, true, false, -1);
                                return true;
                            }, delegate (float e)
                            {
                                if (sp.m_sb[200U].IsFinished())
                                {
                                    sp.RemoveScreen(200U);
                                    return true;
                                }
                                return false;
                            }, true));
                        }
                        else if (selectTgt == 70)
                        {
                            sp.m_sm.PlaySe("se_select", 1f);
                            selectState = 3;
                            isScreenEnd = true;
                            sp.WaitSec(0.01f, false);
                            sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                            {
                                sp.SetScreen(Setting.Screen.s_ColorSelect, 200U, true, false, -1);
                                return true;
                            }, delegate (float e)
                            {
                                if (sp.m_sb[200U].IsFinished())
                                {
                                    sp.RemoveScreen(200U);
                                    return true;
                                }
                                return false;
                            }, true));
                        }
                        else if (selectTgt == 2 || (selectTgt >= 10 && selectTgt % 10 > 0))
                        {
                            sp.m_sm.PlaySe("se_select", 1f);
                            selectState = 4;
                            isScreenEnd = true;
                            sp.WaitSec(0.01f, false);
                            sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                            {
                                sp.SetScreen(Setting.Screen.s_Regist, 200U, true, false, -1);
                                return true;
                            }, delegate (float e)
                            {
                                if (sp.m_sb[200U].IsFinished())
                                {
                                    sp.RemoveScreen(200U);
                                    return true;
                                }
                                return false;
                            }, true));
                        }
                    }
                    else if (!inputFieldState.Equals(Enum.Parse(InputFieldStateType, "active")) && (controllerContext.IsButtonPressed(6) || isStart))
                    {
                        isStart = false;
                        if (__instance.m_spriteMap[1U].GetVisible())
                        {
                            sp.m_sm.PlaySe("se_select", 1f);
                            __instance.m_spriteMap[500U].SetVisible(false);
                            __instance.m_spriteMap[501U].SetVisible(false);
                            __instance.m_spriteMap[510U].SetVisible(false);
                            __instance.m_spriteMap[570U].SetVisible(false);
                            __instance.m_spriteMap[580U].SetVisible(false);
                            __instance.m_spriteMap[1900U].SetVisible(false);
                            __instance.m_spriteMap[1901U].SetVisible(false);
                            selectTgt = 71;
                            selectState = 4;
                            isScreenEnd = true;
                            sp.WaitSec(0.01f, false);
                            sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                            {
                                sp.SetScreen(Setting.Screen.s_Regist, 200U, true, false, -1);
                                return true;
                            }, delegate (float e)
                            {
                                if (sp.m_sb[200U].IsFinished())
                                {
                                    sp.RemoveScreen(200U);
                                    return true;
                                }
                                return false;
                            }, true));
                        }
                    }
                }
                else
                {
                    int select = sp.GetSelect(0);
                    if (select >= 0)
                    {
                        if (selectState == 1)
                        {
                            if (controllerContext.IsButtonPressed(1))
                            {
                                inputField.gameObject.SetActive(false);
                                inputFieldState = Enum.Parse(InputFieldStateType, "inactive");
                                selectState = 0;
                                isScreenEnd = false;
                            }
                            else if (inputFieldState.Equals(Enum.Parse(InputFieldStateType, "editEnd")))
                            {
                                inputFieldState = Enum.Parse(InputFieldStateType, "finished");
                                t.Method("ValidateString", inputField.textComponent.text).GetValue<string>();
                                inputField.gameObject.SetActive(false);
                                selectState = 0;
                                isScreenEnd = false;
                            }
                        }
                        else if (selectState == 2)
                        {
                            if (select >= 1)
                            {
                                gd.baseData.takashiSex = (byte)(select - 1);
                                __instance.SetText("sex", Setting.SexName[(int)gd.baseData.takashiSex], true, true);
                            }
                            selectState = 0;
                            isScreenEnd = false;
                        }
                        else if (selectState == 3)
                        {
                            if (select >= 1)
                            {
                                gd.baseData.takashiColor = (byte)(select - 1);
                                __instance.SetText("colorname", Setting.ColorName[(int)gd.baseData.takashiColor], false, true);
                                string text2 = "c" + select.ToString("D2");
                                __instance.RemoveTexture(9500U);
                                __instance.SetPackedTexture(0, __instance.transform, "entry", text2, 9500U, 100U, new Vector2?(new Vector2(127f, 413f)), null, null, false);
                                __instance.m_spriteMap[9500U].SetSize(0.67f);
                                __instance.m_spriteMap[9500U].SetRaycastTarget(false);
                                __instance.m_spriteMap[9500U].SetCenterPosition(new Vector2(158f, 448f));
                                __instance.m_spriteMap[9500U].SetVisible(true);
                            }
                            selectState = 0;
                            isScreenEnd = false;
                        }
                        else if (selectState == 4)
                        {
                            if (select >= 1)
                            {
                                selectState = 5;
                                gd.takashiName = __instance.m_textAreaMap["namae"].strList[0];
                                gd.baseData.takashiName = gd.takashiName;
                                mcTraverse.Field("name").SetValue(gd.takashiName);
                                mcTraverse.Field("sex").SetValue(gd.baseData.takashiSex);
                                mcAbil[0] = (float)gd.baseData.takashiAbil[0] / 50f;
                                mcAbil[1] = (float)gd.baseData.takashiAbil[1] / 50f;
                                mcAbil[2] = (float)gd.baseData.takashiAbil[3] / 50f;
                                mcAbil[3] = (float)gd.baseData.takashiAbil[2] / 50f;
                                mcAbil[4] = (float)gd.baseData.takashiAbil[4] / 50f;
                                mcAbil[5] = (float)gd.baseData.takashiAbil[5] / 50f;
                                gd.SetColorData(0, (int)gd.baseData.takashiColor);
                                switch (gd.baseData.takashiColor)
                                {
                                    case 0:
                                        gd.baseData.s_loveList[0].data[2] += 3276U;
                                        gd.baseData.s_loveList[0].data[3] += 3276U;
                                        gd.baseData.s_loveList[0].data[8] += 3276U;
                                        gd.baseData.s_loveList[0].data[4] -= 2457U;
                                        gd.baseData.s_loveList[0].data[7] -= 2457U;
                                        break;
                                    case 1:
                                        gd.baseData.s_loveList[0].data[5] += 3276U;
                                        gd.baseData.s_loveList[0].data[6] += 3276U;
                                        gd.baseData.s_loveList[0].data[10] += 3276U;
                                        gd.baseData.s_loveList[0].data[13] -= 2457U;
                                        gd.baseData.s_loveList[0].data[1] -= 2457U;
                                        gd.baseData.s_loveList[0].data[12] -= 2457U;
                                        gd.baseData.s_loveList[0].data[7] -= 2457U;
                                        break;
                                    case 2:
                                        gd.baseData.s_loveList[0].data[2] += 3276U;
                                        gd.baseData.s_loveList[0].data[3] += 3276U;
                                        gd.baseData.s_loveList[0].data[8] += 3276U;
                                        gd.baseData.s_loveList[0].data[11] -= 2457U;
                                        gd.baseData.s_loveList[0].data[13] -= 2457U;
                                        gd.baseData.s_loveList[0].data[12] -= 2457U;
                                        break;
                                    case 3:
                                        gd.baseData.s_loveList[0].data[5] += 3276U;
                                        gd.baseData.s_loveList[0].data[6] += 3276U;
                                        gd.baseData.s_loveList[0].data[10] += 3276U;
                                        gd.baseData.s_loveList[0].data[1] -= 2457U;
                                        break;
                                    case 4:
                                        gd.baseData.s_loveList[0].data[1] += 3276U;
                                        gd.baseData.s_loveList[0].data[4] += 3276U;
                                        gd.baseData.s_loveList[0].data[13] += 3276U;
                                        gd.baseData.s_loveList[0].data[2] -= 2457U;
                                        gd.baseData.s_loveList[0].data[10] -= 2457U;
                                        break;
                                    case 5:
                                        gd.baseData.s_loveList[0].data[11] += 3276U;
                                        gd.baseData.s_loveList[0].data[12] += 3276U;
                                        gd.baseData.s_loveList[0].data[7] += 3276U;
                                        gd.baseData.s_loveList[0].data[10] -= 2457U;
                                        gd.baseData.s_loveList[0].data[8] -= 2457U;
                                        break;
                                    case 6:
                                        gd.baseData.s_loveList[0].data[2] += 3276U;
                                        gd.baseData.s_loveList[0].data[3] += 3276U;
                                        gd.baseData.s_loveList[0].data[8] += 3276U;
                                        gd.baseData.s_loveList[0].data[1] -= 2457U;
                                        break;
                                    case 7:
                                        gd.baseData.s_loveList[0].data[1] += 3276U;
                                        gd.baseData.s_loveList[0].data[4] += 3276U;
                                        gd.baseData.s_loveList[0].data[13] += 3276U;
                                        gd.baseData.s_loveList[0].data[5] -= 2457U;
                                        gd.baseData.s_loveList[0].data[6] -= 2457U;
                                        gd.baseData.s_loveList[0].data[7] -= 2457U;
                                        break;
                                    case 8:
                                        gd.baseData.s_loveList[0].data[11] += 3276U;
                                        gd.baseData.s_loveList[0].data[12] += 3276U;
                                        gd.baseData.s_loveList[0].data[7] += 3276U;
                                        gd.baseData.s_loveList[0].data[1] += 3276U;
                                        gd.baseData.s_loveList[0].data[2] -= 2457U;
                                        gd.baseData.s_loveList[0].data[6] -= 2457U;
                                        gd.baseData.s_loveList[0].data[8] -= 2457U;
                                        break;
                                    case 9:
                                        gd.baseData.s_loveList[0].data[5] += 3276U;
                                        gd.baseData.s_loveList[0].data[6] += 3276U;
                                        gd.baseData.s_loveList[0].data[10] += 3276U;
                                        gd.baseData.s_loveList[0].data[11] -= 2457U;
                                        gd.baseData.s_loveList[0].data[7] -= 2457U;
                                        gd.baseData.s_loveList[0].data[4] -= 2457U;
                                        break;
                                    case 10:
                                        gd.baseData.s_loveList[0].data[1] += 3276U;
                                        gd.baseData.s_loveList[0].data[4] += 3276U;
                                        gd.baseData.s_loveList[0].data[13] += 3276U;
                                        gd.baseData.s_loveList[0].data[6] -= 2457U;
                                        gd.baseData.s_loveList[0].data[8] -= 2457U;
                                        break;
                                    case 11:
                                        gd.baseData.s_loveList[0].data[11] += 3276U;
                                        gd.baseData.s_loveList[0].data[12] += 3276U;
                                        gd.baseData.s_loveList[0].data[7] += 3276U;
                                        gd.baseData.s_loveList[0].data[1] -= 2457U;
                                        gd.baseData.s_loveList[0].data[6] -= 2457U;
                                        gd.baseData.s_loveList[0].data[2] -= 2457U;
                                        gd.baseData.s_loveList[0].data[10] -= 2457U;
                                        break;
                                }
                                if (mcTraverse.Field("sex").GetValue<byte>() == 0)
                                {
                                    gd.baseData.s_loveList[0].data[4] += 6553U;
                                }
                                else if (mcTraverse.Field("sex").GetValue<byte>() == 1)
                                {
                                    gd.baseData.s_loveList[0].data[13] += 6553U;
                                }
                                else
                                {
                                    gd.baseData.s_loveList[0].data[3] += 6553U;
                                    gd.baseData.s_loveList[0].data[13] += 3276U;
                                }
                                sp.WaitSec(0.01f, false);
                                //Do not start intro animation and stuff
                                sp.scriptQueue.Enqueue(new ScriptParser.Script(delegate (float e)
                                {
                                    ScriptParser scriptParser = sp;
                                    //Instead, go back to Yuriko6Scenario
                                    gd.forwardNext = true;
                                    gd.forward = true;
                                    scriptParser.SetFadeScreen(new List<uint> { 0U, 20U, 50U }, 50002U, 0.25f, 0, false, true, true);
                                    scriptParser.LoadTexture("charaIndexG_0_" + gd.baseData.takashiColor.ToString());
                                    scriptParser.WaitLoad();
                                    scriptParser.LoadTexture("charaIndex_0_" + gd.baseData.takashiColor.ToString());
                                    scriptParser.WaitLoad();
                                    scriptParser.UnloadTexture("entry");
                                    scriptParser.WaitFade(new List<uint> { 50002U }, true, true);
                                    return true;
                                }, (float e) => true, true));
                            }
                            else
                            {
                                selectState = 0;
                                isScreenEnd = false;
                            }
                        }
                    }
                }
                isMouseEnable = true;
            }
            else
            {
                isMouseEnable = !isScreenEnd;
            }
            __result = 1;
            //Write back reflection variables
            t.Field("selectState").SetValue(selectState);
            t.Field("selectTgt").SetValue(selectTgt);
            t.Field("abilLeft").SetValue(abilLeft);
            t.Field("isEnter").SetValue(isEnter);
            t.Field("isStart").SetValue(isStart);
            t.Field("isOnPoint").SetValue(isOnPoint);
            t.Field("isEnterOnPointer").SetValue(isEnterOnPointer);
            t.Field("isDownUpDownKey").SetValue(isDownUpDownKey);
            t.Field("isMouseEnable").SetValue(isMouseEnable);
            t.Field("isScreenEnd").SetValue(isScreenEnd);
            t.Field("inputFieldState").SetValue(inputFieldState);
            chara.SetValue(mainChara, 0);
            return false;
        }
    }
}
