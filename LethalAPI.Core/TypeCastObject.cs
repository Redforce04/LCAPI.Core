// -----------------------------------------------------------------------
// <copyright file="TypeCastObject.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the GPL-3.0 license.
// </copyright>
// Taken from EXILED (https://github.com/Exiled-Team/EXILED)
// Licensed under the CC BY SA 3 license. View it here:
// https://github.com/Exiled-Team/EXILED/blob/master/LICENSE.md
// Changes: Namespace adjustments.
// -----------------------------------------------------------------------

namespace LethalAPI.Core;

/// <summary>
/// The interface which allows defined objects to be cast to each other.
/// </summary>
/// <typeparam name="T">The type of the object to cast.</typeparam>
public abstract class TypeCastObject<T>
    where T : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeCastObject{T1}"/> class.
    /// </summary>
    protected TypeCastObject()
    {
    }

    /// <summary>
    /// Unsafely casts the current <typeparamref name="T"/> instance to the specified <typeparamref name="TObject"/> type.
    /// </summary>
    /// <typeparam name="TObject">The type to which to cast the <typeparamref name="T"/> instance.</typeparam>
    /// <returns>The cast <typeparamref name="T"/> instance.</returns>
    public TObject? Cast<TObject>()
        where TObject : class, T => this as T as TObject;

    /// <summary>
    /// Safely casts the current <typeparamref name="TObject"/> instance to the specified <typeparamref name="TObject"/> type.
    /// </summary>
    /// <typeparam name="TObject">The type to which to cast the <typeparamref name="TObject"/> instance.</typeparam>
    /// <param name="param">The cast object.</param>
    /// <returns><see langword="true"/> if the <typeparamref name="TObject"/> instance was successfully cast; otherwise, <see langword="false"/>.</returns>
    public bool Cast<TObject>(out TObject? param)
        where TObject : class, T
    {
        param = default;

        if (this as TObject is not { } cast)
            return false;

        param = cast;
        return true;
    }

    /// <inheritdoc cref="Cast{T}()"/>
    public TObject? As<TObject>()
        where TObject : class, T => Cast<TObject>();

    /// <inheritdoc cref="Cast{T}(out T)"/>
    public bool Is<TObject>(out TObject? param)
        where TObject : class, T => Cast(out param);
}