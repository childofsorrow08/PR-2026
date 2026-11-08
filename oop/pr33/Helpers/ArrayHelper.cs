public static class ArrayHelper {
    public static int IndexOf<T>(T[] array, T value) {
        ArgumentNullException.ThrowIfNull(array);

        for (int i = 0; i < array.Length; i++) {
            if (EqualityComparer<T>.Default.Equals(array[i], value)) return i;
        }

        return -1;
    }

    public static void Reverse<T>(T[] array) {
        ArgumentNullException.ThrowIfNull(array);

        for (int i = 0; i < array.Length / 2; i++) {
            T temp = array[i];
            array[i] = array[array.Length - i - 1];
            array[array.Length - i - 1] = temp;
        }
    }

    public static T Min<T>(T[] array) where T : IComparable<T> {
        check_not_empty(array);
        T result = array[0];

        for (int i = 1; i < array.Length; i++) {
            if (Comparer<T>.Default.Compare(array[i], result) < 0) result = array[i];
        }

        return result;
    }

    public static T Max<T>(T[] array) where T : IComparable<T> {
        check_not_empty(array);
        T result = array[0];

        for (int i = 1; i < array.Length; i++) {
            if (Comparer<T>.Default.Compare(array[i], result) > 0) result = array[i];
        }

        return result;
    }

    private static void check_not_empty<T>(T[] array) {
        ArgumentNullException.ThrowIfNull(array);
        if (array.Length == 0) throw new ArgumentException("Масив порожній.", nameof(array));
    }
}
