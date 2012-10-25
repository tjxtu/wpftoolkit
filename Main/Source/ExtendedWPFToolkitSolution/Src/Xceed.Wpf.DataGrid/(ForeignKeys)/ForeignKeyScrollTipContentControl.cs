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
using System.Windows;
using System.Windows.Data;

namespace Xceed.Wpf.DataGrid
{
  internal class ForeignKeyScrollTipContentControl : ForeignKeyContentControl
  {
    #region Static Fields

    private static Binding MainColumnBinding; // = null;

    #endregion

    #region Constructors

    static ForeignKeyScrollTipContentControl()
    {
      ForeignKeyScrollTipContentControl.MainColumnBinding = new Binding();
      ForeignKeyScrollTipContentControl.MainColumnBinding.Mode = BindingMode.OneWay;
      ForeignKeyScrollTipContentControl.MainColumnBinding.RelativeSource = new RelativeSource( RelativeSourceMode.FindAncestor, typeof( ScrollTip ), 1 );
      ForeignKeyScrollTipContentControl.MainColumnBinding.Path = new PropertyPath( "(0).Columns.MainColumn", DataGridControl.DataGridContextProperty );
    }

    public ForeignKeyScrollTipContentControl()
    {
      BindingOperations.SetBinding(
        this,
        ForeignKeyScrollTipContentControl.MainColumnProperty,
        ForeignKeyScrollTipContentControl.MainColumnBinding );

      // Ensure this control is not Focusable, it only displays converted value between
      // ID and ForeignKey
      this.Focusable = false;
    }

    #endregion

    #region MainColumn Property

    public static readonly DependencyProperty MainColumnProperty = DependencyProperty.Register(
      "MainColumn",
      typeof( ColumnBase ),
      typeof( ForeignKeyScrollTipContentControl ),
      new FrameworkPropertyMetadata(
        null,
        new PropertyChangedCallback( ForeignKeyScrollTipContentControl.OnMainColumnChanged ) ) );

    public ColumnBase MainColumn
    {
      get
      {
        return ( ColumnBase )this.GetValue( ForeignKeyScrollTipContentControl.MainColumnProperty );
      }
      set
      {
        this.SetValue( ForeignKeyScrollTipContentControl.MainColumnProperty, value );
      }
    }

    private static void OnMainColumnChanged( DependencyObject sender, DependencyPropertyChangedEventArgs e )
    {
      ForeignKeyScrollTipContentControl contentControl = sender as ForeignKeyScrollTipContentControl;

      if( contentControl != null )
      {
        // Reset ContentTemplate/Selector when the MainColum changes
        // since it is set locally
        contentControl.ContentTemplate = null;
        contentControl.ContentTemplateSelector = null;

        contentControl.UpdateKeyBinding();
        contentControl.UpdateContentTemplate();
      }
    }

    #endregion

    #region Private Methods

    private void UpdateContentTemplate()
    {
      Column mainColumn = this.MainColumn as Column;

      if( mainColumn == null )
        return;

      DataGridContext dataGridContext = DataGridControl.GetDataGridContext( this );

      Xceed.Wpf.DataGrid.Views.UIViewBase uiViewBase = null;

      if( dataGridContext != null )
      {
        DataGridControl dataGridControl = dataGridContext.DataGridControl;

        if( dataGridControl != null )
        {
          uiViewBase = dataGridControl.GetView() as Xceed.Wpf.DataGrid.Views.UIViewBase;
        }
      }

      if( uiViewBase != null )
      {
        if( uiViewBase.ScrollTipContentTemplate != null )
        {
          this.ContentTemplate = uiViewBase.ScrollTipContentTemplate;
        }
        else
        {
          this.ContentTemplateSelector = uiViewBase.ScrollTipContentTemplateSelector;
        }
      }

      if( ( this.ContentTemplate == null ) && ( this.ContentTemplateSelector == null ) )
      {
        if( mainColumn.CellContentTemplate != null )
        {
          this.ContentTemplate = mainColumn.CellContentTemplate;
        }
        else
        {
          this.ContentTemplateSelector = mainColumn.CellContentTemplateSelector;
        }
      }
    }

    private void UpdateKeyBinding()
    {
      Column mainColumn = this.MainColumn as Column;

      if( mainColumn == null )
        return;

      // Disable warning for DisplayMemberBinding when internaly used
#pragma warning disable 618
      BindingBase displayMemberBinding = mainColumn.DisplayMemberBinding;

      if( displayMemberBinding != null )
      {
        // Set the DisplayMemberBinding to the KeyProperty
        this.SetBinding( ForeignKeyScrollTipContentControl.KeyProperty, displayMemberBinding );
      }
#pragma warning restore 618
    }

    #endregion
  }
}
