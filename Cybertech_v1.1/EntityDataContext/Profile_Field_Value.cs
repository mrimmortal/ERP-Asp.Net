//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cybertech_v1._1.EntityDataContext
{
    using System;
    using System.Collections.Generic;
    
    public partial class Profile_Field_Value
    {
        public long id { get; set; }
        public Nullable<int> user_id { get; set; }
        public Nullable<int> field_id { get; set; }
        public string field_value { get; set; }
    
        public virtual Core_Users Core_Users { get; set; }
        public virtual Profile_Fields Profile_Fields { get; set; }
    }
}
