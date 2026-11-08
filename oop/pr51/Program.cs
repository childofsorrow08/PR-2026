int[] fullArray = new int[1000];
for (int i = 0; i < fullArray.Length; i++) fullArray[i] = Random.Shared.Next(1, 11);

List<int> evenList = [];
List<int> oddList = [];

object index_lock = new();
object even_lock = new();
object odd_lock = new();
int next_index = 0;

Thread[] threads = new Thread[3];

for (int i = 0; i < threads.Length; i++) {
    threads[i] = new Thread(process);
    threads[i].Start();
}

foreach (Thread thread in threads) thread.Join();

Console.WriteLine($"Парні числа ({evenList.Count}): {string.Join(", ", evenList)}");
Console.WriteLine($"Непарні числа ({oddList.Count}): {string.Join(", ", oddList)}");
Console.WriteLine($"Оброблено: {evenList.Count + oddList.Count} із {fullArray.Length}");

if (evenList.Count + oddList.Count != fullArray.Length ||
    evenList.Any(x => x % 2 != 0) || oddList.Any(x => x % 2 == 0) ||
    !fullArray.Order().SequenceEqual(evenList.Concat(oddList).Order())) {
    throw new InvalidOperationException("Результат розподілу некоректний.");
}

void process() {
    while (true) {
        int value;

        lock (index_lock) {
            if (next_index == fullArray.Length) return;
            value = fullArray[next_index++];
        }

        if (value % 2 == 0) {
            lock (even_lock) evenList.Add(value);
        } else {
            lock (odd_lock) oddList.Add(value);
        }
    }
}
