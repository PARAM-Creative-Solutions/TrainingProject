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
    
    public partial class SystemDetail
    {
        public int SystemDetailId { get; set; }
        public string SystemName { get; set; }
        public string CompanyName { get; set; }
        public int VersionId { get; set; }
        public string Discription { get; set; }
        public int RememberMeMinutes { get; set; }
        public bool SSL { get; set; }
        public bool ShareableCookies { get; set; }
        public bool HttpOnlyCookies { get; set; }
    
        public virtual SystemVersion SystemVersion { get; set; }
    }
}
