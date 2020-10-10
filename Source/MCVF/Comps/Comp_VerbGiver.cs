using System.Collections;
using System.Collections.Generic;
using MCVF.Utilities;
using RimWorld;
using Verse;

namespace MCVF.Comps
{
    public class Comp_VerbGiver : ThingComp, IVerbOwner
    {
        private VerbTracker verbTracker;

        public CompProperties_VerbGiver Props => (CompProperties_VerbGiver) props;

        public VerbTracker VerbTracker => verbTracker;

        public List<VerbProperties> VerbProperties => parent.def.Verbs;

        public List<Tool> Tools => parent.def.tools;

        Thing IVerbOwner.ConstantCaster => null;

        ImplementOwnerTypeDef IVerbOwner.ImplementOwnerTypeDef => ImplementOwnerTypeDefOf.NativeVerb;

        public Comp_VerbGiver()
        {
            verbTracker = new VerbTracker(this);
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Deep.Look(ref verbTracker, "verbTracker", (object) this);
            if (Scribe.mode != LoadSaveMode.PostLoadInit)
                return;
            if (verbTracker == null)
                verbTracker = new VerbTracker(this);
            if (!(parent?.holdingOwner?.Owner is Pawn_ApparelTracker tracker)) return;
            foreach (var verb in verbTracker.AllVerbs)
            {
                verb.caster = tracker.pawn;
            }
        }

        public override void CompTick()
        {
            base.CompTick();
            verbTracker.VerbsTick();
        }

        string IVerbOwner.UniqueVerbOwnerID()
        {
            return parent.GetUniqueLoadID() + "_" + parent.AllComps.IndexOf(this);
        }

        bool IVerbOwner.VerbsStillUsableBy(Pawn p)
        {
            return p.equipment.Contains(parent) || p.apparel.Contains(parent) || p.inventory.Contains(parent);
        }

        public void Notify_Worn(Pawn pawn)
        {
            foreach (var verb in verbTracker.AllVerbs)
            {
                verb.caster = pawn;
                verb.Notify_PickedUp();
            }
        }

        public void Notify_Unworn()
        {
            foreach (var verb in verbTracker.AllVerbs)
            {
                verb.Notify_EquipmentLost();
                verb.caster = null;
            }
        }

        public void Notify_PickedUp(Pawn pawn)
        {
            foreach (var verb in verbTracker.AllVerbs)
            {
                verb.caster = pawn;
                verb.Notify_PickedUp();
            }
        }

        public void Notify_Dropped()
        {
            foreach (var verb in verbTracker.AllVerbs)
            {
                verb.Notify_PickedUp();
                verb.caster = null;
            }
        }

        public override IEnumerable<Gizmo> CompGetWornGizmosExtra()
        {
            foreach (var gizmo in base.CompGetWornGizmosExtra())
                yield return gizmo;

            foreach (var verb in verbTracker.AllVerbs)
                yield return verb.GetGizmoForVerb();
        }
    }
}