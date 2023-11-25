// -----------------------------------------------------------------------
// <copyright file="TurretStatic.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the GPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable once CheckNamespace
namespace LethalAPI.Core;

using System.Collections.Generic;
using System.Linq;
using GameTurret = global::Turret;

public partial class Turret
{
    /// <summary>
    /// Gets a list of the turrets found.
    /// </summary>
    public static List<Turret> List => new();

    /// <summary>
    /// Gets a turret from an instance.
    /// </summary>
    /// <param name="turret">The turret.</param>
    /// <returns>The <see cref="Turret"/> object.</returns>
    public static Turret Get(GameTurret turret)
    {
        Turret? foundTurret = List.FirstOrDefault(x => x.Base == turret);
        if (foundTurret is not null)
        {
            return foundTurret;
        }

        foundTurret = new Turret(turret);
        List.Add(foundTurret);
        return foundTurret;
    }

    /// <summary>
    /// Creates a turret.
    /// </summary>
    /// <returns>The newly created turret.</returns>
    public static Turret Create()
    {
        Turret turret = new();
        List.Add(turret);
        
        return turret;
    }
}