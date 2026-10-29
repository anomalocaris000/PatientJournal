namespace PatientJournal.Models;

public class PatState
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public DateTime RecordDate { get; set; }
    public string Condition { get; set; } = "";
    public string Temperature { get; set; } = "";
    public string Notes { get; set; } = "";
}
