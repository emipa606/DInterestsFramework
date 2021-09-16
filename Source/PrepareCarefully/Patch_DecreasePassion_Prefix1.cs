using HarmonyLib;
using RimWorld;
using Verse;

// Decrease Passion for PanelSkills

namespace DInterests
{
    internal class Patch_DecreasePassion_Prefix1
    {
        public delegate void UpdateSkillPassionHandler(SkillDef skill, Passion level);

        public static bool Prefix(SkillRecord record, UpdateSkillPassionHandler ___SkillPassionUpdated)
        {
            if (!(AccessTools.Field(typeof(SkillRecord), "pawn").GetValue(record) is Pawn pawn))
            {
                Log.Error("Failed to retrieve pawn in Patch_DecreasePassion_Prefix1, using original function");
                return true;
            }

            var passion = (int) record.passion;
            passion = PatchPrepareCarefullyBase.DecreasePassion(passion, pawn, record.def);
            ___SkillPassionUpdated(record.def, (Passion) passion);
            return false;
        }
    }
}