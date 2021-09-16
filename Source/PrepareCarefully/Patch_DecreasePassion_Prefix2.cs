using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

// Decrease Passion for CustomPawn

namespace DInterests
{
    internal class Patch_DecreasePassion_Prefix2
    {
        public static bool Prefix(SkillDef def, object __instance, ref Pawn ___pawn)
        {
            var IsSkillDisabled = AccessTools.Method(PatchPrepareCarefullyBase.cp, "IsSkillDisabled");
            var f = (bool) IsSkillDisabled.Invoke(__instance, new object[] {def});
            if (f)
                //if (__instance.IsSkillDisabled(def))
            {
                return false;
            }

            var cur = AccessTools.Field(PatchPrepareCarefullyBase.cp, "currentPassions");
            if (cur.GetValue(__instance) is Dictionary<SkillDef, Passion> currentPassions)
            {
                var passion = (int) currentPassions[def];
                //int passion = (int)__instance.currentPassions[def];
                passion = PatchPrepareCarefullyBase.DecreasePassion(passion, ___pawn, def);
                ___pawn.skills.GetSkill(def).passion = (Passion) passion;
            }

            var CopySkillsAndPassionsToPawn =
                AccessTools.Method(PatchPrepareCarefullyBase.cp, "CopySkillsAndPassionsToPawn");
            CopySkillsAndPassionsToPawn.Invoke(__instance, null);
            // __instance.CopySkillsAndPassionsToPawn();
            return false;
        }
    }
}