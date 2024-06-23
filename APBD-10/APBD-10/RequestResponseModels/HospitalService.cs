using System.Data;
using APBD_10.Context;
using APBD_10.Models;
using APBD_10.RequestResponseModels.ResponseModels;
using Microsoft.EntityFrameworkCore;
using Doctor = APBD_10.Models.Doctor;
using Prescription = APBD_10.Models.Prescription;


namespace APBD_10.RequestResponseModels;

public class HospitalService : IHospitalService
{
    private readonly HospitalDbContext _context;

    public HospitalService(HospitalDbContext context)
    {
        _context = context;
    }

    
    // Task AddPrescription(Prescription request);
    // Task<PatientPrescriptions> GetPatientPrescriptions(int id);

    public async Task AddPrescription(Prescription request)
    {
        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.IdPatient == request.Patient.IdPatient);

        //in case there is no patient found in database
        if (patient == null)
        {
            Console.WriteLine("Patient not found in the DB. Proceeding to add a new patient...");
            patient = new Models.Patient
            {
                FirstName = request.Patient.FirstName,
                LastName = request.Patient.LastName,
                Birthdate = request.Patient.Birthdate
            };
            _context.Patients.Add(patient);
        }
        
        //in case there is no doctor found in database
        var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.IdDoctor == request.IdDoctor);
        if (doctor == null)
        {
            throw new DataException("Doctors not found in DB.");
        }

        var requestedmedicamentsIds = request.PrescriptionMedicaments.Select(m => m.IdMedicament).ToList();
        var medicaments = await _context.Medicaments.Where(m => requestedmedicamentsIds.Contains(m.IdMedicament))
            .ToListAsync();
        
        //sprawdzenie DueDate
        if (request.DueDate <= request.Date)
        {
            throw new DataException("Due date must be later than the date of the prescription.");
        }

        //sprawdzenie liczby pozycji
        if (medicaments.Count() > 10)
        {
            throw new DataException("Medicament count over 10.");
        }

        //zwrot danych jako lista zawierajaca wszyskie zmienne
        var prescription = new Models.Prescription()
        {
            Date = request.Date,
            DueDate = request.DueDate,
            IdDoctor = request.IdDoctor,
            Patient = patient,
            PrescriptionMedicaments = request.PrescriptionMedicaments.Select(m => new Prescription_Medicament()
            {
                IdMedicament = m.IdMedicament,
                Dose = m.Dose,
                Details = m.Details
            }).ToList()
        };

        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync();
    }

    public async Task<PatientPrescriptions> GetPatientPrescriptions(int id)
    {
        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.IdPatient == id);

        if (patient == null)
        {
            throw new DataException("Patient of given ID not found in DB.");
        }

        var prescriptions = await _context.Prescriptions
            .Where(p => p.IdPatient == id)
            .Include(p => p.Doctor)
            .Include(p => p.PrescriptionMedicaments)
            .ThenInclude(pm => pm.Medicament)
            .ToListAsync();

        return new PatientPrescriptions
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            BirthDate = patient.Birthdate,
            Prescriptions = prescriptions.Select(p => new ResponseModels.Prescription
            {
                IdPrescription = p.IdPrescription,
                Date = p.Date,
                DueDate = p.DueDate,
                Doctor = new ResponseModels.Doctor
                {
                    IdDoctor = p.Doctor.IdDoctor,
                    FirstName = p.Doctor.FirstName,
                },
                Medicaments = p.PrescriptionMedicaments.Select(pm => new ResponseModels.Medicament()
                {
                    IdMedicament = pm.Medicament.IdMedicament,
                    Name = pm.Medicament.Name,
                    //Dose = pm.Dose,           nullable - jak to tu zrobic?
                    Details = pm.Details
                }).ToList()
            }).ToList()
        };
    }
}