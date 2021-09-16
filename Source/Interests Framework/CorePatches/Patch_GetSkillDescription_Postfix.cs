using System.Text;
using HarmonyLib;
using RimWorld;
using Verse;

namespace DInterests
{
    [HarmonyPatch(typeof(SkillUI))]
    [HarmonyPatch("GetSkillDescription")]
    internal class Patch_GetSkillDescription_Postfix
    {
        private static void Postfix(ref string __result, SkillRecord sk)
        {
            var passionNum = (int)sk.passion;
            var rebuild = new StringBuilder(__result);
            var toFind = new StringBuilder();

            toFind.Append("Passion".Translate() + ": ");

            switch (passionNum)
            {
                case 0:
                    toFind.Append("PassionNone".Translate(0.35f.ToStringPercent("F0")));
                    break;
                case 1:
                    toFind.Append("PassionMinor".Translate(1f.ToStringPercent("F0")));
                    break;
                case 2:
                    toFind.Append("PassionMajor".Translate(1.5f.ToStringPercent("F0")));
                    break;
            }

            var toAdd = InterestBase.GetSkillDescription(sk);
            rebuild.Replace(toFind.ToString(), toAdd);
            __result = rebuild.ToString();
        }
    }
}