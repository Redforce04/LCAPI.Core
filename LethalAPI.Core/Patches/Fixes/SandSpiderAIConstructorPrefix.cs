// -----------------------------------------------------------------------
// <copyright file="SandSpiderAIConstructorPrefix.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the GPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Core.Patches.Fixes;

using Features;

/// <summary>
/// Gets the instance of the SandSpiderAI.
/// </summary>
[HarmonyPatch(typeof(SandSpiderAI), MethodType.Constructor)]
internal static class SandSpiderAIConstructorPrefix
{
    [HarmonyPrefix]
    private static bool Prefix(SandSpiderAI __instance)
    {
        Log.Debug("Got SandSpiderAI Instance.");
        Web.SpiderAI = __instance;
        Web.WebPrefab = __instance.webTrapPrefab;
        return true;
    }
}