using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR01.Model
{
    public class LexemRow
    {
        public string Name { get; set; }
        public int Id { get; set; }
        /// <summary>
        /// true - lexem is splitter
        /// false - lexem is not splitter
        /// </summary>
        public bool IsSplitter { get; set; }
        
    }
}
