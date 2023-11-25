// -----------------------------------------------------------------------
// <copyright file="TurretInstance.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the GPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable once CheckNamespace
namespace LethalAPI.Core;

using GameTurret = global::Turret;

/// <summary>
/// Represents a turret and abstractions for a turret.
/// </summary>
public partial class Turret : TypeCastObject<GameTurret>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Turret"/> class.
    /// </summary>
    /// <remarks>
    /// Developers must use <see cref="Turret.Get"/> or <see cref="Turret.Create"/> to create an instance.
    /// </remarks>
    /// <param name="turret">The existing turret to use.</param>
    private Turret(GameTurret turret)
    {
        this.Base = turret;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Turret"/> class.
    /// </summary>
    private Turret()
    {
        // Base = new GameTurret();
    }

    /// <summary>
    /// Gets the underlying base instance of the turret.
    /// </summary>
    public GameTurret Base { get; init; } = null!;
}