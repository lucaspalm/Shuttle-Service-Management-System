//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SSMSDataModel.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class SYSTEM_LOGS
    {
        public string USER_ID { get; set; }
        public System.DateTime LOGIN_DATETIME { get; set; }
    
        public virtual USER_ACCOUNTS USER_ACCOUNTS { get; set; }
    }
}
