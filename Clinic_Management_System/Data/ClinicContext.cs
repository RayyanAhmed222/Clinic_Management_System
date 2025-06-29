using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Clinic_Management_System.Models;

namespace Clinic_Management_System.Data
{
    public partial class ClinicContext : DbContext
    {
        public ClinicContext()
        {
        }

        public ClinicContext(DbContextOptions<ClinicContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bank> Banks { get; set; } = null!;
        public virtual DbSet<Carousel> Carousels { get; set; } = null!;
        public virtual DbSet<Category> Categorys { get; set; } = null!;
        public virtual DbSet<Faculty> Faculties { get; set; } = null!;
        public virtual DbSet<Feedback> Feedbacks { get; set; } = null!;
        public virtual DbSet<Lecture> Lectures { get; set; } = null!;
        public virtual DbSet<MedicineOrder> MedicineOrders { get; set; } = null!;
        public virtual DbSet<MedicinesCategory> MedicinesCategories { get; set; } = null!;
        public virtual DbSet<MedicinesInfo> MedicinesInfos { get; set; } = null!;
        public virtual DbSet<Practical> Practicals { get; set; } = null!;
        public virtual DbSet<PracticalCategory> PracticalCategories { get; set; } = null!;
        public virtual DbSet<RecruiterRequest> RecruiterRequests { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<ScientificCategory> ScientificCategories { get; set; } = null!;
        public virtual DbSet<ScientificInfo> ScientificInfos { get; set; } = null!;
        public virtual DbSet<ScientificOrder> ScientificOrders { get; set; } = null!;
        public virtual DbSet<SellMedicinesInfo> SellMedicinesInfos { get; set; } = null!;
        public virtual DbSet<SellScientificInfo> SellScientificInfos { get; set; } = null!;
        public virtual DbSet<Seminar> Seminars { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-F446CCN\\SQLEXPRESS;Initial Catalog=Clinic;Persist Security Info=False;User ID=sa;Password=rayyan;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bank>(entity =>
            {
                entity.HasKey(e => e.AccId)
                    .HasName("PK__bank__A471AFDAD132BA22");

                entity.ToTable("bank");

                entity.Property(e => e.AccId).HasColumnName("accId");

                entity.Property(e => e.AccName)
                    .HasMaxLength(200)
                    .HasColumnName("accName");

                entity.Property(e => e.AccNumber)
                    .HasMaxLength(34)
                    .HasColumnName("accNumber");
            });

            modelBuilder.Entity<Carousel>(entity =>
            {
                entity.ToTable("Carousel");

                entity.Property(e => e.CarouselId).HasColumnName("Carousel_id");

                entity.Property(e => e.CarouselImg)
                    .IsUnicode(false)
                    .HasColumnName("Carousel_img");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategorysId)
                    .HasName("PK__Category__A654D724F0353244");

                entity.HasIndex(e => e.CategorysName, "UQ__Category__BD902C16BD48718B")
                    .IsUnique();

                entity.Property(e => e.CategorysId).HasColumnName("Categorys_id");

                entity.Property(e => e.CategorysImg)
                    .IsUnicode(false)
                    .HasColumnName("Categorys_img");

                entity.Property(e => e.CategorysName)
                    .HasMaxLength(100)
                    .HasColumnName("Categorys_Name");
            });

            modelBuilder.Entity<Faculty>(entity =>
            {
                entity.ToTable("Faculty");

                entity.Property(e => e.FacultyId).HasColumnName("Faculty_id");

                entity.Property(e => e.FacultyAge).HasColumnName("Faculty_age");

                entity.Property(e => e.FacultyEmail)
                    .HasMaxLength(100)
                    .HasColumnName("Faculty_email");

                entity.Property(e => e.FacultyGender)
                    .HasMaxLength(10)
                    .HasColumnName("Faculty_gender");

                entity.Property(e => e.FacultyImage)
                    .IsUnicode(false)
                    .HasColumnName("Faculty_Image");

                entity.Property(e => e.FacultyName)
                    .HasMaxLength(100)
                    .HasColumnName("Faculty_name");

                entity.Property(e => e.FacultyPhone)
                    .HasMaxLength(20)
                    .HasColumnName("Faculty_phone");

                entity.Property(e => e.FacultyQualification)
                    .HasMaxLength(100)
                    .HasColumnName("Faculty_qualification");

                entity.Property(e => e.FacultySpecialization)
                    .HasMaxLength(100)
                    .HasColumnName("Faculty_specialization");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("Feedback");

                entity.Property(e => e.FeedbackId).HasColumnName("Feedback_id");

                entity.Property(e => e.ClientFeedback)
                    .HasMaxLength(500)
                    .HasColumnName("Client_feedback");

                entity.Property(e => e.ClientName)
                    .HasMaxLength(255)
                    .HasColumnName("Client_name");

                entity.Property(e => e.ProductImg)
                    .IsUnicode(false)
                    .HasColumnName("Product_img");

                entity.Property(e => e.ProductName)
                    .HasMaxLength(255)
                    .HasColumnName("Product_name");
            });

            modelBuilder.Entity<Lecture>(entity =>
            {
                entity.HasKey(e => e.LecId)
                    .HasName("PK__lecture__CAEBE89F59937781");

                entity.ToTable("lecture");

                entity.Property(e => e.LecId).HasColumnName("lec_id");

                entity.Property(e => e.LecDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("lec_datetime");

                entity.Property(e => e.LecDescription)
                    .HasMaxLength(500)
                    .HasColumnName("lec_description");

                entity.Property(e => e.LecDuration)
                    .HasMaxLength(50)
                    .HasColumnName("lec_duration");

                entity.Property(e => e.LecFaculty).HasColumnName("lec_faculty");

                entity.Property(e => e.LecImg)
                    .IsUnicode(false)
                    .HasColumnName("lec_img");

                entity.Property(e => e.LecName)
                    .HasMaxLength(255)
                    .HasColumnName("lec_name");

                entity.Property(e => e.LecPrice)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("lec_price");

                entity.Property(e => e.LecShortDescription)
                    .HasMaxLength(100)
                    .HasColumnName("lec_short_description");

                entity.Property(e => e.LecStockstatus)
                    .HasMaxLength(50)
                    .HasColumnName("lec_stockstatus");

                entity.HasOne(d => d.LecFacultyNavigation)
                    .WithMany(p => p.Lectures)
                    .HasForeignKey(d => d.LecFaculty)
                    .HasConstraintName("FK__lecture__lec_fac__07C12930");
            });

            modelBuilder.Entity<MedicineOrder>(entity =>
            {
                entity.Property(e => e.MedCategory).HasMaxLength(100);

                entity.Property(e => e.MedName).HasMaxLength(200);

                entity.Property(e => e.MedPrice).HasMaxLength(50);

                entity.Property(e => e.OrderDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TransactionScreenshot).IsUnicode(false);

                entity.Property(e => e.UserEmail).HasMaxLength(255);
            });

            modelBuilder.Entity<MedicinesCategory>(entity =>
            {
                entity.HasKey(e => e.MedCatId)
                    .HasName("PK__Medicine__C5586AB2313A88BB");

                entity.ToTable("Medicines_Category");

                entity.HasIndex(e => e.MedCatName, "UQ__Medicine__DC981A741A7F82A8")
                    .IsUnique();

                entity.Property(e => e.MedCatId).HasColumnName("Med_Cat_id");

                entity.Property(e => e.MedCatName)
                    .HasMaxLength(100)
                    .HasColumnName("Med_Cat_Name");
            });

            modelBuilder.Entity<MedicinesInfo>(entity =>
            {
                entity.HasKey(e => e.MedId)
                    .HasName("PK__Medicine__B58A7BF59DDFD914");

                entity.ToTable("Medicines_Info");

                entity.Property(e => e.MedId).HasColumnName("Med_Id");

                entity.Property(e => e.MedCat).HasColumnName("Med_Cat");

                entity.Property(e => e.MedDescription)
                    .HasMaxLength(500)
                    .HasColumnName("Med_Description");

                entity.Property(e => e.MedImage)
                    .IsUnicode(false)
                    .HasColumnName("Med_Image");

                entity.Property(e => e.MedName)
                    .HasMaxLength(100)
                    .HasColumnName("Med_Name");

                entity.Property(e => e.MedPrice)
                    .HasMaxLength(100)
                    .HasColumnName("Med_Price");

                entity.Property(e => e.StockStatus)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('Available')");

                entity.HasOne(d => d.MedCatNavigation)
                    .WithMany(p => p.MedicinesInfos)
                    .HasForeignKey(d => d.MedCat)
                    .HasConstraintName("FK__Medicines__Med_C__70DDC3D8");
            });

            modelBuilder.Entity<Practical>(entity =>
            {
                entity.HasKey(e => e.PracId)
                    .HasName("PK__Practica__E8A46B82FBEBE64C");

                entity.ToTable("Practical");

                entity.Property(e => e.PracId).HasColumnName("Prac_id");

                entity.Property(e => e.ParcDuration)
                    .HasMaxLength(50)
                    .HasColumnName("Parc_duration");

                entity.Property(e => e.PracDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("Prac_datetime");

                entity.Property(e => e.PracDescription)
                    .HasMaxLength(500)
                    .HasColumnName("Prac_description");

                entity.Property(e => e.PracImg)
                    .IsUnicode(false)
                    .HasColumnName("Prac_img");

                entity.Property(e => e.PracName)
                    .HasMaxLength(255)
                    .HasColumnName("Prac_name");

                entity.Property(e => e.PracPrice)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("Prac_price");

                entity.Property(e => e.PracShortDescription)
                    .HasMaxLength(100)
                    .HasColumnName("Prac_short_description");

                entity.Property(e => e.PracStockstatus)
                    .HasMaxLength(50)
                    .HasColumnName("Prac_stockstatus");

                entity.Property(e => e.PracticalCategory).HasColumnName("Practical_category");

                entity.Property(e => e.PracticalFaculty).HasColumnName("Practical_faculty");

                entity.HasOne(d => d.PracticalCategoryNavigation)
                    .WithMany(p => p.Practicals)
                    .HasForeignKey(d => d.PracticalCategory)
                    .HasConstraintName("FK__Practical__Pract__2CF2ADDF");

                entity.HasOne(d => d.PracticalFacultyNavigation)
                    .WithMany(p => p.Practicals)
                    .HasForeignKey(d => d.PracticalFaculty)
                    .HasConstraintName("FK__Practical__Pract__2BFE89A6");
            });

            modelBuilder.Entity<PracticalCategory>(entity =>
            {
                entity.HasKey(e => e.PracCatId)
                    .HasName("PK__Practica__0EF9D8434F012F1E");

                entity.ToTable("Practical_Category");

                entity.HasIndex(e => e.PracCatName, "UQ__Practica__2DC6DC0A36262006")
                    .IsUnique();

                entity.Property(e => e.PracCatId).HasColumnName("Prac_Cat_id");

                entity.Property(e => e.PracCatName)
                    .HasMaxLength(100)
                    .HasColumnName("Prac_Cat_Name");
            });

            modelBuilder.Entity<RecruiterRequest>(entity =>
            {
                entity.HasKey(e => e.RequestId)
                    .HasName("PK__Recruite__33A8517A971F661E");

                entity.Property(e => e.Email).HasMaxLength(150);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.RequestDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('Pending')");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RecruiterRequests)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Recruiter__UserI__7FEAFD3E");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B6160C76BE403")
                    .IsUnique();

                entity.Property(e => e.RoleName).HasMaxLength(50);
            });

            modelBuilder.Entity<ScientificCategory>(entity =>
            {
                entity.HasKey(e => e.SciCatId)
                    .HasName("PK__Scientif__3F20D26A2B124FBD");

                entity.ToTable("Scientific_Category");

                entity.Property(e => e.SciCatId).HasColumnName("Sci_Cat_id");

                entity.Property(e => e.SciCatName)
                    .HasMaxLength(100)
                    .HasColumnName("Sci_Cat_Name");
            });

            modelBuilder.Entity<ScientificInfo>(entity =>
            {
                entity.HasKey(e => e.ScientificId)
                    .HasName("PK__Scientif__DFEDA0376655771F");

                entity.ToTable("Scientific_Info");

                entity.Property(e => e.ScientificId).HasColumnName("Scientific_Id");

                entity.Property(e => e.ScientificCat).HasColumnName("Scientific_Cat");

                entity.Property(e => e.ScientificDescription)
                    .HasMaxLength(500)
                    .HasColumnName("Scientific_Description");

                entity.Property(e => e.ScientificImage)
                    .IsUnicode(false)
                    .HasColumnName("Scientific_Image");

                entity.Property(e => e.ScientificName)
                    .HasMaxLength(100)
                    .HasColumnName("Scientific_Name");

                entity.Property(e => e.ScientificPrice)
                    .HasMaxLength(100)
                    .HasColumnName("Scientific_Price");

                entity.Property(e => e.StockStatus)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('Available')");

                entity.HasOne(d => d.ScientificCatNavigation)
                    .WithMany(p => p.ScientificInfos)
                    .HasForeignKey(d => d.ScientificCat)
                    .HasConstraintName("FK__Scientifi__Scien__503BEA1C");
            });

            modelBuilder.Entity<ScientificOrder>(entity =>
            {
                entity.Property(e => e.OrderDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SciCategory).HasMaxLength(100);

                entity.Property(e => e.SciName).HasMaxLength(200);

                entity.Property(e => e.SciPrice).HasMaxLength(50);

                entity.Property(e => e.TransactionScreenshot).IsUnicode(false);

                entity.Property(e => e.UserEmail).HasMaxLength(255);
            });

            modelBuilder.Entity<SellMedicinesInfo>(entity =>
            {
                entity.HasKey(e => e.MedId)
                    .HasName("PK__Sell_Med__B58A7BF5345C4AA1");

                entity.ToTable("Sell_Medicines_Info");

                entity.Property(e => e.MedId).HasColumnName("Med_Id");

                entity.Property(e => e.MedBrandName)
                    .HasMaxLength(100)
                    .HasColumnName("Med_Brand_Name");

                entity.Property(e => e.MedCat).HasColumnName("Med_Cat");

                entity.Property(e => e.MedDescription)
                    .HasMaxLength(500)
                    .HasColumnName("Med_Description");

                entity.Property(e => e.MedImage)
                    .IsUnicode(false)
                    .HasColumnName("Med_Image");

                entity.Property(e => e.MedName)
                    .HasMaxLength(100)
                    .HasColumnName("Med_Name");

                entity.Property(e => e.MedPrice)
                    .HasMaxLength(100)
                    .HasColumnName("Med_Price");

                entity.Property(e => e.StockStatus)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('Available')");

                entity.HasOne(d => d.MedCatNavigation)
                    .WithMany(p => p.SellMedicinesInfos)
                    .HasForeignKey(d => d.MedCat)
                    .HasConstraintName("FK__Sell_Medi__Med_C__2334397B");
            });

            modelBuilder.Entity<SellScientificInfo>(entity =>
            {
                entity.HasKey(e => e.ScientificId)
                    .HasName("PK__Sell_Sci__DFEDA037A6CD3794");

                entity.ToTable("Sell_Scientific_Info");

                entity.Property(e => e.ScientificId).HasColumnName("Scientific_Id");

                entity.Property(e => e.ScientificBrandName)
                    .HasMaxLength(100)
                    .HasColumnName("Scientific_Brand_Name");

                entity.Property(e => e.ScientificCat).HasColumnName("Scientific_Cat");

                entity.Property(e => e.ScientificDescription)
                    .HasMaxLength(500)
                    .HasColumnName("Scientific_Description");

                entity.Property(e => e.ScientificImage)
                    .IsUnicode(false)
                    .HasColumnName("Scientific_Image");

                entity.Property(e => e.ScientificName)
                    .HasMaxLength(100)
                    .HasColumnName("Scientific_Name");

                entity.Property(e => e.ScientificPrice)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("Scientific_Price");

                entity.Property(e => e.StockStatus)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('Available')");

                entity.HasOne(d => d.ScientificCatNavigation)
                    .WithMany(p => p.SellScientificInfos)
                    .HasForeignKey(d => d.ScientificCat)
                    .HasConstraintName("FK__Sell_Scie__Scien__2F9A1060");
            });

            modelBuilder.Entity<Seminar>(entity =>
            {
                entity.HasKey(e => e.SemId)
                    .HasName("PK__Seminar__9CC68A930FF8D865");

                entity.ToTable("Seminar");

                entity.Property(e => e.SemId).HasColumnName("Sem_id");

                entity.Property(e => e.SemDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("Sem_datetime");

                entity.Property(e => e.SemDescription)
                    .HasMaxLength(500)
                    .HasColumnName("Sem_description");

                entity.Property(e => e.SemDuration)
                    .HasMaxLength(50)
                    .HasColumnName("Sem_duration");

                entity.Property(e => e.SemFaculty).HasColumnName("Sem_faculty");

                entity.Property(e => e.SemImg)
                    .IsUnicode(false)
                    .HasColumnName("Sem_img");

                entity.Property(e => e.SemName)
                    .HasMaxLength(255)
                    .HasColumnName("Sem_name");

                entity.Property(e => e.SemPrice)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("Sem_price");

                entity.Property(e => e.SemShortDescription)
                    .HasMaxLength(100)
                    .HasColumnName("Sem_short_description");

                entity.Property(e => e.SemStockstatus)
                    .HasMaxLength(50)
                    .HasColumnName("Sem_stockstatus");

                entity.HasOne(d => d.SemFacultyNavigation)
                    .WithMany(p => p.Seminars)
                    .HasForeignKey(d => d.SemFaculty)
                    .HasConstraintName("FK__Seminar__Sem_fac__160F4887");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email, "UQ__Users__A9D105344D084343")
                    .IsUnique();

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email).HasMaxLength(150);

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.Password).HasMaxLength(150);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Users__RoleId__7B264821");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
