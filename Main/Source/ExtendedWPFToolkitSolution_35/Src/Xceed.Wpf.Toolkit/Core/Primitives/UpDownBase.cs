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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Xceed.Wpf.Toolkit.Core;
using Xceed.Wpf.Toolkit.Core.Input;
using System.Globalization;

namespace Xceed.Wpf.Toolkit.Primitives
{
  [TemplatePart( Name = PART_TextBox, Type = typeof( TextBox ) )]
  [TemplatePart( Name = PART_Spinner, Type = typeof( Spinner ) )]
  public abstract class UpDownBase<T> : InputBase, IValidateInput
  {
    #region Members

    /// <summary>
    /// Name constant for Text template part.
    /// </summary>
    internal const string PART_TextBox = "PART_TextBox";

    /// <summary>
    /// Name constant for Spinner template part.
    /// </summary>
    internal const string PART_Spinner = "PART_Spinner";

    /// <summary>
    /// Flags if the Text and Value properties are in the process of being sync'd
    /// </summary>
    private bool _isSyncingTextAndValueProperties;
    private bool _isTextChangedFromUI;

    #endregion //Members

    #region Properties

    protected Spinner Spinner
    {
      get;
      private set;
    }
    protected TextBox TextBox
    {
      get;
      private set;
    }

    #region AllowSpin

    public static readonly DependencyProperty AllowSpinProperty = DependencyProperty.Register( "AllowSpin", typeof( bool ), typeof( UpDownBase<T> ), new UIPropertyMetadata( true ) );
    public bool AllowSpin
    {
      get
      {
        return ( bool )GetValue( AllowSpinProperty );
      }
      set
      {
        SetValue( AllowSpinProperty, value );
      }
    }

    #endregion //AllowSpin

    #region DefaultValue

    public static readonly DependencyProperty DefaultValueProperty = DependencyProperty.Register( "DefaultValue", typeof( T ), typeof( UpDownBase<T> ), new UIPropertyMetadata( default( T ), OnDefaultValueChanged ) );
    public T DefaultValue
    {
      get
      {
        return ( T )GetValue( DefaultValueProperty );
      }
      set
      {
        SetValue( DefaultValueProperty, value );
      }
    }

    private static void OnDefaultValueChanged( DependencyObject source, DependencyPropertyChangedEventArgs args )
    {
      ( ( UpDownBase<T> )source ).OnDefaultValueChanged( ( T )args.OldValue, ( T )args.NewValue );
    }

    private void OnDefaultValueChanged( T oldValue, T newValue )
    {
      if( this.IsInitialized && string.IsNullOrEmpty( Text ))
      {
        this.SyncTextAndValueProperties( true, Text );
      }
    }

    #endregion //DefaultValue

    #region MouseWheelActiveOnFocus

    public static readonly DependencyProperty MouseWheelActiveOnFocusProperty = DependencyProperty.Register( "MouseWheelActiveOnFocus", typeof( bool ), typeof( UpDownBase<T> ), new UIPropertyMetadata( true ) );
    public bool MouseWheelActiveOnFocus
    {
      get
      {
        return ( bool )GetValue( MouseWheelActiveOnFocusProperty );
      }
      set
      {
        SetValue( MouseWheelActiveOnFocusProperty, value );
      }
    }

    #endregion //MouseWheelActiveOnFocus

    #region ShowButtonSpinner

    public static readonly DependencyProperty ShowButtonSpinnerProperty = DependencyProperty.Register( "ShowButtonSpinner", typeof( bool ), typeof( UpDownBase<T> ), new UIPropertyMetadata( true ) );
    public bool ShowButtonSpinner
    {
      get
      {
        return ( bool )GetValue( ShowButtonSpinnerProperty );
      }
      set
      {
        SetValue( ShowButtonSpinnerProperty, value );
      }
    }

    #endregion //ShowButtonSpinner

    #region Value

    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register( "Value", typeof( T ), typeof( UpDownBase<T> ), new FrameworkPropertyMetadata( default( T ), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged, OnCoerceValue, false, UpdateSourceTrigger.PropertyChanged ) );
    public T Value
    {
      get
      {
        return ( T )GetValue( ValueProperty );
      }
      set
      {
        SetValue( ValueProperty, value );
      }
    }

    private static object OnCoerceValue( DependencyObject o, object basevalue )
    {
      return ( ( UpDownBase<T> )o ).OnCoerceValue( basevalue );
    }

    protected virtual object OnCoerceValue( object newValue )
    {
      return newValue;
    }

