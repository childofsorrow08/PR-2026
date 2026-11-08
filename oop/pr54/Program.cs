using System.Text;

List<Student> students = [
    new() { OrderNo = 1, StudentName = "Іван" },
    new() { OrderNo = 2, StudentName = "Олена Коваль" },
    new() { OrderNo = 3, StudentName = "Марія" },
    new() { OrderNo = 4, StudentName = "Олександр Петренко" },
    new() { OrderNo = 5, StudentName = "" }
];

string path = Path.Combine(AppContext.BaseDirectory, "students.bin");

using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
using (var writer = new BinaryWriter(stream, Encoding.UTF8)) {
    foreach (Student student in students) write_student(writer, student);
}

List<Student> restored = [];

using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
using (var reader = new BinaryReader(stream, Encoding.UTF8)) {
    while (stream.Position < stream.Length) restored.Add(read_student(reader));
}

Console.WriteLine("Відновлений список:");
foreach (Student student in restored) Console.WriteLine($"{student.OrderNo}: {student.StudentName}");

bool same = students.Count == restored.Count;

for (int i = 0; same && i < students.Count; i++) {
    same = students[i].OrderNo == restored[i].OrderNo &&
        students[i].StudentName == restored[i].StudentName;
}
Console.WriteLine($"Списки збігаються: {same}");

int expected_size = 0;

foreach (Student student in students) {
    int name_size = Encoding.UTF8.GetByteCount(student.StudentName);
    expected_size += 8 + name_size;
    Console.WriteLine($"{student.OrderNo}: ім'я {name_size} байтів, запис {8 + name_size} байтів");
}

long actual_size = new FileInfo(path).Length;
Console.WriteLine($"Очікуваний розмір: {expected_size}; фактичний: {actual_size} байтів");
Console.WriteLine($"Файл: {path}");

if (!same || actual_size != expected_size) throw new InvalidDataException("Перевірка файла не пройдена.");

static void write_student(BinaryWriter writer, Student student) {
    byte[] bytes = Encoding.UTF8.GetBytes(student.StudentName);

    writer.Write(student.OrderNo);
    writer.Write(bytes.Length);
    writer.Write(bytes);
}

static Student read_student(BinaryReader reader) {
    int order_no = reader.ReadInt32();
    int length = reader.ReadInt32();

    if (length < 0 || length > reader.BaseStream.Length - reader.BaseStream.Position) {
        throw new InvalidDataException("Некоректна довжина імені.");
    }

    byte[] bytes = reader.ReadBytes(length);
    if (bytes.Length != length) throw new EndOfStreamException("Запис обірвано.");

    return new Student {
        OrderNo = order_no,
        StudentName = new UTF8Encoding(false, true).GetString(bytes)
    };
}
