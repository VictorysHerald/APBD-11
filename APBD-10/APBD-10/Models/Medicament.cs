namespace APBD_10.Models;

public class Medicament
{
    public int IdMedicament { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    
    //polaczenie do Medicament_Prescription
    public ICollection<Prescription_Medicament> PrescriptionMedicaments { get; set; } 
}