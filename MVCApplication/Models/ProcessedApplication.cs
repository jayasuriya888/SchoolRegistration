//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVCApplication.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProcessedApplication
    {
        public int application_id { get; set; }
        public string comments { get; set; }
        public Nullable<System.DateTime> date_of_resolve { get; set; }
        public string resolved_by { get; set; }
    
        public virtual Application Application { get; set; }
    }
}