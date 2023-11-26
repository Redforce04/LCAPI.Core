// -----------------------------------------------------------------------
// <copyright file="TurretStatic.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the GPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable once CheckNamespace
namespace LethalAPI.Core.Features;

using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using GameTurret = global::Turret;

/// <summary>
/// Represents a turret and the static methods for the turret.
/// </summary>
public partial class Turret
{
    private static GameObject? turretPrefab = null;

    /// <summary>
    /// Gets a list of the turrets found.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public static List<Turret> List => new();

    /// <summary>
    /// Gets the base-game prefab for the component.
    /// </summary>
    internal static GameObject TurretPrefab
    {
        get
        {
            if (turretPrefab is not null)
                return turretPrefab;

            turretPrefab = RoundManager.Instance.spawnableMapObjects.First(x => x.prefabToSpawn.name == "TurretContainer").prefabToSpawn;
            return TurretPrefab;
        }
    }

    /// <summary>
    /// Gets a turret from a <see cref="GameTurret"/> instance.
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

        foundTurret = new Turret(turret.transform.parent.gameObject);
        List.Add(foundTurret);
        return foundTurret;
    }

    /// <summary>
    /// Gets a turret from the base GameObject instance.
    /// </summary>
    /// <param name="turret">The turret.</param>
    /// <returns>The <see cref="Turret"/> object.</returns>
    public static Turret Get(GameObject turret)
    {
        Turret? foundTurret = List.FirstOrDefault(x => x.GameObject == turret);
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
    /// <param name="position">The position which to spawn the turret.</param>
    /// <param name="rotation">The rotation which to spawn the turret.</param>
    /// <returns>The newly created turret.</returns>
    public static Turret Create(Vector3 position, Quaternion rotation)
    {
        Turret turret = new(GetNewPrefab(position, rotation));
        List.Add(turret);
        return turret;
    }

    /// <summary>
    /// Creates and spawns a turret.
    /// </summary>
    /// <param name="position">The position which to spawn the turret.</param>
    /// <param name="rotation">The rotation which to spawn the turret.</param>
    /// <returns>The newly created turret.</returns>
    public static Turret CreateAndSpawn(Vector3 position, Quaternion rotation)
    {
        Turret turret = new(GetNewPrefab(position, rotation));
        turret.Spawn();
        List.Add(turret);
        return turret;
    }

    private static GameObject GetNewPrefab(Vector3 position, Quaternion rotation) => Object.Instantiate(TurretPrefab, position, rotation, RoundManager.Instance.mapPropsContainer.transform);
}
