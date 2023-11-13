// -----------------------------------------------------------------------
// <copyright file="IPrefixableItem.cs" company="Lethal Company Modding Community">
// Copyright (c) Lethal Company Modding Community. All rights reserved.
// Licensed under the GPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LCAPI.Core.Saves;

/// <summary>
/// Adds a prefix to an item. Used for collections.
/// </summary>
public interface IPrefixableItem
{
    /// <summary>
    /// Gets the prefix of the item.
    /// </summary>
    string Prefix { get; }
}