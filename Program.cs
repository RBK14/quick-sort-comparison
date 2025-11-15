using QuickSortComparison.Algorithms;
using QuickSortComparison.Utils;

namespace QuickSortComparison
{
    internal class Program
    {
        static void Main()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Wybierz tryb:");
                Console.WriteLine("1 - Tryb testowy");
                Console.WriteLine("2 - Tryb symulacji");
                Console.WriteLine("0 - Wyjście");
                Console.Write("Twój wybór: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        RunTestMode();
                        break;
                    case "2":
                        RunSimulationMode();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Niepoprawny wybór. Naciśnij Enter...");
                        Console.ReadLine();
                        break;
                }
            }
        }

        static void RunTestMode()
        {
            Console.Clear();
            Console.WriteLine("=== Tryb testowy ===\n");
            Console.Write("Podaj rozmiar tablicy: ");
            if (!int.TryParse(Console.ReadLine(), out int size) || size <= 0)
            {
                Console.WriteLine("Niepoprawna wartość. Naciśnij Enter...");
                Console.ReadLine();
                return;
            }

            int[] original = GenerateRandomArray(size);
            int[] arrNormal = (int[])original.Clone();
            int[] arrParallel = (int[])original.Clone();

            Console.WriteLine($"\nAlgorytm QuickSort dla tablicy {size} elementów\n");

            double timeNormal = HighResTimer.Measure(() =>
            {
                QuickSort.Sort(arrNormal, 0, arrNormal.Length - 1);
            });
            Console.WriteLine($"Czas normalny:   {timeNormal:F0} ms");

            double timeParallel = HighResTimer.Measure(() =>
            {
                QuickSort.SortParallel(arrParallel, 0, arrParallel.Length - 1);
            });
            Console.WriteLine($"Czas równoległy: {timeParallel:F0} ms");

            Console.WriteLine("\nPoprawność wyników: " +
                (IsSorted(arrNormal) && IsSorted(arrParallel) ? "OK" : "BŁĄD!"));

            Console.WriteLine("\nNaciśnij Enter, aby wrócić do menu...");
            Console.ReadLine();
        }

        static void RunSimulationMode()
        {
            Console.Clear();
            Console.WriteLine("=== Tryb symulacji ===\n");

            Console.Write("Podaj rozmiary tablic, oddzielone spacją: ");
            string input = Console.ReadLine();
            int[] sizes;

            if (string.IsNullOrWhiteSpace(input))
            {
                sizes = new int[] { 100000, 250000, 500000, 1000000, 2500000, 5000000, 10000000, 25000000 };
                Console.WriteLine("Użyto domyślnych rozmiarów: " + string.Join(", ", sizes));
            }
            else
            {
                sizes = Array.ConvertAll(input.Split(' ', StringSplitOptions.RemoveEmptyEntries), s => int.Parse(s));
            }


            Console.Write("Podaj liczbę prób: ");
            int trials = int.Parse(Console.ReadLine());

            Console.Write("Podaj nazwę pliku do zapisu (np. scores.txt): ");
            string fileNameInput = Console.ReadLine();

            if (string.IsNullOrEmpty(fileNameInput))
                fileNameInput = "scores.txt";

            string scoresDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scores");

            if (!Directory.Exists(scoresDir))
            {
                Directory.CreateDirectory(scoresDir);
            }

            string fileName = Path.Combine(scoresDir, fileNameInput);

            using var scoreWriter = new ScoreWriter(fileName);
            Random rand = new Random();

            foreach (int size in sizes)
            {
                for (int t = 1; t <= trials; t++)
                {
                    int[] original = new int[size];
                    for (int i = 0; i < size; i++)
                        original[i] = rand.Next();

                    int[] arrNormal = (int[])original.Clone();
                    int[] arrParallel = (int[])original.Clone();

                    double timeNormal = HighResTimer.Measure(() =>
                    {
                        QuickSort.Sort(arrNormal, 0, arrNormal.Length - 1);
                    });

                    double timeParallel = HighResTimer.Measure(() =>
                    {
                        QuickSort.SortParallel(arrParallel, 0, arrParallel.Length - 1);
                    });

                    scoreWriter.AddResult(size, timeNormal, timeParallel);

                    if (trials <= 5)
                    {
                        Console.WriteLine($"Rozmiar: {size}, Próba: {t}, Normal: {timeNormal:F2} ms, Parallel: {timeParallel:F2} ms");
                    }
                }
            }

            Console.WriteLine($"\nSymulacja zakończona. Średnie wyniki zapisane w pliku: {fileName}");
            Console.WriteLine("Naciśnij Enter, aby wrócić do menu...");
            Console.ReadLine();
        }
        static int[] GenerateRandomArray(int size)
        {
            Random rand = new Random();
            int[] arr = new int[size];

            for (int i = 0; i < size; i++)
                arr[i] = rand.Next();

            return arr;
        }

        static bool IsSorted(int[] arr)
        {
            for (int i = 1; i < arr.Length; i++)
                if (arr[i] < arr[i - 1])
                    return false;
            return true;
        }
    }
}
