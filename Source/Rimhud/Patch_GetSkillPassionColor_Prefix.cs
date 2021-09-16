using RimWorld;
using UnityEngine;

namespace DInterests.RimHUDPatches
{
    internal class Patch_GetSkillPassionColor_Prefix
    {
        private static readonly Color veryNegativeColor = new Color(0.27f, 0.50f, 0.70f);
        private static readonly Color negativeColor = new Color(0.0f, 0.75f, 1.0f);
        private static readonly Color neutralColor = new Color(1f, 1f, 1f);
        private static readonly Color positiveColor = new Color(1f, 0.9f, 0.7f);
        private static readonly Color veryPositiveColor = new Color(1f, 0.8f, 0.4f);

        private static bool Prefix(Passion passion, ref Color __result)
        {
            var p = (int) passion;
            var interest = InterestBase.interestList[p];
            var val = interest.GetValue();
            switch (val)
            {
                case < 0 when val / 75.0f < 1.0f:
                    __result = veryNegativeColor;
                    break;
                case < 0:
                    __result = negativeColor;
                    break;
                case 0:
                    __result = neutralColor;
                    break;
                default:
                {
                    __result = val / 75.0f > 1.0f ? veryPositiveColor : positiveColor;

                    break;
                }
            }

            return false;
        }
    }
}