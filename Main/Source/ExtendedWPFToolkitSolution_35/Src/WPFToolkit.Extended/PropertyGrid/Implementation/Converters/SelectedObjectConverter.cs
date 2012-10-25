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
using System.Windows.Data;
using System.Globalization;
using System.ComponentModel;
using System.Windows;
using System.Reflection;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Converters
{
  public class SelectedObjectConverter : IValueConverter
  {
    private const string ValidParameterMessage = @"parameter must be one of the following strings: 'Type', 'TypeName'";
    #region IValueConverter Members

    public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
    {
      if( parameter == null )
        throw new ArgumentNullException( "parameter" );

      if( !( parameter is string ) )
        throw new ArgumentException( SelectedObjectConverter.ValidParameterMessage );

      if( this.CompareParam(parameter, "Type") )
      {
        return this.ConvertToType( value, culture );
      }
      else if( this.CompareParam( parameter, "TypeName" ) )
      {
        return this.ConvertToTypeName( value, culture );
      }
      else
      {
        throw new ArgumentException( SelectedObjectConverter.ValidParameterMessage );
      }
    }

    private bool CompareParam(object parameter, string parameterValue )
    {
      return string.Compare( ( string )parameter, parameterValue, true ) == 0;
    }

    private object ConvertToType( object value, CultureInfo culture )
    {
      return ( value != null )
        ? value.GetType()
        : null;
    }

    private object ConvertToTypeName( object value, CultureInfo culture )
    {
      if( value == null )
        return string.Empty;

      Type newType = value.GetType();

      DisplayNameAttribute displayNameAttribute = newType.GetCustomAttributes( false ).OfType<DisplayNameAttribute>().FirstOrDefault();

      return (displayNameAttribute == null)
        ? newType.Name 
        : displayNameAttribute.DisplayName;
    }

    public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}
