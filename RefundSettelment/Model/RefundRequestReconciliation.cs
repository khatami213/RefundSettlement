//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RefundSettelment.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class RefundRequestReconciliation
    {
        public long Id { get; set; }
        public long RefrenceNumber { get; set; }
        public string EncryptedPan { get; set; }
        public string CardTransferEncryptedSourcePan { get; set; }
        public Nullable<long> ReverseAmount { get; set; }
        public System.DateTime InsertDateTime { get; set; }
        public string ReverseRrn { get; set; }
        public Nullable<System.DateTime> ReverseDateTime { get; set; }
        public string ReverseStatus { get; set; }
    }
}
