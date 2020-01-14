using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR01.Model
{
    public class ResultRow
    {
        public int Number { get; set; }
        public string Stack { get; set; }
        public string Symbol { get; set; }
        public string InputChain { get; set; }

        public void SetStack(Stack<string> stack)
        {
            var list = stack.ToList();
            list.Reverse();
            foreach(var it in list)
            {
                Stack += " " + it;
            }
        }

        public void SetInputChain(List<string> inputChain)
        {
            foreach (var it in inputChain)
            {
                InputChain += " " + it;
            }
        }

    }
}
