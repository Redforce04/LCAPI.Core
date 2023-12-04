﻿// -----------------------------------------------------------------------
// <copyright file="GlobalSaveDataAttribute.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Core.ModData.Attributes;

using System;

using Enums;
using Internal;

/// <summary>
/// An attribute that specify whether a given field is used to store/retrieve save data.
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public sealed class GlobalSaveDataAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalSaveDataAttribute"/> class.
    /// </summary>
    /// <param name="autoLoad">Indicate whether this will be automatically loaded when the game begin to load it data.</param>
    /// <param name="autoSave">Indicate whether this will be automatically saved when the game begin to save it data.</param>
    public GlobalSaveDataAttribute(bool autoLoad = true, AutoSaveMode autoSave = AutoSaveMode.GameSaved)
    {
        this.GlobalSaveSettings = new SaveDataSettings(autoLoad, autoSave);
    }

    /// <summary>
    /// Gets contains the general settings for saving and loading data.
    /// </summary>
    internal SaveDataSettings GlobalSaveSettings { get; }
}