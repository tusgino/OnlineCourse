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
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Chapter> Chapters { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Degree> Degrees { get; set; } = null!;
        public virtual DbSet<Lesson> Lessons { get; set; } = null!;
        public virtual DbSet<Purchase> Purchases { get; set; } = null!;
        public virtual DbSet<Quiz> Quizzes { get; set; } = null!;
        public virtual DbSet<Rate> Rates { get; set; } = null!;
        public virtual DbSet<Study> Studies { get; set; } = null!;
        public virtual DbSet<TradeDetail> TradeDetails { get; set; } = null!;
        public virtual DbSet<TypeOfUser> TypeOfUsers { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-OLCDUI2\\SQLEXPRESS;Initial Catalog=WebsiteKhoaHocOnline_V4;Integrated Security=True");
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

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.IdCategory);

                entity.ToTable("CATEGORY");

                entity.Property(e => e.IdCategory)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_Category");
            });

            modelBuilder.Entity<Chapter>(entity =>
            {
                entity.HasKey(e => e.IdChapter);

                entity.ToTable("CHAPTER");

                entity.Property(e => e.IdChapter)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_Chapter");

                entity.Property(e => e.IdCourse).HasColumnName("ID_Course");

                entity.HasOne(d => d.IdCourseNavigation)
                    .WithMany(p => p.Chapters)
                    .HasForeignKey(d => d.IdCourse)
                    .HasConstraintName("FK_CHAPTER_COURSE");
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

                entity.HasOne(d => d.IdCategoryNavigation)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.IdCategory)
                    .HasConstraintName("FK_COURSE_CATEGORY");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK_COURSE_USER");
            });

            modelBuilder.Entity<Degree>(entity =>
            {
                entity.HasKey(e => e.IdDegree);

                entity.ToTable("DEGREE");

                entity.Property(e => e.IdDegree)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_Degree");

                entity.Property(e => e.IdUser).HasColumnName("ID_User");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Degrees)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK_DEGREE_USER");
            });

            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.HasKey(e => e.IdLesson);

                entity.ToTable("LESSON");

                entity.Property(e => e.IdLesson)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_Lesson");

                entity.Property(e => e.IdChapter).HasColumnName("ID_Chapter");

                entity.HasOne(d => d.IdChapterNavigation)
                    .WithMany(p => p.Lessons)
                    .HasForeignKey(d => d.IdChapter)
                    .HasConstraintName("FK_LESSON_CHAPTER");
            });

            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.HasKey(e => new { e.IdUser, e.IdCourse });

                entity.ToTable("PURCHASE");

                entity.Property(e => e.IdUser).HasColumnName("ID_User");

                entity.Property(e => e.IdCourse).HasColumnName("ID_Course");

                entity.Property(e => e.DateOfPurchase).HasColumnType("datetime");

                entity.Property(e => e.IdTrade).HasColumnName("ID_Trade");

                entity.HasOne(d => d.IdCourseNavigation)
                    .WithMany(p => p.Purchases)
                    .HasForeignKey(d => d.IdCourse)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PURCHASE_COURSE");

                entity.HasOne(d => d.IdTradeNavigation)
                    .WithMany(p => p.Purchases)
                    .HasForeignKey(d => d.IdTrade)
                    .HasConstraintName("FK_PURCHASE_TRADE_DETAIL");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Purchases)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PURCHASE_USER");
            });

            modelBuilder.Entity<Quiz>(entity =>
            {
                entity.HasKey(e => e.IdQuiz);

                entity.ToTable("QUIZ");

                entity.Property(e => e.IdQuiz)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_Quiz");

                entity.Property(e => e.IdLesson).HasColumnName("ID_Lesson");

                entity.Property(e => e.Option1).HasColumnName("Option_1");

                entity.Property(e => e.Option2).HasColumnName("Option_2");

                entity.Property(e => e.Option3).HasColumnName("Option_3");

                entity.Property(e => e.Option4).HasColumnName("Option_4");

                entity.HasOne(d => d.IdLessonNavigation)
                    .WithMany(p => p.Quizzes)
                    .HasForeignKey(d => d.IdLesson)
                    .HasConstraintName("FK_QUIZ_LESSON");
            });

            modelBuilder.Entity<Rate>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("RATE");

                entity.Property(e => e.IdCourse).HasColumnName("ID_Course");

                entity.Property(e => e.IdUser).HasColumnName("ID_User");

                entity.Property(e => e.Rate1).HasColumnName("Rate");

                entity.HasOne(d => d.IdCourseNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdCourse)
                    .HasConstraintName("FK_RATE_COURSE");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RATE_USER");
            });

            modelBuilder.Entity<Study>(entity =>
            {
                entity.HasKey(e => new { e.IdUser, e.IdLesson });

                entity.ToTable("STUDY");

                entity.Property(e => e.IdUser).HasColumnName("ID_User");

                entity.Property(e => e.IdLesson).HasColumnName("ID_Lesson");

                entity.HasOne(d => d.IdLessonNavigation)
                    .WithMany(p => p.Studies)
                    .HasForeignKey(d => d.IdLesson)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_STUDY_LESSON");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Studies)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_STUDY_USER");
            });

            modelBuilder.Entity<TradeDetail>(entity =>
            {
                entity.HasKey(e => e.IdTrade);

                entity.ToTable("TRADE_DETAIL");

                entity.Property(e => e.IdTrade)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_Trade");

                entity.Property(e => e.Balance).HasMaxLength(50);

                entity.Property(e => e.DateOfTrade).HasColumnType("datetime");

                entity.Property(e => e.IdUser).HasColumnName("ID_User");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.TradeDetails)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK_TRADE_DETAIL_USER");
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
