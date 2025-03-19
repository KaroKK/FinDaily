﻿// <auto-generated />
using System;
using FinBackend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FinBackend.Migrations
{
    [DbContext(typeof(FinContext))]
    partial class FinContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FinBackend.Api.Models.ActionLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LogAction")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LogDetails")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ActionLogs");
                });

            modelBuilder.Entity("FinBackend.Api.Models.Categ", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CatInfo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CatName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Categ");
                });

            modelBuilder.Entity("FinBackend.Api.Models.PayWay", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("PayInfo")
                        .HasColumnType("text");

                    b.Property<string>("PayLabel")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("PayWays");
                });

            modelBuilder.Entity("FinBackend.Api.Models.PlanBud", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CatId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("LimitAmt")
                        .HasColumnType("numeric");

                    b.Property<string>("PeriodTxt")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CatId");

                    b.HasIndex("UserId");

                    b.ToTable("PlanBuds");
                });

            modelBuilder.Entity("FinBackend.Api.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PassHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("FinBackend.Models.CashFlow", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("CatId")
                        .HasColumnType("integer");

                    b.Property<decimal>("FlowAmount")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("FlowDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FlowDesc")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FlowType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("PayId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CatId");

                    b.HasIndex("PayId");

                    b.ToTable("CashFlows");
                });

            modelBuilder.Entity("FinBackend.Api.Models.ActionLog", b =>
                {
                    b.HasOne("FinBackend.Api.Models.User", "TheUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TheUser");
                });

            modelBuilder.Entity("FinBackend.Api.Models.Categ", b =>
                {
                    b.HasOne("FinBackend.Api.Models.User", "TheUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TheUser");
                });

            modelBuilder.Entity("FinBackend.Api.Models.PayWay", b =>
                {
                    b.HasOne("FinBackend.Api.Models.User", "TheUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TheUser");
                });

            modelBuilder.Entity("FinBackend.Api.Models.PlanBud", b =>
                {
                    b.HasOne("FinBackend.Api.Models.Categ", "TheCat")
                        .WithMany()
                        .HasForeignKey("CatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FinBackend.Api.Models.User", "TheUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TheCat");

                    b.Navigation("TheUser");
                });

            modelBuilder.Entity("FinBackend.Models.CashFlow", b =>
                {
                    b.HasOne("FinBackend.Api.Models.Categ", "Category")
                        .WithMany()
                        .HasForeignKey("CatId");

                    b.HasOne("FinBackend.Api.Models.PayWay", "PayWay")
                        .WithMany()
                        .HasForeignKey("PayId");

                    b.Navigation("Category");

                    b.Navigation("PayWay");
                });
#pragma warning restore 612, 618
        }
    }
}
