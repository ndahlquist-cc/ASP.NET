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
    
    public partial class membershipType
    {
        public membershipType()
        {
            this.memberships = new HashSet<membership>();
        }
    
        public string membershipTypeName { get; set; }
        public string description { get; set; }
        public double ratioToFull { get; set; }
    
        public virtual ICollection<membership> memberships { get; set; }
    }
}