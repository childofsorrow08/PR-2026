int[] numbers = [8, 2, 5, 2, 9];
string[] words = ["кіт", "птах", "лев", "птах"];

show(numbers, 2);
show(words, "птах");
Console.WriteLine($"Відсутнє значення: {ArrayHelper.IndexOf(numbers, 100)}");

static void show<T>(T[] values, T target) where T : IComparable<T> {
    Console.WriteLine($"Масив: {string.Join(", ", values)}");
    Console.WriteLine($"Перший індекс {target}: {ArrayHelper.IndexOf(values, target)}");
    Console.WriteLine($"Мінімум: {ArrayHelper.Min(values)}, максимум: {ArrayHelper.Max(values)}");

    ArrayHelper.Reverse(values);
    Console.WriteLine($"Після розвороту: {string.Join(", ", values)}");
}
