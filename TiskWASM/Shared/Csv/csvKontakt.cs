using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiskWASM.Shared.Csv
{
    public class csvKontakt
    {
        public string Name { get; set; } = string.Empty;

        public string Surname { get; set; } = string.Empty;

        public string Office { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

    }
}
