using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using MCVF.Utilities;
using RimWorld;
using Verse;

namespace MCVF.Harmony
{
    [HarmonyPatch(typeof(PawnAttackGizmoUtility), "GetAttackGizmos")]
    public class PawnAttackGizmoUtility_GetAttackGizmos
    {
        public static IEnumerable<Gizmo> Postfix(IEnumerable<Gizmo> __result, Pawn pawn)
        {
            foreach (var gizmo in __result)
            {
                yield return gizmo;
            }
            
            if (pawn.AllRangedVerbsPawnNoEquipment().Count() + pawn.equipment.GetGizmos().Count() >= 2)
            {
                yield return pawn.GetMainAttackGizmoForPawn();
            }

            foreach (var verb in pawn.AllRangedVerbsPawnNoEquipmentNoApparel())
            {
                if (verb.verbProps.hasStandardCommand)
                {
                    yield return verb.GetGizmoForVerb();
                }
            }
        }
    }
}