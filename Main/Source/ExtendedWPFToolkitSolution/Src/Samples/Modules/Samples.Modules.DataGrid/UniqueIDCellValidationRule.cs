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
using Xceed.Wpf.DataGrid.ValidationRules;
using System.Windows.Controls;
using System.Globalization;
using Xceed.Wpf.DataGrid;
using System.Data;

namespace Samples.Modules.DataGrid
{
  public class UniqueIDCellValidationRule : CellValidationRule
  {
    public UniqueIDCellValidationRule()
    {
    }

    public override ValidationResult Validate( object value, CultureInfo culture, CellValidationContext context )
    {
      // Get the DataItem from the context and cast it to a DataRow
      DataRowView dataRowView = context.DataItem as DataRowView;

      // Convert the value to a long to make sure it is numerical.
      // When the value is not numerical, then an InvalidFormatException will be thrown.
      // We let it pass unhandled to demonstrate that an exception can be thrown when validating
      // and the grid will handle it nicely.
      long id = Convert.ToInt64( value, CultureInfo.CurrentCulture );

      // Try to find another row with the same ID
      System.Data.DataRow[] existingRows = dataRowView.Row.Table.Select( context.Cell.FieldName + "=" + id.ToString( CultureInfo.InvariantCulture ) );

      // If a row is found, we return an error
      if( ( existingRows.Length != 0 ) && ( existingRows[ 0 ] != dataRowView.Row ) )
        return new ValidationResult( false, "The value must be unique" );

      // If no row was found, we return a ValidResult
      return ValidationResult.ValidResult;
    }
  }
}
