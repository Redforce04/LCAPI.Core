// -----------------------------------------------------------------------
// <copyright file="TimingsFixPrefix.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Core.Patches.Fixes;

using System;

using MEC;
using UnityEngine;

/// <summary>
/// Patches the <see cref="GameNetworkManager"/> Constructor to attach the timing instance to.
/// </summary>
[HarmonyPatch(typeof(GameNetworkManager), MethodType.Constructor)]
internal static class TimingsFixPrefix
{
    [HarmonyPrefix]
    private static void Prefix(GameNetworkManager __instance)
    {
        Timing.Instance = new GameObject("Timings").AddComponent<Timing>();
        Timing.Instance.name = "Timing Controller";
        Timing.Instance.OnException += OnError;
    }

    // ReSharper disable once ParameterHidesMember
    private static void OnError(Exception exception, string tag)
    {
        Log.Error($"Timings has caught an error during the execution of a coroutine{(tag == "Unknown" ? string.Empty : $" [{tag}]")}. Exception: \n" + exception.Message, "MEC Timings");
    }
}