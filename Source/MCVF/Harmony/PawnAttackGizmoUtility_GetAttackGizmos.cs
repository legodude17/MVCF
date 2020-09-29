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

            var verbs = pawn.AllRangedVerbsPawnNoEquipment().ToList();
            
//            Log.Message("Found " + verbs.Count + " verbs");

            if (!verbs.Any())
            {
                yield break;
            }

            foreach (var verb in verbs)
            {
                yield return verb.GetGizmoForVerb();
            }

            if (verbs.Count() + pawn.equipment.GetGizmos().Count() >= 2)
            {
                yield return pawn.GetMainAttackGizmoForPawn();
            }
        }
    }
}