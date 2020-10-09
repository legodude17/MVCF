using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MCVF.Comps;
using RimWorld;
using Verse;

namespace MCVF.Utilities
{
    public static class PawnVerbUtility
    {
        public static IEnumerable<Verb> AllRangedVerbsPawn(this Pawn p)
        {
            return AllRangedVerbsFromEquipment(p)
                .Concat(AllRangedVerbsFromApparel(p))
                .Concat(AllRangedVerbsFromHediffs(p))
                .Concat(AllRangedVerbsFromPawn(p));
        }

        public static IEnumerable<Verb> AllRangedVerbsPawnNoEquipment(this Pawn p)
        {
            return AllRangedVerbsFromApparel(p)
                .Concat(AllRangedVerbsFromHediffs(p))
                .Concat(AllRangedVerbsFromPawn(p));
        }
        
        public static IEnumerable<Verb> AllRangedVerbsPawnNoEquipmentNoApparel(this Pawn p)
        {
            return AllRangedVerbsFromHediffs(p)
                .Concat(AllRangedVerbsFromPawn(p));
        }
        
        private static IEnumerable<Verb> AllRangedVerbsFromHediffs(Pawn p)
        {
            var hediffs = p?.health.hediffSet.hediffs;
            if (hediffs == null) yield break;
            foreach (var hediff in hediffs)
            {
                var comp = hediff.TryGetComp<HediffComp_VerbGiver>();
                if (comp == null) continue;
                foreach (var verb in comp.VerbTracker.AllVerbs)
                {
                    if (!verb.IsMeleeAttack)
                    {
                        yield return verb;
                    }
                }
            }
        }

        private static IEnumerable<Verb> AllRangedVerbsFromEquipment(Pawn p)
        {
            var verbs = p?.equipment?.AllEquipmentVerbs.ToList();
            if (verbs == null) yield break;
            foreach (var verb in verbs)
            {
                if (!verb.IsMeleeAttack)
                {
                    yield return verb;
                }
            }
        }

        private static IEnumerable<Verb> AllRangedVerbsFromApparel(Pawn p)
        {
            var apparel = p?.apparel?.WornApparel;
            if (apparel == null) yield break;
            foreach (var item in apparel)
            {
                var comp = item.TryGetComp<Comp_VerbGiver>();
                if (comp == null) continue;
                foreach (var verb in comp.VerbTracker.AllVerbs)
                {
                    if (!verb.IsMeleeAttack)
                    {
                        yield return verb;
                    }
                }
            }
        }

        private static IEnumerable<Verb> AllRangedVerbsFromPawn(Pawn p)
        {
            var verbs = p?.verbTracker?.AllVerbs;
            if (verbs == null) yield break;
            foreach (var verb in verbs)
            {
                if (!verb.IsMeleeAttack)
                {
                    yield return verb;
                }
            }
        }
        
        public static Verb BestVerbForTarget(this Pawn p, LocalTargetInfo target, IEnumerable<Verb> verbs)
        {
//            Log.Message("BestVerbForTarget: " + p + ", " + target);
            Verb bestVerb = null;
            float bestScore = 0;
            foreach (var verb in verbs)
            {
                if (!verb.CanHitTarget(target)) continue;
                var score = VerbScore(p, verb, target);
//                Log.Message("    Verb " + verb.Label() + " has score " + score);
                if (!(score > bestScore)) continue;
                bestScore = score;
                bestVerb = verb;
            }

//            Log.Message("    Best verb is " + bestVerb?.Label());

            return bestVerb;
        }

        private static float VerbScore(Pawn p, Verb verb, LocalTargetInfo target)
        {
            var report = ShotReport.HitReportFor(p, verb, target);
            var damage = report.TotalEstimatedHitChance * verb.verbProps.burstShotCount * GetDamage(verb);
            var timeSpent = verb.verbProps.AdjustedCooldownTicks(verb, p) + verb.verbProps.warmupTime.SecondsToTicks();
            return damage / timeSpent;
        }

        private static int GetDamage(Verb verb)
        {
            switch (verb)
            {
                case Verb_LaunchProjectile launch:
                    return launch.Projectile.projectile.GetDamageAmount(1f);
                case Verb_Bombardment _:
                case Verb_PowerBeam _:
                case Verb_MechCluster _:
                    return Int32.MaxValue;
                case Verb_CastAbility cast:
                    return cast.ability.EffectComps.Count * 100;
                default:
                    return 1;
            }
        }

        public static ExtendedPawnStorage StorageFor(this Pawn p)
        {
            return WorldComponent_ExtendedPawnStorage.GetStorage().GetStorageFor(p);
        }
    }
}