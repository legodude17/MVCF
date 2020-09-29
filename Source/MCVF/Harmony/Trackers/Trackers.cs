using HarmonyLib;
using MCVF.Comps;
using RimWorld;
using Verse;

namespace MCVF.Harmony.Trackers
{
    [HarmonyPatch(typeof(Pawn_ApparelTracker), "Notify_ApparelAdded")]
    public class Pawn_ApparelTracker_Notify_ApparelAdded
    {
        public static void Postfix(Pawn_ApparelTracker __instance, Apparel apparel)
        {
            apparel.TryGetComp<Comp_VerbGiver>()?.Notify_Worn(__instance.pawn);
        }
    }
    
    [HarmonyPatch(typeof(Pawn_ApparelTracker), "Notify_ApparelRemoved")]
    public class Pawn_ApparelTracker_Notify_ApparelRemoved
    {
        public static void Postfix(Apparel apparel)
        {
            apparel.TryGetComp<Comp_VerbGiver>()?.Notify_Unworn();
        }
    }
    
    [HarmonyPatch(typeof(Pawn_InventoryTracker), "Notify_ItemRemoved")]
    public class Pawn_InventoryTracker_Notify_ItemRemoved
    {
        public static void Postfix(Thing item)
        {
            item.TryGetComp<Comp_VerbGiver>()?.Notify_Dropped();
        }
    }
    
    [HarmonyPatch(typeof(ThingOwner), "NotifyAdded")]
    public class ThingOwner_NotifyAdded
    {
        public static void Postfix(ThingOwner __instance, Thing item)
        {
            if (__instance.Owner is Pawn_InventoryTracker tracker)
            {
                item.TryGetComp<Comp_VerbGiver>()?.Notify_PickedUp(tracker.pawn);
            }
        }
    }
}