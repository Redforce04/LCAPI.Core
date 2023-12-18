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
using System.IO;
using System.Linq;
using System.Reflection;

using Features;
using Interfaces;
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

        // Ensure that these are registered by loading the reference.
        _ = new UnknownResourceParser();
        _ = new DllParser();

        // Instance is stored in the type.
        _ = new EmbeddedResourceLoader();

        ConfigDirectory = BepInEx.Paths.ConfigPath;
        DependencyDirectory = Path.GetFullPath(Path.Combine(BepInEx.Paths.PluginPath, "../", "Dependencies"));

        if(!Directory.Exists(ConfigDirectory))
            Directory.CreateDirectory(ConfigDirectory);

        if(!Directory.Exists(DependencyDirectory))
            Directory.CreateDirectory(DependencyDirectory);
        try
        {
            LoadDependencies();
        }
        catch (Exception e)
        {
            Log.Exception(e);
        }
    }

    /// <summary>
    /// Gets or sets the base directory for configs.
    /// </summary>
    public static string ConfigDirectory { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the base directory of the dependency folder.
    /// </summary>
    public static string DependencyDirectory { get; set; } = string.Empty;

    /// <summary>
    /// Gets the version of the assembly.
    /// </summary>
    public static Version Version { get; } = Assembly.GetExecutingAssembly().GetName().Version;

    /// <summary>
    /// Gets plugin dependencies.
    /// </summary>
    // ReSharper disable once CollectionNeverQueried.Global
    // ReSharper disable once CollectionNeverUpdated.Global
    public static List<Assembly> Dependencies { get; private set; } = new();

    /// <summary>
    /// Gets a dictionary containing the file paths of assemblies.
    /// </summary>
    // ReSharper disable once CollectionNeverQueried.Global
    public static Dictionary<Assembly, string> Locations { get; private set; } = new();

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
    /// Gets a plugin with its name.
    /// </summary>
    /// <param name="name">The name of the plugin.</param>
    /// <returns>The desired plugin, null if not found.</returns>
    public static IPlugin? GetPlugin(string name) =>
        Plugins.ContainsKey(name) ? Plugins[name] : null;

    /// <summary>
    /// Gets a plugin with its type.
    /// </summary>
    /// <typeparam name="TPlugin">The plugin's type.</typeparam>
    /// <returns>The desired plugin, null if not found.</returns>
    public static IPlugin? GetPlugin<TPlugin>()
        where TPlugin : IPlugin =>
        Plugins.FirstOrDefault(x => x.Value.GetType() == typeof(TPlugin)).Value;

    /// <summary>
    /// Gets a plugin with its assembly.
    /// </summary>
    /// <param name="assembly">The assembly of the plugin to find.</param>
    /// <returns>The desired plugin, null if not found.</returns>
    public static IPlugin? GetPlugin(Assembly assembly) =>
        Plugins.FirstOrDefault(x => x.Value.Assembly == assembly).Value;

    /// <summary>
    /// Loads an assembly.
    /// </summary>
    /// <param name="path">The path to load the assembly from.</param>
    /// <returns>Returns the loaded assembly or <see langword="null"/>.</returns>
    public static Assembly? LoadAssembly(string path)
    {
        try
        {
            Assembly assembly = Assembly.Load(File.ReadAllBytes(path));

            try
            {
                EmbeddedResourceLoader.Instance.GetEmbeddedObjects(assembly);
            }
            catch (Exception e)
            {
                Log.Warn($"Could not load embedded assemblies.");
                Log.Exception(e);
            }

            return assembly;
        }
        catch (Exception exception)
        {
            Log.Error($"Error while loading an assembly at {path}! {exception}");
        }

        return null;
    }

    /// <summary>
    /// Checks the required api version against a plugin's required api version.
    /// </summary>
    /// <param name="plugin">The plugin the check.</param>
    /// <returns>True if the required api version is present. False otherwise.</returns>
    public static bool CheckPluginRequiredAPIVersion(IPlugin plugin)
    {
        Version requiredVersion = plugin.RequiredAPIVersion;
        Version actualVersion = PluginLoader.Version;

        // Check Major version
        // It's increased when an incompatible API change was made
        if (requiredVersion.Major != actualVersion.Major)
        {
            // Assume that if the Required Major version is greater than the Actual Major version,
            // LethalAPI is outdated
            if (requiredVersion.Major > actualVersion.Major)
            {
                Log.Error($"You're running an older version of LethalAPI ({PluginLoader.Version.ToString(3)})! {plugin.Name} won't be loaded! " +
                    $"Required version to load it: {plugin.RequiredAPIVersion.ToString(3)}");

                return true;
            }

            if (requiredVersion.Major < actualVersion.Major)
            {
                Log.Error($"You're running an older version of {plugin.Name} ({plugin.Version.ToString(3)})! " +
                    $"Its Required Major version is {requiredVersion.Major}, but the actual version is: {actualVersion.Major}. This plugin will not be loaded!");

                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Loads all BepInEx plugins.
    /// </summary>
    internal static void LoadBepInExPlugins()
    {
        foreach (BepInEx.PluginInfo plugin in BepInEx.Bootstrap.Chainloader.PluginInfos.Values)
        {
            BepInExPlugin bepInExPlugin = new (plugin);
            RegisterPlugin(bepInExPlugin);
        }
    }

    /// <summary>
    /// Loads all dependencies.
    /// </summary>
    // ReSharper disable once UnusedMember.Local
    private static void LoadDependencies()
    {
        try
        {
            Log.Info($"Loading dependencies at {DependencyDirectory}");

            foreach (string dependency in Directory.GetFiles(DependencyDirectory, "*.dll"))
            {
                Assembly? assembly = LoadAssembly(dependency);
                if (assembly is null)
                    continue;

                Locations[assembly] = dependency;

                Dependencies.Add(assembly);

                Log.Info($"Loaded &fDependency &h'&3{assembly.GetName().Name}&h'&7@&gv{assembly.GetName().Version.ToString(3)}&7");
            }

            Log.Info("Dependencies loaded successfully!");
        }
        catch (Exception exception)
        {
            Log.Error($"An error has occurred while loading dependencies! {exception}");
        }
    }
}