using System;
using System.Collections.Generic;
using RimWorld;
using static_quality_plus;
using Verse;

namespace DInterests
{
    internal class Patch_CheckPassionIncrease_Prefix
    {
        private static bool Prefix(Pawn pawn, SkillDef skilldef)
        {
            var random = new Random();
            var num = random.Next(1, 11);
            var sr = pawn.skills.GetSkill(skilldef);
            if (sr.Level <= num)
            {
                return false;
            }

            var betterPassion = InterestBase.GetBetterPassion((int) sr.passion);
            sr.passion = (Passion) betterPassion;

            var num2 = 0;
            var list = new List<SkillRecord>();
            foreach (var skillRecord in pawn.skills.skills)
            {
                if (skillRecord.passion <= Passion.None || skillRecord.def == skilldef)
                {
                    continue;
                }

                num2++;
                list.Add(skillRecord);
            }

            if (!sqp_mod.settings.passion_cap || num2 <= 4)
            {
                return false;
            }

            num = random.Next(0, num2);
            var skillRecord2 = list[num];
            skillRecord2.passion = (Passion) InterestBase.GetWorsePassion((int) skillRecord2.passion);

            return false;
        }
    }
}