﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gauss.TccUnifaat.Common.Resources {
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
    public class querys {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal querys() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Gauss.TccUnifaat.Common.Resources.querys", typeof(querys).Assembly);
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
        ///   Looks up a localized string similar to SELECT Count(*) as qtd, TipoNoticia
        ///FROM Noticias
        ///WHERE Excluido = 0
        ///Group by TipoNoticia
        ///Order by qtd desc.
        /// </summary>
        public static string noticias_dashboard {
            get {
                return ResourceManager.GetString("noticias_dashboard", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT 
        ///    u.Id as Id,
        ///    u.NomeCompleto as NomeCompleto,
        ///    u.Email as Email,
        ///    t.Nome as Turma, 
        ///    r.Name as Funcao
        ///FROM 
        ///    AspNetUsers u
        ///    LEFT JOIN Turmas t ON u.TurmaId = t.TurmaId 
        ///    LEFT JOIN AspNetUserRoles ur ON u.Id = ur.UserId
        ///    LEFT JOIN AspNetRoles r ON ur.RoleId = r.Id
        ///WHERE 
        ///    u.Excluido = 0
        ///ORDER BY 
        ///    u.NomeCompleto, Turma, Funcao;
        ///.
        /// </summary>
        public static string usuarios {
            get {
                return ResourceManager.GetString("usuarios", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT r.Name as Funcao, COUNT(ur.UserId) as QuantidadeUsuarios
        ///    FROM AspNetRoles r
        ///    LEFT JOIN AspNetUserRoles ur ON r.Id = ur.RoleId
        ///    GROUP BY r.Name
        ///    ORDER BY QuantidadeUsuarios DESC;.
        /// </summary>
        public static string usuarios_dashboard {
            get {
                return ResourceManager.GetString("usuarios_dashboard", resourceCulture);
            }
        }
    }
}
