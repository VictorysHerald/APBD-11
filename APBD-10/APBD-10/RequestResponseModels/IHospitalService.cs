using APBD_10.RequestResponseModels.ResponseModels;
using Prescription = APBD_10.Models.Prescription;

namespace APBD_10.RequestResponseModels;

public interface IHospitalService
{
    Task AddPrescription(Prescription request);

    Task<PatientPrescriptions> GetPatientPrescriptions(int id);
}