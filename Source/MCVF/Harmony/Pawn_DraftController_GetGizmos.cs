using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using MCVF.Utilities;
using RimWorld;
using Verse;

namespace MCVF.Harmony
{
    [HarmonyPatch(typeof(Pawn_DraftController), "GetGizmos")]
    public class Pawn_DraftController_GetGizmos
    {
        // ReSharper disable InconsistentNaming
        public static IEnumerable<Gizmo> Postfix(IEnumerable<Gizmo> __result, Pawn_DraftController __instance)
        // ReSharper enable InconsistentNaming
        {
            foreach (var gizmo in __result)
            {
                yield return gizmo;
            }

            if (!__instance.Drafted || !__instance.pawn.AllRangedVerbsPawnNoEquipment().Any()) yield break;
            yield return new Command_Toggle
            {
                hotKey = KeyBindingDefOf.Misc6,
                isActive = () => __instance.FireAtWill,
                toggleAction = () => { __instance.FireAtWill = !__instance.FireAtWill; },
                icon = TexCommand.FireAtWill,
                defaultLabel = "CommandFireAtWillLabel".Translate(),
                defaultDesc = "CommandFireAtWillDesc".Translate(),
                tutorTag = "FireAtWillToggle"
            };
        }
    }
}