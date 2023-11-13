// -----------------------------------------------------------------------
// <copyright file="SaveDataAttribute.cs" company="Lethal Company Modding Community">
// Copyright (c) Lethal Company Modding Community. All rights reserved.
// Licensed under the GPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LCAPI.Core.Saves.Attributes;

using System;

/// <summary>
/// Represents a field to be serialized into global save data.
/// </summary>
// Dont seal this because a custom save system might use it.
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class SaveDataAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SaveDataAttribute"/> class.
    /// </summary>
    /// <param name="loadAutomatically">Indicates whether the save should be loaded and saved with the main game save.</param>
    public SaveDataAttribute(bool loadAutomatically = true)
    {
        this.LoadAutomatically = loadAutomatically;
    }

    /// <summary>
    /// Gets a value indicating whether or not the data should be loaded and saved alongside the vanilla save system.
    /// </summary>
    public bool LoadAutomatically { get; }
}