using System.ComponentModel.DataAnnotations;

namespace APBD_10.Models;

public class Doctor
{
    public int IdDoctor { get; set; }
    public string FirstName  { get; set; }
    public string LastName  { get; set; }
    public string Email  { get; set; }
    
    //polaczenie doktora do prescriptions
    public ICollection<Prescription> Prescriptions { get; set; }
}