using HarmonyLib;
using Verse;


// Pretty weighty postfix, but this system needs a major rework for our mod to make sense.
// Don't want to do a prefix, and this is called rarely enough (pawn generation) that the extra operations aren't egregious

namespace DInterests
{
    [HarmonyPatch(typeof(PawnGenerator))]
    [HarmonyPatch("GenerateSkills")]
    internal class Patch_GenerateSkills_Postfix
    {
        private static void Postfix(Pawn pawn)
        {
            InterestBase.GenerateInterests(pawn);
        }
    }
}