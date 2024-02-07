﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TiskWASM.Server.Data;

#nullable disable

namespace TiskWASM.Server.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20240206233901_mysql-initial")]
    partial class mysqlinitial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("TiskWASM.Shared.dtComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("SolutionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SolutionId");

                    b.ToTable("tb_comment");
                });

            modelBuilder.Entity("TiskWASM.Shared.dtDevice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("tb_device");
                });

            modelBuilder.Entity("TiskWASM.Shared.dtSolution", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("RequestedDevice")
                        .HasColumnType("int");

                    b.Property<int>("SuggestedDevice")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("tb_solution");
                });

            modelBuilder.Entity("TiskWASM.Shared.dtUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Office")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("tb_user");
                });

            modelBuilder.Entity("TiskWASM.Shared.dtComment", b =>
                {
                    b.HasOne("TiskWASM.Shared.dtSolution", "_Solution")
                        .WithMany("_Comments")
                        .HasForeignKey("SolutionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("_Solution");
                });

            modelBuilder.Entity("TiskWASM.Shared.dtSolution", b =>
                {
                    b.HasOne("TiskWASM.Shared.dtUser", "_User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("_User");
                });

            modelBuilder.Entity("TiskWASM.Shared.dtSolution", b =>
                {
                    b.Navigation("_Comments");
                });
#pragma warning restore 612, 618
        }
    }
}
