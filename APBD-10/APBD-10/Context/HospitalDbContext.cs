using APBD_10.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD_10.Context;

public class HospitalDbContext(DbContextOptions<HospitalDbContext> options) : DbContext(options)
{
    
    //DbSet do zapamietania setow tebelek
    public DbSet<Doctor> Doctors  { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Prescription_Medicament> PrescriptionMedicaments { get; set; }

    //connection string
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("");
    }

    //konteksty do tworzenia tabel
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Doctor>(opt =>
        {
            opt.HasKey(e => e.IdDoctor);
            opt.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
            opt.Property(e => e.LastName).HasMaxLength(100).IsRequired();
            opt.Property(e => e.Email).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<Patient>(opt =>
        {
            opt.HasKey(e => e.IdPatient);
            opt.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
            opt.Property(e => e.LastName).HasMaxLength(100).IsRequired();
            //na date urodzenia nie trzeba bo domyslnie musi byc
        });

        modelBuilder.Entity<Medicament>(opt =>
        {
            opt.HasKey(e => e.IdMedicament);
            opt.Property(e => e.Name).HasMaxLength(100).IsRequired();
            opt.Property(e => e.Description).HasMaxLength(100).IsRequired();
            opt.Property(e => e.Type).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<Prescription>(opt =>
        {
            opt.HasKey(e => e.IdPrescription);
            //itd definicje pol
            
            //relacje do doktora
            opt.HasOne(e => e.Doctor)
                .WithMany(e => e.Prescriptions)
                .HasForeignKey(e => e.IdDoctor);
            
            //relacje do pacjenta
            opt.HasOne(e => e.Patient)
                .WithMany(e => e.Prescriptions)
                .HasForeignKey(e => e.IdPatient);
        });

        modelBuilder.Entity<Prescription_Medicament>(opt =>
        {
            //laczony klucz glowny
            opt.HasKey(e => new
            {
                e.IdPrescription,
                e.IdMedicament
            });
            opt.Property(e => e.Details).HasMaxLength(100).IsRequired();
            
            //relacje do Prescription
            opt.HasOne(e => e.Prescription)
                .WithMany(e => e.PrescriptionMedicaments)
                .HasForeignKey(e => e.IdPrescription);
            
            //relacje do Medicament
            opt.HasOne(e => e.Medicament)
                .WithMany(e => e.PrescriptionMedicaments)
                .HasForeignKey(e => e.IdMedicament);
        });
    }
}