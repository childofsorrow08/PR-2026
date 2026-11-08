Console.Write("Введіть ім'я: ");
string? name = Console.ReadLine()?.Trim();
if (string.IsNullOrWhiteSpace(name)) {
    Console.WriteLine("Ім'я не може бути порожнім.");
    return;
}
Console.WriteLine($"Вітаю {name}!");
while (true) {
    Console.Write("Введіть вік: ");
    string? input = Console.ReadLine();
    if (input is null) return;
    if (int.TryParse(input, out int age) && age >= 0) {
        Console.WriteLine($"Користувач: {name}. Вік: {age}.");
        Console.WriteLine(age >= 18 ? "Повнолітній користувач." : "Неповнолітній користувач.");
        break;
    }
    Console.WriteLine("Вік має бути невід'ємним цілим числом.");
}
