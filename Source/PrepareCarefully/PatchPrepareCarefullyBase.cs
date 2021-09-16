using System;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace DInterests
{
    [StaticConstructorOnStartup]
    public static class PatchPrepareCarefullyBase
    {
        public static Type ps;
        public static Type cp;

        static PatchPrepareCarefullyBase()
        {
            Log.Message("DInterests: EdB Prepare Carefully running, attempting to patch");

            var harmony = new Harmony("io.github.dametri.interests.PrepareCarefully");

            ps = AccessTools.TypeByName("EdB.PrepareCarefully.PanelSkills");
            cp = AccessTools.TypeByName("EdB.PrepareCarefully.CustomPawn");


            var target1 = AccessTools.Method(ps, "DecreasePassion");
            var invoke1 = AccessTools.Method(typeof(Patch_DecreasePassion_Prefix1), "Prefix");
            if (target1 != null && invoke1 != null)
            {
                harmony.Patch(target1, new HarmonyMethod(invoke1));
            }

            var target2 = AccessTools.Method(cp, "DecreasePassion");
            var invoke2 = AccessTools.Method(typeof(Patch_DecreasePassion_Prefix2), "Prefix");
            if (target2 != null && invoke2 != null)
            {
                harmony.Patch(target2, new HarmonyMethod(invoke2));
            }

            var target3 = AccessTools.Method(ps, "IncreasePassion");
            var invoke3 = AccessTools.Method(typeof(Patch_IncreasePassion_Prefix1), "Prefix");
            if (target3 != null && invoke3 != null)
            {
                harmony.Patch(target3, new HarmonyMethod(invoke3));
            }

            var target4 = AccessTools.Method(cp, "IncreasePassion");
            var invoke4 = AccessTools.Method(typeof(Patch_IncreasePassion_Prefix2), "Prefix");
            if (target4 != null && invoke4 != null)
            {
                harmony.Patch(target4, new HarmonyMethod(invoke4));
            }

            var target6 = AccessTools.Method(ps, "DrawPanelContent");
            var invoke6 = AccessTools.Method(typeof(Patch_DrawPanelContent_Transpiler), "Transpiler");
            if (target6 != null && invoke6 != null)
            {
                harmony.Patch(target6, transpiler: new HarmonyMethod(invoke6));
            }
        }

        public static int DecreasePassion(int passion, Pawn pawn, SkillDef def)
        {
            var oldPassion = passion;
            do
            {
                passion--;
                if (passion < 0)
                {
                    passion = InterestBase.interestList.Count - 1;
                }

                if (InterestBase.interestList[passion].IsValidForPawn(pawn, def))
                {
                    return passion;
                }
            } while (passion != oldPassion);

            return oldPassion;
        }


        public static void DrawInterest(Passion passion, Rect position)
        {
            var image = InterestBase.interestList.GetDefaultIndex() == (int)passion
                ? ContentFinder<Texture2D>.Get("EdB/PrepareCarefully/NoPassion")
                : InterestBase.interestList[(int)passion].GetTexture();

            GUI.color = Color.white;
            if (image != null)
            {
                GUI.DrawTexture(position, image);
            }
        }
    }
}