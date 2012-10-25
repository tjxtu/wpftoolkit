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

namespace Xceed.Wpf.DataGrid
{
  internal partial class DetailGeneratorNode : GeneratorNode
  {
    public DetailGeneratorNode( DataGridContext context, CustomItemContainerGenerator generator )
      : base( null )
    {
      if( generator == null )
        throw new ArgumentNullException( "generator" );

      if( context == null )
        throw new ArgumentNullException( "context" );

      m_generator = generator;
      m_context = context;
      m_itemCount = generator.ItemCount;
    }

    public CustomItemContainerGenerator DetailGenerator
    {
      get
      {
        return m_generator;
      }
    }

    public DataGridContext DetailContext
    {
      get
      {
        return m_context;
      }
    }

    internal override int ItemCount
    {
      get
      {
        return m_itemCount;
      }
    }

    internal override void CleanGeneratorNode()
    {
      base.CleanGeneratorNode();

      m_generator.CleanupGenerator( true );
      m_context.CleanDataGridContext();

      m_generator = null;
    }

    public void UpdateItemCount()
    {
      m_itemCount = m_generator.ItemCount;
    }

    private CustomItemContainerGenerator m_generator;
    private DataGridContext m_context;
    private int m_itemCount;
  }
}
