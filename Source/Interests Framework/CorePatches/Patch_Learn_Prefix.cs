using HarmonyLib;
using RimWorld;
using Verse;

namespace DInterests.CorePatches
{
    [HarmonyPatch(typeof(SkillRecord))]
    [HarmonyPatch("Learn")]
    internal class Patch_Learn_Prefix
    {
        private static bool Prefix(ref float xp, ref bool direct, SkillRecord __instance, Pawn ___pawn)
        {
            InterestBase.HandleLearn(ref xp, ref direct, __instance, ___pawn);
            return true;
        }
    }
}