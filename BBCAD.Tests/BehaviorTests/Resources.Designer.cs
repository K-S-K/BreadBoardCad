﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BBCAD.Tests.BehaviorTests {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("BBCAD.Tests.BehaviorTests.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to &lt;Board&gt;
        ///  &lt;Id&gt;8344FA9A-763F-4074-A893-58CC56E3805B&lt;/Id&gt;
        ///  &lt;Name&gt;Amazing Project&lt;/Name&gt;
        ///  &lt;SizeX&gt;8&lt;/SizeX&gt;
        ///  &lt;SizeY&gt;8&lt;/SizeY&gt;
        ///  &lt;Description&gt;Experiment One&lt;/Description&gt;
        ///  &lt;Lines /&gt;
        ///&lt;/Board&gt;.
        /// </summary>
        internal static string Board_01_CRC {
            get {
                return ResourceManager.GetString("Board_01_CRC", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to // Prototyping Board Sample Project
        ///
        ///CREATE BOARD Name = &quot;Amazing Project&quot; X = 8 Y = 13 Description = &quot;Experiment One&quot;
        ///
        ///RESIZE BOARD X = 8  Y = 8
        ///
        ////*
        ///ADD LINE X1 = 2 Y1 = 2 X2 = 5
        ///*/
        ///.
        /// </summary>
        internal static string Script_01_CRC {
            get {
                return ResourceManager.GetString("Script_01_CRC", resourceCulture);
            }
        }
    }
}
