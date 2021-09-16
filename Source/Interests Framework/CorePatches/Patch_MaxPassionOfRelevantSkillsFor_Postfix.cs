using HarmonyLib;
using RimWorld;
using Verse;

// Replicates original function, but I think this is pretty light and safe

namespace DInterests
{
    [HarmonyPatch(typeof(Pawn_SkillTracker))]
    [HarmonyPatch("MaxPassionOfRelevantSkillsFor")]
    internal class Patch_MaxPassionOfRelevantSkillsFor_Postfix
    {
        private static void Postfix(ref Passion __result, WorkTypeDef workDef, Pawn_SkillTracker __instance)
        {
            var highestPassion = InterestBase.interestList.GetDefaultIndex();

            if (workDef.relevantSkills.Count == 0)
            {
                __result = (Passion)highestPassion;
                return;
            }

            foreach (var skillDef in workDef.relevantSkills)
            {
                var passion2 = (int)__instance.GetSkill(skillDef).passion;
                if (InterestBase.interestList[passion2] > InterestBase.interestList[highestPassion])
                {
                    highestPassion = passion2;
                }
            }

            __result = (Passion)highestPassion;
        }
    }
}