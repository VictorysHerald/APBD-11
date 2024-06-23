namespace APBD_10.Models;

public class Prescription
{
    public int IdPrescription { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime Date { get; set; }
    public int IdPatient { get; set; }
    public int IdDoctor { get; set; }

    //polaczenie do obiektow - doktora i pacienta
    public Patient Patient { get; set; }
    public Doctor Doctor { get; set; }
    
    //polaczenie do Medicament_Prescription
    public ICollection<Prescription_Medicament> PrescriptionMedicaments { get; set; } 
}