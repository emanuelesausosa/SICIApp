﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SICIApp.Entities
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SICIBD2Entities1 : DbContext
    {
        public SICIBD2Entities1()
            : base("name=SICIBD2Entities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<CITY> CITies { get; set; }
        public virtual DbSet<COUNTRY> COUNTRies { get; set; }
        public virtual DbSet<COUNTRYLANGUAGE> COUNTRYLANGUAGEs { get; set; }
        public virtual DbSet<DATOSPERSONALE> DATOSPERSONALES { get; set; }
        public virtual DbSet<CENTROTERAPEUTICO> CENTROTERAPEUTICOes { get; set; }
        public virtual DbSet<USUARIOCENTROCONSULTA> USUARIOCENTROCONSULTAs { get; set; }
    }
}
