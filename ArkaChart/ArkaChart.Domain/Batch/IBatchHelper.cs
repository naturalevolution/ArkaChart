using ArkaChart.Domain.Mapping.Entities;

namespace ArkaChart.Domain.Batch {
    public interface IBatchHelper {
        void StartProcessing(DataFile dataFile);

        void PauseProcessing(DataFile dataFile);
    }
}