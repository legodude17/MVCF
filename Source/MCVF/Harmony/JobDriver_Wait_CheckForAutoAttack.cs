using HarmonyLib;
using Verse.AI;

namespace MCVF.Harmony
{
    [HarmonyPatch(typeof(JobDriver_Wait), "CheckForAutoAttack")]
    public class JobDriver_Wait_CheckForAutoAttack
    {
        public static void Prefix(JobDriver_Wait __instance)
        {
            var storage = WorldComponent_ExtendedPawnStorage.GetStorage().GetStorageFor(__instance.pawn);
//            storage.currentVerb = null;
//            Log.Message("Resetting currentVerb");
        }
    }
}