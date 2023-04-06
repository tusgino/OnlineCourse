using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DAL.Models
{
    public partial class WebsiteKhoaHocOnline_V4Context : DbContext
    {
        public WebsiteKhoaHocOnline_V4Context()
        {
        }

        public WebsiteKhoaHocOnline_V4Context(DbContextOptions<WebsiteKhoaHocOnline_V4Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<BankInfo> BankInfos { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Degree> Degrees { get; set; } = null!;
        public virtual DbSet<TypeOfUser> TypeOfUsers { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=HUAN\\SQLEXPRESS;Initial Catalog=WebsiteKhoaHocOnline_V4;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.IdAccount);

                entity.ToTable("ACCOUNT");

                entity.Property(e => e.IdAccount)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_Account");

                entity.Property(e => e.DateCreate).HasColumnType("date");

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Username).HasMaxLength(50);
            });

            modelBuilder.Entity<BankInfo>(entity =>
            {
                entity.HasKey(e => e.IdBankAccount);

                entity.ToTable("BANK_INFO");

                entity.Property(e => e.IdBankAccount)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_BankAccount");

                entity.Property(e => e.AccountName).HasMaxLength(50);

                entity.Property(e => e.BankAccountNumber).HasMaxLength(50);

                entity.Property(e => e.BankName).HasMaxLength(50);
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.IdCourse);

                entity.ToTable("COURSE");

                entity.Property(e => e.IdCourse)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_Course");

                entity.Property(e => e.DateOfUpload).HasColumnType("date");

                entity.Property(e => e.IdCategory).HasColumnName("ID_Category");

                entity.Property(e => e.IdUser).HasColumnName("ID_User");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK_COURSE_USER");
            });

            modelBuilder.Entity<Degree>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DEGREE");

                entity.Property(e => e.IdDegree).HasColumnName("ID_Degree");

                entity.Property(e => e.IdUser).HasColumnName("ID_User");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK_DEGREE_USER");
            });

            modelBuilder.Entity<TypeOfUser>(entity =>
            {
                entity.HasKey(e => e.IdTypeOfUser);

                entity.ToTable("TYPE_OF_USER");

                entity.Property(e => e.IdTypeOfUser)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_TypeOfUser");

                entity.Property(e => e.TypeOfUserName).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.IdUser);

                entity.ToTable("USER");

                entity.Property(e => e.IdUser)
                    .HasColumnName("ID_User")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.IdAccount).HasColumnName("ID_Account");

                entity.Property(e => e.IdBankAccount).HasColumnName("ID_BankAccount");

                entity.Property(e => e.IdCard)
                    .HasMaxLength(50)
                    .HasColumnName("ID_Card");

                entity.Property(e => e.IdTypeOfUser).HasColumnName("ID_TypeOfUser");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.HasOne(d => d.IdAccountNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.IdAccount)
                    .HasConstraintName("FK_USER_ACCOUNT");

                entity.HasOne(d => d.IdBankAccountNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.IdBankAccount)
                    .HasConstraintName("FK_USER_BANK_INFO");

                entity.HasOne(d => d.IdTypeOfUserNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.IdTypeOfUser)
                    .HasConstraintName("FK_USER_TYPE_OF_USER");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
