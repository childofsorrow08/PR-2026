using System.Globalization;

while (true) {
    Console.Write("Введіть ціле число: ");
    string? input = Console.ReadLine();
    if (input is null) break;

    try {
        int number = Int32.Parse(input, CultureInfo.InvariantCulture);
        Console.WriteLine($"Отримане число: {number}");
        break;
    } catch (FormatException) {
        Console.WriteLine("Введене значення не є цілим числом. Спробуйте знову.");
    } catch (OverflowException) {
        Console.WriteLine("Значення виходить за межі діапазону Int32. Спробуйте знову.");
    }
}
