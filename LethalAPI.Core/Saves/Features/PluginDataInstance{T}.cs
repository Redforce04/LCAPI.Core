// -----------------------------------------------------------------------
// <copyright file="PluginDataInstance{T}.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the GPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Core.Saves.Features;

using System.Reflection;

/// <summary>
/// Represents save data for a plugin.
/// </summary>
/// <typeparam name="T">The type of save data to be deserialized.</typeparam>
public class PluginDataInstance<T> : PluginDataInstance
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PluginDataInstance{T}"/> class.
    /// </summary>
    /// <param name="instance">The object instance for non-static fields.</param>
    /// <param name="field">The field to get or set.</param>
    /// <param name="pluginName">The name of the plugin being saved or loaded.</param>
    /// <param name="isGlobalData">Is the data from the global data save slot.</param>
    /// <param name="shouldAutoLoadData">Should the data be auto-loaded by default.</param>
    public PluginDataInstance(object instance, FieldInfo field, string pluginName, bool isGlobalData = false, bool shouldAutoLoadData = true)
        : base(instance, field, pluginName, isGlobalData, shouldAutoLoadData)
    {
    }

    /// <summary>
    /// Gets or sets the value of the <see cref="PluginDataInstance.Field"/>.
    /// </summary>
    public new T Value
    {
        get { return (T)Field.GetValue(this.instance); }

        set { Field.SetValue(this.instance, value); }
    }

    /// <inheritdoc />
    public override void LoadData()
    {
    }

    /// <inheritdoc />
    public override void SaveData()
    {
    }

    /// <inheritdoc />
    internal override void AutoLoadData()
    {
        if (!this.shouldAutoLoadData)
        {
            return;
        }

        this.LoadData();
    }
}