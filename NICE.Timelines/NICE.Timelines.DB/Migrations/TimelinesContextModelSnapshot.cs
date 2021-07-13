﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NICE.Timelines.DB.Models;

namespace NICE.Timelines.DB.Migrations
{
    [DbContext(typeof(TimelinesContext))]
    partial class TimelinesContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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
                });

            modelBuilder.Entity("NICE.Timelines.DB.Models.TaskType", b =>
                {
                    b.Property<int>("TaskTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("TaskTypeId");

                    b.ToTable("TaskType");
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

                    b.Property<DateTime?>("ActualDate")
                        .HasColumnType("datetime2");

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

                    b.Property<DateTime?>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("PhaseId")
                        .HasColumnType("int");

                    b.Property<int>("TaskTypeId")
                        .HasColumnType("int");

                    b.HasKey("TimelineTaskId");

                    b.HasIndex("PhaseId");

                    b.HasIndex("TaskTypeId");

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

                    b.HasOne("NICE.Timelines.DB.Models.TaskType", "TaskType")
                        .WithMany("TimelineTasks")
                        .HasForeignKey("TaskTypeId")
                        .HasConstraintName("TimelineTasks_Step")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Phase");

                    b.Navigation("TaskType");
                });

            modelBuilder.Entity("NICE.Timelines.DB.Models.Phase", b =>
                {
                    b.Navigation("TimelineTasks");
                });

            modelBuilder.Entity("NICE.Timelines.DB.Models.TaskType", b =>
                {
                    b.Navigation("TimelineTasks");
                });
#pragma warning restore 612, 618
        }
    }
}
