using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

// Replicates original function, but I think this is pretty light and safe

namespace DInterests
{
    [HarmonyPatch(typeof(InspirationWorker))]
    [HarmonyPatch("CommonalityFor")]
    internal class Patch_CommonalityFor_Postfix
    {
        private static void Postfix(ref float __result, Pawn pawn, InspirationDef ___def)
        {
            var num = 1f;
            if (pawn.skills != null && ___def.associatedSkills != null)
            {
                foreach (var skillDef in ___def.associatedSkills)
                {
                    foreach (var record in pawn.skills.skills)
                    {
                        if (skillDef != record.def)
                        {
                            continue;
                        }

                        var s = (int)record.passion;
                        num = Mathf.Max(num, InterestBase.GetInspirationFactor(s));
                    }
                }
            }

            __result = ___def.baseCommonality * num;
        }
    }
}