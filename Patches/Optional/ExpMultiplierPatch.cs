using System;
using System.Collections.Generic;
using System.Reflection;
using coreSystem;
using GnosiaArchipelagoRandomizer.Archipelago;
using HarmonyLib;
using setting;
using UnityEngine;

namespace GnosiaArchipelagoRandomizer.Patches.Optional
{
    [HarmonyPatch]
    class ExpMultiplierPatch
    {
        static MethodBase TargetMethod()
        {
            Type type = AccessTools.TypeByName("application.ResultScreen");
            return AccessTools.Method(type, "InitializeGlm");
        }

        static void Postfix(object __instance)
        {
            float expMultiplier = Convert.ToSingle(ArchipelagoClient.ServerData.SlotData.Options?.ExpMultiplier ?? 1f);

            Traverse t = Traverse.Create(__instance);
            gnosia.GameData mydata = t.Field("mydata").GetValue<gnosia.GameData>();
            Dictionary<string, TextArea> m_textAreaMap = t.Field("m_textAreaMap").GetValue<Dictionary<string, TextArea>>();
            resource.ResourceManager m_resourceManager = t.Field("m_resourceManager").GetValue<resource.ResourceManager>();

            //Calculate multiplied experience
            uint gotExp = mydata.baseData.gainExp;
            uint totalExp = (uint)(gotExp * expMultiplier);
            uint bonusExp = totalExp - gotExp;

            if (mydata.baseData.takashiExp + bonusExp > 9999999U)
                mydata.baseData.takashiExp = 9999999U;
            else
                mydata.baseData.takashiExp += bonusExp;

            //Replace exp substring in result screen
            string text = string.Join("\n", m_textAreaMap["kekka"].strList);
            text = text.Replace(gotExp.ToString(), totalExp.ToString());
            t.Method("SetText", new object[] { "kekka", text, true, true }).GetValue();

            //Show levelup is possible (if not already shown)
            if (!m_textAreaMap.ContainsKey("lvup") && mydata.baseData.takashiLv < 266 && (mydata.baseData.sce_all_flg & 32768UL) > 0 && mydata.baseData.takashiExp >= m_resourceManager.m_config.m_needExp[mydata.baseData.takashiLv])
            {
                TextArea textArea = UnityEngine.Object.Instantiate<TextArea>(m_resourceManager.textAreaPrefab, t.Field("transform").GetValue<Transform>());
                textArea.name = "lvupTextArea";
                float[] array2 = new float[] { 723f, 697f, 723f };
                t.Method("SetTextArea", new object[] { textArea, "lvup", 10, 1, 30, new Vector2(array2[Setting.language], 122f), 0, 10200, m_resourceManager.m_defaultFont, TextAlign.k_text_Left, new Vector4?(new Vector4(1f, 0.9216f, 0.247f, 1f)) }).GetValue();
                m_textAreaMap["lvup"].SetSize(0.6f);
                t.Method("SetText", new object[] { "lvup", m_resourceManager.GetScreenText(23, 32), true, true }).GetValue();
            }
        }
    }
}
