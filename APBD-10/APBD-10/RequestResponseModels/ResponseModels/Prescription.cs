namespace APBD_10.RequestResponseModels.ResponseModels;

public class Prescription
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public List<Medicament> Medicaments { get; set; }
    public Doctor Doctor { get; set; }
}