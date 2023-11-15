﻿// -----------------------------------------------------------------------
// <copyright file="Player.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the GPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Core.Events.Handlers;

using EventArgs.Player;
using Features;

/// <summary>
/// Contains event handlers for player events.
/// </summary>
public static class Player
{
    /// <summary>
    ///     Gets or sets the event that is invoked when a player is critically injured.
    /// </summary>
    public static Event<CriticallyInjureEventArgs> CriticallyInjure { get; set; } = new ();

    /// <summary>
    ///     Gets or sets the event that is invoked when a player is healed.
    /// </summary>
    public static Event<HealingEventArgs> Healing { get; set; } = new ();

    /// <summary>
    ///     Gets or sets the event that is invoked when a player is using a key.
    /// </summary>
    public static Event<UsingKeyEventArgs> UsingKey { get; set; } = new ();

    /// <summary>
    ///     Gets or sets the event that is invoked when a player is using an item.
    /// </summary>
    public static Event<UsingItemEventArgs> UsingItem { get; set; } = new ();

    /// <summary>
    /// Called when a player becomes critically injured.
    /// </summary>
    /// <param name="ev">
    ///     The <see cref="CriticallyInjureEventArgs"/> instance.
    /// </param>
    public static void OnCriticalInjury(CriticallyInjureEventArgs ev) => CriticallyInjure.InvokeSafely(ev);

    /// <summary>
    /// Called when a player is healing from the critically injured state.
    /// </summary>
    /// <param name="ev">
    ///     The <see cref="HealingEventArgs"/> instance.
    /// </param>
    public static void OnHealing(HealingEventArgs ev) => Healing.InvokeSafely(ev);

    /// <summary>
    /// Called when a key is used.
    /// </summary>
    /// <param name="ev">
    ///     The <see cref="UsingKey"/> instance.
    /// </param>
    public static void OnUsingKey(UsingKeyEventArgs ev) => UsingKey.InvokeSafely(ev);

    /// <summary>
    /// Called when an item is used.
    /// </summary>
    /// <param name="ev">
    ///     The <see cref="UsingItem"/> instance.
    /// </param>
    public static void OnUsingItem(UsingItemEventArgs ev) => UsingItem.InvokeSafely(ev);
}