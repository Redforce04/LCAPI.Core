// -----------------------------------------------------------------------
// <copyright file="IPlugin.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Core.Interfaces;

using System;
using System.Reflection;

using Models;

/// <summary>
/// The main interface for implementing a plugin.
/// </summary>
public interface IPlugin
{
    /// <summary>
    /// Gets the assembly that the plugin is located in.
    /// </summary>
    public Assembly Assembly { get; init; }

    /// <summary>
    /// Gets the root instance of the plugin.
    /// </summary>
    public object RootInstance { get; }

    /// <summary>
    /// Gets the <see cref="PluginInfoRecord"/> pertaining to this plugin.
    /// </summary>
    public PluginInfoRecord Info { get; }

    /// <summary>
    /// Gets the name of the plugin.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the description of the plugin.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the author of the plugin.
    /// </summary>
    public string Author { get; }

    /// <summary>
    /// Gets the Version of the plugin.
    /// </summary>
    public Version Version { get; }

    /// <summary>
    /// Gets the minimum required version of the api for the plugin to load.
    /// </summary>
    public Version RequiredAPIVersion { get; }

    /// <summary>
    /// Occurs when the plugin is enabled.
    /// </summary>
    public void OnEnabled();
}