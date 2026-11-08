using System.Text;

string path = Path.Combine(AppContext.BaseDirectory, "numbers.txt");
var encoding = new UTF8Encoding(false);
int first = 1, second = 25, third = 307, replacement = 42;

using (var writer = new StreamWriter(path, false, encoding)) {
    writer.NewLine = "\n";
    writer.WriteLine(first.ToString("D8"));
}
Console.WriteLine($"1. Записано: {first:D8}");

using (var reader = new StreamReader(path, encoding)) {
    Console.WriteLine($"2. Прочитано число: {int.Parse(reader.ReadLine()!)}");
}

using (var writer = new StreamWriter(path, true, encoding)) {
    writer.NewLine = "\n";
    writer.WriteLine(second.ToString("D8"));
    writer.WriteLine(third.ToString("D8"));
}
Console.WriteLine($"3. Дописано: {second:D8}, {third:D8}");

Console.WriteLine("4. Вміст до заміни:");
print_lines();

using (var stream = new FileStream(path, FileMode.Open, FileAccess.Write)) {
    stream.Seek(encoding.GetByteCount(first.ToString("D8") + "\n"), SeekOrigin.Begin);
    using var writer = new StreamWriter(stream, encoding);
    writer.Write(replacement.ToString("D8"));
}

Console.WriteLine("5. Вміст після заміни другого числа:");
print_lines();
Console.WriteLine($"Файл: {path}");

void print_lines() {
    using var reader = new StreamReader(path, encoding);
    int line_number = 0;

    while (reader.ReadLine() is { } line) {
        Console.WriteLine($"{++line_number}: {line}");
    }
}
