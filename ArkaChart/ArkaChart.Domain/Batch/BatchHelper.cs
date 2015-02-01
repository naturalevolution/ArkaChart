using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ArkaChart.Domain.Mapping.Entities;
using ArkaChart.Tools;

namespace ArkaChart.Domain.Batch {
    public static class BatchHelper  {
        public static ConcurrentDictionary<string, ThreadOfBatch> Threads = new ConcurrentDictionary<string, ThreadOfBatch>();

        public static void StartProcessing(DataFile dataFile) {
            dataFile.Status = (int)StatusHelper.Processing;
            Factory.Repositories.Files.Update(dataFile);
            Factory.Repositories.SaveChanges();
            var currentName = GetName(dataFile.Id);
            var newThread = new ThreadOfBatch(currentName);
            ParameterizedThreadStart pts = newThread.DoWork;
            var workerThread = new Thread(pts);
            workerThread.Start(dataFile);
            Threads.TryAdd(currentName, newThread);
        }

        public static void PauseProcessing(DataFile dataFile) {
            var currentName = GetName(dataFile.Id);
            ThreadOfBatch currentThread;
            if (Threads.ContainsKey(currentName) && Threads.TryRemove(currentName, out currentThread)) {
                currentThread.RequestPause();
            }
        }

        private static string GetName(int id) {
            return "Thread Of : " + id;
        }
    }
}