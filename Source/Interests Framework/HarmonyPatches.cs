using System.Reflection;
using HarmonyLib;
using Verse;

namespace DInterests
{
    public class HarmonyPatches : Mod
    {
        public HarmonyPatches(ModContentPack content) : base(content)
        {
            var harmony = new Harmony("io.github.dametri.interestsframework");
            var assembly = Assembly.GetExecutingAssembly();
            harmony.PatchAll(assembly);
        }
    }
}