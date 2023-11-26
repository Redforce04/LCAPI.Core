// -----------------------------------------------------------------------
// <copyright file="PreInitStartScreenPostfix.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the GPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Core.Patches.Events.Server;

using LethalAPI.Core.Events.Attributes;
using MEC;
using UnityEngine;

/// <summary>
///     Patches the <see cref="HandlersServer.GameOpened"/> event.
/// </summary>
[EventPatch(typeof(HandlersServer), nameof(HandlersServer.GameOpened))]
[HarmonyPatch(typeof(PreInitSceneScript), "Start")]
internal static class PreInitStartScreenPostfix
{
    [HarmonyPostfix]
    private static void Postfix(PreInitSceneScript __instance)
    {
        HandlersServer.GameOpened.InvokeSafely();
    }
}