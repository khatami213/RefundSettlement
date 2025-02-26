﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class PNA_RefundServiceEntities : DbContext
    {
        public PNA_RefundServiceEntities()
            : base("name=PNA_RefundServiceEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CardToCard> CardToCard { get; set; }
        public virtual DbSet<CardTransfer> CardTransfer { get; set; }
        public virtual DbSet<DailyTransactionAmount> DailyTransactionAmount { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<NationalCodeInquiry> NationalCodeInquiry { get; set; }
        public virtual DbSet<RefundRequest> RefundRequest { get; set; }
        public virtual DbSet<RefundRequestReconciliation> RefundRequestReconciliation { get; set; }
        public virtual DbSet<ServerInfo> ServerInfo { get; set; }
        public virtual DbSet<Token> Token { get; set; }
        public virtual DbSet<WebServicePermission> WebServicePermission { get; set; }
        public virtual DbSet<WebServiceUser> WebServiceUser { get; set; }
        public virtual DbSet<WebServiceUsersPermission> WebServiceUsersPermission { get; set; }
        public virtual DbSet<RefundProvider> RefundProvider { get; set; }
        public virtual DbSet<CardsInfo> CardsInfo { get; set; }
        public virtual DbSet<IBANTransfer> IBANTransfer { get; set; }
        public virtual DbSet<ProviderTransferDetailes> ProviderTransferDetailes { get; set; }
    
        public virtual ObjectResult<SP_RefundStatistics_Result> SP_RefundStatistics()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_RefundStatistics_Result>("SP_RefundStatistics");
        }
    }
}
