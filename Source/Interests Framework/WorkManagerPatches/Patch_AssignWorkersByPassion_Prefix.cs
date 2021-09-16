using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace DInterests
{
    internal class Patch_AssignWorkersByPassion_Prefix
    {
        private static bool Prefix(ref object __instance, ref HashSet<Pawn> ____capablePawns,
            ref HashSet<Pawn> ____managedPawns, ref HashSet<WorkTypeDef> ____managedWorkTypes,
            IEnumerable<WorkTypeDef> ____commonWorkTypes)
        {
            if (!____capablePawns.Any())
            {
                return false;
            }

            using var enumerator = ____capablePawns.Intersect(____managedPawns).GetEnumerator();
            while (enumerator.MoveNext())
            {
                var pawn = enumerator.Current;
                IEnumerable<WorkTypeDef> managedWorkTypes = ____managedWorkTypes;
                var isBadWork = AccessTools.Method(PatchWorkManagerBase.priority, "IsBadWork");
                var isPawnWorkTypeActive =
                    AccessTools.Method(PatchWorkManagerBase.priority, "IsPawnWorkTypeActive");
                var setPawnWorkTypePriority =
                    AccessTools.Method(PatchWorkManagerBase.priority, "SetPawnWorkTypePriority");
                foreach (var workType in managedWorkTypes)
                {
                    if (pawn != null && (____commonWorkTypes.Contains(workType) || workType == WorkTypeDefOf.Doctor ||
                                         workType == WorkTypeDefOf.Hunting || pawn.WorkTypeIsDisabled(workType)
                                         || (bool) isBadWork.Invoke(__instance, new object[] {pawn, workType}) ||
                                         (bool) isPawnWorkTypeActive.Invoke(__instance, new object[] {pawn, workType})))
                    {
                        continue;
                    }

                    if (pawn == null)
                    {
                        continue;
                    }

                    var passion = pawn.skills.MaxPassionOfRelevantSkillsFor(workType);
                    var val = InterestBase.interestList[(int) passion].GetValue();
                    if (val > 0)
                    {
                        if (val >= 100)
                        {
                            var priority = 2;
                            setPawnWorkTypePriority.Invoke(__instance, new object[] {pawn, workType, priority});
                        }
                        else
                        {
                            var priority = 3;
                            setPawnWorkTypePriority.Invoke(__instance, new object[] {pawn, workType, priority});
                        }
                    }
                    else if (val < -50)
                    {
                        var priority = 4;
                        setPawnWorkTypePriority.Invoke(__instance, new object[] {pawn, workType, priority});
                    }
                }
            }

            return false;
        }
    }
}