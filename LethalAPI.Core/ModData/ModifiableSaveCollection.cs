// -----------------------------------------------------------------------
// <copyright file="ModifiableSaveCollection.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Core.ModData;

using System;
using System.Collections.Generic;

using Features;
using Interfaces;

/// <summary>
/// Allows for modifying save data without repeatedly saving to the file unnecessarily.
/// </summary>
public class ModifiableSaveCollection : Collection<SaveItem>, IDisposable
{
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
}
