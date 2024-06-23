namespace APBD_10.RequestResponseModels.RequestModels;

public class Prescription
{
    public Patient patient { get; set; }
    public List<Medicament> medicaments { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public int DoctorId { get; set; }
}