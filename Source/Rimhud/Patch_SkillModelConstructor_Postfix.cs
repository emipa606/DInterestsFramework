using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace DInterests.RimHUDPatches
{
    internal class Patch_SkillModelConstructor_Postfix
    {
        public static void Postfix(object model, SkillDef def, object __instance)
        {
            var r = AccessTools.PropertyGetter(Patch_RimHUDBase.hudPawnModel, "Base");
            var b = (Pawn)r.Invoke(model, null);
            var st = b.skills;

            var skillRecord = st?.GetSkill(def);
            if (skillRecord == null)
            {
                return;
            }

            var label = AccessTools.Field(Patch_RimHUDBase.hudSkillModel, "<Label>k__BackingField");
            if (label == null)
            {
                return;
            }

            var i = InterestBase.interestList[(int)skillRecord.passion];
            var end = "";
            var val = i.GetValue();
            switch (val)
            {
                case < 0:
                    end = new string('-', -(int)Math.Floor(val / 75.0f));
                    break;
                case > 0:
                    end = new string('+', (int)Math.Floor(val / 75.0f));
                    break;
            }

            string l = def.LabelCap + end;
            label.SetValue(__instance, l);
        }
    }
}