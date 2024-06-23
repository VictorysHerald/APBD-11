namespace APBD_10.Models;

public class Prescription_Medicament
{
    public int IdMedicament { get; set; }
    public int IdPrescription { get; set; }
    public int? Dose { get; set; }
    public string Details { get; set; }
    
    //odniesienie do obiektow medicament i prescription
    public Medicament Medicament { get; set; }
    public Prescription Prescription { get; set; }
}