using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace DInterests
{
    [StaticConstructorOnStartup]
    public static class InterestBase
    {
        public static InterestList interestList = new InterestList();
        public static InterestList positiveInterestList;
        public static InterestList negativeInterestList;


        static InterestBase()
        {
            var defCount = DefDatabase<InterestDef>.DefCount;
            Log.Message("DInterests, found " + defCount + " PassionDefs");

            positiveInterestList = new InterestList(interestList.FindAll(x => x.GetValue() > 0 || x.ignoreTraits));
            negativeInterestList = new InterestList(interestList.FindAll(x => x.GetValue() <= 0 || x.ignoreTraits));
        }


        public static void DrawSkill(Rect position, SkillRecord sk)
        {
            var image = interestList[(int)sk.passion].GetTexture();
            if (image != null)
            {
                GUI.DrawTexture(position, image);
            }
        }

        public static string GetSkillDescription(SkillRecord sk)
        {
            return interestList[(int)sk.passion].GetDescription();
        }

        public static float GetInspirationFactor(int interest)
        {
            return interestList[interest].GetInspirationFactor();
        }

        public static float GetValue(int interest)
        {
            return interestList[interest].GetValue();
        }

        public static void DrawWorkBoxBackground(Passion passion, Rect rect)
        {
            //Log.Message(((int)passion).ToString());
            interestList[(int)passion].drawWorkBox(rect);
        }


        public static float SumWeights(InterestList interests, SkillRecord skill, Pawn pawn)
        {
            var sum = 0.0f;
            foreach (var passion in interests)
            {
                var result = passion.GetWeight(skill, pawn);
                sum += result;
            }

            return sum;
        }

        public static InterestDef GetInterest(InterestList interests, SkillRecord skill, Pawn pawn)
        {
            float totalWeight = -1;
            float random = -1;
            InterestDef selected = null;
            try
            {
                totalWeight = SumWeights(interests, skill, pawn);
                if (totalWeight == 0)
                {
                    return interests[0];
                }

                random = Rand.Range(0.0f, totalWeight);
                // random number generates is inclusive, so we make sure we don't hit the ends or errors result
                // this is a bit odd, but done so that we can do a strict < comparison below which
                // allows us to avoid the possibility of a 0-weight interest being chosen when totalweight > 0
                if (random == 0)
                {
                    random = 0.0001f;
                    Log.Message("Generated perfect 0 from random number generator, wow!");
                }

                if (random == totalWeight)
                {
                    random -= 0.0001f;
                    Log.Message("Generated perfect 1 from random number generator, wow!");
                }

                foreach (var passion in interests)
                {
                    var weight = passion.GetWeight(skill, pawn);
                    if (random < weight)
                    {
                        selected = passion;
                        break;
                    }

                    random -= weight;
                }

                if (selected == null)
                {
                    throw new NullReferenceException("selected still null at end of loop");
                }

                return selected;
            }
            catch (NullReferenceException e)
            {
                Log.Message(e.Message);
                Log.Message("level " + (float)skill.Level);
                Log.Message("Random " + random);
                Log.Message("TotalWeight " + totalWeight);
                foreach (var passion in interests)
                {
                    Log.Message(passion.defName + " weight " + passion.GetWeight(skill, pawn));
                }
            }

            return null;
        }

        public static void InheritSkill(SkillRecord sr, Pawn mother, Pawn father)
        {
            if (mother == null)
            {
                sr.passion = father.skills.GetSkill(sr.def).passion;
            }
            else if (father == null)
            {
                sr.passion = mother.skills.GetSkill(sr.def).passion;
            }
            else
            {
                var coin = Rand.Value;
                sr.passion = coin < 0.5
                    ? mother.skills.GetSkill(sr.def).passion
                    : father.skills.GetSkill(sr.def).passion;
            }
        }

        public static void Inherit(Pawn pawn, Pawn mother, Pawn father, bool random)
        {
            /*if (!random)
                foreach (SkillRecord sr in pawn.skills.skills)
                    InheritSkill(sr, mother, father);
            
            else*/
            GenerateInterests(pawn, mother, father, true);
        }

        public static void GenerateInterests(Pawn pawn, Pawn father = null, Pawn mother = null, bool inherit = false)
        {
            var allDefsListForReading = DefDatabase<SkillDef>.AllDefsListForReading;

            foreach (var skillDef in allDefsListForReading)
            {
                var skill = pawn.skills.GetSkill(skillDef);

                if (skill.TotallyDisabled)
                {
                    continue;
                }

                if (inherit)
                {
                    var coin = Rand.Value;
                    if (coin < 0.5)
                    {
                        InheritSkill(skill, mother, father);
                        continue;
                    }
                }

                var forbidPositive = NoPositiveInterests(pawn, skillDef);
                var requirePositive = ForcePositiveInterest(pawn, skillDef);

                InterestDef selected;
                if (!forbidPositive) // conflicts take precedence over requirements
                {
                    if (!requirePositive) // all interests are possible
                    {
                        selected = GetInterest(interestList, skill, pawn);
                    }
                    else // only positives
                    {
                        selected = positiveInterestList.Count == 0
                            ? interestList.GetDefault()
                            : GetInterest(positiveInterestList, skill, pawn);
                    }
                }
                else // no positives
                {
                    selected = negativeInterestList.Count == 0
                        ? interestList.GetDefault()
                        : GetInterest(negativeInterestList, skill, pawn);
                }

                skill.passion = (Passion)interestList.IndexOf(selected);
            }
        }

        public static bool NoPositiveInterests(Pawn pawn, SkillDef skillDef)
        {
            foreach (var trait in pawn.story.traits.allTraits)
            {
                if (trait.def.ConflictsWithPassion(skillDef))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool ForcePositiveInterest(Pawn pawn, SkillDef skillDef)
        {
            foreach (var trait in pawn.story.traits.allTraits)
            {
                if (trait.def.RequiresPassion(skillDef))
                {
                    return true;
                }
            }

            return false;
        }

        public static float LearnRateFactor(Passion passion)
        {
            return interestList[(int)passion].GetLearnRate();
        }

        public static void RecordMasterTale(SkillRecord sr, Pawn p)
        {
            interestList[(int)sr.passion].RecordMasterTale(sr, p);
        }

        public static void HandleLearn(ref float xp, ref bool direct, SkillRecord sr, Pawn pawn)
        {
            interestList[(int)sr.passion].HandleLearn(ref xp, sr, pawn, ref direct);
        }

        public static void HandleSkillTick(Pawn_SkillTracker pst, Pawn pawn)
        {
            if (!pawn.IsColonist)
            {
                return;
            }

            if (interestList.tickHandlers.Count == 0)
            {
                return;
            }

            if (!pawn.IsHashIntervalTick(200))
            {
                return;
            }

            var handleSkills = pst.skills.Where(x => interestList.tickHandlers.ContainsKey((int)x.passion))
                .ToList();
            foreach (var sr in handleSkills)
            {
                interestList[(int)sr.passion].HandleTick(sr, pawn);
            }
        }

        public static List<InterestDef> getInspiringDefs()
        {
            return interestList.FindAll(x => x.inspires);
        }

        public static InterestDef getBestInspirer(List<int> l)
        {
            return interestList[l.MaxBy(x => interestList[x].inspirationChanceMultiplier)];
        }

        // runs every 100 ticks, postfix to stub StartInspirationMTBDays patch
        public static void HandleInspirationMTB(ref float result, InspirationHandler ih)
        {
            if (interestList.inspirers.Count == 0)
            {
                return;
            }

            if (ih.pawn.needs.mood == null) // result = -1, no mood value yet, so return
            {
                return;
            }

            // go through each skill this pawn has, see if they have an inspiring passion
            var pst = ih.pawn.skills;
            if (pst == null)
            {
                return;
            }

            if (pst.skills == null)
            {
                return;
            }

            // find any inspiring interests this pawn has and get their indices
            var inspiringList = pst.skills.Select(s => (int)s.passion).Where(x => interestList[x].inspires).ToList();

            if (inspiringList.Count < 1) // no pawn skill with inspiring passion
            {
                return;
            }

            var interest = getBestInspirer(inspiringList);

            result = interest.MTBDays(ih);
        }

        public static SkillDef GetActiveSkill(Pawn pawn)
        {
            var jt = pawn.jobs;
            if (pawn.skills == null)
            {
                return null;
            }

            if (jt == null)
            {
                return null;
            }

            var curDriver = jt.curDriver;
            if (curDriver == null)
            {
                return null;
            }

            return curDriver.ActiveSkill;
        }

        public static void UpdateMapwideEffect(Pawn p, int passion, Pawn instigator)
        {
            interestList[passion].UpdatePersistentWorkEffect(p, instigator);
        }

        public static int GetBetterPassion(int passion)
        {
            var value = interestList[passion].GetValue();
            int found;
            if (value <= 0 && positiveInterestList.Count > 0)
            {
                found = interestList.IndexOf(positiveInterestList.MinBy(x => x.GetValue()));
                return found;
            }

            var foundList = interestList.FindAll(x => x.GetValue() > value);
            if (foundList.Count == 0)
            {
                return passion;
            }

            found = interestList.IndexOf(foundList.MinBy(y => y.GetValue()));
            return found;
        }

        public static int IncreasePassion(int passion, Pawn pawn, SkillDef def)
        {
            var oldPassion = passion;
            do
            {
                passion++;
                if (passion > interestList.Count - 1)
                {
                    passion = 0;
                }

                if (interestList[passion].IsValidForPawn(pawn, def))
                {
                    return passion;
                }
            } while (passion != oldPassion);

            return oldPassion;
        }

        public static int GetWorsePassion(int passion)
        {
            var value = interestList[passion].GetValue();
            int found;
            if (value > 0 && negativeInterestList.Count > 0)
            {
                found = interestList.IndexOf(negativeInterestList.MaxBy(x => x.GetValue()));
                return found;
            }

            var foundList = interestList.FindAll(x => x.GetValue() < value);
            if (foundList.Count == 0)
            {
                return passion;
            }

            found = interestList.IndexOf(foundList.MaxBy(y => y.GetValue()));
            return found;
        }
    }
}