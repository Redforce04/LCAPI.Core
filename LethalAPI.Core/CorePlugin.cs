// -----------------------------------------------------------------------
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

using BepInEx;
using HarmonyLib;
using Loader;
using Loader.Configs;
using MEC;
using Patches.Fixes;

/// <inheritdoc />
// We name this "CorePlugin" so it won't be confused with "Plugin."
[BepInPlugin("LethalAPI.Core", "LethalAPI.Core", "1.0.0")]
public sealed class CorePlugin : BaseUnityPlugin
{
    /*
     * Keeping these here in-case another plugin wants to get more detailed plugin information.
     */

    /// <summary>
    /// The name of the plugin.
    /// </summary>
    public const string Name = PluginInfo.PLUGIN_NAME;

    /// <summary>
    /// The description of the plugin.
    /// </summary>
    public const string Description = "The core library for lethal api.";

    /// <summary>
    /// The author of the plugin.
    /// </summary>
    public const string Author = "Lethal API Modding Community";

    /// <summary>
    /// The version of the plugin.
    /// </summary>
    public static readonly Version Version = Version.Parse(PluginInfo.PLUGIN_VERSION);

    /// <summary>
    /// Gets the main instance of the core plugin.
    /// </summary>
    public static CorePlugin Instance = null!;

    /// <summary>
    /// The harmony instance.
    /// </summary>
    internal static Harmony Harmony = null!;

    /// <summary>
    /// Called on the plugin start.
    /// </summary>
    public void Awake()
    {
        Instance = this;
        Harmony = new(PluginInfo.PLUGIN_GUID);
        BepInExLogFix.Patch(Harmony);

        _ = new PluginLoader();

        // Events.Events contains the instance. This should become a plugin for loading and config purposes, in the future.
        // Events..cctor -> Patcher.PatchAll will do the patching. This is necessary for dynamic patching.
        _ = new Events.Events();

        Instance = this;

        Events.Handlers.Server.GameOpened += Init;
        Log.Debug($"Started {Name} successfully.");
    }

    private void Init()
    {
        PluginLoader.LoadAllPlugins();
        ConfigLoader.LoadAllConfigs();
        Timing.Instance.name = "Timing Controller";
        Timing.Instance.OnException += OnError;
    }

    // ReSharper disable once ParameterHidesMember
    private void OnError(Exception exception, string tag)
    {
        Log.Error($"Timings has caught an error during the execution of a coroutine{(tag == "Unknown" ? string.Empty : $" [{tag}]")}. Exception: \n" + exception.Message, "MEC Timings");
    }
}