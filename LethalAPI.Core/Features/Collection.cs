// -----------------------------------------------------------------------
// <copyright file="Collection.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// Inspiration taken from O5Zereths BananaPlugin / Feature Collection.
// -----------------------------------------------------------------------

namespace LethalAPI.Core.Features;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Interfaces;

#pragma warning disable SA1402 // only one type allowed.
#pragma warning disable SA1401

/// <summary>
/// Used to contain all a set of items.
/// </summary>
/// <typeparam name="T">The type item the collection holds.</typeparam>
// ReSharper disable MemberCanBePrivate.Global
public class Collection<T> : IEnumerable<T>
    where T : IPrefixableItem
{
    /// <summary>
    /// The items but sorted by a prefix.
    /// </summary>
    protected readonly Dictionary<string, T> itemsByPrefix;

    /// <summary>
    /// Used to notate whether the collection has been loaded or not.
    /// </summary>
    protected bool isLoaded;

    /// <summary>
    /// Initializes a new instance of the <see cref="Collection{T}"/> class.
    /// </summary>
    public Collection()
    {
        this.itemsByPrefix = new ();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Collection{T}"/> class.
    /// </summary>
    /// <param name="items">The items to use.</param>
    public Collection(List<T> items)
    {
        this.itemsByPrefix = new();
        foreach (T item in items)
        {
            itemsByPrefix.Add(item.Prefix, item);
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Collection{T}"/> class.
    /// </summary>
    /// <param name="items">The items to use.</param>
    public Collection(Dictionary<string, T> items)
    {
        this.itemsByPrefix = items;
    }

    /// <summary>
    /// Gets a value indicating whether or not the collection should be lockable.
    /// </summary>
    protected virtual bool Lockable => false;

    /// <inheritdoc cref="TryGetItem"/>
    /// <summary>
    /// Safely retrieves or modifies an item.
    /// </summary>
    /// <param name="prefix">The item prefix.</param>
    public T? this[string prefix]
    {
        get
        {
            this.itemsByPrefix.TryGetValue(prefix, out T? item);
            return item;
        }

        set
        {
            if (value is null)
                return;

            if (this.Lockable && isLoaded)
                return;

            if (this.itemsByPrefix.ContainsKey(prefix))
                this.itemsByPrefix[prefix] = value;

            this.itemsByPrefix.Add(prefix, value);
        }
    }

    /// <summary>
    /// Gets the count of the amount of items in the collection.
    /// </summary>
    /// <returns>The count of items in the collection.</returns>
    public int GetCount() => this.itemsByPrefix.Count;

    /// <summary>
    /// Attempts to get an item by its prefix.
    /// </summary>
    /// <param name="prefix">The prefix to find.</param>
    /// <param name="item">The item, if found.</param>
    /// <returns>A value indicating whether the operation was a success.</returns>
    public bool TryGetItem(string prefix, [NotNullWhen(true)] out T? item)
    {
        return this.itemsByPrefix.TryGetValue(prefix, out item);
    }

    /// <summary>
    /// Gets the enumerator.
    /// </summary>
    /// <returns>An enumerator over the list of items.</returns>
    public IEnumerator<T> GetEnumerator()
    {
        return this.itemsByPrefix.Values.GetEnumerator();
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    /// <summary>
    /// Adds an item to the list if it isn't already present.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <returns>A value indicating whether the operation was a success.</returns>
    public virtual bool TryAddItem(T item)
    {
        if (this.Lockable && this.isLoaded)
        {
            return false;
        }

        try
        {
            if (this.itemsByPrefix.ContainsKey(item.Prefix))
            {
                return false;
            }

            this.itemsByPrefix.Add(item.Prefix, item);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// Gets the value of an item or adds a new item if a value is not found.
    /// </summary>
    /// <param name="prefix">The prefix to search.</param>
    /// <param name="item">The item to add if an item is not found.</param>
    /// <returns>The resulting item. If the collection is locked this may be null.</returns>
    /// <exception cref="LockedCollectionException">Thrown if the collection is lockable and has been loaded.</exception>
    public T GetOrAddItem(string prefix, ref T item)
    {
        if (this.itemsByPrefix.TryGetValue(prefix, out T? addItem))
            return addItem;

        if (this.Lockable && this.isLoaded)
        {
            throw new LockedCollectionException();
        }

        this.itemsByPrefix.Add(prefix, item);
        return this.itemsByPrefix[prefix];
    }

    /// <summary>
    /// Modifies the value of an existing item or adds the item to the collection.
    /// </summary>
    /// <param name="prefix">The prefix to search.</param>
    /// <param name="item">The new value of the item or the item to add.</param>
    /// <returns>The resulting item.</returns>
    /// <exception cref="LockedCollectionException">Thrown if the collection is lockable and has been loaded.</exception>
    public virtual T ModifyOrAddItem(string prefix, T item)
    {
        if (this.Lockable && this.isLoaded)
            throw new LockedCollectionException();

        this.itemsByPrefix[prefix] = item;
        return this.itemsByPrefix[prefix];
    }

    /// <summary>
    /// Marks the collection as loaded, and no more items can be added.
    /// </summary>
    internal void MarkAsLoaded()
    {
        this.isLoaded = true;
    }
}

/// <summary>
/// Called when a locked collection is modified.
/// </summary>
public class LockedCollectionException : Exception
{
    /// <inheritdoc />
    public override string Message => "The collection you tried to modified is locked and can no-longer be modified.";
}