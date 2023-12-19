using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiskWASM.Shared
{
    [Table("tb_comment")]
    public class dtComment
    {
        [Key]
        public int Id { get; set; }
        public int SolutionId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
