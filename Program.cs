using PatientJournal.Services;

var service = new HospitalService();

while (true)
{
    Console.WriteLine("1. Добавить палату");
    Console.WriteLine("2. Список палат");
    Console.WriteLine("3. Редактировать палату");
    Console.WriteLine("4. Добавить врача");
    Console.WriteLine("5. Список врачей");
    Console.WriteLine("6. Редактировать врача");
    Console.WriteLine("7. Добавить больного");
    Console.WriteLine("8. Список больных");
    Console.WriteLine("9. Редактировать больного");
    Console.WriteLine("10. Добавить состояние больного");
    Console.WriteLine("11. Редактировать состояние");
    Console.WriteLine("12. Состояние заданного больного");
    Console.WriteLine("13. Количество больных в палате");
    Console.WriteLine("14. Количество больных у врача");
    Console.WriteLine("0. Выход");
    Console.Write("Выбор: ");

    switch (Console.ReadLine())
    {
        case "1":
            Console.Write("Номер палаты: ");
            var roomNumber = ReadText();
            Console.Write("Вместимость: ");
            var capacity = ReadInt();
            var room = service.AddRoom(roomNumber, capacity);
            Console.WriteLine(room == null ? "Неверная вместимость" : $"Палата добавлена. ID: {room.Id}");
            break;

        case "2":
            foreach (var r in service.Rooms)
                Console.WriteLine($"{r.Id}. Палата {r.Number}, вместимость {r.Capacity}, больных {service.CountPatientsInRoom(r.Id)}");
            break;

        case "3":
            Console.Write("ID палаты: ");
            var editRoomId = ReadInt();
            Console.Write("Номер палаты: ");
            var editRoomNumber = ReadText();
            Console.Write("Вместимость: ");
            var editCapacity = ReadInt();
            Console.WriteLine(service.EditRoom(editRoomId, editRoomNumber, editCapacity) ? "Палата изменена" : "Палата не найдена или вместимость меньше числа больных");
            break;

        case "4":
            Console.Write("ФИО врача: ");
            var doctorName = ReadText();
            Console.Write("Специализация: ");
            var specialization = ReadText();
            Console.WriteLine($"Врач добавлен. ID: {service.AddDoctor(doctorName, specialization).Id}");
            break;

        case "5":
            foreach (var d in service.Doctors)
                Console.WriteLine($"{d.Id}. {d.FullName} | {d.Specialization} | больных: {service.CountPatientsForDoctor(d.Id)}");
            break;

        case "6":
            Console.Write("ID врача: ");
            var editDoctorId = ReadInt();
            Console.Write("ФИО врача: ");
            var editDoctorName = ReadText();
            Console.Write("Специализация: ");
            var editSpecialization = ReadText();
            Console.WriteLine(service.EditDoctor(editDoctorId, editDoctorName, editSpecialization) ? "Врач изменен" : "Врач не найден");
            break;

        case "7":
            if (!ShowReferences(service))
                break;

            Console.Write("ФИО больного: ");
            var patientName = ReadText();
            Console.Write("Возраст: ");
            var age = ReadInt();
            Console.Write("ID палаты: ");
            var patientRoomId = ReadInt();
            Console.Write("ID лечащего врача: ");
            var patientDoctorId = ReadInt();

            var selectedRoom = service.FindRoom(patientRoomId);
            if (age <= 0 || selectedRoom == null || service.FindDoctor(patientDoctorId) == null)
            {
                Console.WriteLine("Палата или врач не найдены");
                break;
            }

            if (service.CountPatientsInRoom(patientRoomId) >= selectedRoom.Capacity)
            {
                Console.WriteLine("В палате нет свободных мест");
                break;
            }

            Console.WriteLine($"Больной добавлен. ID: {service.AddPatient(patientName, age, patientRoomId, patientDoctorId).Id}");
            break;

        case "8":
            foreach (var p in service.Patients)
                Console.WriteLine($"{p.Id}. {p.FullName}, {p.Age} лет, палата {service.FindRoom(p.RoomId)?.Number}, врач {service.FindDoctor(p.DoctorId)?.FullName}");
            break;

        case "9":
            Console.Write("ID больного: ");
            var editPatientId = ReadInt();
            if (!ShowReferences(service))
                break;

            Console.Write("ФИО: ");
            var editPatientName = ReadText();
            Console.Write("Возраст: ");
            var editAge = ReadInt();
            Console.Write("ID палаты: ");
            var editPatientRoomId = ReadInt();
            Console.Write("ID лечащего врача: ");
            var editPatientDoctorId = ReadInt();

            Console.WriteLine(service.EditPatient(editPatientId, editPatientName, editAge, editPatientRoomId, editPatientDoctorId)
                ? "Данные больного изменены"
                : "Проверьте ID, возраст и вместимость палаты");
            break;

        case "10":
            if (!service.Patients.Any())
            {
                Console.WriteLine("Сначала добавьте больного");
                break;
            }

            foreach (var p in service.Patients)
                Console.WriteLine($"{p.Id}. {p.FullName}");

            Console.Write("ID больного: ");
            var statePatientId = ReadInt();
            Console.Write("Состояние: ");
            var condition = ReadText();
            Console.Write("Температура: ");
            var temperature = ReadText();
            Console.Write("Примечания: ");
            var notes = ReadText();

            var state = service.AddState(statePatientId, condition, temperature, notes);
            Console.WriteLine(state == null ? "Больной не найден" : $"Запись добавлена. ID: {state.Id}");
            break;

        case "11":
            Console.Write("ID записи состояния: ");
            var stateId = ReadInt();
            Console.Write("Состояние: ");
            var editCondition = ReadText();
            Console.Write("Температура: ");
            var editTemperature = ReadText();
            Console.Write("Примечания: ");
            var editNotes = ReadText();

            Console.WriteLine(service.EditState(stateId, editCondition, editTemperature, editNotes)
                ? "Состояние изменено"
                : "Запись не найдена");
            break;

        case "12":
            Console.Write("ID больного: ");
            var searchPatientId = ReadInt();
            var patient = service.FindPatient(searchPatientId);

            if (patient == null)
            {
                Console.WriteLine("Больной не найден");
                break;
            }

            Console.WriteLine($"Больной: {patient.FullName}");
            Console.WriteLine($"Возраст: {patient.Age}");
            Console.WriteLine($"Палата: {service.FindRoom(patient.RoomId)?.Number}");
            Console.WriteLine($"Лечащий врач: {service.FindDoctor(patient.DoctorId)?.FullName}");

            var patientStates = service.GetPatientStates(searchPatientId).ToList();
            if (patientStates.Count == 0)
                Console.WriteLine("Записей о состоянии пока нет");

            foreach (var s in patientStates)
                Console.WriteLine($"{s.Id}. {s.RecordDate:dd.MM.yyyy HH:mm} | {s.Condition} | Температура: {s.Temperature} | {s.Notes}");
            break;

        case "13":
            foreach (var r in service.Rooms)
                Console.WriteLine($"{r.Id}. Палата {r.Number}");
            Console.Write("ID палаты: ");
            var countRoomId = ReadInt();
            var countRoom = service.FindRoom(countRoomId);
            Console.WriteLine(countRoom == null
                ? "Палата не найдена"
                : $"Количество больных в палате {countRoom.Number}: {service.CountPatientsInRoom(countRoomId)}");
            break;

        case "14":
            foreach (var d in service.Doctors)
                Console.WriteLine($"{d.Id}. {d.FullName}");
            Console.Write("ID врача: ");
            var countDoctorId = ReadInt();
            var countDoctor = service.FindDoctor(countDoctorId);
            Console.WriteLine(countDoctor == null
                ? "Врач не найден"
                : $"Количество больных у врача {countDoctor.FullName}: {service.CountPatientsForDoctor(countDoctorId)}");
            break;

        case "0":
            return;

        default:
            Console.WriteLine("Нет такого пункта");
            break;
    }

    Console.WriteLine();
}

static string ReadText()
{
    while (true)
    {
        var value = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(value))
            return value.Trim();

        Console.Write("Введите значение: ");
    }
}

static int ReadInt()
{
    while (true)
    {
        if (int.TryParse(Console.ReadLine(), out var value) && value > 0)
            return value;

        Console.Write("Введите положительное целое число: ");
    }
}

static bool ShowReferences(HospitalService service)
{
    if (!service.Rooms.Any() || !service.Doctors.Any())
    {
        Console.WriteLine("Сначала добавьте хотя бы одну палату и одного врача");
        return false;
    }

    Console.WriteLine("Палаты:");
    foreach (var r in service.Rooms)
        Console.WriteLine($"{r.Id}. {r.Number} ({service.CountPatientsInRoom(r.Id)}/{r.Capacity})");

    Console.WriteLine("Врачи:");
    foreach (var d in service.Doctors)
        Console.WriteLine($"{d.Id}. {d.FullName}");

    return true;
}
