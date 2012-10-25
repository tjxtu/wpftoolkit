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

namespace Xceed.Wpf.DataGrid
{
  internal class VirtualizingCellCollectionChangedEventManager : WeakEventManager
  {
    internal static void AddListener( VirtualizingCellCollection source, IWeakEventListener listener )
    {
      VirtualizingCellCollectionChangedEventManager.CurrentManager.ProtectedAddListener( source, listener );
    }

    internal static void RemoveListener( VirtualizingCellCollection source, IWeakEventListener listener )
    {
      VirtualizingCellCollectionChangedEventManager.CurrentManager.ProtectedRemoveListener( source, listener );
    }

    protected override void StartListening( object source )
    {
      VirtualizingCellCollection virtualizingCellCollection = source as VirtualizingCellCollection;
      if( source == null )
      {
        throw new InvalidOperationException( "An attempt was made to use a source other than a VirtualizingCellCollection" );
      }

      virtualizingCellCollection.CollectionChanged += this.OnVirtualizingCellCollectionChanged;
    }

    protected override void StopListening( object source )
    {
      VirtualizingCellCollection virtualizingCellCollection = source as VirtualizingCellCollection;
      if( source == null )
      {
        throw new InvalidOperationException( "An attempt was made to use a source other than a VirtualizingCellCollection" );
      }

      virtualizingCellCollection.CollectionChanged -= this.OnVirtualizingCellCollectionChanged;
    }

    private static VirtualizingCellCollectionChangedEventManager CurrentManager
    {
      get
      {
        Type managerType = typeof( VirtualizingCellCollectionChangedEventManager );
        VirtualizingCellCollectionChangedEventManager currentManager = WeakEventManager.GetCurrentManager( managerType ) as VirtualizingCellCollectionChangedEventManager;

        if( currentManager == null )
        {
          currentManager = new VirtualizingCellCollectionChangedEventManager();
          WeakEventManager.SetCurrentManager( managerType, currentManager );
        }

        return currentManager;
      }
    }

    private void OnVirtualizingCellCollectionChanged( object sender, EventArgs args )
    {
      this.DeliverEvent( sender, args );
    }
  }
}
