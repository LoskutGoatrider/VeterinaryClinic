﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VeterinaryClinic.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class VeterinaryСlinicEntities : DbContext
    {
        public VeterinaryСlinicEntities()
            : base("name=VeterinaryСlinicEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<animal> animal { get; set; }
        public virtual DbSet<breed> breed { get; set; }
        public virtual DbSet<client> client { get; set; }
        public virtual DbSet<contract> contract { get; set; }
        public virtual DbSet<employees> employees { get; set; }
        public virtual DbSet<gender> gender { get; set; }
        public virtual DbSet<medicines> medicines { get; set; }
        public virtual DbSet<provisionservices> provisionservices { get; set; }
        public virtual DbSet<role> role { get; set; }
        public virtual DbSet<services> services { get; set; }
        public virtual DbSet<typeanimal> typeanimal { get; set; }
        public virtual DbSet<usedmedicines> usedmedicines { get; set; }
        public virtual DbSet<User> User { get; set; }
    }
}
