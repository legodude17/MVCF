using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace MCVF
{
    public class WorldComponent_ExtendedPawnStorage : WorldComponent
    {
        private Dictionary<Pawn, ExtendedPawnStorage> storage = new Dictionary<Pawn, ExtendedPawnStorage>();

        private List<Pawn> _pawnList;
        private List<ExtendedPawnStorage> _storageList;

        public static WorldComponent_ExtendedPawnStorage GetStorage()
        {
            return Find.World.GetComponent<WorldComponent_ExtendedPawnStorage>();
        }
        
        public WorldComponent_ExtendedPawnStorage(World world) : base(world)
        {
            
        }

        public override void ExposeData()
        {
            Scribe_Collections.Look(ref storage, "storage", LookMode.Reference, LookMode.Deep, ref _pawnList, ref _storageList);
        }

        public ExtendedPawnStorage GetStorageFor(Pawn pawn)
        {
            ExtendedPawnStorage store;
            if (storage.TryGetValue(pawn, out store)) return store;
            store = new ExtendedPawnStorage(pawn);
            storage.Add(pawn, store);
            return store;
        }
    }
}