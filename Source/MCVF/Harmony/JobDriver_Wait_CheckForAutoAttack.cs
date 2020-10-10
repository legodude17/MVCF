using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace MCVF.Harmony
{
    [HarmonyPatch(typeof(JobDriver_Wait), "CheckForAutoAttack")]
    public class JobDriver_Wait_CheckForAutoAttack
    {
        // ReSharper disable once InconsistentNaming
        public static void Postfix(JobDriver_Wait __instance)
        {
            if (__instance.pawn.Downed ||
                __instance.pawn.stances.FullBodyBusy ||
                !__instance.pawn.RaceProps.Animal ||
                !__instance.job.canUseRangedWeapon ||
                __instance.job.def != JobDefOf.Wait_Combat)
                return;
            var currentEffectiveVerb = __instance.pawn.CurrentEffectiveVerb;
            if (currentEffectiveVerb == null || currentEffectiveVerb.verbProps.IsMeleeAttack)
                return;
            var flags = TargetScanFlags.NeedLOSToAll | TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable;
            if (currentEffectiveVerb.IsIncendiary())
                flags |= TargetScanFlags.NeedNonBurning;
            var thing = (Thing) AttackTargetFinder.BestShootTargetFromCurrentPosition(__instance.pawn, flags);
//            Log.Message("Found target for auto attack: " + thing?.Label);
            if (thing == null)
                return;
            __instance.pawn.TryStartAttack((LocalTargetInfo) thing);
            __instance.collideWithPawns = true;
        }
    }
}