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
    
    public partial class ServerPathDetail
    {
        public int ServerPathId { get; set; }
        public string ServerPathName { get; set; }
        public string Description { get; set; }
        public string ServerPath { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public Nullable<short> LastModifiedBy { get; set; }
    }
}