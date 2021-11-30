﻿using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SocialMedia.Core.Entities;
using SocialMedia.Infraestructure.Data.Configuration;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMedia.Infraestructure.Data
{
    public partial class SocialMediaContext : DbContext
    {
        public SocialMediaContext()
        {
        }

        public SocialMediaContext(DbContextOptions<SocialMediaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Security> Securities { get; set; }
    /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=SocialMedia;Integrated Security = true");
            }
        }
    */
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {/*
      modelBuilder.ApplyConfiguration(new CommentConfiguration());
      modelBuilder.ApplyConfiguration(new PostConfiguration());
      modelBuilder.ApplyConfiguration(new UserConfiguration());
      */
      //Configuramos el modelBuilder a nivel de assembly, para no estar registrando los modelos uno por uno independientemente
      modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
      /*OnModelCreatingPartial(modelBuilder);*/
    }
    /*
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    */
  }
}
