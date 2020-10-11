using System.Collections.Generic;
using System.Linq;
using Verse;

namespace MCVF
{
    public class VerbManager : IExposable
    {
        private List<ManagedVerb> verbs = new List<ManagedVerb>();
        public IEnumerable<Verb> AllVerbs => verbs.Select(mv => mv.Verb);
        public IEnumerable<Verb> AllRangedVerbs => verbs.Select(mv => mv.Verb).Where(verb => !verb.IsMeleeAttack);
        public IEnumerable<Verb> AllRangedVerbsNoEquipment =>
            verbs.Where(mv => mv.Source != VerbSource.Equipment).Select(mv => mv.Verb);
        public IEnumerable<Verb> AllRangedVerbsNoEquipmentNoApparel => verbs
            .Where(mv => mv.Source != VerbSource.Equipment && mv.Source != VerbSource.Apparel).Select(mv => mv.Verb);

        public void AddVerb(Verb verb, VerbSource source)
        {
            verbs.Add(new ManagedVerb(verb, source));
        }

        public void RemoveVerb(Verb verb)
        {
            verbs.RemoveAt(verbs.FindIndex(mv => mv.Verb == verb));
        }

        public void ExposeData()
        {
            Scribe_Collections.Look(ref verbs, "verbs", LookMode.Reference);
        }
    }

    public class ManagedVerb
    {
        public Verb Verb;
        public VerbSource Source;

        public ManagedVerb(Verb verb, VerbSource source)
        {
            Verb = verb;
            Source = source;
        }
    }

    public enum VerbSource
    {
        Apparel,
        Equipment,
        Hediff,
        RaceDef
    }
}