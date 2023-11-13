// -----------------------------------------------------------------------
// <copyright file="PluginDataInstance.cs" company="Lethal Company Modding Community">
// Copyright (c) Lethal Company Modding Community. All rights reserved.
// Licensed under the GPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LCAPI.Core.Saves.Features;

using System.Reflection;

#pragma warning disable SA1401 // field should be made private.

/// <summary>
/// Represents save data for a plugin.
/// </summary>
// ReSharper disable InconsistentNaming
public class PluginDataInstance
{
    /// <summary>
    /// A value representing whether or not the save data is a global data instance.
    /// </summary>
    protected readonly bool isGlobalData;

    /// <summary>
    /// The instance of the object for the type. Used as a reference for setting the type.
    /// </summary>
    protected readonly object instance;

    /// <summary>
    /// The name of the plugin being instantiated.
    /// </summary>
    protected readonly string pluginName;

    /// <summary>
    /// Indicates whether the field should be loaded during the auto-loading process.
    /// </summary>
    protected readonly bool shouldAutoLoadData;

    /// <summary>
    /// Initializes a new instance of the <see cref="PluginDataInstance"/> class.
    /// </summary>
    /// <param name="instance">The object instance for non-static fields.</param>
    /// <param name="field">The field to get or set.</param>
    /// <param name="pluginName">The name of the plugin being saved or loaded.</param>
    /// <param name="isGlobalData">Is the data from the global data save slot.</param>
    /// <param name="shouldAutoLoadData">Should the data be auto-loaded by default.</param>
    protected PluginDataInstance(object instance, FieldInfo field, string pluginName, bool isGlobalData = false, bool shouldAutoLoadData = true)
    {
        this.instance = instance;
        this.Field = field;
        this.pluginName = pluginName;
        this.isGlobalData = isGlobalData;
        this.shouldAutoLoadData = shouldAutoLoadData;
    }

    /// <summary>
    /// Gets the Field that is set or read.
    /// </summary>
    public FieldInfo Field { get; }

    /// <summary>
    /// Gets or sets the value of the <see cref="Field"/>.
    /// </summary>
    public object Value
    {
        get { return this.Field.GetValue(instance); }

        set { this.Field.SetValue(instance, value); }
    }

    /// <summary>
    /// Gets the data from this instance and saves it.
    /// </summary>
    public virtual void SaveData()
    {
    }

    /// <summary>
    /// Gets the data for this instance and updates the field.
    /// </summary>
    public virtual void LoadData()
    {
    }

    /// <summary>
    /// Initiates an autoload for the data.
    /// </summary>
    internal virtual void AutoLoadData()
    {
    }
}