using System.Collections.Generic;
using gnosia;
using HarmonyLib;
using setting;
using UnityEngine;
using util;

namespace GnosiaArchipelagoRandomizer.Patches.Optional
{
    [HarmonyPatch(typeof(gnosia.GameData), "MakeLoop")]
    class CharacterRandomizerPatch
    {
        static bool Prefix(gnosia.GameData __instance)
        {
            //If we're in a tutorial loop
            if (__instance.baseData.loop < 14)
                return true; //Don't change anything!!!
            foreach (gnosia.GameData.scenarioData scenario in __instance.sceOn)
            {
                //If this is the bug loop
                if (scenario.id == 29)
                    __instance.charaUseList.Add(11); //Add Setsu to the loop
            }
            //First let's decide which characters will be in the loop
            if (__instance.charaUseList.Count == 0)
            {
                //charaUseList is unused. Let's use it ourselves
                for (int i = 0; i < Plugin.found_characters.Length; i++)
                {
                    if (Plugin.found_characters[i])
                        __instance.charaUseList.Add(i);
                }
            }
            else
            {
                //charaUseList is being used!!!
                List<int> eligible = new List<int>();
                for (int i = 0; i < Plugin.found_characters.Length; i++)
                {
                    if (!Plugin.found_characters[i])
                    {
                        //Remove characters that were not found
                        __instance.charaUseList.Remove(i);
                    }
                    else if (!__instance.charaUseList.Contains(i))
                    {
                        //Add characters that were found and are not in charaUseList to eligible
                        eligible.Add(i);
                    }
                }
                while (__instance.charaUseList.Count < __instance.baseData.totalNum && eligible.Count > 0)
                {
                    //Now let's add random characters until the game is full
                    int randomIndex = UnityEngine.Random.Range(0, eligible.Count);
                    int random_character = eligible[randomIndex];
                    eligible.Remove(random_character);
                    __instance.charaUseList.Add(random_character);
                }
            }
            //Original method (slightly modified)
            Debug.Log("make Loop");
            for (int i = 0; i < 9; i++)
            {
                __instance.baseData.yakuStart[i] = byte.MaxValue;
                __instance.yakuLieNum[i] = 0;
                __instance.yakuAliveNum[i] = 0;
                __instance.yakuAlivableNum[i] = 0;
                __instance.yakuCONum[i] = 0;
                __instance.yakuCOAliveNum[i] = 0;
                __instance.yakuLowRatePerson[i] = -1;
                __instance.yakuHighRatePerson[i] = -1;
                __instance.yakuAverageRate[i] = 0f;
                __instance.canCoByYaku[i] = false;
                __instance.canRollerYaku[i] = false;
            }
            for (int j = 0; j < 15; j++)
            {
                __instance.personFromId[j] = -1;
            }
            __instance.aliveNum = (int)__instance.baseData.totalNum;
            __instance.deadNum = 0;
            __instance.coldNum = 0;
            __instance.otherNum = 0;
            __instance.kt_allNo = (ushort)(Mathf.Pow(2f, (float)__instance.baseData.totalNum) - 1f);
            __instance.chara.Clear();
            __instance.rate.Clear();
            __instance.lowrate.Clear();
            __instance.rateTbl.Clear();
            __instance.knowTable.Clear();
            __instance.gTable.Clear();
            __instance.t_knowTable.Clear();
            __instance.b_trust.Clear();
            __instance.b_trust_max.Clear();
            __instance.b_trust_min.Clear();
            __instance.b_hate.Clear();
            __instance.b_love.Clear();
            __instance.b_love_max.Clear();
            __instance.b_love_min.Clear();
            __instance.ba_trust.Clear();
            __instance.ba_hate.Clear();
            __instance.ba_love.Clear();
            for (int k = 0; k < (int)__instance.baseData.totalNum; k++)
            {
                __instance.chara.Add(default(gnosia.GameData.character));
                __instance.rate.Add(0f);
                __instance.lowrate.Add(0f);
                __instance.rateTbl.Add(new List<float>(0));
                __instance.knowTable.Add(0);
                __instance.gTable.Add(0);
                __instance.t_knowTable.Add(0);
                __instance.b_trust.Add(new List<float>(0));
                __instance.b_trust_max.Add(new List<float>(0));
                __instance.b_trust_min.Add(new List<float>(0));
                __instance.b_hate.Add(0f);
                __instance.b_love.Add(new List<float>(0));
                __instance.b_love_max.Add(new List<float>(0));
                __instance.b_love_min.Add(new List<float>(0));
                __instance.ba_trust.Add(new List<float>(0));
                __instance.ba_hate.Add(0f);
                __instance.ba_love.Add(new List<float>(0));
            }
            for (int l = 0; l < (int)__instance.baseData.totalNum; l++)
            {
                gnosia.GameData.character character = __instance.chara[l];
                character.Initialize((int)__instance.baseData.totalNum);
                __instance.chara[l] = character;
                __instance.rateTbl[l].Clear();
                __instance.baseData.deadList[l] = byte.MaxValue;
                __instance.baseData.shokeiList[l] = byte.MaxValue;
                __instance.b_trust[l].Clear();
                __instance.b_trust_max[l].Clear();
                __instance.b_trust_min[l].Clear();
                __instance.b_love[l].Clear();
                __instance.b_love_max[l].Clear();
                __instance.b_love_min[l].Clear();
                __instance.ba_trust[l].Clear();
                __instance.ba_love[l].Clear();
                for (int m = 0; m < (int)__instance.baseData.totalNum; m++)
                {
                    __instance.rateTbl[l].Add(0f);
                    __instance.b_trust[l].Add(0f);
                    __instance.b_trust_max[l].Add(1f);
                    __instance.b_trust_min[l].Add(0f);
                    __instance.b_love[l].Add(0f);
                    __instance.b_love_max[l].Add(1f);
                    __instance.b_love_min[l].Add(-1f);
                    __instance.ba_trust[l].Add(0f);
                    __instance.ba_love[l].Add(0f);
                }
            }
            byte b = (byte)(__instance.baseData.totalNum - (__instance.baseData.yakuNum[1] + __instance.baseData.yakuNum[2] + __instance.baseData.yakuNum[3] + __instance.baseData.yakuNum[4] + __instance.baseData.yakuNum[6] + __instance.baseData.yakuNum[7] + __instance.baseData.yakuNum[8]));
            if (__instance.baseData.yakuNum[5] != b)
            {
                __instance.baseData.yakuNum[5] = b;
            }
            List<int> list;
            if (__instance.baseData.totalNum <= 12 && ((__instance.baseData.sce_all_flg & 34359738368UL) == 0UL || (__instance.baseData.sce_all_flg & 1073741824UL) > 0UL))
            {
                list = new List<int>
                {
                    1, 2, 3, 4, 5, 11, 6, 8, 10, 14,
                    7, 12, 13
                };
            }
            else
            {
                list = new List<int>
                {
                    1, 2, 3, 4, 5, 11, 6, 8, 10, 14,
                    7, 9, 12, 13
                };
            }
            List<int> list2 = new List<int>();
            if (__instance.charaUseList.Count == 0)
            {
                for (int n = 0; n < (int)__instance.baseData.totalNum; n++)
                {
                    int num;
                    if (n == 0)
                    {
                        num = 0;
                    }
                    else
                    {
                        do
                        {
                            int rand = Util.GetRand(list.Count);
                            num = list[rand];
                        }
                        while (list2.IndexOf(num) != -1);
                    }
                    list2.Add(num);
                    list.Remove(num);
                }
            }
            else
            {
                list2.Add(0);
                int num2 = 0;
                while (num2 < __instance.charaUseList.Count && list2.Count < (int)__instance.baseData.totalNum)
                {
                    if (list2.IndexOf(__instance.charaUseList[num2]) == -1)
                    {
                        list2.Add(__instance.charaUseList[num2]);
                        list.Remove(__instance.charaUseList[num2]);
                    }
                    num2++;
                }
                if (list2.Count < (int)__instance.baseData.totalNum)
                {
                    for (int num3 = list2.Count; num3 < (int)__instance.baseData.totalNum; num3++)
                    {
                        int num;
                        do
                        {
                            num = list[Util.GetRand(list.Count)];
                        }
                        while (list2.IndexOf(num) != -1);
                        list2.Add(num);
                        list.Remove(num);
                    }
                }
            }
            List<byte> list3 = new List<byte>
            {
                0, 11, 1, 2, 3, 4, 5, 6, 8, 10,
                14, 12, 13, 7, 9
            };
            List<int> list4 = new List<int>();
            int num4 = 0;
            for (int num5 = 0; num5 < list3.Count; num5++)
            {
                if (list2.IndexOf((int)list3[num5]) != -1)
                {
                    list4.Add((int)list3[num5]);
                    gnosia.GameData.character character2 = __instance.chara[num4];
                    character2.id = list3[num5];
                    __instance.chara[num4] = character2;
                    __instance.personFromId[(int)list3[num5]] = num4;
                    num4++;
                }
            }
            for (int num6 = 0; num6 < (int)__instance.baseData.totalNum; num6++)
            {
                gnosia.GameData.character character3 = __instance.chara[num6];
                character3.i_yaku = Setting.Yakuwari.y_Unknown;
                character3.p_yaku = Setting.Yakuwari.y_Murabito;
                character3.knowledge[num6] = Setting.Yakuwari.y_Murabito;
                for (int num7 = 0; num7 < (int)__instance.baseData.totalNum; num7++)
                {
                    if (num6 == num7)
                    {
                        character3.i_knowTable[num7] = (ushort)((int)__instance.kt_allNo - (1 << num6));
                    }
                    else
                    {
                        character3.i_knowTable[num7] = (ushort)((int)__instance.chara[num6].i_knowTable[num7] | (1 << num6));
                    }
                }
                __instance.chara[num6] = character3;
            }
            List<List<int>> list5 = new List<List<int>>();
            for (int num8 = 0; num8 < 9; num8++)
            {
                list5.Add(new List<int>(0));
            }
            for (int num9 = 0; num9 < (int)__instance.baseData.totalNum; num9++)
            {
                list5[(int)__instance.charaYakuList[(int)__instance.chara[num9].id]].Add(num9);
            }
            for (int num10 = 1; num10 < 9; num10++)
            {
            }
            for (int num11 = 1; num11 < 9; num11++)
            {
                if (num11 != 5 && __instance.baseData.yakuNum[num11] > 0)
                {
                    list4.Clear();
                    int num12 = 0;
                    if (list5[num11].Count > 0)
                    {
                        while (num12 < list5[num11].Count)
                        {
                            list4.Add(list5[num11][num12]);
                            gnosia.GameData.character character4 = __instance.chara[list5[num11][num12]];
                            character4.i_yaku = (Setting.Yakuwari)num11;
                            __instance.chara[list5[num11][num12]] = character4;
                            num12++;
                        }
                    }
                    List<int> list6 = new List<int>();
                    for (int num13 = 0; num13 <= (int)__instance.baseData.totalNum; num13++)
                    {
                        if (!list4.Contains(num13))
                        {
                            list6.Add(num13);
                        }
                    }
                    while (num12 < (int)__instance.baseData.yakuNum[num11])
                    {
                        int num;
                        do
                        {
                            num = Util.GetRand((int)__instance.baseData.totalNum);
                        }
                        while (list4.Contains(num) || __instance.chara[num].i_yaku != Setting.Yakuwari.y_Unknown || __instance.charaYakuList[(int)__instance.chara[num].id] != Setting.Yakuwari.y_Unknown);
                        gnosia.GameData.character character5 = __instance.chara[num];
                        character5.i_yaku = (Setting.Yakuwari)num11;
                        __instance.chara[num] = character5;
                        list4.Add(num);
                        list6.Remove(num);
                        num12++;
                    }
                    for (int num14 = 0; num14 < list4.Count; num14++)
                    {
                        for (int num15 = 0; num15 < list4.Count; num15++)
                        {
                            if (num14 != num15)
                            {
                                gnosia.GameData.character character6 = __instance.chara[list4[num14]];
                                character6.knowledge[list4[num15]] = (Setting.Yakuwari)num11;
                                Jinro.PileTableOR(ref character6.i_knowTable, __instance.chara[list4[num15]].i_knowTable);
                                __instance.chara[list4[num14]] = character6;
                            }
                        }
                    }
                }
            }
            for (int num16 = 0; num16 < (int)__instance.baseData.totalNum; num16++)
            {
                if (__instance.chara[num16].i_yaku == Setting.Yakuwari.y_Unknown)
                {
                    gnosia.GameData.character character7 = __instance.chara[num16];
                    character7.i_yaku = Setting.Yakuwari.y_Murabito;
                    __instance.chara[num16] = character7;
                }
            }
            __instance.shujinkoYaku = __instance.chara[0].i_yaku;
            for (int num17 = 0; num17 < (int)__instance.baseData.totalNum; num17++)
            {
                if (__instance.baseData.yakuStart[(int)__instance.chara[num17].i_yaku] == 255)
                {
                    __instance.baseData.yakuStart[(int)__instance.chara[num17].i_yaku] = (byte)num17;
                }
                for (int num18 = 0; num18 < (int)__instance.baseData.totalNum; num18++)
                {
                    if (__instance.chara[num18].i_yaku > Setting.Yakuwari.y_Murabito)
                    {
                        __instance.chara[num17].p_knowTable[num18] = (ushort)((int)__instance.chara[num17].p_knowTable[num18] | (1 << num17));
                    }
                }
            }
            int[] array = new int[]
            {
                2, 6, 8, 5, 4, 11, 3, 1, 12, 10,
                13, 7
            };
            for (int num19 = 0; num19 < (int)__instance.baseData.totalNum; num19++)
            {
                uint num20 = (uint)((int)(__instance.baseData.s_chara_resource[(int)__instance.chara[num19].id] + 12) + Util.GetRand(12));
                if (num20 > 255U)
                {
                    num20 = 255U;
                }
                __instance.baseData.s_chara_resource[(int)__instance.chara[num19].id] = (byte)num20;
            }
            for (int num21 = 0; num21 < (int)__instance.baseData.totalNum; num21++)
            {
                __instance.InitYakuBias(num21);
                gnosia.GameData.character character8 = __instance.chara[num21];
                for (int num22 = 0; num22 < 32; num22++)
                {
                    character8.buf[num22] = 0;
                }
                character8.hate = 0.5f;
                character8.gnos = (float)__instance.baseData.s_gnosList[(int)character8.id] / 65535f;
                character8.scenarioRes = (float)__instance.baseData.s_chara_resource[(int)character8.id] / 255f;
                character8.allFlg = __instance.baseData.s_chara_all_flg[(int)character8.id];
                __instance.chara[num21] = character8;
                __instance.CalGnos(num21);
                character8 = __instance.chara[num21];
                for (int num23 = 0; num23 < (int)__instance.baseData.totalNum; num23++)
                {
                    character8.love[num23] = (__instance.baseData.s_loveList[(int)__instance.chara[num23].id].data[(int)character8.id] / 65535f - 0.5f) * 2f;
                    if (num21 == 0)
                    {
                        character8.friendship[num23] = character8.love[num23];
                    }
                    else
                    {
                        character8.friendship[num23] = Util.GetRandF() * 1f - 0.5f;
                        __instance.ChangeFriend(num21, num23, character8.love[num23] * 0.5f, true);
                        if (character8.i_yaku != Setting.Yakuwari.y_Jinro && Util.GetRandF() < (float)(__instance.baseData.yakuNum[7] - 1) / (float)(__instance.baseData.totalNum - 1))
                        {
                            __instance.ChangeFriend(num21, num23, 0.3f + Util.GetRandF() * 0.5f, true);
                        }
                        if (num23 == 0 && array[(int)__instance.baseData.takashiColor] == (int)character8.id)
                        {
                            __instance.ChangeFriend(num21, num23, 0.1f + Util.GetRandF() * 0.2f, true);
                        }
                    }
                    if (character8.knowledge[num23] == Setting.Yakuwari.y_Jinro)
                    {
                        character8.i_trust[num23] = 0f;
                        __instance.ChangeFriend(num21, num23, Util.GetRandF() * 0.4f + __instance.GetAttr(num21, Setting.E_att.at_Neat, false) * 0.2f + __instance.GetAttr(num21, Setting.E_att.at_Courage, false) * 0.2f, true);
                    }
                    else if (character8.knowledge[num23] == Setting.Yakuwari.y_Lover)
                    {
                        character8.i_trust[num23] = 1f;
                        __instance.ChangeFriend(num21, num23, __instance.GetAttr(num21, Setting.E_att.at_Neat, false) * 0.25f + __instance.GetAttr(num21, Setting.E_att.at_Desire, true) * 0.25f, true);
                    }
                    else if (num21 == 0)
                    {
                        character8.i_trust[num23] = 0.4f;
                    }
                    else if (character8.i_yaku == Setting.Yakuwari.y_Jinro)
                    {
                        character8.i_trust[num23] = 0.3f + character8.friendship[num23] * 0.3f + Util.GetRandF() * 0.5f;
                    }
                    else
                    {
                        character8.i_trust[num23] = 0.3f + character8.friendship[num23] * 0.3f + Util.GetRandF() * 0.3f;
                    }
                    if (character8.i_trust[num23] < 0f)
                    {
                        character8.i_trust[num23] = 0f;
                    }
                    else if (character8.i_trust[num23] > 1f)
                    {
                        character8.i_trust[num23] = 1f;
                    }
                    character8.p_trust[num23] = 0.5f;
                    if ((__instance.baseData.sce_all_flg & 8589934592UL) > 0UL && __instance.chara[num23].id == 3 && __instance.chara[num23].i_yaku == Setting.Yakuwari.y_Fox && (__instance.baseData.sce_all_flg & 17179869184UL) == 0UL)
                    {
                        __instance.ChangeFriend(num21, num23, 0.15f + Util.GetRandF() * 0.35f, true);
                        __instance.ChangeInsideTrust(num21, num23, 0.05f + Util.GetRandF() * 0.15f);
                    }
                }
                __instance.chara[num21] = character8;
            }
            __instance.madeData = true;
            __instance.madeRateOnce = false;
            __instance.tClipPos = new Vector4(0f, 0f, 960f, 540f);
            //Prevent original from running
            return false;
        }
    }
}