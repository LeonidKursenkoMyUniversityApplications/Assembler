using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR01.Model
{
    class LexicException : Exception
    {
        public LexicException(string message) : base(message)
        {
        }
    }
}
