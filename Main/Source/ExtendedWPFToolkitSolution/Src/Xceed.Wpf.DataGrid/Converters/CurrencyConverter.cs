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
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows;

namespace Xceed.Wpf.DataGrid.Converters
{
  [ValueConversion( typeof( double ), typeof( string ) )]
  public class CurrencyConverter : IValueConverter
  {
    public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
    {
      if( ( value != null ) && ( !object.Equals( string.Empty, value ) ) )
      {
        Type valueType = value.GetType();

        // Only Decimal or Double values can be converted
        if( ( valueType.IsAssignableFrom( typeof( decimal ) ) == false ) && ( valueType.IsAssignableFrom( typeof( double ) ) == false ) )
        {
          return Binding.DoNothing;
        }

        try
        {
          // Convert the string value provided by an editor to a double before formatting. 
          double tempDouble = System.Convert.ToDouble( value, CultureInfo.CurrentCulture );
          return string.Format( CultureInfo.CurrentCulture, "{0:C}", tempDouble );
        }
        catch
        {
          return Binding.DoNothing;
        }
      }

      return string.Format( CultureInfo.CurrentCulture, "{0}", value );
    }

    public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
    {
      double result;

      if( double.TryParse( value as string, NumberStyles.Currency, CultureInfo.CurrentCulture, out result ) )
        return result;

      return Binding.DoNothing;
    }
  }
}
