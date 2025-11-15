namespace QuickSortComparison.Utils
{
    public class ScoreWriter : IDisposable
    {
        private readonly StreamWriter writer;
        private readonly Dictionary<int, List<(double normal, double parallel)>> results;

        public ScoreWriter(string fileName)
        {
            writer = new StreamWriter(fileName);
            writer.WriteLine("n;time_normal;time_parallel");
            results = new Dictionary<int, List<(double, double)>>();
        }

        public void AddResult(int n, double timeNormal, double timeParallel)
        {
            if (!results.ContainsKey(n))
                results[n] = new List<(double, double)>();

            results[n].Add((timeNormal, timeParallel));
        }

        public void SaveAverages()
        {
            foreach (var kvp in results)
            {
                int n = kvp.Key;
                var list = kvp.Value;

                double avgNormal = 0;
                double avgParallel = 0;
                foreach (var r in list)
                {
                    avgNormal += r.normal;
                    avgParallel += r.parallel;
                }
                avgNormal /= list.Count;
                avgParallel /= list.Count;

                writer.WriteLine($"{n};{avgNormal:F6};{avgParallel:F6}");
            }
        }

        public void Dispose()
        {
            SaveAverages();
            writer.Dispose();
        }
    }
}
