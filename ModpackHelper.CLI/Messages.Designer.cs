﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ModpackHelper.CLI {
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
    public class Messages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Messages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ModpackHelper.CLI.Messages", typeof(Messages).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Input directory not found..
        /// </summary>
        public static string InputDirectoryNotFound {
            get {
                return ResourceManager.GetString("InputDirectoryNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You haven&apos;t specified the input directory like so: -i &lt;path&gt;..
        /// </summary>
        public static string MissingInputDirectory {
            get {
                return ResourceManager.GetString("MissingInputDirectory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You haven&apos;t specified the input directory like so: -o &lt;path&gt;..
        /// </summary>
        public static string MissingOutputDirectory {
            get {
                return ResourceManager.GetString("MissingOutputDirectory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified input directory is not a mods directory. It has to be..
        /// </summary>
        public static string NotAModsDirectory {
            get {
                return ResourceManager.GetString("NotAModsDirectory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Usage: TechnicSolderHelper.CLI.exe [-options] 
        ///Where options include:
        ///	-i &lt;dir&gt;				Specifies the input directory
        ///							Remember to specify the -o flag
        ///	-o &lt;dir&gt;				Specifies the output directory
        ///							Remember to specify the -i flag
        ///	-c						Clear output directory on run
        ///	-f 						Get Forge/Minecraft versions
        ///	-r 						Repack everything
        ///	-u						Update stored permissions
        ///	-g						Generate permissions
        ///	-v						Enable debugging/verbose output
        ///	-Mn	&lt;value&gt;				Specifies the modpack name
        ///	-Mv	&lt;val [rest of string was truncated]&quot;;.
        /// </summary>
        public static string Usage {
            get {
                return ResourceManager.GetString("Usage", resourceCulture);
            }
        }
    }
}
