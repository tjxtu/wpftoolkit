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
using System.Collections;

namespace Xceed.Utils.Data
{
  internal sealed class ObjectDataStore : DataStore
  {
    public ObjectDataStore( int initialCapacity )
    {
      this.SetCapacity( initialCapacity );
    }

    public override object GetData( int index )
    {
      return m_values[ index ];
    }

    public override void SetData( int index, object data )
    {
      m_values[ index ] = data;
    }

    public override int Compare( int xRecordIndex, int yRecordIndex )
    {
      object xData = m_values[ xRecordIndex ];
      object yData = m_values[ yRecordIndex ];

      return ObjectDataStore.CompareData( xData, yData );
    }

    public override void SetCapacity( int capacity )
    {
      object[] newValues = new object[ capacity ];

      if( m_values != null )
        Array.Copy( m_values, 0, newValues, 0, System.Math.Min( capacity, m_values.Length ) );

      m_values = newValues;
    }

    public static int CompareData( object xData, object yData )
    {
      // Code in there should be indentical to ObjectComparer.Compare

      if( ( xData == null ) || ( xData == DBNull.Value ) )
      {
        if( ( yData != null ) && ( yData != DBNull.Value ) )
        {
          return -1;
        }
      }
      else
      {
        if( ( yData == null ) || ( yData == DBNull.Value ) )
          return 1;

        IComparable xDataComparer = xData as IComparable;

        if( xDataComparer != null )
          return xDataComparer.CompareTo( yData );
      }

      return 0;
    }

    private object[] m_values;
  }
}
