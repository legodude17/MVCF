using System.Linq;
using HarmonyLib;
using MVCF.Utilities;
using Verse;

namespace MVCF.Harmony
{
    [HarmonyPatch(typeof(VerbUtility))]
    public class VerbUtilityPatches
    {
        [HarmonyPatch("IsEMP")]
        [HarmonyPrefix]
        public static bool IsEMP_Prefix(Verb verb, ref bool __result)
        {
            if (verb.verbProps.label != Base.SearchLabel) return true;
            if (!(verb.caster is Pawn p)) return true;
            var man = p.Manager();
            __result = man.AllVerbs.Any(v => v.IsEMP());
            return false;
        }

        [HarmonyPatch("IsIncendiary")]
        [HarmonyPrefix]
        public static bool IsIncendiary_Prefix(Verb verb, ref bool __result)
        {
            if (verb.verbProps.label != Base.SearchLabel) return true;
            if (!(verb.caster is Pawn p)) return true;
            var man = p.Manager();
            __result = man.AllVerbs.Any(v => v.IsIncendiary());
            return false;
        }

        [HarmonyPatch("UsesExplosiveProjectiles")]
        [HarmonyPrefix]
        public static bool UsesExplosiveProjectiles_Prefix(Verb verb, ref bool __result)
        {
            if (verb.verbProps.label != Base.SearchLabel) return true;
            if (!(verb.caster is Pawn p)) return true;
            var man = p.Manager();
            __result = man.AllVerbs.Any(v => v.UsesExplosiveProjectiles());
            return false;
        }

        [HarmonyPatch("ProjectileFliesOverhead")]
        [HarmonyPrefix]
        public static bool ProjectileFliesOverhead_Prefix(Verb verb, ref bool __result)
        {
            if (verb.verbProps.label != Base.SearchLabel) return true;
            if (!(verb.caster is Pawn p)) return true;
            var man = p.Manager();
            __result = man.AllVerbs.Any(v => v.ProjectileFliesOverhead());
            return false;
        }

        [HarmonyPatch("HarmsHealth")]
        [HarmonyPrefix]
        public static bool HarmsHealth_Prefix(Verb verb, ref bool __result)
        {
            if (verb.verbProps.label != Base.SearchLabel) return true;
            if (!(verb.caster is Pawn p)) return true;
            var man = p.Manager();
            __result = man.AllVerbs.Any(v => v.HarmsHealth());
            return false;
        }
    }
}