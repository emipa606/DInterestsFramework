using System;
using HarmonyLib;
using Verse;

namespace DInterests
{
    [StaticConstructorOnStartup]
    public static class PatchStaticQualityPlusBase
    {
        public static Type _SR;

        static PatchStaticQualityPlusBase()
        {
            Log.Message("DInterests: Static Quality Plus running, attempting to patch");

            var harmony = new Harmony("io.github.dametri.interests.StaticQualityPlus");

            _SR = AccessTools.TypeByName("static_quality_plus._SkillRecord");

            var target1 = AccessTools.Method(_SR, "_LearningFactor");
            var invoke1 = AccessTools.Method(typeof(Patch__LearningFactor_Prefix), "Prefix");
            if (target1 != null && invoke1 != null)
            {
                harmony.Patch(target1, new HarmonyMethod(invoke1));
            }

            var target2 = AccessTools.Method(_SR, "_CheckPassionIncrease");
            var invoke2 = AccessTools.Method(typeof(Patch_CheckPassionIncrease_Prefix), "Prefix");
            if (target2 != null && invoke2 != null)
            {
                harmony.Patch(target2, new HarmonyMethod(invoke2));
            }
        }
    }
}