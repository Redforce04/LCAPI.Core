// -----------------------------------------------------------------------
// <copyright file="AutoSaveMode.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Core.ModData.Enums;

/// <summary>
/// Indicates the method of auto-saving to use.
/// </summary>
public enum AutoSaveMode
{
    /// <summary>
    /// Saves the data whenever an item is changed.
    /// </summary>
    ItemChanged,

    /// <summary>
    /// Saves will automatically occur whenever a game save takes place.
    /// </summary>
    GameSaved,

    /// <summary>
    /// No saves will occur unless triggered in the <see cref="SaveManager"/>.
    /// </summary>
    Manual,
}