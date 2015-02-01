using System;
using System.Data;
using System.IO;
using System.Linq;
using ArkaChart.Domain.Mapping.Context;
using ArkaChart.Domain.Mapping.Entities;
using ArkaChart.Tools;

namespace ArkaChart.Domain.Batch {
    public class ThreadOfBatch {
        public static DataFile DataFile;
        public PathHelper PathHelper = new PathHelper();
        private volatile bool _shouldStop;

        public ThreadOfBatch(string currentName) {
            Name = currentName;
        }

        public string Name { get; set; }

        public void DoWork(object obj) {
            DataFile = obj as DataFile;
            while (!_shouldStop) {
                ReadDataFile();
            }
        }
        public virtual void RequestPause() {
            ChangeState(StatusHelper.Waiting);
            _shouldStop = true;
        }

        private void ChangeState(StatusHelper status) {
            using (EntitiesContext _context = (new EntityObjectContext()).GetContext()) {
                DataFile df = _context.Files.First(x => x.Id == DataFile.Id);
                df.Status = (int) status;
                _context.Entry(df).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }

        private void ReadDataFile() {
            string path = PathHelper.AbsolutePath(DataFile.Name);
            int lastLine = DataFile.LastLine;
            var currentStatus = StatusHelper.Processing;
            var hasBeenStopped = false;
            try {
                using (EntitiesContext _context = (new EntityObjectContext()).GetContext()) {
                    DataFile df = _context.Files.First(x => x.Id == DataFile.Id);
                    StreamReader file = new StreamReader(path);
                    string line;
                    int counter = 0;
                    while ((line = file.ReadLine()) != null) {
                        if (_shouldStop) {
                            hasBeenStopped = true;
                            break;
                        }
                        if (counter >= lastLine) {
                            InsertLine(line, df, _context);
                            lastLine++;
                            UpdateLastLineAndStatus(df, _context, lastLine);
                            _context.SaveChanges();
                        }
                        counter++;
                    }
                    /*foreach (string line in File.ReadAllLines(path).Skip(lastLine)) {
                        InsertLine(line, df, _context);
                        lastLine++;
                        UpdateLastLineAndStatus(df, _context, lastLine);
                        _context.SaveChanges();
                    }*/
                }
                if (!hasBeenStopped) {
                    currentStatus = StatusHelper.Finished;
                } else {
                    currentStatus = StatusHelper.Waiting;
                }
            } catch (OutOfMemoryException e) {
                currentStatus = StatusHelper.OutOfMemory;
                Console.WriteLine(e);
            } catch (Exception e) {
                currentStatus = StatusHelper.Waiting;
                Console.WriteLine(e);
            }
            using (EntitiesContext _context = (new EntityObjectContext()).GetContext()) {
                DataFile df = _context.Files.First(x => x.Id == DataFile.Id);
                UpdateLastLineAndStatus(df, _context, lastLine, currentStatus);
                _context.SaveChanges();
            }
            _shouldStop = true;
        }

        private void UpdateLastLineAndStatus(DataFile dataFile, EntitiesContext context, int lastLine,
            StatusHelper status = StatusHelper.Processing) {
            dataFile.LastLine = lastLine;
            dataFile.Status = (int) status;
            context.Entry(dataFile).State = EntityState.Modified;
        }

        private void InsertLine(string line, DataFile dataFile, EntitiesContext context) {
            string[] splitted = line.Split(Convert.ToChar(","));
            if (splitted != null && splitted.Count() == 3) {
                DataLine dataLine = dataFile.AddLine(splitted[0], splitted[1], splitted[2]);
                context.Lines.Add(dataLine);
            }
        }
    }
}