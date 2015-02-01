using System.Collections.Generic;
using System.IO;
using System.Linq;
using ArkaChart.Domain.Mapping.Entities;
using ArkaChart.Tools;

namespace ArkaChart.Models {
    public class FilesViewModel {
        public FilesViewModel(IEnumerable<FileInfo> avalaibleFiles, IList<DataFile> processingFiles ) {
            Files = new List<DataFileViewModel>();
            foreach (var avalaibleFile in avalaibleFiles) {
                Files.Add(new DataFileViewModel(avalaibleFile, processingFiles.FirstOrDefault(x => x.Name.Equals(avalaibleFile.Name))));
            }
        }

        public IList<DataFileViewModel> Files { get; set; }
    }

    public class DataFileViewModel {
        public DataFileViewModel(FileInfo file, DataFile processingFile) {
            Name = file.Name;
            if (processingFile != null) {
                Status = processingFile.Status;
                ProcessingFile = processingFile;
            } else {
                Status = (int)StatusHelper.NeverProcessed;
            }
        }

        public string Name { get; set; }
        public int Status { get; set; }
        public DataFile ProcessingFile { get; set; }

        public string GetStatus() {
            switch (Status) {
                case (int)StatusHelper.Processing :
                    return Resources.Batch.Status_Processing;
                case (int)StatusHelper.Waiting:
                    return Resources.Batch.Status_Waiting;
                case (int)StatusHelper.Cancelled:
                    return Resources.Batch.Status_Cancelled;
                case (int)StatusHelper.NeverProcessed:
                    return Resources.Batch.Status_NeverProcessed;
                case (int)StatusHelper.Finished:
                    return Resources.Batch.Status_Finished;
                case (int)StatusHelper.OutOfMemory:
                    return Resources.Batch.Status_OutOfMemory;
            }
            return string.Empty;
        }

        public bool IsStatus(StatusHelper status) {
            return Status == (int) status;
        }

        public bool HasProcessingFiles() {
            return ProcessingFile != null;
        }
    }
}