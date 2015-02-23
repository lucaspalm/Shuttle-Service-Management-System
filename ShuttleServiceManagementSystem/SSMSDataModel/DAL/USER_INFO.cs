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
    
    public partial class USER_INFO
    {
        public string USER_ID { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string STREET_ADDRESS { get; set; }
        public string CITY { get; set; }
        public string STATE { get; set; }
        public string ZIP_CODE { get; set; }
        public string EMAIL_ADDRESS { get; set; }
        public string CELL_NUMBER { get; set; }
        public Nullable<bool> RECEIVE_TEXT { get; set; }
        public Nullable<bool> RECEIVE_EMAIL { get; set; }
        public Nullable<int> CELL_CARRIER_ID { get; set; }
    
        public virtual USER_ACCOUNTS USER_ACCOUNTS { get; set; }
        public virtual CELL_CARRIERS CELL_CARRIERS { get; set; }
    }
}
