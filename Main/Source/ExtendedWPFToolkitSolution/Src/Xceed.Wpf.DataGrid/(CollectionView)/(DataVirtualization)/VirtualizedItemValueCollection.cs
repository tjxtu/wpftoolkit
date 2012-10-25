﻿/************************************************************************

   Extended WPF Toolkit

   Copyright (C) 2010-2012 Xceed Software Inc.

   This program is provided to you under the terms of the Microsoft Public
   License (Ms-PL) as published at http://wpftoolkit.codeplex.com/license 

   This program can be provided to you by Xceed Software Inc. under a
   proprietary commercial license agreement for use in non-Open Source
   projects. The commercial version of Extended WPF Toolkit also includes
   priority technical support, commercial updates, and many additional 
   useful WPF controls if you license Xceed Business Suite for WPF.

   Visit http://xceed.com and follow @datagrid on Twitter.

  **********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Xceed.Wpf.DataGrid
{
  public class VirtualizedItemValueCollection : ICollection
  {
    internal VirtualizedItemValueCollection( string[] names, object[] values )
    {
      if( names.Length != values.Length )
        throw new ArgumentException( "Names and Values must be of the same length." );

      m_nameValueDictionary = new Dictionary<string, object>( names.Length );
      m_valueList = new List<object>( values.Length );

      for( int i = 0; i < names.Length; i++ )
      {
        object value = values[ i ];
        m_nameValueDictionary.Add( names[ i ], value );
        m_valueList.Add( value );
      }

    }

    public object this[ int index ]
    {
      get
      {
        if( ( index < 0 ) || ( index >= m_valueList.Count ) )
          throw new ArgumentOutOfRangeException( "index", index, "The index must be greater than or equal to zero and less than count." );

        return m_valueList[ index ];
      }
    }

    public object this[ string name ]
    {
      get
      {
        if( name == null )
          throw new ArgumentNullException( "name" );

        if( name == string.Empty )
          throw new ArgumentException( "name" );

        object val;

        if( !m_nameValueDictionary.TryGetValue( name, out val ) )
          throw new ArgumentException( "No value could be found for the specified property name." );

        return val;
      }
    }

    public int Count
    {
      get
      {
        return m_valueList.Count;
      }
    }

    #region PRIVATE FIELDS

    private Dictionary<string, object> m_nameValueDictionary;
    private List<object> m_valueList;

    #endregion PRIVATE FIELDS


    #region ICollection Members

    public void CopyTo( Array array, int index )
    {
      if( array == null )
        throw new ArgumentNullException( "array" );

      if( array.Rank != 1 )
        throw new NotSupportedException( "Multi-dimensional arrays are not supported." );

      if( index < 0 )
        throw new ArgumentOutOfRangeException( "index", index, "index must be greater than or equal to zero." );

      int count = this.Count;

      if( ( array.Length - index ) < count )
        throw new ArgumentException( "Invalid offset position." );

      if( array is object[] )
      {
        object[] objects = ( object[] )array;

        for( int i = 0; i < count; i++ )
        {
          objects[ i + index ] = m_valueList[ i ];
        }
      }
      else
      {
        for( int i = 0; i < count; i++ )
        {
          array.SetValue( m_valueList[ i ], ( i + index ) );
        }
      }
    }

    public bool IsSynchronized
    {
      get 
      { 
        return false; 
      }
    }

    public object SyncRoot
    {
      get 
      {
        return this;
      }
    }

    #endregion

    #region IEnumerable Members

    public IEnumerator GetEnumerator()
    {
      return m_valueList.GetEnumerator();
    }

    #endregion
  }
}
