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
using System.Collections.ObjectModel;
using System.Text;

namespace Xceed.Wpf.DataGrid
{
  // We use an intermediate liste to always acces the source with the index
  // and never directly by keeping a variable that contains the source item directly.
  // ( We do that to be able to work correctly with a DataView has source )
  //
  // RawItem is always pointing at the same item.
  // The dataIndex may be changing to always represent the same item if the item is moving in the source.
  internal class RawItem
  {
    public RawItem( int dataIndex, object dataItem )
    {
      m_dataIndex = dataIndex;
      m_dataItem = dataItem;
      m_sortedIndex = -1;
    }

    public int Index
    {
      get
      {
        return m_dataIndex;
      }
    }

    public int SortedIndex
    {
      get
      {
        return m_sortedIndex;
      }
    }

    public object DataItem
    {
      get
      {
        return m_dataItem;
      }
    }

    public DataGridCollectionViewGroup ParentGroup
    {
      get
      {
        return m_parentGroup;
      }
    }

    public int GetGlobalSortedIndex()
    {
      if( m_parentGroup == null )
        return -1;

      return m_sortedIndex + m_parentGroup.GetFirstRawItemGlobalSortedIndex();
    }

    internal void SetIndex( int newIndex )
    {
      m_dataIndex = newIndex;
    }

    internal void SetParentGroup( DataGridCollectionViewGroup parentGroup )
    {
      m_parentGroup = parentGroup;
    }

    internal void SetSortedIndex( int newIndex )
    {
      m_sortedIndex = newIndex;
    }

    internal void SetDataItem( object dataItem )
    {
      m_dataItem = dataItem;
    }

    private object m_dataItem;
    private int m_dataIndex;
    private int m_sortedIndex;
    private DataGridCollectionViewGroup m_parentGroup;
  }
}
