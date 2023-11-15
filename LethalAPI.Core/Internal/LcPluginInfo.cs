// -----------------------------------------------------------------------
// <copyright file="LcPluginInfo.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the GPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Core.Internal;

using System;
using System.Collections.Generic;
using System.Reflection;

using BepInEx;
using Saves.Attributes;
using Saves.Features;

/// <summary>
/// Information related to a plugin.
/// </summary>
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable UnusedMember.Local
// ReSharper disable MemberCanBePrivate.Global
public sealed class LcPluginInfo
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LcPluginInfo"/> class.
    /// </summary>
    /// <param name="plugin">The plugin to register.</param>
    public LcPluginInfo(PluginInfo plugin)
    {
        if (plugin is null || !plugin.Instance)
        {
            throw new ArgumentNullException(nameof(plugin));
        }

        this.Info = plugin;
        this.Instance = plugin.Instance;
        this.Type = plugin.GetType();
        this.Assembly = this.Type.Assembly;

        FindAllFieldInfo();
    }

    /// <summary>
    /// Gets a list of information for all registered plugins.
    /// </summary>
    // ReSharper disable once CollectionNeverQueried.Global
    public static List<LcPluginInfo> Plugins { get; private set; }

    /// <summary>
    /// Gets the base instance of the plugin.
    /// </summary>
    public BaseUnityPlugin Instance { get; }

    /// <summary>
    /// Gets info about the dependencies and other important information.
    /// </summary>
    public PluginInfo Info { get; }

    /// <summary>
    /// Gets the assembly of the plugin.
    /// </summary>
    public Assembly Assembly { get; }

    /// <summary>
    /// Gets the name of the plugin.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the Guid of the plugin.
    /// </summary>
    public string Guid { get; private set; }

    /// <summary>
    /// Gets the version of the plugin.
    /// </summary>
    public Version Version { get; private set; }

    /// <summary>
    /// Gets the type of the plugin.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public Type Type { get; private set; }

    /// <summary>
    /// Gets a collection of all found global save data instances.
    /// </summary>
    public List<PluginDataInstance> GlobalSaveData => new();

    /// <summary>
    /// Gets a collection of all found local save data instances.
    /// </summary>
    public List<PluginDataInstance> LocalSaveData => new();

    /// <summary>
    /// Gets a cached collection of the most recent local save data.
    /// </summary>
    public SaveDataCollection CachedLocalSaveData { get; private set; }

    /// <summary>
    /// Gets a cached collection of the most recent local save data.
    /// </summary>
    public SaveDataCollection CachedGlobalSaveData { get; private set; }

    /// <summary>
    /// Loads all plugins.
    /// </summary>
    internal static void LoadPlugins()
    {
        foreach (PluginInfo plg in BepInEx.Bootstrap.Chainloader.PluginInfos.Values)
        {
            Plugins.Add(new LcPluginInfo(plg));
        }
    }

    /// <summary>
    /// Gets and sets the information relating to the plugin info.
    /// </summary>
    private void FindPluginInfo()
    {
        BepInPlugin pluginInfo = this.Instance.GetType().GetCustomAttribute<BepInPlugin>();
        if (pluginInfo is null)
        {
            return;
        }

        this.Name = pluginInfo.Name;
        this.Version = pluginInfo.Version;
    }

    /// <summary>
    /// Gets all fields related to the save system.
    /// </summary>
    private void FindAllFieldInfo()
    {
        foreach (FieldInfo field in this.Instance.GetType().GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
        {
            GlobalSaveDataAttribute globalSave = field.GetCustomAttribute<GlobalSaveDataAttribute>();
            SaveDataAttribute localSave = field.GetCustomAttribute<SaveDataAttribute>();
            if (globalSave is null && localSave is null)
            {
                continue;
            }

            PluginDataInstance dataInstance = (PluginDataInstance)typeof(PluginDataInstance<>).MakeGenericType(field.FieldType)
                .GetConstructor(new[] { typeof(object), typeof(FieldInfo), typeof(string), typeof(bool) })!
                .Invoke(null, new object[] { this.Instance, field, this.Name, (globalSave is not null) });

            if (globalSave is not null)
            {
                this.GlobalSaveData.Add(dataInstance);
            }
            else
            {
                this.LocalSaveData.Add(dataInstance);
            }
        }
    }
}