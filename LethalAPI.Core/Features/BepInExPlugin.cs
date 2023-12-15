// -----------------------------------------------------------------------
// <copyright file="BepInExPlugin.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable InconsistentNaming
#pragma warning disable SA1402 // file may only contain a single type
namespace LethalAPI.Core.Features;

using System;
using System.Linq;
using System.Reflection;

using LethalAPI.Core.Attributes;
using LethalAPI.Core.Interfaces;
using LethalAPI.Core.Models;

/// <summary>
/// An implementation of plugins for BepInEx or MelonLoader.
/// </summary>
internal sealed class BepInExPlugin : IPlugin
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BepInExPlugin"/> class.
    /// </summary>
    /// <param name="pluginInfo">The plugin instance.</param>
    internal BepInExPlugin(BepInEx.PluginInfo pluginInfo)
    {
        this.RootInstance = pluginInfo.Instance;
        this.Assembly = pluginInfo.Instance.GetType().Assembly;
        this.Name = pluginInfo.Metadata.Name;
        this.Version = pluginInfo.Metadata.Version;
        this.Author = "Unknown";
        bool isRequired = this.RootInstance.GetType().GetCustomAttributes<LethalRequiredPluginAttribute>().Any();
        this.Info = new PluginInfoRecord(this.Name, this.Version, isRequired);
    }

    /// <inheritdoc />
    public Assembly Assembly { get; init; }

    /// <inheritdoc />
    public object RootInstance { get; }

    /// <inheritdoc />
    public PluginInfoRecord Info { get; }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public string Description => "Unknown";

    /// <inheritdoc />
    public string Author { get; }

    /// <inheritdoc />
    public Version Version { get; }

    /// <inheritdoc />
    public Version RequiredAPIVersion => new (0, 0, 0);

    /// <inheritdoc />
    public void OnEnabled()
    {
    }
}