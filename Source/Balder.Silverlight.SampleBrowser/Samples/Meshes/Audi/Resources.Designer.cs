﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Balder.Silverlight.SampleBrowser.Samples.Meshes.Audi {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Balder.Silverlight.SampleBrowser.Samples.Meshes.Audi.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;UserControl x:Class=&quot;Balder.Silverlight.SampleBrowser.Samples.Mesh.Audi.Content&quot;
        ///    xmlns=&quot;http://schemas.microsoft.com/winfx/2006/xaml/presentation&quot; 
        ///    xmlns:x=&quot;http://schemas.microsoft.com/winfx/2006/xaml&quot; 
        ///	xmlns:local=&quot;clr-namespace:Balder.Silverlight.SampleBrowser.Samples.Mesh.Audi&quot;
        ///    &gt;
        ///    &lt;Grid x:Name=&quot;LayoutRoot&quot;&gt;
        ///        &lt;local:MyGame Width=&quot;800&quot; Height=&quot;600&quot;/&gt;
        ///
        ///    &lt;/Grid&gt;
        ///&lt;/UserControl&gt;.
        /// </summary>
        internal static string Content_xaml {
            get {
                return ResourceManager.GetString("Content_xaml", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to using System;
        ///using System.Windows;
        ///using Balder.Math;
        ///using Balder.Objects.Geometries;
        ///
        ///namespace Balder.Silverlight.SampleBrowser.Samples.Mesh.Audi
        ///{
        ///	public partial class Content
        ///	{
        ///		public Content()
        ///		{
        ///			InitializeComponent();
        ///		}
        ///	}
        ///}
        ///.
        /// </summary>
        internal static string Content_xaml_cs {
            get {
                return ResourceManager.GetString("Content_xaml_cs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to using Balder.Execution;
        ///using Balder.Lighting;
        ///using Balder.Materials;
        ///using Balder.Objects.Geometries;
        ///using Balder.Math;
        ///using Matrix = Balder.Math.Matrix;
        ///
        ///namespace Balder.Silverlight.SampleBrowser.Samples.Mesh.Audi
        ///{
        ///	public class MyGame : Game
        ///	{
        ///		private Mesh _teapot;
        ///        private Mesh _audi;
        ///        private float _audiRotation;
        ///
        ///		public override void OnInitialize()
        ///		{
        ///			ContentManager.AssetsRoot = &quot;Assets&quot;;
        ///            //Display.BackgroundColor = Colors.Yellow;
        ///
        ///			var lig [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string MyGame {
            get {
                return ResourceManager.GetString("MyGame", resourceCulture);
            }
        }
    }
}
