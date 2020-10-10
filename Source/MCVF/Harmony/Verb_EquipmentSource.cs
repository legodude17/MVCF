using HarmonyLib;
using MCVF.Comps;
using MCVF.Utilities;
using Verse;

namespace MCVF.Harmony
{
    [HarmonyPatch(typeof(Verb), "get_EquipmentSource")]
    public class Verb_EquipmentSource
    {
        // ReSharper disable InconsistentNaming
        public static void Postfix(ref ThingWithComps __result, Verb __instance)
        {
            switch (__instance.DirectOwner)
            {
                case Comp_VerbGiver giver:
                    __result = giver.parent;
                    break;
                case HediffComp_VerbGiver giver2:
                    __result = giver2.Pawn;
                    break;
                case Pawn pawn:
                    __result = pawn;
                    break;
            }
        }
    }
}