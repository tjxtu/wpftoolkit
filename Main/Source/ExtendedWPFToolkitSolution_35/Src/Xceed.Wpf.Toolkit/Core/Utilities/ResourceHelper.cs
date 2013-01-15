﻿/************************************************************************

   Extended WPF Toolkit

   Copyright (C) 2010-2012 Xceed Software Inc.

   This program is provided to you under the terms of the Microsoft Public
   License (Ms-PL) as published at http://wpftoolkit.codeplex.com/license 

   For more features, controls, and fast professional support,
   pick up the Plus edition at http://xceed.com/wpf_toolkit

   Visit http://xceed.com and follow @datagrid on Twitter

  **********************************************************************/

using System.IO;
using System.Reflection;
using System.Resources;

namespace Xceed.Wpf.Toolkit.Core.Utilities
{
  internal class ResourceHelper
  {
    internal static Stream LoadResourceStream( Assembly assembly, string resId )
    {
      string basename = System.IO.Path.GetFileNameWithoutExtension( assembly.ManifestModule.Name ) + ".g";
      ResourceManager resourceManager = new ResourceManager( basename, assembly );

      // resource names are lower case and contain only forward slashes
      resId = resId.ToLower();
      resId = resId.Replace( '\\', '/' );
      return ( resourceManager.GetObject( resId ) as Stream );
    }
  }
}
