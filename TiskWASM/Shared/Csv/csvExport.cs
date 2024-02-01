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
        public string Name { get; set; } = "NULL";
        public string Email { get; set; } = "NULL";
        public string Department { get; set; } = "NULL";
        public string Office { get; set; } = "NULL";

        public int SolutionID { get; set; }
        public string RequestedPrinter { get; set; } = "NULL";
        public string SuggestedPrinter { get; set; } = "NULL";
        public string Description { get; set; } = "NULL";
        public string Comment1 { get; set; } = "NULL";
        public string Comment2 { get; set; } = "NULL";
        public string Comment3 { get; set; } = "NULL";
        public string Comment4 { get; set; } = "NULL";
    }
}
