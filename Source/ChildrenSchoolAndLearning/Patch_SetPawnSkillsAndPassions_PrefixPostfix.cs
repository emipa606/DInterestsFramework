using RimWorld;
using Verse;

namespace DInterests
{
    internal class Patch_SetPawnSkillsAndPassions_PrefixPostfix
    {
        private static bool Prefix(Pawn pawn, bool OnlyIfSkillsAndPassionsAre0, out bool __state)
        {
            if (OnlyIfSkillsAndPassionsAre0)
            {
                var hasSkill = false;
                var hasPassion = false;
                var allDefsListForReading = DefDatabase<SkillDef>.AllDefsListForReading;
                foreach (var skillDef in allDefsListForReading)
                {
                    var skill = pawn.skills.GetSkill(skillDef);
                    if (skill.Level != 0)
                    {
                        hasSkill = true;
                    }

                    if (skill.passion != Passion.None)
                    {
                        hasPassion = true;
                    }
                }

                if (hasSkill || hasPassion)
                {
                    __state = true;
                    return true;
                }
            }

            __state = false;
            return true;
        }

        private static void Postfix(Pawn pawn, Pawn mother1, Pawn father1, bool GenerateRandom, bool __state)
        {
            if (__state)
            {
                return;
            }

            InterestBase.Inherit(pawn, mother1, father1, GenerateRandom);
        }
    }
}