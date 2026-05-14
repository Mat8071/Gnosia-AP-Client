using GnosiaArchipelagoRandomizer.Archipelago;
using HarmonyLib;
using systemService.saveData;
using System.IO;
using LitJson;

namespace GnosiaArchipelagoRandomizer.Patches.Core
{
    [HarmonyPatch(typeof(SaveDataManager), "Initialize")]
    class SeparateSavesPatch
    {
        static bool Prefix(SaveDataManager __instance, ref int __result)
        {
            __instance.isSaving = false;
            //Change start
            string seed = ArchipelagoClient.ServerData.GetSeed();
            string relativeSavePath = "/Archipelago/" + seed;
            string savePath = UnityEngine.Application.persistentDataPath + relativeSavePath;
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            AccessTools.Field(typeof(SaveDataManager), "SaveDirectory").SetValue(null, savePath);
            //Change end
            __instance.m_saveDataImage.Init();
            __instance.m_prefDataImage.Init();
            __instance.isSaving = true;
            __instance.initialState = 0;
            bool flag = __instance.LoadPrefData();
            if (!flag)
            {
                __instance.m_prefDataImage.SetDefaultValues();
                __instance.initialState = 1;
                for (int i = 0; i < 3; i++)
                {
                    if (__instance.IsDataExist(i, "auto.data"))
                    {
                        __instance.m_prefDataImage.usedSlot[i] = true;
                        __instance.initialState = 2;
                    }
                }
                if (!__instance.SavePrefData())
                {
                    __instance.initialState = 3;
                }
            }
            else if (!flag)
            {
                __instance.initialState = 4;
            }
            __instance.LoadKeyConfig();
            __instance.isSaving = false;
            __instance.m_saveDataImage.SetDefaultValues();
            __instance.m_gameData = __instance.gameObject.AddComponent<gnosia.GameData>();
            __instance.m_gameData.Initialize(__instance.m_saveDataImage);
            __instance.MakePrefData();
            Traverse.Create(__instance).Field("m_currentSlotId").SetValue(0);
            JsonMapper.RegisterImporter<long, ulong>((long value) => (ulong)value);
            __result = 1;
            return false;
        }
    }
}