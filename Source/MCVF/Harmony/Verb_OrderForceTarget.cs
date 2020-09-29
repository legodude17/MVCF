using HarmonyLib;
using Verse;

namespace MCVF.Harmony
{
//    [HarmonyPatch(typeof(Verb), "OrderForceTarget")]
    public class Verb_OrderForceTarget
    {
        // ReSharper disable once InconsistentNaming
        public static bool Prefix(LocalTargetInfo target, Verb __instance)
        {
            var num = __instance.verbProps.EffectiveMinRange(target, __instance.CasterPawn);
            if (__instance.verbProps.IsMeleeAttack || 
                __instance.CasterPawn.Position.DistanceToSquared(target.Cell) < num * (double) num &&
                __instance.CasterPawn.Position.AdjacentTo8WayOrInside(target.Cell))
            {
                return true;
            }

            __instance.TryStartCastOn(target);
            return false;
        }
    }
}