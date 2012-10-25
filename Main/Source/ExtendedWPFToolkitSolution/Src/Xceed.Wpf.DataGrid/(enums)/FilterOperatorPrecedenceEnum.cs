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

namespace Xceed.Wpf.DataGrid
{
  /// <summary>
  /// The precedence value of an expression operator determining the order in which the
  /// parameters of the expression will be consumed. A smaller value means a greater
  /// priority. The values closely follows the standard operator precedences. This type
  /// should not be confused with the FilterTokenPriority enum.
  /// </summary>
  internal enum FilterOperatorPrecedence
  {
    /// <summary>
    /// 0: RelationalOperator (=, &gt;, &lt;, ...)
    /// </summary>
    RelationalOperator,
    /// <summary>
    /// 1: Not operator
    /// </summary>
    NotOperator,
    /// <summary>
    /// 2: And operator
    /// </summary>
    AndOperator,
    /// <summary>
    /// 3: Or operator
    /// </summary>
    OrOperator,
    /// <summary>
    /// 4: Default priority
    /// </summary>
    Default
  }
}
