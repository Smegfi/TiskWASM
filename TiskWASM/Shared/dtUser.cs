using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiskWASM.Shared
{
    [Table("tb_user")]
    public class dtUser
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Office { get; set; } = string.Empty;

        public virtual ICollection<dtSolution>? _Solutions { get; set; }
    }
}
