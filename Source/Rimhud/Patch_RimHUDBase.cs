using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace DInterests.RimHUDPatches
{
    [StaticConstructorOnStartup]
    public static class Patch_RimHUDBase
    {
        public static readonly Type hudPawnModel;
        public static readonly Type hudSkillModel;

        static Patch_RimHUDBase()
        {
            Log.Message("DInterests: RimHUD running, attempting to patch");
            var harmony = new Harmony("io.github.dametri.interests.rimhud");
            hudPawnModel = AccessTools.TypeByName("RimHUD.Data.Models.PawnModel");
            hudSkillModel = AccessTools.TypeByName("RimHUD.Data.Models.SkillModel");

            var hudTarget1 = AccessTools.Constructor(hudSkillModel, new[] { hudPawnModel, typeof(SkillDef) });
            //var hudInvoke1 = AccessTools.Method(typeof(Patch_SkillModelConstructor_Postfix), "Postfix");
            if (hudTarget1 != null)
            {
                harmony.Patch(hudTarget1,
                    postfix: new HarmonyMethod(typeof(Patch_SkillModelConstructor_Postfix), "Postfix"));
            }

            var hudTarget2 = AccessTools.Method(hudSkillModel, "GetSkillPassionColor");
            var hudInvoke2 = AccessTools.Method(typeof(Patch_GetSkillPassionColor_Prefix), "Prefix");
            if (hudTarget2 != null && hudInvoke2 != null)
            {
                harmony.Patch(hudTarget2, new HarmonyMethod(hudInvoke2));
            }

            Log.Message("DInterests: RimHUD patch done");
        }
    }


    //try

    //{
    //        ((Action) (() =>
    //        {
    //            if (!LoadedModManager.RunningModsListForReading.Any(x => x.Name.Contains("RimHUD")))
    //            {
    //                return;
    //            }


    //            Log.Message("1");
    //            hudPawnModel = AccessTools.TypeByName("RimHUD.Data.Models.PawnModel");
    //            Log.Message("2");
    //            hudSkillModel = AccessTools.TypeByName("RimHUD.Data.Models.SkillModel");
    //            Log.Message("3");

    //            var hudTarget1 = AccessTools.Constructor(hudSkillModel, new[] {hudPawnModel, typeof(SkillDef)});
    //            Log.Message("4");
    //            var hudInvoke1 = AccessTools.Method(typeof(Patch_SkillModelConstructor_Postfix), "Postfix");
    //            Log.Message("5");
    //            if (hudTarget1 != null && hudInvoke1 != null)
    //            {
    //                Log.Message("6");
    //                harmony.Patch(hudTarget1, postfix: new HarmonyMethod(hudInvoke1));
    //            }

    //            Log.Message("7");
    //            var hudTarget2 = AccessTools.Method(hudSkillModel, "GetSkillPassionColor");
    //            Log.Message("8");
    //            var hudInvoke2 = AccessTools.Method(typeof(Patch_GetSkillPassionColor_Prefix), "Prefix");
    //            Log.Message("9");
    //            if (hudTarget2 != null && hudInvoke2 != null)
    //            {
    //                Log.Message("10");
    //                harmony.Patch(hudTarget2, new HarmonyMethod(hudInvoke2));
    //            }
    //        }))();
    //    }
    //    catch (Exception ex)
    //    {
    //        Log.Message(ex.ToString());
    //    }
    //}
}