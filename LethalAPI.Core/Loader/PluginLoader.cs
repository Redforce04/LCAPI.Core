// -----------------------------------------------------------------------
// <copyright file="PluginLoader.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// Taken from EXILED (https://github.com/Exiled-Team/EXILED)
// Licensed under the CC BY SA 3 license. View it here:
// https://github.com/Exiled-Team/EXILED/blob/master/LICENSE.md
// Changes: Namespace adjustments. Loader has been adjusted to use our custom plugin types.
// It also now includes the attribute based plugin loading.
// Some new features have also been added for better plugin loading.
// -----------------------------------------------------------------------

// ReSharper disable MemberCanBePrivate.Global
namespace LethalAPI.Core.Loader;

#pragma warning disable SA1401 // field should be private
#pragma warning disable SA1202 // public before private fields - methods
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Features;
using Interfaces;
using MonoMod.RuntimeDetour;
using Patches.Fixes;
using Resources;

/// <summary>
/// Loads plugins.
/// </summary>
public sealed class PluginLoader
{
    /// <summary>
    /// Gets the main instance of the <see cref="PluginLoader"/>.
    /// </summary>
    public static PluginLoader Singleton = null!;

    private static readonly Dictionary<string, IPlugin> PluginsValue = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="PluginLoader"/> class.
    /// </summary>
    internal PluginLoader()
    {
        Log.Raw("[LethalAPI-Loader] Initializing Loader.");
        Singleton = this;

        // Hooks and fixes the exception stacktrace il.
        _ = new ILHook(typeof(StackTrace).GetMethod("AddFrames", BindingFlags.Instance | BindingFlags.NonPublic), FixExceptionIL.IlHook);

        // Ensure that these are registered by loading the reference.
        _ = new UnknownResourceParser();
        _ = new DllParser();

        // Instance is stored in the type.
        _ = new EmbeddedResourceLoader();

        ConfigDirectory = BepInEx.Paths.ConfigPath;
        if(!Directory.Exists(ConfigDirectory))
            Directory.CreateDirectory(ConfigDirectory);
    }

    /// <summary>
    /// Gets or sets the base directory for configs.
    /// </summary>
    public static string ConfigDirectory { get; set; } = string.Empty;

    /// <summary>
    /// Gets the version of the assembly.
    /// </summary>
    public static Version Version { get; } = Assembly.GetExecutingAssembly().GetName().Version;

    /// <summary>
    /// Gets plugin dependencies.
    /// </summary>
    // ReSharper disable once CollectionNeverQueried.Global
    public static List<Assembly> Dependencies { get; private set; } = new();

    /// <summary>
    /// Gets a dictionary of all registered plugins, with the key being the plugin name.
    /// </summary>
    public static ReadOnlyDictionary<string, IPlugin> Plugins => new(PluginsValue);

    /// <summary>
    /// Registers a plugin.
    /// </summary>
    /// <param name="plugin">The plugin to register.</param>
    public static void RegisterPlugin(IPlugin plugin)
    {
        if (Plugins.ContainsKey(plugin.Name))
        {
            Log.Warn($"Tried to load two plugins with the same name! This is not allowed! Plugin: '{plugin.Name}'");
            return;
        }

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        Log.Info($"Loaded plugin '&3{plugin.Name}' &7@ &6v{plugin.Version.Major}.{plugin.Version.Minor}.{plugin.Version.Build} &r[{plugin.Assembly.GetName().Name}]");
        RegisterPlugin(plugin);
        PluginsValue.Add(plugin.Name, plugin);
    }

    /// <summary>
    /// Gets a plugin with its prefix or name.
    /// </summary>
    /// <param name="args">The name or prefix of the plugin (Using the prefix is recommended).</param>
    /// <returns>The desired plugin, null if not found.</returns>
    public static IPlugin? GetPlugin(string args) =>
        Plugins.ContainsKey(args) ? Plugins[args] : null;

    /// <summary>
    /// Gets a plugin with its type.
    /// </summary>
    /// <typeparam name="TPlugin">The plugin's type.</typeparam>
    /// <returns>The desired plugin, null if not found.</returns>
    public static IPlugin? GetPlugin<TPlugin>()
        where TPlugin : IPlugin =>
        Plugins.FirstOrDefault(x => x.Value.GetType() == typeof(TPlugin)).Value;

    /// <summary>
    /// Loads all plugins.
    /// </summary>
    internal static void LoadAllPlugins()
    {
    }

    /// <summary>
    /// Loads all BepInEx plugins.
    /// </summary>
    internal static void LoadBepInExPlugins()
    {
        foreach (BepInEx.PluginInfo plugin in BepInEx.Bootstrap.Chainloader.PluginInfos.Values)
        {
            BepInExPlugin bepInExPlugin = new (plugin);
            PluginsValue.Add(bepInExPlugin.Name, bepInExPlugin);
        }
    }
}