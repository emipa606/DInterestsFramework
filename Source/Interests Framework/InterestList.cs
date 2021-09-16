using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace DInterests
{
    public class InterestList : List<InterestDef>
    {
        public Dictionary<int, InterestDef> inspirers;

        public Dictionary<int, InterestDef> mapwideWorkers;
        public Dictionary<int, InterestDef> tickHandlers;

        public InterestList()
        {
            var tmpList = new List<InterestDef>(DefDatabase<InterestDef>.AllDefsListForReading);
            if (tmpList.Count == 0)
            {
                throw new ArgumentException(
                    "DInterests: Must have at least 1 PassionDef, failed to find any. Loading at least 3 is highly recommended.");
            }

            // For compatability, try to make our first indices match the original enum
            InterestDef[] defaults =
                {InterestDefOf.DNoPassion, InterestDefOf.DMinorPassion, InterestDefOf.DMajorPassion}; // 0, 1, 2
            foreach (var def in defaults)
            {
                var find = tmpList.FindAll(x => x == def);
                if (find.NullOrEmpty())
                {
                    continue;
                }

                AddRange(find);
                tmpList.Remove(find[0]);
            }

            tmpList.SortBy(x => x.priority);
            AddRange(tmpList);

            process();
        }

        public InterestList(List<InterestDef> p) : base(p)
        {
            process();
        }

        public int this[string s]
        {
            get { return FindIndex(x => x.defName == s); }
        }

        private void process()
        {
            mapwideWorkers = this.Where(x => x.mapwideWorkImpacter).ToDictionary(i => FindIndex(m => m == i));
            inspirers = this.Where(x => x.inspires).ToDictionary(i => FindIndex(m => m == i));
            tickHandlers = this.Where(x => x.handlesTicks).ToDictionary(i => FindIndex(m => m == i));
        }

        public InterestDef GetDefault()
        {
            return this.MinBy(x => x.priority);
        }

        public int GetDefaultIndex()
        {
            return IndexOf(GetDefault());
        }
    }
}