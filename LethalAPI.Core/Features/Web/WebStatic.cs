// -----------------------------------------------------------------------
// <copyright file="WebStatic.cs" company="LethalAPI Modding Community">
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
/// Contains the static implementations for the web.
/// </summary>
public partial class Web
{
    private static GameObject? webPrefab;

    /// <summary>
    /// Gets the main instance of the <see cref="SandSpiderAI"/>.
    /// </summary>
    public static SandSpiderAI SpiderAI { get; internal set; } = null!;

    /// <summary>
    /// Gets or sets a list of the webs found.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public static List<Web> List { get; set; } = new();

    /// <summary>
    /// Gets or sets the base-game prefab for the component.
    /// </summary>
    internal static GameObject WebPrefab
    {
        get
        {
            if (webPrefab is not null)
                return webPrefab;

            UnityEngine.Object.FindObjectsOfType(typeof(SandSpiderAI));
            global::UnityEngine.Object.FindObjectsOfType<EnemyAI>();
            foreach (EnemyAI ai in Object.FindObjectsOfType<EnemyAI>())
            {
                if (ai is not SandSpiderAI spiderAi)
                    continue;

                SpiderAI = spiderAi;
                break;
            }

            webPrefab = SpiderAI.webTrapPrefab;
            return webPrefab;
        }

        set
        {
            if (webPrefab is not null)
                return;
            webPrefab = value;
        }
    }

    /// <summary>
    /// Gets a web from a <see cref="SandSpiderWebTrap"/> instance.
    /// </summary>
    /// <param name="web">The <see cref="SandSpiderWebTrap"/> to find.</param>
    /// <returns>The <see cref="Web"/> object.</returns>
    public static Web Get(SandSpiderWebTrap web)
    {
        Web? foundWeb = List.FirstOrDefault(x => x.Base == web);
        if (foundWeb is not null)
        {
            return foundWeb;
        }

        foundWeb = new Web(web.transform.parent.gameObject);
        List.Add(foundWeb);
        return foundWeb;
    }

    /// <summary>
    /// Gets a web from the base <see cref="SandSpiderWebTrap"/> GameObject instance.
    /// </summary>
    /// <param name="web">The base <see cref="SandSpiderWebTrap"/> to find.</param>
    /// <returns>The <see cref="Web"/> object.</returns>
    public static Web Get(GameObject web)
    {
        Web? foundWeb = List.FirstOrDefault(x => x.GameObject == web);
        if (foundWeb is not null)
        {
            return foundWeb;
        }

        foundWeb = new Web(web);
        List.Add(foundWeb);
        return foundWeb;
    }

    /// <summary>
    /// Creates a web.
    /// </summary>
    /// <param name="position">The position which to spawn the web.</param>
    /// <param name="endPosition">The position which the web will face.</param>
    /// <returns>The newly created web.</returns>
    public static Web Create(Vector3 position, Vector3 endPosition)
    {
        Web web = new(GetNewPrefab(position));
        web.GameObject.transform.LookAt(endPosition);
        List.Add(web);
        return web;
    }

    private static GameObject GetNewPrefab(Vector3 position) => Object.Instantiate(WebPrefab, position, Quaternion.identity, RoundManager.Instance.mapPropsContainer.transform);
}