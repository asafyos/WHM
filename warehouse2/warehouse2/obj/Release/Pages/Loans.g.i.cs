﻿#pragma checksum "..\..\..\Pages\Loans.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "CEEE81F507FABC0DE598EA33C442D9AB60A769B4"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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
using warehouse2;


namespace warehouse2 {
    
    
    /// <summary>
    /// Loans
    /// </summary>
    public partial class Loans : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 26 "..\..\..\Pages\Loans.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textBoxBarcode;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\Pages\Loans.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textBoxReturn;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\Pages\Loans.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textBoxSearch;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\..\Pages\Loans.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox listBoxWantedTools;
        
        #line default
        #line hidden
        
        
        #line 92 "..\..\..\Pages\Loans.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dataGridLoaned;
        
        #line default
        #line hidden
        
        
        #line 110 "..\..\..\Pages\Loans.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button buttonReturnAll;
        
        #line default
        #line hidden
        
        
        #line 128 "..\..\..\Pages\Loans.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ReturnButton;
        
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
            System.Uri resourceLocater = new System.Uri("/warehouse2;component/pages/loans.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Pages\Loans.xaml"
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
            this.textBoxBarcode = ((System.Windows.Controls.TextBox)(target));
            
            #line 26 "..\..\..\Pages\Loans.xaml"
            this.textBoxBarcode.KeyUp += new System.Windows.Input.KeyEventHandler(this.textBoxBarcode_KeyUp);
            
            #line default
            #line hidden
            return;
            case 2:
            this.textBoxReturn = ((System.Windows.Controls.TextBox)(target));
            
            #line 39 "..\..\..\Pages\Loans.xaml"
            this.textBoxReturn.KeyUp += new System.Windows.Input.KeyEventHandler(this.textBoxReturn_KeyUp);
            
            #line default
            #line hidden
            return;
            case 3:
            this.textBoxSearch = ((System.Windows.Controls.TextBox)(target));
            
            #line 52 "..\..\..\Pages\Loans.xaml"
            this.textBoxSearch.KeyUp += new System.Windows.Input.KeyEventHandler(this.textBoxSearch_KeyUp);
            
            #line default
            #line hidden
            return;
            case 4:
            this.listBoxWantedTools = ((System.Windows.Controls.ListBox)(target));
            return;
            case 5:
            
            #line 77 "..\..\..\Pages\Loans.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.CancelRequestButton_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.dataGridLoaned = ((System.Windows.Controls.DataGrid)(target));
            
            #line 92 "..\..\..\Pages\Loans.xaml"
            this.dataGridLoaned.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.dataGridLoaned_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 8:
            this.buttonReturnAll = ((System.Windows.Controls.Button)(target));
            return;
            case 9:
            this.ReturnButton = ((System.Windows.Controls.Button)(target));
            
            #line 128 "..\..\..\Pages\Loans.xaml"
            this.ReturnButton.Click += new System.Windows.RoutedEventHandler(this.ReturnButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 7:
            
            #line 104 "..\..\..\Pages\Loans.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.GridButton_Click);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