    private static void OnValueChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
    {
      UpDownBase<T> upDownBase = o as UpDownBase<T>;
      if( upDownBase != null )
        upDownBase.OnValueChanged( ( T )e.OldValue, ( T )e.NewValue );
    }

    protected virtual void OnValueChanged( T oldValue, T newValue )
    {
      if( this.IsInitialized )
      {
        SyncTextAndValueProperties( false, null );
      }

      SetValidSpinDirection();

      RoutedPropertyChangedEventArgs<object> args = new RoutedPropertyChangedEventArgs<object>( oldValue, newValue );
      args.RoutedEvent = ValueChangedEvent;
      RaiseEvent( args );
    }

    #endregion //Value

    #endregion //Properties

    #region Constructors

    internal UpDownBase()
    {
    }

    #endregion //Constructors

    #region Base Class Overrides

    protected override void OnAccessKey( AccessKeyEventArgs e )
    {
      if( TextBox != null )
        TextBox.Focus();

      base.OnAccessKey( e );
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();

      TextBox = GetTemplateChild( PART_TextBox ) as TextBox;
      if( TextBox != null )
      {
        TextBox.Text = Text;
        TextBox.LostFocus += new RoutedEventHandler( TextBox_LostFocus );
        TextBox.TextChanged += new TextChangedEventHandler( TextBox_TextChanged );
      }

      if( Spinner != null )
        Spinner.Spin -= OnSpinnerSpin;

      Spinner = GetTemplateChild( PART_Spinner ) as Spinner;

      if( Spinner != null )
        Spinner.Spin += OnSpinnerSpin;

      SetValidSpinDirection();
    }

    protected override void OnGotFocus( RoutedEventArgs e )
    {
      if( TextBox != null )
        TextBox.Focus();
    }

    protected override void OnPreviewKeyDown( KeyEventArgs e )
    {
      switch( e.Key )
      {
        case Key.Up:
          {
            if( AllowSpin && !IsReadOnly )
              DoIncrement();
            e.Handled = true;
            break;
          }
        case Key.Down:
          {
            if( AllowSpin && !IsReadOnly )
              DoDecrement();
            e.Handled = true;
            break;
          }
      }
    }

    protected override void OnKeyDown( KeyEventArgs e )
    {
      switch( e.Key )
      {
        case Key.Enter:
          {
            // Commit Text on "Enter" to raise Error event 
            bool commitSuccess = CommitInput();
            //Only handle if an exception is detected (Commit fails)
            e.Handled = !commitSuccess;
            break;
          }
      }
    }

    protected override void OnMouseWheel( MouseWheelEventArgs e )
    {
      base.OnMouseWheel( e );

      if( !e.Handled && AllowSpin && !IsReadOnly && ( TextBox.IsFocused && MouseWheelActiveOnFocus ) )
      {
        if( e.Delta < 0 )
        {
          DoDecrement();
        }
        else if( 0 < e.Delta )
        {
          DoIncrement();
        }

        e.Handled = true;
      }
    }

    protected override void OnTextChanged( string oldValue, string newValue )
    {
      if( this.IsInitialized )
      {
        SyncTextAndValueProperties( true, Text );
      }
    }

    protected override void OnCultureInfoChanged( CultureInfo oldValue, CultureInfo newValue )
    {
      if( IsInitialized )
      {
        SyncTextAndValueProperties( false, null );
      }
    }

    protected override void OnReadOnlyChanged( bool oldValue, bool newValue )
    {
      SetValidSpinDirection();
    }

    #endregion //Base Class Overrides

    #region Event Handlers

    private void OnSpinnerSpin( object sender, SpinEventArgs e )
    {
      if( AllowSpin && !IsReadOnly )
        OnSpin( e );
    }

    #endregion //Event Handlers

    #region Events

    public event InputValidationErrorEventHandler InputValidationError;

    #region ValueChanged Event

    //Due to a bug in Visual Studio, you cannot create event handlers for generic T args in XAML, so I have to use object instead.
    public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent( "ValueChanged", RoutingStrategy.Bubble, typeof( RoutedPropertyChangedEventHandler<object> ), typeof( UpDownBase<T> ) );
    public event RoutedPropertyChangedEventHandler<object> ValueChanged
    {
      add
      {
        AddHandler( ValueChangedEvent, value );
      }
      remove
      {
        RemoveHandler( ValueChangedEvent, value );
      }
    }

    #endregion

    #endregion //Events

    #region Methods

