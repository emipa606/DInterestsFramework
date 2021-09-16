using System;
using RimWorld;
using Verse;

namespace DInterests
{
    internal class MinorPassionDef : InterestDef
    {
        public override float GetWeight(SkillRecord skill, Pawn pawn)
        {
            float level = skill.Level;
            if (level <= 9)
            {
                return base.GetWeight(skill, pawn);
            }

            return 100 - ((float) Math.Pow(level, weightLevelExponent) * weightLevelFactor / 4.0f);
        }
    }
}