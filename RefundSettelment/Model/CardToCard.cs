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
    
    public partial class CardToCard
    {
        public int Id { get; set; }
        public string SourcePan { get; set; }
        public string DestinationPan { get; set; }
        public Nullable<long> Amount { get; set; }
        public string Rrn { get; set; }
        public string ResponseCode { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> InsertDateTime { get; set; }
    }
}
