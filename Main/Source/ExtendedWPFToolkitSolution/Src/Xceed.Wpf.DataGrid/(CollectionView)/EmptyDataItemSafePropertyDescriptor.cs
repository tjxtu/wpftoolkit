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
using System.ComponentModel;

namespace Xceed.Wpf.DataGrid
{
  internal class EmptyDataItemSafePropertyDescriptor : PropertyDescriptor
  {
    #region STATIC MEMBERS

    private const string PropertyName = "__EmptyDataItemPropertyDescriptor";

    private static readonly Attribute[] EmptyAttributeArray = new Attribute[ 0 ];

    #endregion STATIC MEMBERS

    #region SINGLETON

    internal static EmptyDataItemSafePropertyDescriptor Singleton
    {
      get
      {
        if( ms_Singleton == null )
          ms_Singleton = new EmptyDataItemSafePropertyDescriptor( EmptyDataItemSafePropertyDescriptor.PropertyName );

        return ms_Singleton;
      }
    }

    private static EmptyDataItemSafePropertyDescriptor ms_Singleton;

    #endregion SINGLETON

    #region CONSTRUCTORS

    public EmptyDataItemSafePropertyDescriptor( string name )
      : base( name, EmptyDataItemSafePropertyDescriptor.EmptyAttributeArray )
    {
    }

    #endregion CONSTRUCTORS

    public override bool CanResetValue( object component )
    {
      return false;
    }

    public override Type ComponentType
    {
      get { return typeof( object ); }
    }

    public override object GetValue( object component )
    {
      if( component is EmptyDataItem )
        return null;

      return component;
    }

    public override bool IsReadOnly
    {
      get { return true; }
    }

    public override Type PropertyType
    {
      get { return typeof( object ); }
    }

    public override void ResetValue( object component )
    {
      throw new NotImplementedException();
    }

    public override void SetValue( object component, object value )
    {
      throw new NotImplementedException();
    }

    public override bool ShouldSerializeValue( object component )
    {
      return false;
    }
  }
}
