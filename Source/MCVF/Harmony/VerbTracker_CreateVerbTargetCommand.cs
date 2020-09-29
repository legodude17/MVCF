using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using Verse;

namespace MCVF.Harmony
{
    [HarmonyPatch(typeof(VerbTracker), "CreateVerbTargetCommand")]
    public class VerbTracker_CreateVerbTargetCommand
    {
        public static IEnumerable<CodeInstruction> Transpile(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Newobj && (Type) instruction.operand == typeof(Command_VerbTarget))
                {
                    instruction.operand = typeof(Command_VerbTargetFixed);
                }
                yield return instruction;
            }
        }
    }
}