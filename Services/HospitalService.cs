using PatientJournal.Models;

namespace PatientJournal.Services;

public class HospitalService
{
    private readonly Dictionary<int, Patient> patients = new();
    private readonly Dictionary<int, Room> rooms = new();
    private readonly Dictionary<int, PatState> states = new();
    private readonly Dictionary<int, Doctor> doctors = new();

    private int nextPatientId = 1;
    private int nextRoomId = 1;
    private int nextStateId = 1;
    private int nextDoctorId = 1;

    public IEnumerable<Patient> Patients => patients.Values;
    public IEnumerable<Room> Rooms => rooms.Values;
    public IEnumerable<PatState> States => states.Values;
    public IEnumerable<Doctor> Doctors => doctors.Values;

    public Patient AddPatient(string name, int age, int roomId, int doctorId)
    {
        var patient = new Patient
        {
            Id = nextPatientId++,
            FullName = name,
            Age = age,
            RoomId = roomId,
            DoctorId = doctorId
        };

        patients.Add(patient.Id, patient);
        return patient;
    }

    public bool EditPatient(int id, string name, int age, int roomId, int doctorId)
    {
        if (!patients.TryGetValue(id, out var patient) ||
            !rooms.ContainsKey(roomId) ||
            !doctors.ContainsKey(doctorId) ||
            age <= 0)
            return false;

        if (rooms[roomId].Capacity < patients.Values.Count(p => p.RoomId == roomId && p.Id != id))
            return false;

        patient.FullName = name;
        patient.Age = age;
        patient.RoomId = roomId;
        patient.DoctorId = doctorId;
        return true;
    }

    public Room? AddRoom(string number, int capacity)
    {
        if (capacity <= 0)
            return null;

        var room = new Room
        {
            Id = nextRoomId++,
            Number = number,
            Capacity = capacity
        };

        rooms.Add(room.Id, room);
        return room;
    }

    public bool EditRoom(int id, string number, int capacity)
    {
        if (!rooms.TryGetValue(id, out var room) ||
            capacity <= 0 ||
            patients.Values.Count(p => p.RoomId == id) > capacity)
            return false;

        room.Number = number;
        room.Capacity = capacity;
        return true;
    }

    public Doctor AddDoctor(string name, string specialization)
    {
        var doctor = new Doctor
        {
            Id = nextDoctorId++,
            FullName = name,
            Specialization = specialization
        };

        doctors.Add(doctor.Id, doctor);
        return doctor;
    }

    public bool EditDoctor(int id, string name, string specialization)
    {
        if (!doctors.TryGetValue(id, out var doctor))
            return false;

        doctor.FullName = name;
        doctor.Specialization = specialization;
        return true;
    }

    public PatState? AddState(int patientId, string condition, string temperature, string notes)
    {
        if (!patients.ContainsKey(patientId))
            return null;

        var state = new PatState
        {
            Id = nextStateId++,
            PatientId = patientId,
            RecordDate = DateTime.Now,
            Condition = condition,
            Temperature = temperature,
            Notes = notes
        };

        states.Add(state.Id, state);
        return state;
    }

    public bool EditState(int id, string condition, string temperature, string notes)
    {
        if (!states.TryGetValue(id, out var state))
            return false;

        state.Condition = condition;
        state.Temperature = temperature;
        state.Notes = notes;
        state.RecordDate = DateTime.Now;
        return true;
    }

    public IEnumerable<PatState> GetPatientStates(int patientId) =>
        states.Values
            .Where(s => s.PatientId == patientId)
            .OrderByDescending(s => s.RecordDate);

    public int CountPatientsInRoom(int roomId) =>
        patients.Values.Count(p => p.RoomId == roomId);

    public int CountPatientsForDoctor(int doctorId) =>
        patients.Values.Count(p => p.DoctorId == doctorId);

    public Patient? FindPatient(int id) => patients.GetValueOrDefault(id);
    public Room? FindRoom(int id) => rooms.GetValueOrDefault(id);
    public Doctor? FindDoctor(int id) => doctors.GetValueOrDefault(id);
}
