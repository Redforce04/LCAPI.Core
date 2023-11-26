// -----------------------------------------------------------------------
// <copyright file="TimingInitializerPostfix.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the GPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Core.Patches.Fixes;

using System;

using MEC;

/// <summary>
/// Patches the constructor for <see cref="GameNetworkManager"/> to attach the timings instance to.
/// </summary>
[HarmonyPatch(typeof(GameNetworkManager), nameof(GameNetworkManager.Awake))]
internal static class TimingInitializerPostfix
{
    [HarmonyPostfix]
    private static void Postfix(GameNetworkManager __instance)
    {
        Log.Debug("Initializing Timings");
        try
        {
            Timing.Instance = __instance.gameObject.AddComponent<Timing>();
            Timing.Instance.name = "Timing Controller";
            Timing.Instance.OnException += OnError;
        }
        catch (Exception e)
        {
            Log.Error($"An issue has occured while creating the main timings instance.");
            Log.Exception(e);
        }

        Log.Debug($"Timings Successfully attached.");
    }

    // ReSharper disable once ParameterHidesMember
    private static void OnError(Exception exception, string tag)
    {
        Log.Error($"Timings has caught an error during the execution of a coroutine{(tag == "Unknown" ? string.Empty : $" [{tag}]")}. Exception: \n" + exception.Message, "MEC Timings");
    }
}