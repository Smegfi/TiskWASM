using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiskWASM.Shared.Csv
{
    public class csvExport
    {
        public int UserID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Office { get; set; } = string.Empty;
        public int SolutionID { get; set; }
        public string RequestedPrinter { get; set; } = string.Empty;
        public string SuggestedPrinter { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Comment1 { get; set; } = string.Empty;
        public string Comment2 { get; set; } = string.Empty;
        public string Comment3 { get; set; } = string.Empty;
    }
}
