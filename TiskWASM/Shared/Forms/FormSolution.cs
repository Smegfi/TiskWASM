using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiskWASM.Shared.Forms
{
    public class FormSolution
    {
        public int UserId { get; set; }
        public int RequestedDevice { get; set; }
        public int SuggestedDevice { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
