using HarmonyLib;
using RimWorld;
using Verse;


// Replicated initial code, but light and unlikely to be changed (and transpiling would be a pain for this)

namespace DInterests
{
    [HarmonyPatch(typeof(Page_ConfigureStartingPawns))]
    [HarmonyPatch("FindBestSkillOwner")]
    internal class Patch_FindBestSkillOwner_Postfix
    {
        private static void Postfix(ref Pawn __result, SkillDef skill)
        {
            var pawn = Find.GameInitData.startingAndOptionalPawns[0];
            var skillRecord = pawn.skills.GetSkill(skill);
            for (var i = 1; i < Find.GameInitData.startingPawnCount; i++)
            {
                var skill2 = Find.GameInitData.startingAndOptionalPawns[i].skills.GetSkill(skill);
                var passionValue1 = InterestBase.GetValue((int)skillRecord.passion);
                var passionValue2 = InterestBase.GetValue((int)skill2.passion);
                if (!skillRecord.TotallyDisabled && skill2.Level <= skillRecord.Level &&
                    (skill2.Level != skillRecord.Level || !(passionValue2 > passionValue1)))
                {
                    continue;
                }

                pawn = Find.GameInitData.startingAndOptionalPawns[i];
                skillRecord = skill2;
            }

            __result = pawn;
        }
    }
}