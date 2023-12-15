// -----------------------------------------------------------------------
// <copyright file="IConfigurablePlugin.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Core.Interfaces;

/// <summary>
/// The wrapper interface for plugins utilizing Lethal Configs.
/// </summary>
/// <typeparam name="TConfig">The config type.</typeparam>
public interface IConfigurablePlugin<TConfig>
    where TConfig : IConfig
{
    /// <summary>
    /// Gets or sets the config.
    /// </summary>
    /// <remarks>If utilizing the <see cref="IConfig"/> interface, this should be = new() / not null.</remarks>
    public TConfig Config { get; set; }
}