    protected virtual void OnSpin( SpinEventArgs e )
    {
      if( e == null )
        throw new ArgumentNullException( "e" );

      if( e.Direction == SpinDirection.Increase )
        DoIncrement();
      else
        DoDecrement();
    }

    protected override void OnInitialized( EventArgs e )
    {
      base.OnInitialized( e );
      // When both Value and Text are initialized, Value has priority.
      bool updateValueFromText = ( this.Value == null );
      this.SyncTextAndValueProperties( updateValueFromText, Text );
    }

    /// <summary>
    /// Performs an increment if conditions allow it.
    /// </summary>
    private void DoDecrement()
    {
      if( Spinner == null || ( Spinner.ValidSpinDirection & ValidSpinDirections.Decrease ) == ValidSpinDirections.Decrease )
      {
        OnDecrement();
      }
    }

    /// <summary>
    /// Performs a decrement if conditions allow it.
    /// </summary>
    private void DoIncrement()
    {
      if( Spinner == null || ( Spinner.ValidSpinDirection & ValidSpinDirections.Increase ) == ValidSpinDirections.Increase )
      {
        OnIncrement();
      }
    }

    private void TextBox_TextChanged( object sender, TextChangedEventArgs e )
    {
      try
      {
        _isTextChangedFromUI = true;
        Text = ( ( TextBox )sender ).Text;
      }
      finally
      {
        _isTextChangedFromUI = false;
      }
    }

    private void TextBox_LostFocus( object sender, RoutedEventArgs e )
    {
      CommitInput();
    }

    private void RaiseInputValidationError( Exception e )
    {
      if( InputValidationError != null )
      {
        InputValidationErrorEventArgs args = new InputValidationErrorEventArgs( e );
        InputValidationError( this, args );
        if( args.ThrowException )
        {
          throw args.Exception;
        }
      }
    }

    public bool CommitInput()
    {
      return this.SyncTextAndValueProperties( true, Text );
    }

    protected bool SyncTextAndValueProperties(bool updateValueFromText, string text )
    {
      if( _isSyncingTextAndValueProperties )
        return true;

      _isSyncingTextAndValueProperties = true;
      bool parsedTextIsValid = true;
      try
      {
        if( updateValueFromText )
        {
          if( string.IsNullOrEmpty( text ) )
          {
            // An empty input set the value to the default value.
            Value = this.DefaultValue;
          }
          else
          {
            try
            {
              Value = this.ConvertTextToValue( text );
            }
            catch( Exception e )
            {
              parsedTextIsValid = false;

              // From the UI, just allow any input.
              if( !_isTextChangedFromUI )
              {
                // This call may throw an exception. 
                // See RaiseInputValidationError() implementation.
                this.RaiseInputValidationError( e );
              }
            }
          }
        }

        // Do not touch the ongoing text input from user.
        if(!_isTextChangedFromUI)
        {
          // Don't replace the empty Text with the non-empty representation of DefaultValue.
          bool shouldKeepEmpty = string.IsNullOrEmpty( Text ) && object.Equals( Value, DefaultValue );
          if( !shouldKeepEmpty )
          {
            Text = ConvertValueToText();
          }

          // Sync Text and textBox
          if( TextBox != null )
            TextBox.Text = Text;
        }

        if( _isTextChangedFromUI && !parsedTextIsValid )
        {
          // Text input was made from the user and the text
          // repesent an invalid value. Make the spinner "disable" 
          // in these case.
          if( Spinner != null )
          {
            Spinner.ValidSpinDirection = ValidSpinDirections.None;
          }
        }
        else
        {
          this.SetValidSpinDirection();
        }
      }
      finally
      {
        _isSyncingTextAndValueProperties = false;
      }
      return parsedTextIsValid;
    }

    #region Abstract

    /// <summary>
    /// Converts the formatted text to a value.
    /// </summary>
    protected abstract T ConvertTextToValue( string text );

    /// <summary>
    /// Converts the value to formatted text.
    /// </summary>
    /// <returns></returns>
    protected abstract string ConvertValueToText();

    /// <summary>
    /// Called by OnSpin when the spin direction is SpinDirection.Increase.
    /// </summary>
    protected abstract void OnIncrement();

    /// <summary>
    /// Called by OnSpin when the spin direction is SpinDirection.Descrease.
    /// </summary>
    protected abstract void OnDecrement();

    /// <summary>
    /// Sets the valid spin directions.
    /// </summary>
    protected abstract void SetValidSpinDirection();

    #endregion //Abstract

    #endregion //Methods
  }
}
