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
        public int RequestedDevice { get; set; }
        public int SuggestedDevice { get; set; }
        public string Description { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public virtual dtUser _User { get; set; }
        public virtual ICollection<dtComment>? _Comments { get; set; }
    }
}
