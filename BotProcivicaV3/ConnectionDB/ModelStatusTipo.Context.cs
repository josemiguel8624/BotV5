﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BotProcivicaV3.ConnectionDB
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Chatbot_PGBEntitiesSTC : DbContext
    {
        public Chatbot_PGBEntitiesSTC()
            : base("name=Chatbot_PGBEntitiesSTC")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CasosStatus> CasosStatus { get; set; }
        public virtual DbSet<TipoCasos> TipoCasos { get; set; }
    }
}