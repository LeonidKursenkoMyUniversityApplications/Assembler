using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR01.Model
{
    public class Data
    {
        /// <summary>
        /// Source code of existing program.
        /// </summary>
        public string Source { get; set; }
        public List<LexemRow> LexemTable { get; set; }
        public List<ClassRow> ClassTable { get; set; }
        public List<IdentificatorRow> IdentificatorTable { get; set; }
        public List<ConstantRow> ConstantTable { get; set; }
        public List<OutputRow> OutputTable { get; set; }
        
    }
}
