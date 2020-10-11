using System.Reflection;
using HarmonyLib;
using MCVF.Comps;
using MCVF.Utilities;
using RimWorld;
using Verse;

namespace MCVF.Harmony
{
    [HarmonyPatch(typeof(Pawn_ApparelTracker), "Notify_ApparelAdded")]
    public class Pawn_ApparelTracker_Notify_ApparelAdded
    {
        // ReSharper disable once InconsistentNaming
        public static void Postfix(Pawn_ApparelTracker __instance, Apparel apparel)
        {
            var comp = apparel.TryGetComp<Comp_VerbGiver>();
            if (comp == null) return;
            comp.Notify_Worn(__instance.pawn);
            var manager = __instance.pawn?.StorageFor()?.Manager;
            if (manager == null) return;
            foreach (var verb in comp.VerbTracker.AllVerbs)
            {
                manager.AddVerb(verb, VerbSource.Apparel);
            }
        }
    }
    
    [HarmonyPatch(typeof(Pawn_ApparelTracker), "Notify_ApparelRemoved")]
    public class Pawn_ApparelTracker_Notify_ApparelRemoved
    {
        // ReSharper disable once InconsistentNaming
        public static void Postfix(Apparel apparel, Pawn_ApparelTracker __instance)
        {
            var comp = apparel.TryGetComp<Comp_VerbGiver>();
            if (comp == null) return;
            comp.Notify_Unworn();
            var manager = __instance.pawn?.StorageFor()?.Manager;
            if (manager == null) return;
            foreach (var verb in comp.VerbTracker.AllVerbs)
            {
                manager.RemoveVerb(verb);
            }
        }
    }

    [HarmonyPatch(typeof(Pawn_HealthTracker), "AddHediff", typeof(Hediff), typeof(BodyPartRecord), typeof(DamageInfo), typeof(DamageWorker.DamageResult))]
    public class Pawn_HealthTracker_AddHediff
    {
       // ReSharper disable once InconsistentNaming
        public static void Postfix(Hediff hediff, Pawn_HealthTracker __instance)
        {
            var comp = hediff.TryGetComp<HediffComp_VerbGiver>();
            if (comp == null) return;
            var pawn = __instance.hediffSet.pawn;
            var manager = pawn?.StorageFor()?.Manager;
            if (manager == null) return;
            foreach (var verb in comp.VerbTracker.AllVerbs)
            {
                manager.AddVerb(verb, VerbSource.Hediff);
            }
        }
    }

    [HarmonyPatch(typeof(Pawn_HealthTracker), "RemoveHediff")]
    public class Pawn_HealthTracker_RemoveHediff
    {
        // ReSharper disable once InconsistentNaming
        public static void Postfix(Hediff hediff, Pawn_HealthTracker __instance)
        {
            var comp = hediff.TryGetComp<HediffComp_VerbGiver>();
            if (comp == null) return;
            var pawn = __instance.hediffSet.pawn;
            var manager = pawn?.StorageFor()?.Manager;
            if (manager == null) return;
            foreach (var verb in comp.VerbTracker.AllVerbs)
            {
                manager.RemoveVerb(verb);
            }
        }
    }

    [HarmonyPatch(typeof(Pawn_EquipmentTracker), "Notify_EquipmentAdded")]
    public class Pawn_EquipmentTracker_Notify_EquipmentAdded
    {
        // ReSharper disable once InconsistentNaming
        public static void Postfix(ThingWithComps eq, Pawn_EquipmentTracker __instance)
        {
            var comp = eq.TryGetComp<CompEquippable>();
            if (comp == null) return;
            var manager = __instance.pawn?.StorageFor()?.Manager;
            if (manager == null) return;
            foreach (var verb in comp.VerbTracker.AllVerbs)
            {
                manager.AddVerb(verb, VerbSource.Equipment);
            }
        }
    }
    
    [HarmonyPatch(typeof(Pawn_EquipmentTracker), "Notify_EquipmentRemoved")]
    public class Pawn_EquipmentTracker_Notify_EquipmentRemoved
    {
        // ReSharper disable once InconsistentNaming
        public static void Postfix(ThingWithComps eq, Pawn_EquipmentTracker __instance)
        {
            var comp = eq.TryGetComp<CompEquippable>();
            if (comp == null) return;
            var manager = __instance.pawn?.StorageFor()?.Manager;
            if (manager == null) return;
            foreach (var verb in comp.VerbTracker.AllVerbs)
            {
                manager.RemoveVerb(verb);
            }
        }
    }
}