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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace Xceed.Wpf.DataGrid
{
  public class QueryGroupsEventArgs : EventArgs
  {
    #region CONSTRUCTORS

    internal QueryGroupsEventArgs( 
      DataGridVirtualizingCollectionView collectionView, 
      DataGridVirtualizingCollectionViewGroup parentGroup, 
      GroupDescription childGroupDescription )
    {
      m_dataGridVirtualizingCollectionView = collectionView;
      m_readonlyGroupPath = parentGroup.GroupPath.AsReadOnly();
      m_childGroupDescription = childGroupDescription;
      this.ChildGroupPropertyName = DataGridCollectionViewBase.GetPropertyNameFromGroupDescription( childGroupDescription );

      m_sortDirection = SortDirection.None;

      SortDescriptionCollection sortDescriptions = m_dataGridVirtualizingCollectionView.SortDescriptions;

      int sortDescriptionCount = ( sortDescriptions == null ) ? 0 : sortDescriptions.Count;

      for( int i = 0; i < sortDescriptions.Count ; i++ )
      {
        SortDescription sortDescription = sortDescriptions[ i ];

        if( string.Equals( sortDescription.PropertyName, this.ChildGroupPropertyName ) )
        {
          m_sortDirection = ( sortDescription.Direction == ListSortDirection.Ascending ) ? SortDirection.Ascending : SortDirection.Descending;
          break;
        }
      }

      m_childGroupNameCountPairs = new List<GroupNameCountPair>();
    }

    #endregion CONSTRUCTORS

    #region CollectionView PROPERTY

    public DataGridVirtualizingCollectionView CollectionView
    {
      get
      {
        return m_dataGridVirtualizingCollectionView;
      }
    }

    #endregion CollectionView PROPERTY

    #region ChildGroupDescription PROPERTY

    public GroupDescription ChildGroupDescription
    {
      get
      {
        return m_childGroupDescription;
      }
    }

    #endregion ChildGroupDescription PROPERTY

    #region ChildGroupPropertyName PROPERTY

    public string ChildGroupPropertyName
    {
      get;
      private set;
    }

    #endregion ChildGroupPropertyName PROPERTY

    #region ChildSortDirection PROPERTY

    public SortDirection ChildSortDirection
    {
      get
      {
        return m_sortDirection;
      }
    }

    #endregion ChildSortDirection PROPERTY

    #region ChildGroupNameCountPairs PROPERTY

    public List<GroupNameCountPair> ChildGroupNameCountPairs
    {
      get
      {
        return m_childGroupNameCountPairs;
      }
    }

    #endregion ChildGroupNameCountPairs PROPERTY


    #region GroupPath PROPERTY

    public ReadOnlyCollection<DataGridGroupInfo> GroupPath
    {
      get
      {
        return m_readonlyGroupPath;
      }
    }

    #endregion GroupPath PROPERTY


    #region PRIVATE FIELDS

    private DataGridVirtualizingCollectionView m_dataGridVirtualizingCollectionView;
    private GroupDescription m_childGroupDescription;
    private SortDirection m_sortDirection;
    private ReadOnlyCollection<DataGridGroupInfo> m_readonlyGroupPath;

    private List<GroupNameCountPair> m_childGroupNameCountPairs;

    #endregion PRIVATE FIELDS
  }

}
