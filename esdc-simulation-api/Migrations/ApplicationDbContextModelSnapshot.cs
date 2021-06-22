﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using maternity_benefits.Storage.EF;

namespace esdc_simulation_api.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("maternity_benefits.Storage.EF.Models.MaternityBenefitsPerson", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<decimal>("AverageIncome")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Flsah")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("maternity_benefits.Storage.EF.Models.MaternityBenefitsSimulation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BaseCaseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("SimulationName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("VariantCaseId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("BaseCaseId");

                    b.HasIndex("VariantCaseId");

                    b.ToTable("Simulations");
                });

            modelBuilder.Entity("maternity_benefits.Storage.EF.Models.MaternityBenefitsSimulationCase", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("MaxWeeklyAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("NumWeeks")
                        .HasColumnType("int");

                    b.Property<double>("Percentage")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("MaternityBenefitsSimulationCase");
                });

            modelBuilder.Entity("maternity_benefits.Storage.EF.Models.MaternityBenefitsSimulationResult", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SimulationId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("SimulationResults");
                });

            modelBuilder.Entity("maternity_benefits.Storage.EF.Models.PersonResult", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("BaseAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("MaternityBenefitsPersonId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MaternityBenefitsSimulationResultId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("VariantAmount")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("MaternityBenefitsPersonId");

                    b.HasIndex("MaternityBenefitsSimulationResultId");

                    b.ToTable("PersonResult");
                });

            modelBuilder.Entity("maternity_benefits.Storage.EF.Models.MaternityBenefitsSimulation", b =>
                {
                    b.HasOne("maternity_benefits.Storage.EF.Models.MaternityBenefitsSimulationCase", "BaseCase")
                        .WithMany()
                        .HasForeignKey("BaseCaseId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("maternity_benefits.Storage.EF.Models.MaternityBenefitsSimulationCase", "VariantCase")
                        .WithMany()
                        .HasForeignKey("VariantCaseId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("maternity_benefits.Storage.EF.Models.PersonResult", b =>
                {
                    b.HasOne("maternity_benefits.Storage.EF.Models.MaternityBenefitsPerson", "MaternityBenefitsPerson")
                        .WithMany()
                        .HasForeignKey("MaternityBenefitsPersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("maternity_benefits.Storage.EF.Models.MaternityBenefitsSimulationResult", "MaternityBenefitsSimulationResult")
                        .WithMany("PersonResults")
                        .HasForeignKey("MaternityBenefitsSimulationResultId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
