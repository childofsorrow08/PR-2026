int[] numbers = new int[20];
for (int i = 0; i < numbers.Length; i++) numbers[i] = Random.Shared.Next(1, 13);

Thread[] threads = new Thread[numbers.Length];

for (int i = 0; i < numbers.Length; i++) {
    int num = numbers[i];

    threads[i] = new Thread(() => {
        long factorial = 1;
        for (int factor = 2; factor <= num; factor++) factorial *= factor;
        Console.WriteLine($"факторіал числа: {num}! = {factorial}");
    });

    threads[i].Start();
}

foreach (Thread thread in threads) thread.Join();
Console.WriteLine("Розрахунок закінчено.");
