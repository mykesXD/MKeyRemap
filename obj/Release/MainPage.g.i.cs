﻿#pragma checksum "..\..\MainPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "DFA6CB13A6F4BCFB976AA5CC7A377B679650032E0BE8AA68EDE5CCB1C7A47FA3"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using KeyRemap;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace KeyRemap {
    
    
    /// <summary>
    /// MainPage
    /// </summary>
    public partial class MainPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal KeyRemap.MainPage MenuWindow;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas ContainerCanvas;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle CloseButton;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle MinimizeButton;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle BodyBackground;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid BodyContainer;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle AddButton;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle ImportButton;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle EditButton;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle DeleteButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/KeyRemap;component/mainpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MainPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.MenuWindow = ((KeyRemap.MainPage)(target));
            
            #line 10 "..\..\MainPage.xaml"
            this.MenuWindow.Loaded += new System.Windows.RoutedEventHandler(this.MenuWindow_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.ContainerCanvas = ((System.Windows.Controls.Canvas)(target));
            
            #line 11 "..\..\MainPage.xaml"
            this.ContainerCanvas.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.ContainerCanvas_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.CloseButton = ((System.Windows.Shapes.Rectangle)(target));
            
            #line 12 "..\..\MainPage.xaml"
            this.CloseButton.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.CloseButton_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 4:
            this.MinimizeButton = ((System.Windows.Shapes.Rectangle)(target));
            return;
            case 5:
            this.BodyBackground = ((System.Windows.Shapes.Rectangle)(target));
            
            #line 24 "..\..\MainPage.xaml"
            this.BodyBackground.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.BodyBackground_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 6:
            this.BodyContainer = ((System.Windows.Controls.Grid)(target));
            return;
            case 7:
            this.AddButton = ((System.Windows.Shapes.Rectangle)(target));
            
            #line 40 "..\..\MainPage.xaml"
            this.AddButton.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.AddButton_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 8:
            this.ImportButton = ((System.Windows.Shapes.Rectangle)(target));
            
            #line 45 "..\..\MainPage.xaml"
            this.ImportButton.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.ImportButton_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 9:
            this.EditButton = ((System.Windows.Shapes.Rectangle)(target));
            
            #line 50 "..\..\MainPage.xaml"
            this.EditButton.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.EditButton_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 10:
            this.DeleteButton = ((System.Windows.Shapes.Rectangle)(target));
            
            #line 55 "..\..\MainPage.xaml"
            this.DeleteButton.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.DeleteButton_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

