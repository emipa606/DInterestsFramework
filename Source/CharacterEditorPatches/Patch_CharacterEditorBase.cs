using CharacterEditor;
using HarmonyLib;
using RimWorld;
using Verse;

namespace DInterests.CharacterEditorPatches
{
    [StaticConstructorOnStartup]
    public static class Patch_CharacterEditorBase
    {
        static Patch_CharacterEditorBase()
        {
            Log.Message("DInterests: Character Editor running, attempting to patch");

            var harmony = new Harmony("io.github.dametri.interests.CharacterEditor");

            var target1 = AccessTools.Method(typeof(BlockBio), "ATogglePassion");
            var invoke1 = AccessTools.Method(typeof(Patch_ATogglePassion_Prefix), "Prefix");
            if (target1 != null && invoke1 != null)
            {
                harmony.Patch(target1, new HarmonyMethod(invoke1));
            }
        }

        public static int IncreasePassion(int passion, Pawn pawn, SkillDef def)
        {
            return InterestBase.IncreasePassion(passion, pawn, def);
        }
    }
}