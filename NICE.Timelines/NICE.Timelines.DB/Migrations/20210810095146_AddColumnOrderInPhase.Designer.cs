﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NICE.Timelines.DB.Models;

namespace NICE.Timelines.DB.Migrations
{
    [DbContext(typeof(TimelinesContext))]
    [Migration("20210810095146_AddColumnOrderInPhase")]
    partial class AddColumnOrderInPhase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:Collation", "Latin1_General_CI_AS")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NICE.Timelines.DB.Models.Phase", b =>
                {
                    b.Property<int>("PhaseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("PhaseDescription")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PhaseId");

                    b.ToTable("Phase");

                    b.HasData(
                        new
                        {
                            PhaseId = 12,
                            PhaseDescription = "Invitation to participate"
                        },
                        new
                        {
                            PhaseId = 13,
                            PhaseDescription = "Submissions"
                        },
                        new
                        {
                            PhaseId = 14,
                            PhaseDescription = "Assessment report"
                        },
                        new
                        {
                            PhaseId = 15,
                            PhaseDescription = "Assessment report consultation"
                        },
                        new
                        {
                            PhaseId = 16,
                            PhaseDescription = "Overview"
                        },
                        new
                        {
                            PhaseId = 17,
                            PhaseDescription = "Evidence critique"
                        },
                        new
                        {
                            PhaseId = 18,
                            PhaseDescription = "Pre meeting briefing"
                        },
                        new
                        {
                            PhaseId = 19,
                            PhaseDescription = "First committee meeting"
                        },
                        new
                        {
                            PhaseId = 20,
                            PhaseDescription = "Consultation"
                        },
                        new
                        {
                            PhaseId = 21,
                            PhaseDescription = "FAD sign off"
                        },
                        new
                        {
                            PhaseId = 22,
                            PhaseDescription = "FAD appeal period"
                        },
                        new
                        {
                            PhaseId = 24,
                            PhaseDescription = "Publication"
                        },
                        new
                        {
                            PhaseId = 26,
                            PhaseDescription = "Committee meeting"
                        },
                        new
                        {
                            PhaseId = 27,
                            PhaseDescription = "Scope review"
                        },
                        new
                        {
                            PhaseId = 28,
                            PhaseDescription = "Consultee meeting"
                        },
                        new
                        {
                            PhaseId = 109,
                            PhaseDescription = "Scoping"
                        },
                        new
                        {
                            PhaseId = 113,
                            PhaseDescription = "Technical report"
                        });
                });

            modelBuilder.Entity("NICE.Timelines.DB.Models.TimelineTask", b =>
                {
                    b.Property<int>("TimelineTaskId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ACID")
                        .HasColumnType("int")
                        .HasColumnName("ACID");

                    b.Property<string>("ClickUpFolderId")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ClickUpListId")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ClickUpSpaceId")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ClickUpTaskId")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime?>("CompletedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("OrderInPhase")
                        .HasColumnType("int");

                    b.Property<int>("PhaseId")
                        .HasColumnType("int");

                    b.Property<int?>("TaskTypeId")
                        .HasColumnType("int");

                    b.HasKey("TimelineTaskId");

                    b.HasIndex("PhaseId");

                    b.ToTable("TimelineTask");
                });

            modelBuilder.Entity("NICE.Timelines.DB.Models.TimelineTask", b =>
                {
                    b.HasOne("NICE.Timelines.DB.Models.Phase", "Phase")
                        .WithMany("TimelineTasks")
                        .HasForeignKey("PhaseId")
                        .HasConstraintName("TimelineTasks_Stage")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Phase");
                });

            modelBuilder.Entity("NICE.Timelines.DB.Models.Phase", b =>
                {
                    b.Navigation("TimelineTasks");
                });
#pragma warning restore 612, 618
        }
    }
}
