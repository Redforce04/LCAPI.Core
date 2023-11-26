// -----------------------------------------------------------------------
// <copyright file="Hazard.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the GPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Core.Features;

using System;

using UnityEngine;

/// <summary>
/// Represents an instance of a hazard.
/// </summary>
/// <typeparam name="TBase">The type of the hazard being represented.</typeparam>
/// <typeparam name="THazard">The implementing type.</typeparam>
public abstract class Hazard<TBase, THazard> : TypeCastObject<THazard>
    where THazard : class
    where TBase : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Hazard{BaseHazard,THazard}"/> class.
    /// </summary>
    /// <param name="gameObject">The GameObject representing the hazard.</param>
    protected Hazard(GameObject gameObject)
    {
        try
        {
            this.GameObject = gameObject;
            this.Base = GameObject.GetComponentInChildren<TBase>();
        }
        catch (Exception e)
        {
            Log.Warn($"{this.GetType().Name} Base or GameObject was null!");
            if (CorePlugin.Instance.Config.Debug)
                Log.Exception(e);
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Hazard{TBase,THazard}"/> class.
    /// </summary>
    /// <param name="gameObject">The GameObject representing the hazard.</param>
    /// <param name="baseObj">The instance of the base object.</param>
    protected Hazard(GameObject gameObject, TBase baseObj)
    {
        try
        {
            this.GameObject = gameObject;
            this.Base = baseObj;
        }
        catch (Exception e)
        {
            Log.Warn($"{this.GetType().Name} Base or GameObject was null!");
            if (CorePlugin.Instance.Config.Debug)
                Log.Exception(e);
        }
    }

    /// <summary>
    /// Gets or sets the base instance.
    /// </summary>
    public TBase Base { get; protected set; } = null!;

    /// <summary>
    /// Gets or sets the GameObject of the base instance.
    /// </summary>
    public GameObject GameObject { get; protected set; } = null!;
}