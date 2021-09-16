using System;
using HarmonyLib;
using Verse;

namespace DInterests
{
    [StaticConstructorOnStartup]
    public static class PatchWorkManagerBase
    {
        public static Type priority;

        static PatchWorkManagerBase()
        {
            try
            {
                ((Action) (() =>
                {
                    if (!LoadedModManager.RunningModsListForReading.Any(x => x.Name.Contains("Work Manager")))
                    {
                        return;
                    }

                    Log.Message("DInterests: Work Manager running, attempting to patch");

                    var harmony = new Harmony("io.github.dametri.interests");

                    priority = AccessTools.TypeByName("WorkManager.WorkPriorityUpdater");

                    var target1 = AccessTools.Method(priority, "AssignWorkersByPassion");
                    var invoke1 = AccessTools.Method(typeof(Patch_AssignWorkersByPassion_Prefix), "Prefix");
                    if (target1 != null && invoke1 != null)
                    {
                        harmony.Patch(target1, new HarmonyMethod(invoke1));
                    }
                }))();
            }
            catch (TypeLoadException ex)
            {
                Log.Message(ex.ToString());
            }
        }
    }
}