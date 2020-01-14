using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR01.Model
{
    public class OutputRow
    {
        public int Line { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public int Index { get; set; }

        public List<string> RelatedLabels { get; set; }
    }
}
