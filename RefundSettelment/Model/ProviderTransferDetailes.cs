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
    
    public partial class ProviderTransferDetailes
    {
        public long Id { get; set; }
        public Nullable<long> TransferId { get; set; }
        public Nullable<bool> ResponseStatus { get; set; }
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
        public Nullable<long> TransferAmount { get; set; }
        public Nullable<int> TransferCount { get; set; }
        public string Status { get; set; }
        public long RefrenceNumber { get; set; }
        public string CardStatus { get; set; }
        public string CardOwner { get; set; }
    }
}
