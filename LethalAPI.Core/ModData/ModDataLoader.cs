// -----------------------------------------------------------------------
// <copyright file="ModDataLoader.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Core.ModData;

using System;
using System.Collections.Generic;

public class ModDataLoader : IDisposable
{
    public ModDataLoader()
    {
        this.PluginData = SaveManager.
    }

    public Dictionary<string, SaveItemCollection> PluginData { get; set; }

    /// <summary>
    /// Deserialize
    /// </summary>
    /// <param name="global"></param>
    public static void DeserializeAllSaves(bool global = false)
    {
        Log.Debug($"Loading all plugin data{(global ? " [&2Global&r]" : string.Empty)}.");
        string path = GetCurrentSavePath(global);
        string data = File.Exists(path) ? File.ReadAllText(path) : string.Empty;
        Dictionary<string, List<SaveItem>>? values = JsonConvert.DeserializeObject<Dictionary<string, List<SaveItem>>>(data);
        if (values is null)
        {
            if(global)
                GlobalSaves = new();
            else
                LocalSaves = new();

            Log.Error("The save file has been corrupted and could not be loaded.");
            Log.Debug($"Slot: {(global ? LocalModdedSaveFileName : GlobalModdedSaveFileName)}. Null or empty save slot.");

            try
            {
                if(File.Exists(path))
                    File.Delete(path);

                foreach (IPlugin<IConfig> plugin in PluginLoader.Plugins.Values)
                    GenerateNewSaveCollection(plugin, global);

                SerializeAllSaves();
            }
            catch (Exception e)
            {
                Log.Warn("Could not delete the save file.");
                if(CorePlugin.Instance.Config.Debug)
                    Log.Exception(e);
            }

            return;
        }

        foreach (KeyValuePair<string, List<SaveItem>> kvp in values)
        {
            SaveItemCollection collection = new(kvp.Value);
            collection.MarkAsLoaded();
            if((global ? GlobalSaves : LocalSaves).ContainsKey(kvp.Key))
                (global ? GlobalSaves : LocalSaves)[kvp.Key] = collection;
            else
                (global ? GlobalSaves : LocalSaves).Add(kvp.Key, collection);
        }
    }

    private static void GenerateNewSaveCollection(IPlugin<IConfig> plugin, bool global = false)
    {
        Log.Debug($"Generating new plugin data for plugin {plugin.Name}{(global ? " [&2Global&r]" : string.Empty)}.");
        SaveItemCollection collection = new();
        if (global)
        {
            if (plugin.GlobalSaveHandler is IInstanceSave globalSave)
            {
                object? value = null;
                try
                {
                    value = Activator.CreateInstance(globalSave.Save.GetType(), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance);
                }
                catch (Exception e)
                {
                    Log.Error($"Could not create an instance of the global save data for type '{globalSave.Save.GetType().FullName}'. This is probably due to an invalid type.");
                    if(CorePlugin.Instance.Config.Debug)
                        Log.Exception(e);
                }

                if(value is not null)
                    collection.UpdateCollectionWithObjectValues(value);
            }

            GlobalSaves.Add(plugin.Name, collection);
            return;
        }

        if (plugin.LocalSaveHandler is IInstanceSave localSave)
        {
            object? value = null;
            try
            {
                value = Activator.CreateInstance(localSave.Save.GetType(), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance);
            }
            catch (Exception e)
            {
                Log.Error($"Could not create an instance of the global save data for type '{localSave.Save.GetType().FullName}'. This is probably due to an invalid type.");
                if(CorePlugin.Instance.Config.Debug)
                    Log.Exception(e);
            }

            if(value is not null)
                collection.UpdateCollectionWithObjectValues(value);
        }

        LocalSaves.Add(plugin.Name, collection);
    }

    private static void SerializeAllSaves(bool global = false)
    {
        Log.Debug($"Saving all plugin data{(global ? " [&2Global&r]" : string.Empty)}.");
        Dictionary<string, List<SaveItem>> items = new();
        foreach (KeyValuePair<string, SaveItemCollection> saves in global ? GlobalSaves : LocalSaves)
        {
            items.Add(saves.Key, saves.Value.AsList);
        }

        string json = JsonConvert.SerializeObject(items);
        string path = GetCurrentSavePath(global);
        try
        {
            File.WriteAllText(path, json);
        }
        catch (Exception e)
        {
            Log.Error($"Could not save to file '{path}'.");
            if(CorePlugin.Instance.Config.Debug)
                Log.Exception(e);
        }
    }
}