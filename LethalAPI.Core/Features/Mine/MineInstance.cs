// -----------------------------------------------------------------------
// <copyright file="MineInstance.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the GPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable once CheckNamespace
namespace LethalAPI.Core.Features;

using System;

using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Contains the instance implementations for the mine.
/// </summary>
public partial class Mine
{
    private Mine(GameObject mine)
    {
        try
        {
            this.GameObject = mine;
            this.Base = mine.GetComponentInChildren<Landmine>();
        }
        catch (Exception e)
        {
            Log.Warn($"Mine Base or GameObject was null!");
            if(CorePlugin.Instance.Config.Debug)
                Log.Exception(e);
        }
    }

    /// <summary>
    /// Gets the underlying base instance of the mine.
    /// </summary>
    public Landmine Base { get; init; } = null!;

    /// <summary>
    /// Gets the base GameObject of the mine.
    /// </summary>
    public GameObject GameObject { get; init; } = null!;

    /// <summary>
    /// Makes a mine explode.
    /// </summary>
    public void Explode()
    {
        Base.TriggerMineOnLocalClientByExiting();
    }

    /// <summary>
    /// Spawns the mine on the network.
    /// </summary>
    public void Spawn()
    {
        this.GameObject.GetComponent<NetworkObject>().Spawn(destroyWithScene: true);
    }
}