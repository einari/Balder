﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Balder.Silverlight.SampleBrowser.Samples.Primitives.AribtraryHeightmap {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Balder.Silverlight.SampleBrowser.Samples.Primitives.AribtraryHeightmap.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to &lt;UserControl x:Class=&quot;Balder.Silverlight.SampleBrowser.Samples.Primitives.AribtraryHeightmap.Content&quot;
        ///    xmlns=&quot;http://schemas.microsoft.com/winfx/2006/xaml/presentation&quot; 
        ///    xmlns:x=&quot;http://schemas.microsoft.com/winfx/2006/xaml&quot; 
        ///    xmlns:Execution=&quot;clr-namespace:Balder.Execution;assembly=Balder&quot; 
        ///    xmlns:Debug=&quot;clr-namespace:Balder.Debug;assembly=Balder&quot; 
        ///    xmlns:Geometries=&quot;clr-namespace:Balder.Objects.Geometries;assembly=Balder&quot; 
        ///    xmlns:Lighting=&quot;clr-namespa [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Content_xaml {
            get {
                return ResourceManager.GetString("Content_xaml", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to using System;
        ///using Balder;
        ///using Balder.Objects.Geometries;
        ///
        ///namespace Balder.Silverlight.SampleBrowser.Samples.Primitives.AribtraryHeightmap
        ///{
        ///	public partial class Content
        ///	{
        ///		public Content()
        ///		{
        ///			InitializeComponent();
        ///		}
        ///
        ///		private double _sin;
        ///		private double _movement;
        ///
        ///		private void Heightmap_HeightInput(object sender, HeightmapEventArgs e)
        ///		{
        ///			var height = Math.Sin(_sin + _movement) * 2;
        ///
        ///			e.Height = (float)height;
        ///			var highlight = (byte)((height * 16f)  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Content_xaml_cs {
            get {
                return ResourceManager.GetString("Content_xaml_cs", resourceCulture);
            }
        }
    }
}
