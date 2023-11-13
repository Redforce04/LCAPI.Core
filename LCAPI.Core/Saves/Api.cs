// -----------------------------------------------------------------------
// <copyright file="Api.cs" company="Lethal Company Modding Community">
// Copyright (c) Lethal Company Modding Community. All rights reserved.
// Licensed under the GPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LCAPI.Core.Saves;

using System.Linq;

using Attributes;
using BepInEx;
using Features;
using Internal;

/// <summary>
/// Contains the main api for the saving endpoint.
/// </summary>
public static class Api
{
    /// <summary>
    /// Triggers a save for all <see cref="SaveDataAttribute"/> and <see cref="GlobalSaveDataAttribute"/> instances.
    /// </summary>
    /// <param name="plugin">The plugin instance to save.</param>
    public static void TriggerSave(BaseUnityPlugin plugin) => UpdateFields(plugin, false);

    /// <summary>
    /// Triggers a load for all <see cref="SaveDataAttribute"/> and <see cref="GlobalSaveDataAttribute"/> instances.
    /// </summary>
    /// <param name="plugin">The plugin instance to load.</param>
    public static void TriggerLoad(BaseUnityPlugin plugin) => UpdateFields(plugin);

    /// <summary>
    /// Gets the Global <see cref="SaveDataCollection"/> for a plugin.
    /// </summary>
    /// <param name="plugin">The plugin to get the save data for.</param>
    /// <returns>The global save data.</returns>
    public static SaveDataCollection GetGlobalSaveData(BaseUnityPlugin plugin) =>
        LcPluginInfo.Plugins.First(x => x.Instance == plugin).CachedGlobalSaveData;

    /// <summary>
    /// Gets the Local <see cref="SaveDataCollection"/> for a plugin.
    /// </summary>
    /// <param name="plugin">The plugin to get the save data for.</param>
    /// <returns>The local save data.</returns>
    public static SaveDataCollection GetLocalSaveData(BaseUnityPlugin plugin) =>
        LcPluginInfo.Plugins.First(x => x.Instance == plugin).CachedLocalSaveData;

    /// <summary>
    /// Triggers an update for specific values.
    /// </summary>
    /// <param name="plugin">The instance of the plugin to update fields.</param>
    /// <param name="load">Indicates whether save data fields will be saved or loaded.</param>
    private static void UpdateFields(BaseUnityPlugin plugin, bool load = true)
    {
        foreach (PluginDataInstance info in LcPluginInfo.Plugins.First(x => x.Instance == plugin).GlobalSaveData)
        {
            if (load)
            {
                info.LoadData();
            }
            else
            {
                info.SaveData();
            }
        }

        foreach (PluginDataInstance info in LcPluginInfo.Plugins.First(x => x.Instance == plugin).LocalSaveData)
        {
            if (load)
            {
                info.LoadData();
            }
            else
            {
                info.SaveData();
            }
        }
    }
}