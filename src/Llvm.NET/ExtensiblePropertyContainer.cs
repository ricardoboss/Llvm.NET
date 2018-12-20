﻿// <copyright file="ExtensiblePropertyContainer.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Llvm.NET.Properties;

namespace Llvm.NET
{
    /// <summary>Common implementation of <see cref="IExtensiblePropertyContainer"/></summary>
    /// <remarks>
    /// This class implements <see cref="IExtensiblePropertyContainer"/> through an
    /// internal <see cref="System.Collections.Generic.Dictionary{TKey, TValue}"/>
    /// </remarks>
    public class ExtensiblePropertyContainer
        : IExtensiblePropertyContainer
    {
        /// <inheritdoc/>
        public void AddExtendedPropertyValue( string id, object value )
        {
            lock ( Items )
            {
                if( Items.TryGetValue( id, out object currentValue ) )
                {
                    if( currentValue != null && value != null && currentValue.GetType( ) != value.GetType( ) )
                    {
                        throw new ArgumentException( Resources.Cannot_change_type_of_an_extended_property_once_set, nameof( value ) );
                    }
                }

                Items[ id ] = value;
            }
        }

        /// <inheritdoc/>
        public bool TryGetExtendedPropertyValue<T>( string id, out T value )
        {
            value = default;
            object item;
            lock ( Items )
            {
                if( !Items.TryGetValue( id, out item ) )
                {
                    return false;
                }
            }

            if( !( item is T ) )
            {
                return false;
            }

            value = ( T )item;
            return true;
        }

        private readonly Dictionary<string, object> Items = new Dictionary<string, object>();
    }
}
