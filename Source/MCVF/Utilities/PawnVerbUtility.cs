using System;
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
            if (p?.equipment != null && !p.equipment.AllEquipmentVerbs.EnumerableNullOrEmpty())
            {
                foreach (var verb in p.equipment.AllEquipmentVerbs)
                {
                    if (!verb.IsMeleeAttack)
                    {
                        yield return verb;
                    }
                }
            }

            if (p?.health?.hediffSet != null && !p.health.hediffSet.hediffs.EnumerableNullOrEmpty())
            {
                foreach (var hediff in p.health.hediffSet.hediffs)
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

            if (p?.apparel != null && !p.apparel.WornApparel.NullOrEmpty() && p.inventory != null &&
                p.inventory.innerContainer.NullOrEmpty())
            {
                foreach (var item in p.apparel.WornApparel.Concat(p.inventory.innerContainer))
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
        }

        public static IEnumerable<Verb> AllRangedVerbsPawnNoEquipment(this Pawn p)
        {
            if (p?.health?.hediffSet != null && !p.health.hediffSet.hediffs.EnumerableNullOrEmpty())
            {
                foreach (var hediff in p.health.hediffSet.hediffs)
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

            if (p?.apparel != null && !p.apparel.WornApparel.NullOrEmpty() && p.inventory != null &&
                p.inventory.innerContainer.NullOrEmpty())
            {
                foreach (var item in p.apparel.WornApparel.Concat(p.inventory.innerContainer))
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
    }
}