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
    
    public partial class Department
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Department()
        {
            this.Users = new HashSet<User>();
        }
    
        public int DeptId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int PlantId { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public bool IsHead { get; set; }
    
        public virtual Plant Plant { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }
    }
}
