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
    
    public partial class PlantStatu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PlantStatu()
        {
            this.Plants = new HashSet<Plant>();
        }
    
        public int PlantStatusId { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public Nullable<int> ColorId { get; set; }
    
        public virtual Color Color { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Plant> Plants { get; set; }
    }
}
