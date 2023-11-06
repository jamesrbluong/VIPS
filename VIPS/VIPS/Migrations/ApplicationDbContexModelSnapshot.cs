﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VIPS.Data;

#nullable disable

namespace VIPS.Migrations
{
    [DbContext(typeof(ApplicationDbContex))]
    partial class ApplicationDbContexModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("VIPS.Models.CSV", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AgencyMailingAddress1")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AgencyMailingAddress2")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AgencyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AutoRenewal")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BCH_AgingServicesManagement")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BCH_AthleticTraining")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BCH_College")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BCH_ExerciseScience")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BCH_HealthAdministration")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BCH_InterdisciplinaryHealthStudies")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BCH_MentalHealthCounseling")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BCH_NurseAnesthetist")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BCH_Nursing")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BCH_NutritionDietetics")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BCH_PhysicalTherapy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BCH_PublicHealth")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CCECMajors")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("COEHSPrograms")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContractCategory")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ContractID")
                        .HasColumnType("int");

                    b.Property<string>("ContractName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContractOrigin")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContractTypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedOn")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CurrentStageAssignees")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DaysInCurrStage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("Duplicate")
                        .HasColumnType("bit");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("Error")
                        .HasColumnType("bit");

                    b.Property<string>("ErrorDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExternalContractReferenceID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FacultyInitiator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FolderName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Graduate_Undergraduate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Locked")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Owner")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PrimaryContact")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PrimaryDocument")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProgramsOrCourses")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RelatedToContract")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RelatedToContractID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Renewal")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StageName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TitleCert")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UpdatedOn")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Workflow")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Year")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CSVs");
                });

            modelBuilder.Entity("VIPS.Models.Contract", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AgencyMailingAddress1")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AgencyMailingAddress2")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AgencyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AutoRenewal")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BCH_AgingServicesManagement")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BCH_AthleticTraining")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BCH_College")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BCH_ExerciseScience")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BCH_HealthAdministration")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BCH_InterdisciplinaryHealthStudies")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BCH_MentalHealthCounseling")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BCH_NurseAnesthetist")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BCH_Nursing")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BCH_NutritionDietetics")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BCH_PhysicalTherapy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BCH_PublicHealth")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CCECMajors")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("COEHSPrograms")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContractCategory")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ContractID")
                        .HasColumnType("int");

                    b.Property<string>("ContractName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContractOrigin")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContractTypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedOn")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CurrentStageAssignees")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DaysInCurrStage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExternalContractReferenceID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FacultyInitiator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FolderName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Graduate_Undergraduate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Locked")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Owner")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PrimaryContact")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PrimaryDocument")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProgramsOrCourses")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RelatedToContract")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RelatedToContractID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Renewal")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StageName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TitleCert")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UpdatedOn")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Workflow")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Year")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Contracts");
                });
#pragma warning restore 612, 618
        }
    }
}
