//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NDSailing.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class surcharge
    {
        public int surchargeId { get; set; }
        public Nullable<int> startYear { get; set; }
        public Nullable<int> endYear { get; set; }
        public Nullable<double> amount { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}
