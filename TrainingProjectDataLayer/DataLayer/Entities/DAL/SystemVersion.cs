//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TrainingProjectDataLayer.DataLayer.Entities.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class SystemVersion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SystemVersion()
        {
            this.SystemDetails = new HashSet<SystemDetail>();
        }
    
        public int VersionId { get; set; }
        public string Version { get; set; }
        public System.DateTime ReleaseDate { get; set; }
        public string ChangesDone { get; set; }
        public string ReleaseBy { get; set; }
        public string ChangesDoneBy { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SystemDetail> SystemDetails { get; set; }
    }
}
