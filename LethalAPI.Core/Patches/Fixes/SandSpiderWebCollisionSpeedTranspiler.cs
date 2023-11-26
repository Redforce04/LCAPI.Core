// -----------------------------------------------------------------------
// <copyright file="SandSpiderWebCollisionSpeedTranspiler.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the GPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Core.Patches.Fixes;

using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

using Features;
using HarmonyLib;

using static AccessTools;

/// <summary>
/// Patches the <see cref="SandSpiderWebTrap.OnTriggerStay"/> method. Allows the <see cref="Features.Web.SpeedMultiplier"/> feature to work.
/// </summary>
[HarmonyPatch(typeof(SandSpiderWebTrap), nameof(SandSpiderWebTrap.OnTriggerStay))]
internal static class SandSpiderWebCollisionSpeedTranspiler
{
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        List<CodeInstruction> newInstructions = instructions.ToList();
        int index = newInstructions.FindIndex(x => x.opcode == OpCodes.Ldc_R4 && x.operand is 4f);
        CodeInstruction[] injectedInstructions =
        {
            new(OpCodes.Ldarg_0),
            new(OpCodes.Callvirt, Method(typeof(SandSpiderWebCollisionSpeedTranspiler), nameof(GetMultiplier))),
        };
        newInstructions.RemoveRange(index, 1);
        newInstructions.InsertRange(index, injectedInstructions);

        for (int i = 0; i < newInstructions.Count; i++)
            yield return newInstructions[i];
    }

    private static float GetMultiplier(SandSpiderWebTrap instance)
    {
        Web? web = Web.List.FirstOrDefault(x => x.Base == instance);
        if (web is null)
            return 4f;

        return 1f / web.SpeedMultiplier;
    }
}

/// <summary>
/// Patches the <see cref="SandSpiderWebTrap.PlayerLeaveWeb"/> method. Allows the <see cref="Features.Web.SpeedMultiplier"/> feature to work.
/// </summary>
[HarmonyPatch(typeof(SandSpiderWebTrap), nameof(SandSpiderWebTrap.PlayerLeaveWeb))]
[HarmonyWrapSafe]
internal static class SandSpiderWebCollisionLeaveSpeedTranspiler
{
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        List<CodeInstruction> newInstructions = instructions.ToList();
        int index = newInstructions.FindIndex(x => x.opcode == OpCodes.Ldc_R4 && x.operand is 0.25f);
        CodeInstruction[] injectedInstructions =
        {
            new(OpCodes.Ldarg_0),
            new(OpCodes.Callvirt, Method(typeof(SandSpiderWebCollisionLeaveSpeedTranspiler), nameof(GetMultiplier))),
        };
        newInstructions.RemoveRange(index, 1);
        newInstructions.InsertRange(index, injectedInstructions);

        for (int i = 0; i < newInstructions.Count; i++)
            yield return newInstructions[i];
    }

    private static float GetMultiplier(SandSpiderWebTrap instance)
    {
        Web? web = Web.List.FirstOrDefault(x => x.Base == instance);
        if (web is null)
            return 0.25f;

        return web.SpeedMultiplier;
    }
}
