using System;
using HarmonyLib;
using Verse;

namespace DInterests
{
    [StaticConstructorOnStartup]
    public static class PatchCSALBase
    {
        public static Type mpu;

        static PatchCSALBase()
        {
            Log.Message("DInterests: Children, school and learning running, attempting to patch");

            var harmony = new Harmony("io.github.dametri.interests.ChildrenSchoolAndLearning");

            mpu = AccessTools.TypeByName("Children.MorePawnUtilities");

            var target1 = AccessTools.Method(mpu, "SetPawnSkillsAndPassions",
                new[] { typeof(Pawn), typeof(Pawn), typeof(Pawn), typeof(string), typeof(bool), typeof(bool) });
            var invoke1 = AccessTools.Method(typeof(Patch_SetPawnSkillsAndPassions_PrefixPostfix),
                "Prefix");
            if (target1 != null && invoke1 != null)
            {
                harmony.Patch(target1, new HarmonyMethod(invoke1));
            }

            var target2 = AccessTools.Method(mpu, "SetPawnSkillsAndPassions",
                new[] { typeof(Pawn), typeof(Pawn), typeof(Pawn), typeof(string), typeof(bool), typeof(bool) });
            var invoke2 = AccessTools.Method(typeof(Patch_SetPawnSkillsAndPassions_PrefixPostfix),
                "Postfix");
            if (target2 != null && invoke2 != null)
            {
                harmony.Patch(target2, postfix: new HarmonyMethod(invoke2));
            }
        }
    }
}