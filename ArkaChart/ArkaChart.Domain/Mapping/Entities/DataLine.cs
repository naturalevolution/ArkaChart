using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArkaChart.Domain.Mapping.Entities {
    [Table("DATA_LINE")]
    public class DataLine {

        protected DataLine() {
        }
        public DataLine(string time, string value, string origin, DataFile dataFile) {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0);
            Timestamp = dateTime.AddMilliseconds(long.Parse(time));
            Value = Int32.Parse(value);
            Origin = origin;
            IdDataFile = dataFile.Id;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("TIME")]
        [Timestamp]
        public DateTime Timestamp { get; set; }

        [Column("VALUE")]
        public int Value { get; set; }

        [Column("ORIGIN")]
        public string Origin { get; set; }

        [Column("ID_DATAFILE")]
        public int IdDataFile { get; set; }

        [ForeignKey("IdDataFile")]
        public virtual DataFile DataFile { get; set; }
    }
}