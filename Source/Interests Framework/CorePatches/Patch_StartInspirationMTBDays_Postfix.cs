using HarmonyLib;
using RimWorld;

namespace DInterests.CorePatches
{
    [HarmonyPatch(typeof(InspirationHandler))]
    [HarmonyPatch("StartInspirationMTBDays", MethodType.Getter)]
    internal class Patch_StartInspirationMTBDays_Postfix
    {
        private static void Postfix(ref float __result, InspirationHandler __instance)
        {
            InterestBase.HandleInspirationMTB(ref __result, __instance);
        }
    }
}