using System.Linq;
using HarmonyLib;
using MCVF.Utilities;
using Verse;

// ReSharper disable InconsistentNaming

namespace MCVF.Harmony
{
    [HarmonyPatch(typeof(Pawn), "TryGetAttackVerb")]
    public class Pawn_TryGetAttackVerb
    {
        public static bool Prefix(ref Verb __result, Pawn __instance, Thing target, bool allowManualCastWeapons = false)
        {
            var storage = WorldComponent_ExtendedPawnStorage.GetStorage().GetStorageFor(__instance);
//            Log.Message("Getting attack verb for " + __instance + " with currentVerb " + storage.currentVerb?.Label() +
//                        " and target " + target);

            if (target == null)
            {
                storage.currentVerb = null;
            }

            if (storage.currentVerb != null)
            {
                __result = storage.currentVerb;
                return false;
            }

            var verbs = __instance.AllRangedVerbsPawn();
            if (!allowManualCastWeapons)
            {
                verbs = verbs.Where(v => !v.verbProps.onlyManualCast);
            }

            var verbsToUse = verbs.ToList();
            if (verbsToUse.Count == 0)
            {
                return true;
            }

            if (target == null)
            {
                var maxRange = verbsToUse.Max(verb => verb.verbProps.range);
                __result = verbsToUse.FirstOrDefault(verb => verb.verbProps.range >= maxRange);
                return false;
            }

            var verbToUse = __instance.BestVerbForTarget(target, verbsToUse);

            if (verbToUse == null)
            {
                return true;
            }

            __result = verbToUse;

            return false;
        }

        public static void Postfix(ref Verb __result)
        {
            //Log.Message("TryGetAttackVerb returning " + __result?.Label());
        }
    }
}