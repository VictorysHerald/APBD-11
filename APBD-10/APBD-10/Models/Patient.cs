namespace APBD_10.Models;

public class Patient
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birthdate { get; set; }

    //polaczenie pacjenta do prescriptions
    public ICollection<Prescription> Prescriptions { get; set; }
}