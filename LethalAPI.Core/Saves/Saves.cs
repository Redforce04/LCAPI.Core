// -----------------------------------------------------------------------
// <copyright file="Saves.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the GPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Core.Saves;

using Features;
using Internal;

/// <summary>
/// Contains the internal api for the saves system.
/// </summary>
internal class Saves
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Saves"/> class.
    /// </summary>
    internal Saves()
    {
        if (Singleton is not null)
        {
            return;
        }

        Singleton = this;
    }

    /// <summary>
    /// Gets the singleton for the internal Saves api.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    internal static Saves Singleton { get; private set; }

    // ReSharper disable once UnusedMember.Local
    private void AutoLoadSaveData()
    {
        foreach (LcPluginInfo plugin in LcPluginInfo.Plugins)
        {
            foreach (PluginDataInstance globalSave in plugin.GlobalSaveData)
            {
                globalSave.AutoLoadData();
            }

            foreach (PluginDataInstance localSave in plugin.LocalSaveData)
            {
                localSave.AutoLoadData();
            }
        }
    }
}