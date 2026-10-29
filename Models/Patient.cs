namespace PatientJournal.Models;

public class Patient
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public int Age { get; set; }
    public int RoomId { get; set; }
    public int DoctorId { get; set; }
}
