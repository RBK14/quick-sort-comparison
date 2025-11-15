namespace QuickSortComparison.Algorithms
{
    public static class QuickSort
    {
        private const int MIN_PARALLEL_SIZE = 10000;

        public static void Sort(int[] array, int low, int high)
        {
            if (low < high)
            {
                int pi = Partition(array, low, high);

                Sort(array, low, pi);
                Sort(array, pi + 1, high);
            }
        }

        public static void SortParallel(int[] array, int low, int high)
        {
            if (low < high)
            {
                int pi = Partition(array, low, high);

                if (high - low > MIN_PARALLEL_SIZE)
                {
                    Parallel.Invoke(
                        () => SortParallel(array, low, pi),
                        () => SortParallel(array, pi + 1, high));
                }
                else
                {
                    Sort(array, low, pi);
                    Sort(array, pi + 1, high);
                }
            }
        }

        private static int Partition(int[] array, int low, int high)
        {
            int pivot = array[low];

            int i = low - 1;
            int j = high + 1;

            while (true)
            {
                do { j--; } while (array[j] > pivot);
                do { i++; } while (array[i] < pivot);

                if (i < j)
                {
                    Swap(array, i, j);
                }
                else
                {
                    return j;
                }
            }
        }

        private static void Swap(int[] array, int i, int j)
        {
            (array[j], array[i]) = (array[i], array[j]);
        }
    }
}
