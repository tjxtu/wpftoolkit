﻿/************************************************************************

   Extended WPF Toolkit

   Copyright (C) 2010-2012 Xceed Software Inc.

   This program is provided to you under the terms of the Microsoft Reciprocal
   License (Ms-RL) as published at http://wpftoolkit.codeplex.com/license 

   This program can be provided to you by Xceed Software Inc. under a
   proprietary commercial license agreement for use in non-Open Source
   projects. The commercial version of Extended WPF Toolkit also includes
   priority technical support, commercial updates, and many additional 
   useful WPF controls if you license Xceed Business Suite for WPF.

   Visit http://xceed.com and follow @datagrid on Twitter.

  **********************************************************************/

using System;
using System.Windows;

namespace Xceed.Wpf.Toolkit
{
  public class IntegerUpDown : NumericUpDown<int?>
  {
    #region Constructors

    static IntegerUpDown()
    {
      DefaultStyleKeyProperty.OverrideMetadata( typeof( IntegerUpDown ), new FrameworkPropertyMetadata( typeof( IntegerUpDown ) ) );
      DefaultValueProperty.OverrideMetadata( typeof( IntegerUpDown ), new FrameworkPropertyMetadata( 0 ) );
      IncrementProperty.OverrideMetadata( typeof( IntegerUpDown ), new FrameworkPropertyMetadata( 1 ) );
      MaximumProperty.OverrideMetadata( typeof( IntegerUpDown ), new FrameworkPropertyMetadata( int.MaxValue ) );
      MinimumProperty.OverrideMetadata( typeof( IntegerUpDown ), new FrameworkPropertyMetadata( int.MinValue ) );
    }

    #endregion //Constructors

    #region Base Class Overrides

    protected override int? CoerceValue( int? value )
    {
      if( value < Minimum )
        return Minimum;
      else if( value > Maximum )
        return Maximum;
      else
        return value;
    }

    protected override void OnIncrement()
    {
      if( Value.HasValue )
        Value += Increment;
      else
        Value = DefaultValue;
    }

    protected override void OnDecrement()
    {
      if( Value.HasValue )
        Value -= Increment;
      else
        Value = DefaultValue;
    }

    protected override int? ConvertTextToValue( string text )
    {
      int? result = null;

      if( String.IsNullOrEmpty( text ) )
        return result;

      try
      {
        //don't know why someone would format an integer as %, but just in case they do.
        result = FormatString.Contains( "P" ) ? Decimal.ToInt32( ParsePercent( text, CultureInfo ) ) : ParseInt( text, CultureInfo );
        result = CoerceValue( result );
      }
      catch
      {
        Text = ConvertValueToText();
        return Value;
      }

      return result;
    }

    protected override string ConvertValueToText()
    {
      if( Value == null )
        return string.Empty;

      return Value.Value.ToString( FormatString, CultureInfo );
    }

    protected override void SetValidSpinDirection()
    {
      ValidSpinDirections validDirections = ValidSpinDirections.None;

      if( Value < Maximum || !Value.HasValue )
        validDirections = validDirections | ValidSpinDirections.Increase;

      if( Value > Minimum || !Value.HasValue )
        validDirections = validDirections | ValidSpinDirections.Decrease;

      if( Spinner != null )
        Spinner.ValidSpinDirection = validDirections;
    }

    protected override void ValidateValue( int? value )
    {
      if( value < Minimum )
        Value = Minimum;
      else if( value > Maximum )
        Value = Maximum;
    }

    #endregion //Base Class Overrides
  }
}
