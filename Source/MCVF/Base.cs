using Verse;

namespace MCVF
{
    public class Base : Mod
    {
        public Base(ModContentPack content) : base(content)
        {
            var harm = new HarmonyLib.Harmony("legodude17.mcvf");
            harm.PatchAll();
            Log.Message("Applied patches for " + harm.Id);
        }
    }
}