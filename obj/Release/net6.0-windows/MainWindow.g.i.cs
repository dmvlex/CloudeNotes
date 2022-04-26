// Updated by XamlIntelliSenseFileGenerator 26.04.2022 11:19:52
#pragma checksum "..\..\..\MainWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7966DD5711DF93603F8BFC153A31949A888A4DD6"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using CloudNotes;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
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


namespace CloudNotes
{


    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector
    {

#line default
#line hidden


#line 12 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid MainGrid;

#line default
#line hidden


#line 25 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border DropBoxBorder;

#line default
#line hidden


#line 27 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock DropBox;

#line default
#line hidden


#line 36 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SettingsWindowButton;

#line default
#line hidden


#line 44 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SynchronizationButton;

#line default
#line hidden


#line 52 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button UploadToButtonCloud;

#line default
#line hidden

        private bool _contentLoaded;

        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.2.0")]
        public void InitializeComponent()
        {
            if (_contentLoaded)
            {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/CloudNotes;V1.0.0.0;component/mainwindow.xaml", System.UriKind.Relative);

#line 1 "..\..\..\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);

#line default
#line hidden
        }

        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.2.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.MainWindow1 = ((CloudNotes.MainWindow)(target));

#line 11 "..\..\..\MainWindow.xaml"
                    this.MainWindow1.ContentRendered += new System.EventHandler(this.MainWindowRendered);

#line default
#line hidden
                    return;
                case 2:
                    this.MainGrid = ((System.Windows.Controls.Grid)(target));
                    return;
                case 3:
                    this.DropBoxBorder = ((System.Windows.Controls.Border)(target));
                    return;
                case 4:
                    this.DropBox = ((System.Windows.Controls.TextBlock)(target));

#line 31 "..\..\..\MainWindow.xaml"
                    this.DropBox.Drop += new System.Windows.DragEventHandler(this.DropBoxDrop);

#line default
#line hidden

#line 32 "..\..\..\MainWindow.xaml"
                    this.DropBox.DragEnter += new System.Windows.DragEventHandler(this.DropBoxDragEnter);

#line default
#line hidden

#line 32 "..\..\..\MainWindow.xaml"
                    this.DropBox.DragLeave += new System.Windows.DragEventHandler(this.DropBoxDragLeave);

#line default
#line hidden
                    return;
                case 5:
                    this.SettingsWindowButton = ((System.Windows.Controls.Button)(target));

#line 40 "..\..\..\MainWindow.xaml"
                    this.SettingsWindowButton.Click += new System.Windows.RoutedEventHandler(this.SettingsButtonClick);

#line default
#line hidden
                    return;
                case 6:
                    this.SynchronizationButton = ((System.Windows.Controls.Button)(target));

#line 50 "..\..\..\MainWindow.xaml"
                    this.SynchronizationButton.Click += new System.Windows.RoutedEventHandler(this.SynchronizationButtonClick);

#line default
#line hidden
                    return;
                case 7:
                    this.UploadToButtonCloud = ((System.Windows.Controls.Button)(target));

#line 57 "..\..\..\MainWindow.xaml"
                    this.UploadToButtonCloud.Click += new System.Windows.RoutedEventHandler(this.UploadToCloudClick);

#line default
#line hidden
                    return;
            }
            this._contentLoaded = true;
        }

        internal System.Windows.Window MainWindow1;
    }
}

