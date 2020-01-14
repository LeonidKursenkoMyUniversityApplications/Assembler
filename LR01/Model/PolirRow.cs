using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR01.Model
{
    public class PolirRow
    {

        public List<string> Polir { set; get; }
        public List<string> Stack { set; get; }
        public List<string> InputChain { set; get; }
        
        public string LoopParametr { set; get; }
        public string LoopSign { set; get; }

        public string PolirStr
        {
            get
            {
                return string.Join("\n", Polir.ToArray());
            }
        }

        public string StackStr
        {
            get
            {
                return string.Join("\n", Stack.ToArray());
            }
        }

        public string InputChainStr
        {
            get
            {
                return string.Join("\n", InputChain.ToArray());
            }
        }

        public PolirRow()
        {
            Polir = new List<string>();
            Stack = new List<string>();
            InputChain = new List<string>();
            LoopParametr = "";
            LoopSign = "";
        }

    }
}
