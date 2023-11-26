// -----------------------------------------------------------------------
// <copyright file="MineStatic.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the GPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable once CheckNamespace
namespace LethalAPI.Core.Features;

using System.Collections.Generic;
using System.Linq;

using UnityEngine;

/// <summary>
/// Contains the static implementations for the mine.
/// </summary>
public partial class Mine
{
    private static GameObject? minePrefab;

    /// <summary>
    /// Gets a list of the mines found.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public static List<Mine> List => new();

    /// <summary>
    /// Gets the base-game prefab for the component.
    /// </summary>
    internal static GameObject MinePrefab
    {
        get
        {
            if (minePrefab is not null)
                return minePrefab;

            minePrefab = RoundManager.Instance.spawnableMapObjects.First(x => x.prefabToSpawn.name == "Landmine").prefabToSpawn;
            return MinePrefab;
        }
    }

    /// <summary>
    /// Gets a mine from a <see cref="Landmine"/> instance.
    /// </summary>
    /// <param name="mine">The mine to find.</param>
    /// <returns>The <see cref="Mine"/> object.</returns>
    public static Mine Get(Landmine mine)
    {
        Mine? foundMine = List.FirstOrDefault(x => x.Base == mine);
        if (foundMine is not null)
        {
            return foundMine;
        }

        foundMine = new Mine(mine.transform.parent.gameObject);
        List.Add(foundMine);
        return foundMine;
    }

    /// <summary>
    /// Gets a mine from the base landmine GameObject instance.
    /// </summary>
    /// <param name="mine">The base landmine to find.</param>
    /// <returns>The <see cref="Mine"/> object.</returns>
    public static Mine Get(GameObject mine)
    {
        Mine? foundMine = List.FirstOrDefault(x => x.GameObject == mine);
        if (foundMine is not null)
        {
            return foundMine;
        }

        foundMine = new Mine(mine);
        List.Add(foundMine);
        return foundMine;
    }

    /// <summary>
    /// Creates a mine.
    /// </summary>
    /// <param name="position">The position which to spawn the mine.</param>
    /// <param name="rotation">The rotation which to spawn the mine.</param>
    /// <returns>The newly created mine.</returns>
    public static Mine Create(Vector3 position, Quaternion rotation)
    {
        Mine mine = new(GetNewPrefab(position, rotation));
        List.Add(mine);
        return mine;
    }

    /// <summary>
    /// Creates and spawns a mine.
    /// </summary>
    /// <param name="position">The position which to spawn the mine.</param>
    /// <param name="rotation">The rotation which to spawn the mine.</param>
    /// <returns>The newly created mine.</returns>
    public static Mine CreateAndSpawn(Vector3 position, Quaternion rotation)
    {
        Mine mine = new(GetNewPrefab(position, rotation));
        mine.Spawn();
        List.Add(mine);
        return mine;
    }

    private static GameObject GetNewPrefab(Vector3 position, Quaternion rotation) => Object.Instantiate(MinePrefab, position, rotation, RoundManager.Instance.mapPropsContainer.transform);
}