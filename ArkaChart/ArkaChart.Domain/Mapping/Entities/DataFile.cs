using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using ArkaChart.Tools;

namespace ArkaChart.Domain.Mapping.Entities {
    [Table("DATA_FILE")]
    public class DataFile {

        public DataFile(string filePath, int status) {
            Name = filePath;
            Status = status;
            Lines = new List<DataLine>();
        }
        protected DataFile() {
            Lines = new List<DataLine>();
        }
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
         
        [Column("NAME")]
        public string Name { get; set; }

        [Column("STATUS")]
        public int Status { get; set; }

        [Column("LAST_LINE")]
        public int LastLine { get; set; }

        public virtual ICollection<DataLine> Lines { get; set; }

        public DataLine AddLine(string time, string value, string origin) {
            var newLine = new DataLine(time, value, origin, this);
            Lines.Add(newLine);
            return newLine;
        }
    }
}