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

namespace Xceed.Wpf.DataGrid
{
  internal class ExpansionStateChangedEventArgs : EventArgs
  {
    public ExpansionStateChangedEventArgs( bool newExpansionState, int nodeIndexOffset, int itemCount )
    {
      m_expansionState = newExpansionState;
      m_offset = nodeIndexOffset;
      m_count = itemCount;
    }

    public bool NewExpansionState
    {
      get
      {
        return m_expansionState;
      }
    }

    public int IndexOffset
    {
      get
      {
        return m_offset;
      }
    }

    public int Count
    {
      get
      {
        return m_count;
      }
    }

    private readonly bool m_expansionState;
    private readonly int m_offset;
    private readonly int m_count;
  }
}
