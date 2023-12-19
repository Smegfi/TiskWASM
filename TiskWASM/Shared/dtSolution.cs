using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiskWASM.Shared
{
    [Table("tb_solution")]
    public class dtSolution
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RequestedPrinter { get; set; }
        public int SuggestedPrinter { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
