using Verse;

namespace MCVF
{
    public class ExtendedPawnStorage : IExposable
    {
        public Verb currentVerb;

        public void ExposeData()
        {
            Scribe_References.Look(ref currentVerb, "currentVerb");
        }
    }
}