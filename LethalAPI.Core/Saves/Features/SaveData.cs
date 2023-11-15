// -----------------------------------------------------------------------
// <copyright file="SaveData.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the GPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Core.Saves.Features;

using System;

using LethalAPI.Core.Saves;

#pragma warning disable SA1401 // Protected fields should be private.

/// <summary>
/// Represents data stored in the save file.
/// </summary>
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
public class SaveData : IPrefixableItem
{
    /// <summary>
    /// Indicates whether or not the data has been loaded yet.
    /// </summary>
    protected bool loaded;

    /// <summary>
    /// The internal value of the data.
    /// </summary>
    protected object value;

    /// <summary>
    /// Initializes a new instance of the <see cref="SaveData"/> class.
    /// </summary>
    /// <param name="prefix">The prefix of the item.</param>
    /// <param name="type">The type of this data.</param>
    protected SaveData(string prefix, Type type)
    {
        this.Prefix = prefix;
        this.Type = type;
    }

    /// <inheritdoc />
    public string Prefix { get; }

    /// <summary>
    /// Gets the type of the <see cref="value"/>.
    /// </summary>
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public Type Type { get; }

    /// <summary>
    /// Gets or sets the value of the data.
    /// </summary>
    public object Value
    {
        get
        {
            if (!loaded)
            {
                GetValue();
            }

            return value;
        }

        set
        {
            this.value = value;
            SaveValue();
        }
    }

    /// <summary>
    /// Saves the value to the mod save file.
    /// </summary>
    protected virtual void SaveValue()
    {
        loaded = true;
    }

    /// <summary>
    /// Gets the value to the mod save file.
    /// </summary>
    protected virtual void GetValue()
    {
        loaded = true;
    }
}