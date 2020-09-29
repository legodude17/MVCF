using MCVF.Utilities;
using Verse;

namespace MCVF
{
    public class Command_VerbTargetFixed : Command_VerbTarget
    {
        public override void ProcessInput(UnityEngine.Event ev)
        {
            var storage = WorldComponent_ExtendedPawnStorage.GetStorage().GetStorageFor(verb.CasterPawn);
//            Log.Message("setting currentVerb to " + verb.Label());
            storage.currentVerb = verb;
            base.ProcessInput(ev);
        }

        public override void MergeWith(Gizmo other)
        {
        }
    }
}