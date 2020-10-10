using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Verse;
using Verse.AI;

namespace MCVF.Harmony
{
//    [HarmonyPatch(typeof(AttackTargetFinder), "BestAttackTarget")]
    public class AttackTargetFInder_BestAttackTarget
    {
        public static IEnumerable<CodeInstruction> Transpile(IEnumerable<CodeInstruction> instructions)
        {
            var oldMethod = AccessTools.Method(typeof(Pawn), "get_InAggroMentalState");
            var newMethod =
                AccessTools.Method(typeof(AttackTargetFInder_BestAttackTarget), "ShouldUseRangedAttack");
            
            foreach (var instruction in instructions)
            {
                Log.Message(instruction.ToString());
                if (instruction.opcode == OpCodes.Callvirt && (MethodInfo) instruction.operand == oldMethod)
                {
                    instruction.opcode = OpCodes.Call;
                    instruction.operand = newMethod;
                    Log.Message("Replaced call in AttackTargetFinder");
                }

                yield return instruction;
            }
        }
        
        public static bool ShouldUseRangedAttack(Pawn p)
        {
            return p.RaceProps.Animal || !p.InAggroMentalState;
        }
    }
}