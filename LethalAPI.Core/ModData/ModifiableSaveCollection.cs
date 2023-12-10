// -----------------------------------------------------------------------
// <copyright file="ModifiableSaveCollection.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Core.ModData;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

using Features;
using Interfaces;
using Loader;

/// <summary>
/// Allows for modifying save data without repeatedly saving to the file unnecessarily.
/// </summary>
public class ModifiableSaveCollection : Collection<SaveItem>, IDisposable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ModifiableSaveCollection"/> class.
    /// </summary>
    /// <param name="useGlobalData">Indicates whether or not this data represents the global save slot or the local save slot.</param>
    public ModifiableSaveCollection(bool useGlobalData = false)
        : base(GetPluginData(useGlobalData))
    {
        this.Global = useGlobalData;
        this.Plugin = GetPlugin();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ModifiableSaveCollection"/> class.
    /// </summary>
    private ModifiableSaveCollection(IPlugin<IConfig> plugin, bool global)
        : base(global ? SaveManager.GlobalSaves[plugin.Name].AsList : SaveManager.LocalSaves[plugin.Name].AsList)
    {
        this.Global = global;
        this.Plugin = plugin;
    }

    /// <inheritdoc />
    protected override bool Lockable => false;

    private IPlugin<IConfig> Plugin { get; }

    private bool Global { get; }

    /// <summary>
    /// Disposes the instance.
    /// </summary>
    public void Dispose()
    {
        SaveItemCollection collection = new();
        foreach (KeyValuePair<string, SaveItem> kvp in this.itemsByPrefix)
        {
            collection.TryAddItem(kvp.Value);
        }

        (this.Global ? SaveManager.GlobalSaves : SaveManager.LocalSaves)[this.Plugin.Name] = collection;
        this.itemsByPrefix.Clear();
        SaveManager.SaveData(this.Plugin, this.Global);
    }

    /// <summary>
    /// Gets a new instance of <see cref="ModifiableSaveCollection"/> for a given plugin.
    /// </summary>
    /// <param name="plugin">The plugin to get the data from.</param>
    /// <param name="global">Whether to pull from global data.</param>
    /// <returns>The new instance of <see cref="ModifiableSaveCollection"/>.</returns>
    internal static ModifiableSaveCollection GetModifiablePluginData(IPlugin<IConfig> plugin, bool global)
    {
        return new ModifiableSaveCollection(plugin, global);
    }

    private static List<SaveItem> GetPluginData(bool global = false)
    {
        IPlugin<IConfig> plugin = GetPlugin();
        return global ? SaveManager.GlobalSaves[plugin.Name].AsList : SaveManager.LocalSaves[plugin.Name].AsList;
    }

    private static IPlugin<IConfig> GetPlugin()
    {
        Assembly? assembly = new StackTrace().GetFrame(3).GetMethod().DeclaringType?.Assembly;
        if (assembly is null)
        {
            Log.Error("Could not find the corresponding plugin to get the modifiable save collection for.");
            throw new Exception("Plugin / Assembly Not Found");
        }

        IPlugin<IConfig>? plugin = PluginLoader.GetPlugin(assembly);
        if (plugin is null)
        {
            Log.Error($"Could not find the corresponding plugin to get the modifiable save collection for. The assembly '{assembly.FullName}' has no plugins registered to it!");
            throw new Exception("Plugin Not Found");
        }

        return plugin;
    }
}
