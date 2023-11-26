// -----------------------------------------------------------------------
// <copyright file="TurretDetectionDistanceTranspiler.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the GPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Core.Patches.Fixes;

using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

using HarmonyTools;

using static AccessTools;

/// <summary>
/// Patches the <see cref="Turret.CheckForPlayersInLineOfSight"/> method. Allows the <see cref="Features.Turret.DetectionDistance"/> feature to work.
/// </summary>
[HarmonyPatch(typeof(Turret), "CheckForPlayersInLineOfSight")]
[HarmonyWrapSafe]
internal static class TurretDetectionDistanceTranspiler
{
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        List<CodeInstruction> newInstructions = instructions.ToList();
        int index = newInstructions.FindIndex(x => x.opcode == OpCodes.Ldc_R4 && x.operand is 30f);
        Log.Debug($"Patching turret detection distance transpiler. [{index}]");
        CodeInstruction[] injectedInstructions =
        {
            new (OpCodes.Ldloc_0),
            new (OpCodes.Callvirt, Method(typeof(TurretDetectionDistanceTranspiler), nameof(GetRange))),
        };
        newInstructions.RemoveRange(index, 1);
        newInstructions.InsertRange(index, injectedInstructions);

        for (int i = 0; i < newInstructions.Count; i++)
            yield return newInstructions[i].Log(i, i == index ? 1 : -1);
    }

    private static float GetRange(Turret instance)
    {
        Features.Turret? turret = Features.Turret.List.FirstOrDefault(x => x.Base == instance);
        if (turret is null)
        {
            Log.Debug("Turret unknown, 30.");
            return 30;
        }

        Log.Debug($"Turret found. {turret.DetectionDistance}");
        return turret.DetectionDistance;
    }
}