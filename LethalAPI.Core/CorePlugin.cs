﻿// -----------------------------------------------------------------------
// <copyright file="CorePlugin.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the GPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Core;

// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
#pragma warning disable SA1401 // field should be made private
#pragma warning disable SA1309 // Names should not start with an underscore. ie: _Logger.
using System;

using Features;
using HarmonyLib;
using MEC;

/// <inheritdoc />
public class CorePlugin : Plugin<CoreConfig>
{
    /// <summary>
    /// Gets the main instance of the core plugin.
    /// </summary>
    public static CorePlugin Instance = null!;

    /// <summary>
    /// The harmony instance.
    /// </summary>
    internal static Harmony Harmony = null!;

    /// <inheritdoc />
    // sets this so the config name isn't a mess. :)
    public override string Name => "LethalApi-Core";

    /// <inheritdoc />
    public override string Description => "The core library for lethal api.";

    /// <inheritdoc />
    public override string Author => "Lethal API Modding Community";

    /// <inheritdoc />
    public override Version Version => Version.Parse(PluginInfo.PLUGIN_VERSION);

    /// <inheritdoc />
    public override void OnEnabled()
    {
        Instance = this;
        Harmony = new(PluginInfo.PLUGIN_GUID);

        // Events.Events contains the instance. This should become a plugin for loading and config purposes, in the future.
        // Events..cctor -> Patcher.PatchAll will do the patching. This is necessary for dynamic patching.
        _ = new Events.Events();

        Instance = this;

        Log.Info($"{this.Name} is being loaded...");
    }
}