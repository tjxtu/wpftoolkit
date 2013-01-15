﻿/************************************************************************

   Extended WPF Toolkit

   Copyright (C) 2010-2012 Xceed Software Inc.

   This program is provided to you under the terms of the Microsoft Public
   License (Ms-PL) as published at http://wpftoolkit.codeplex.com/license 

   For more features, controls, and fast professional support,
   pick up the Plus edition at http://xceed.com/wpf_toolkit

   Visit http://xceed.com and follow @datagrid on Twitter

  **********************************************************************/

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Xceed.Wpf.Toolkit.Core.Converters
{
  public class RoundedValueConverter : IValueConverter
  {
    #region Precision Property

    public int Precision
    {
      get
      {
        return _precision;
      }
      set
      {
        _precision = value;
      }
    }

    private int _precision = 0;

    #endregion

    public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
    {
      if( value is double )
      {
        return Math.Round( ( double )value, _precision );
      }
      else if( value is Point )
      {
        return new Point( Math.Round( ( ( Point )value ).X, _precision ), Math.Round( ( ( Point )value ).Y, _precision ) );
      }
      else
      {
        return value;
      }
    }

    public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
    {
      return value;
    }
  }
}
