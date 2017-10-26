using System.ComponentModel.DataAnnotations;

namespace DevStats.Data.Entities
{
    public class DefectMapping
    {
        public int ID { get; set; }

        [MaxLength(50)]
        public string OriginalProduct { get; set; }

        [MaxLength(50)]
        public string OriginalModule { get; set; }

        [MaxLength(50)]
        public string DisplayProduct { get; set; }

        [MaxLength(50)]
        public string DisplayModule { get; set; }
    }
}