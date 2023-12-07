﻿// -----------------------------------------------------------------------
// <copyright file="CorePlugin.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Core;

// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
#pragma warning disable SA1401 // field should be made private
#pragma warning disable SA1309 // Names should not start with an underscore. ie: _Logger.
using System;

using Events.EventArgs.Server;
using Features;
using HarmonyLib;
using MEC;
using ModData;

/// <inheritdoc />
// We name this "CorePlugin" so it won't be confused with "Plugin."
public sealed class CorePlugin : Plugin<CoreConfig>
{
    /// <summary>
    /// Gets the main instance of the core plugin.
    /// </summary>
    public static CorePlugin Instance = null!;

    /// <summary>
    /// Indicates whether debug logs should be shown before the core plugin is loaded.
    /// </summary>
    internal const bool PreLoaderDebugLogs = true;

    /// <inheritdoc />
    // sets this so the config name isn't a mess. :)
    public override string Name => "LethalApi-Core";

    /// <inheritdoc />
    public override string Description => "The core library for lethal api.";

    /// <inheritdoc />
    public override string Author => "Lethal API Modding Community";

    /// <inheritdoc />
    public override Version Version => Version.Parse(PluginInfo.PLUGIN_VERSION);

    /// <summary>
    /// Gets the harmony instance.
    /// </summary>
    internal static Harmony Harmony => new ("LethalAPI.Core");

    /// <inheritdoc />
    public override void OnEnabled()
    {
        this.Config.Debug = PreLoaderDebugLogs;
        Instance = this;

        // Events.Events contains the instance. This should become a plugin for loading and config purposes, in the future.
        // Events..cctor -> Patcher.PatchAll will do the patching. This is necessary for dynamic patching.
        _ = new Events.Events();

        Instance = this;

        Events.Handlers.Server.GameOpened += InitTimings;
        Events.Handlers.Server.LoadingSave += InitModData;
        Log.Debug($"Started {this.Name} successfully.");
    }

    private void InitTimings()
    {
        Timing.Instance.name = "Timing Controller";
        Timing.Instance.OnException += OnError;
    }

    private void InitModData(LoadingSaveEventArgs ev)
    {
        SaveManager.Init();
        Events.Handlers.Server.LoadingSave -= InitModData;
    }

    // ReSharper disable once ParameterHidesMember
    private void OnError(Exception exception, string tag)
    {
        Log.Error($"Timings has caught an error during the execution of a coroutine{(tag == "Unknown" ? string.Empty : $" [{tag}]")}. Exception: \n" + exception.Message, "MEC Timings");
    }
}