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
using System.Windows.Controls;
using System.Windows;
using Xceed.Wpf.DataGrid.Views;
using System.Windows.Input;

namespace Xceed.Wpf.DataGrid
{
  public class GroupNavigationButton : Button
  {
    #region CONSTRUCTORS

    static GroupNavigationButton()
    {
      DefaultStyleKeyProperty.OverrideMetadata( 
        typeof( GroupNavigationButton ), 
        new FrameworkPropertyMetadata( new Markup.ThemeKey( typeof( TableView ), typeof( GroupNavigationButton ) ) ) );

      DataGridControl.ParentDataGridControlPropertyKey.OverrideMetadata(
        typeof( GroupNavigationButton ), new FrameworkPropertyMetadata( new PropertyChangedCallback( GroupNavigationButton.OnParentGridControlChanged ) ) );
    }

    public GroupNavigationButton()
    {
      this.Focusable = false;
    }

    #endregion CONSTRUCTORS

    #region NavigateToGroup Command

    public static readonly RoutedCommand NavigateToGroup =
      new RoutedCommand( "NavigateToGroup", typeof( GroupNavigationButton ) );

    #endregion NavigateToGroup Command

    #region PROTECTED METHODS

    protected internal virtual void PrepareDefaultStyleKey( Xceed.Wpf.DataGrid.Views.ViewBase view )
    {
      this.DefaultStyleKey = view.GetDefaultStyleKey( typeof( GroupNavigationButton ) );
    }

    #endregion PROTECTED METHODS

    #region PRIVATE METHODS

    private static void OnParentGridControlChanged( DependencyObject sender, DependencyPropertyChangedEventArgs e )
    {
      GroupNavigationButton button = sender as GroupNavigationButton;

      if( button == null )
        return;

      DataGridControl parentGridControl = e.NewValue as DataGridControl;

      if( parentGridControl == null )
        return;

      button.PrepareDefaultStyleKey( parentGridControl.GetView() );
    }

    #endregion PRIVATE METHODS
  }
}
