﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace dvhd.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class dvhdEntities : DbContext
    {
        public dvhdEntities()
            : base("name=dvhdEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<DsDvhd> DsDvhds { get; set; }
        public DbSet<TinhThanh> TinhThanhs { get; set; }
        public DbSet<user> users { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<HoSo> HoSoes { get; set; }
    }
}
