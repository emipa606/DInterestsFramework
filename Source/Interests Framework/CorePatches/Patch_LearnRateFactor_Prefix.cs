using HarmonyLib;
using RimWorld;
using Verse;

// Unfortunately, this needs to be a destructive prefix due to a thrown error hardcoded into the function

namespace DInterests
{
    [HarmonyPatch(typeof(SkillRecord))]
    [HarmonyPatch("LearnRateFactor")]
    internal class Patch_LearnRateFactor_Prefix
    {
        private static bool Prefix(ref float __result, bool direct, Passion ___passion, SkillRecord __instance,
            Pawn ___pawn)
        {
            if (DebugSettings.fastLearning)
            {
                __result = 200f;
                return false;
            }

            var num = InterestBase.LearnRateFactor(___passion);
            if (!direct)
            {
                num *= ___pawn.GetStatValue(StatDefOf.GlobalLearningFactor);
                if (__instance.LearningSaturatedToday)
                {
                    num *= 0.2f;
                }
            }

            __result = num;
            return false;
        }
    }
}