﻿// <auto-generated />
using System;
using GeekShopping.Email.Model.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GeekShopping.Email.Migrations
{
    [DbContext(typeof(MySQLContext))]
    [Migration("20220828002231_inicial2")]
    partial class inicial2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "6.0.0-preview.6.21352.1");

            modelBuilder.Entity("GeekShopping.Email.Model.EmailLog", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("email");

                    b.Property<string>("Log")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("log");

                    b.Property<DateTime>("SentDate")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("sent_date");

                    b.HasKey("Id");

                    b.ToTable("email_logs");
                });
#pragma warning restore 612, 618
        }
    }
}