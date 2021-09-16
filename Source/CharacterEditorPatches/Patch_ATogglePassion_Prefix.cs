using CharacterEditor;
using DInterests.CharacterEditorPatches;
using HarmonyLib;
using RimWorld;
using Verse;

namespace DInterests
{
    internal class Patch_ATogglePassion_Prefix
    {
        private static bool Prefix(SkillRecord record)
        {
            var EditorType = AccessTools.TypeByName("CharacterEditor.Editor");
            var APIField = AccessTools.Field(typeof(CharacterEditorMod), "API");
            var pr = AccessTools
                .CreateInstance(
                    typeof(CharacterEditorMod)); // I have absolutely no idea why we have to create an instance here, but
            // I was unable to figure out static ref access in AccessTools, and this works
            var API = APIField.GetValue(pr);
            var p = AccessTools.Field(EditorType, "pawn");
            var pawn = p.GetValue(API) as Pawn;
            record.passion =
                (Passion)Patch_CharacterEditorBase.IncreasePassion((int)record.passion, pawn, record.def);

            return false;
        }
    }
}