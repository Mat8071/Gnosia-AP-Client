using System;
using HarmonyLib;

namespace GnosiaArchipelagoRandomizer.Utils
{
    static class MyUtils
    {
        static public byte GetCharaTotalNotes(Array chara, int id)
        {
            var entry = chara.GetValue(id);
            var nameField = AccessTools.Field(entry.GetType(), "d_tokkiNum");
            return (byte)nameField.GetValue(entry);
        }
        static public string GetCharaName(Array chara, gnosia.GameData gd, int cid)
        {
            var entry = chara.GetValue((int)gd.chara[cid].id);
            var nameField = AccessTools.Field(entry.GetType(), "name");
            return (string)nameField.GetValue(entry);
        }
    }
}
