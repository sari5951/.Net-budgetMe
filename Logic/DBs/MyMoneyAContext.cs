using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Logic;

public partial class MyMoneyAContext : DbContext
{
    public MyMoneyAContext()
    {
    }

    public MyMoneyAContext(DbContextOptions<MyMoneyAContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Area> Areas { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Debt> Debts { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<HeaderDatum> HeaderData { get; set; }

    public virtual DbSet<Moving> Movings { get; set; }

    public virtual DbSet<Organization> Organizations { get; set; }

    public virtual DbSet<PayOption> PayOptions { get; set; }

    public virtual DbSet<Presence> Presences { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<UrgencyDebt> UrgencyDebts { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<User2Area> User2Areas { get; set; }

    public virtual DbSet<UserType> UserTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=MyMoneyA;user id=kollel;password=1234qwe!;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Area>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Subject");

            entity.ToTable("Area");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cities__3214EC07FEB68847");
        });

        modelBuilder.Entity<Debt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Debts__3214EC07332EBDD5");

            entity.HasOne(d => d.Area).WithMany(p => p.Debts)
                .HasForeignKey(d => d.AreaId)
                .HasConstraintName("FK_Debts_User2Area");

            entity.HasOne(d => d.Urgency).WithMany(p => p.Debts)
                .HasForeignKey(d => d.UrgencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Debts__UrgencyId__3F466844");

            entity.HasOne(d => d.User).WithMany(p => p.Debts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Debts__UserId__75A278F5");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Document__3214EC070BBE28C6");

            entity.Property(e => e.Content).HasColumnType("image");

            entity.HasOne(d => d.User).WithMany(p => p.Documents)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Documents__UserI__160F4887");
        });

        modelBuilder.Entity<HeaderDatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HeaderDa__3214EC07E00AD341");

            entity.Property(e => e.FileName).IsUnicode(false);

            entity.HasOne(d => d.Manager).WithMany(p => p.HeaderData)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK__HeaderDat__Manag__5DCAEF64");
        });

        modelBuilder.Entity<Moving>(entity =>
        {
            entity.ToTable("Moving");

            entity.Property(e => e.Date).HasColumnType("datetime");

            entity.HasOne(d => d.PayOption).WithMany(p => p.Movings)
                .HasForeignKey(d => d.PayOptionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Moving_PayOption");

            entity.HasOne(d => d.User2Area).WithMany(p => p.Movings)
                .HasForeignKey(d => d.User2AreaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Moving_User2Area");
        });

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Organiza__3214EC07B9590C3D");

            entity.ToTable("Organization");
        });

        modelBuilder.Entity<PayOption>(entity =>
        {
            entity.ToTable("PayOption");
        });

        modelBuilder.Entity<Presence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Presence__3214EC07E60DBA80");

            entity.ToTable("Presence");

            entity.Property(e => e.Finish).HasColumnType("datetime");
            entity.Property(e => e.Note).IsUnicode(false);
            entity.Property(e => e.Start).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Presences)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Presence__UserId__71D1E811");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Status__3214EC07753F9ACB");

            entity.ToTable("Status");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tasks__3214EC07A4173A94");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DoDate).HasColumnType("datetime");

            entity.HasOne(d => d.Status).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tasks__StatusId__2CF2ADDF");

            entity.HasOne(d => d.Urgency).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.UrgencyId)
                .HasConstraintName("FK__Tasks__UrgencyId__2DE6D218");

            entity.HasOne(d => d.User).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tasks__UserId__2BFE89A6");
        });

        modelBuilder.Entity<UrgencyDebt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UrgencyD__3214EC0756D92D2E");

            entity.ToTable("UrgencyDebt");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.PayDate).HasColumnType("datetime");
            entity.Property(e => e.RegisterDate).HasColumnType("datetime");

            entity.HasOne(d => d.City).WithMany(p => p.Users)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK_User_City");

            entity.HasOne(d => d.Lender).WithMany(p => p.InverseLender)
                .HasForeignKey(d => d.LenderId)
                .HasConstraintName("FK_User_User");

            entity.HasOne(d => d.Manager).WithMany(p => p.InverseManager)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK__User__ManagerId__17036CC0");

            entity.HasOne(d => d.Organization).WithMany(p => p.Users)
                .HasForeignKey(d => d.OrganizationId)
                .HasConstraintName("FK_User_Organization");

            entity.HasOne(d => d.UserType).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_UserType");
        });

        modelBuilder.Entity<User2Area>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User2Are__3214EC07D61CEA9B");

            entity.ToTable("User2Area");

            entity.Property(e => e.IsActive).HasDefaultValue(false);
            entity.Property(e => e.IsMaaser).HasDefaultValue(false);
            entity.Property(e => e.Type).HasDefaultValue(1);

            entity.HasOne(d => d.Debt).WithMany(p => p.User2Areas)
                .HasForeignKey(d => d.DebtId)
                .HasConstraintName("FK__User2Subj__DebtI__02FC7413");

            entity.HasOne(d => d.User).WithMany(p => p.User2Areas)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User2Area_User");
        });

        modelBuilder.Entity<UserType>(entity =>
        {
            entity.ToTable("UserType");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
