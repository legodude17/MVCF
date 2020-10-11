using Verse;

namespace MCVF
{
    public class ExtendedPawnStorage : IExposable
    {
        public Verb CurrentVerb;
        public VerbManager Manager;

        public ExtendedPawnStorage(Pawn pawn)
        {
            Manager = new VerbManager();
            foreach (var verb in pawn.VerbTracker.AllVerbs)
            {
                Manager.AddVerb(verb, VerbSource.RaceDef);
            }
        }

        public void ExposeData()
        {
            Scribe_References.Look(ref CurrentVerb, "currentVerb");
            Scribe_Deep.Look(ref Manager, "Manager");
        }
    }
}