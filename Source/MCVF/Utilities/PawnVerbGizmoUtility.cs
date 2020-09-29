using System.Collections.Generic;
using MCVF.Comps;
using RimWorld;
using Verse;

namespace MCVF.Utilities
{
    public static class PawnVerbGizmoUtility
    {
        private static Dictionary<string, string> __truncateCache = new Dictionary<string, string>();
        public static Gizmo GetGizmoForVerb(this Verb verb)
        {
            var gizmo = new Command_VerbTargetFixed();
            switch (verb.DirectOwner)
            {
                case Thing ownerThing:
                    gizmo.defaultLabel = VerbLabel(verb);
                    gizmo.defaultDesc = ownerThing.LabelCap + ": " + ownerThing.def.description.Truncate(500, __truncateCache).CapitalizeFirst();
                    gizmo.icon = ownerThing.def.uiIcon;
                    gizmo.iconAngle = ownerThing.def.uiIconAngle;
                    gizmo.iconOffset = ownerThing.def.uiIconOffset;
                    break;
                case HediffComp_VerbGiver hediffGiver:
                {
                    var hediff = hediffGiver.parent;
                    gizmo.defaultLabel = VerbLabel(verb);
                    gizmo.defaultDesc = hediff.def.LabelCap + ": " + hediff.def.description.Truncate(500, __truncateCache).CapitalizeFirst();
                    gizmo.icon = TexCommand.Attack;
                    break;
                }
                
                case Comp_VerbGiver giver:
                {
                    var thing = giver.parent;
                    gizmo.defaultLabel = VerbLabel(verb);
                    gizmo.defaultDesc = thing.LabelCap + ": " +
                                        thing.def.description.Truncate(500, __truncateCache).CapitalizeFirst();
                    gizmo.icon = thing.def.uiIcon;
                    gizmo.iconAngle = thing.def.uiIconAngle;
                    gizmo.iconOffset = thing.def.uiIconOffset;
                    break;
                }

                default:
                    Log.Error("Unexpected owner in GetGizmoForVerb!");
                    break;
            }

            gizmo.tutorTag = "VerbTarget";
            gizmo.verb = verb;

            if (verb.caster.Faction != Faction.OfPlayer)
                gizmo.Disable("CannotOrderNonControlled".Translate());
            else if (verb.CasterIsPawn)
            {
                if (verb.CasterPawn.WorkTagIsDisabled(WorkTags.Violent))
                    gizmo.Disable("IsIncapableOfViolence".Translate((NamedArgument) verb.CasterPawn.LabelShort,
                        (NamedArgument) verb.CasterPawn));
                else if (!verb.CasterPawn.drafter.Drafted)
                    gizmo.Disable("IsNotDrafted".Translate((NamedArgument) verb.CasterPawn.LabelShort,
                        (NamedArgument) verb.CasterPawn));
            }

            return gizmo;
        }

        public static Gizmo GetMainAttackGizmoForPawn(this Pawn pawn)
        {
            var gizmo = new Command_Action();
            var verbs = pawn.AllRangedVerbsPawn();
            gizmo.defaultDesc = "Attack";
            gizmo.hotKey = KeyBindingDefOf.Misc1;
            gizmo.icon = TexCommand.SquadAttack;
            gizmo.action = () =>
            {
                Find.Targeter.BeginTargeting(TargetingParameters.ForAttackAny(), target =>
                {
                    var storage = WorldComponent_ExtendedPawnStorage.GetStorage().GetStorageFor(pawn);
                    storage.currentVerb = null;
                    var verb = pawn.BestVerbForTarget(target, verbs);
                    verb.OrderForceTarget(target);
                }, pawn, null, TexCommand.Attack);
            };

            if (pawn.Faction != Faction.OfPlayer)
                gizmo.Disable("CannotOrderNonControlled".Translate());
            if (pawn.WorkTagIsDisabled(WorkTags.Violent))
                gizmo.Disable("IsIncapableOfViolence".Translate((NamedArgument) pawn.LabelShort, (NamedArgument) pawn));
            else if (!pawn.drafter.Drafted)
                gizmo.Disable("IsNotDrafted".Translate((NamedArgument) pawn.LabelShort, (NamedArgument) pawn));

            return gizmo;
        }

        private static string VerbLabel(Verb verb)
        {
            if (!string.IsNullOrEmpty(verb.verbProps.label))
            {
                return verb.verbProps.label;
            }
            switch (verb)
            {
                case Verb_LaunchProjectile proj:
                    return proj.Projectile.LabelCap;
                default:
                    return verb.verbProps.label;
            }
        }

        public static string Label(this Verb verb)
        {
            return VerbLabel(verb);
        }
    }
}