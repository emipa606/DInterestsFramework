using HarmonyLib;
using RimWorld;
using Verse;

namespace DInterests.CorePatches
{
    [HarmonyPatch(typeof(Pawn_SkillTracker))]
    [HarmonyPatch("SkillsTick")]
    internal class Patch_SkillsTick_Postfix
    {
        private static void Postfix(Pawn_SkillTracker __instance, Pawn ___pawn)
        {
            InterestBase.HandleSkillTick(__instance, ___pawn);
        }
    }
}