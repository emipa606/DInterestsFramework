using RimWorld;
using Verse;

namespace DInterests
{
    internal class Patch__LearningFactor_Prefix
    {
        private static bool Prefix(float global_lf, Passion passion, bool learning_saturated, ref float __result)
        {
            var fastLearning = DebugSettings.fastLearning;
            if (fastLearning)
            {
                __result = 200f;
            }
            else
            {
                var num = global_lf - 1f;
                var id = InterestBase.interestList[(int) passion];
                var toAdd = id.learnFactor;
                num += toAdd;
                if (learning_saturated)
                {
                    num *= 0.2f;
                }

                __result = num;
            }

            return false;
        }
    }
}