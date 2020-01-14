using LR01.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TableBuilder;

namespace LR01.Controller
{
    public class SyntaxAnalyzator3 : Data
    {
        #region Attributes
        // index of current lexem
        private int index;
        // number of current step
        private int stepCount;       
        // Table with terms compares
        public DataGridView CompareTable { get; set; }
        // Result table
        public List<ResultRow> ResultTable { get; set; }
        // Current grammatic
        public Grammar Grammar { get; set; }
        // Input chain of terms
        private List<string> inputChain;  
        // Stack
        private Stack<string> stack;
        // Result of compare
        private enum CompareSymbol { Less, Bigger, Equal}
        // Current lexem
        private OutputRow currentLexem;
        // Potential rule
        private List<string> bufferNotTerm;
        #endregion

        #region additional methods
        private void IncorrectStructure(string message)
        {
            throw new SyntaxException("Помилка, неправильна структура програми: " + message);
        }
        // Goes to the next lexem
        private void NextLexem()
        {
            if (index >= OutputTable.Count - 1)
            {
                return;
                throw new SyntaxException("Неправильна структура програми");
            }
            index++;
        }

        /// <summary>
        /// This method generates SyntaxException if currently lexem and previous lexem are located
        /// at the same line. 
        /// </summary>
        /// <param name="index"></param>
        private void IsEnter()
        {

            var output = (OutputRow)OutputTable[index];
            // Enter
            var lex = (LexemRow)LexemTable[33];
            if (output.Id != lex.Id)
                throw new SyntaxException("Неправильна структура програми пропущено \"enter\", рядок " + output.Line);
        }

        #endregion

        #region Methods

        public void Start()
        {
            index = 0;
            ResultTable = new List<ResultRow>();
            stack = new Stack<string>();
            stack.Push("#");
            stepCount = 1;
            GetInputChain();

            while(stack.Peek() != "<программа>")
            {
                DoStep();
            }
            FormResultRow(">");
        }

        // Generates input chain.
        public void GetInputChain()
        {
            inputChain = new List<string>();
            foreach(OutputRow row in OutputTable)
            {
                inputChain.Add(row.Name);
            }
            inputChain.Add("#");
        }

        // Generates result row.
        public void FormResultRow(string compare)
        {
            ResultRow row = new ResultRow();
            row.Number = stepCount;
            row.SetStack(stack);
            row.Symbol = compare;            
            row.SetInputChain(inputChain);
            ResultTable.Add(row);
        }

        // Dones one iteration.
        public void DoStep()
        {
            currentLexem = (OutputRow)OutputTable[index];
            switch (MakeCompare(stack.Peek(), inputChain[0]))
            {
                case CompareSymbol.Less: IfLess(); break;
                case CompareSymbol.Bigger: IfBigger(); break;
                case CompareSymbol.Equal: IfEqual(); break;
            }
            stepCount++;
        }

        // Identifies and makes correct if term is id or con.
        private void Identify(ref string str)
        {
            foreach (OutputRow row in OutputTable)
            {
                // converts values to id or con            
                if ((row.Name == str) && (row.Id == 36))
                {
                    str = "id";
                    break;
                }
                if ((row.Name == str) && (row.Id == 37))
                {
                    str = "con";
                    break;
                }
            }
        }

        // Compares to terms.
        private CompareSymbol MakeCompare(string first, string second)
        {
            Identify(ref first);

            int rowIndex = 0;            
            for (int i = 0; i < CompareTable.Rows.Count; i++)
            {
                if (CompareTable.Rows[i].HeaderCell.Value.ToString() == first)
                {
                    rowIndex = i;
                    break;
                }
            }
            
            Identify(ref second);

            int columnIndex = 0;
            for (int i = 0; i < CompareTable.ColumnCount; i++)
            {
                if (CompareTable.Columns[i].HeaderCell.Value.ToString() == second)
                {
                    columnIndex = i;
                    break;
                }
            }
            string symbol = CompareTable[columnIndex, rowIndex].Value.ToString();

            if (symbol == "<")
                return CompareSymbol.Less;
            if (symbol == ">")
                return CompareSymbol.Bigger;
            if (symbol == "=")
                return CompareSymbol.Equal;
            currentLexem = OutputTable[index];
            throw new SyntaxException("Порушено структуру програми. Помилковий елемент \"" +
                currentLexem.Name +"\", рядок " + currentLexem.Line);
        }

        private void IfLess()
        {
            FormResultRow("<");
            stack.Push(inputChain[0]);
            inputChain.RemoveAt(0);
            NextLexem();
        }

        private void IfBigger()
        {
            FormResultRow(">");
            bufferNotTerm = new List<string>();
            bufferNotTerm.Add(stack.Pop());
            while(MakeCompare(second: bufferNotTerm[bufferNotTerm.Count - 1], 
                first: stack.Peek()) != CompareSymbol.Less)
            {
                bufferNotTerm.Add(stack.Pop());
            }
            bufferNotTerm.Reverse();

            string ruleName = FindRule();
            if (ruleName == "")
                throw new SyntaxException("Error. Невідома конструкція \"" + 
                    string.Join(" ", bufferNotTerm.ToArray()) +
                    "\", рядок " + currentLexem.Line + "\n");
            stack.Push(ruleName);
        }

        private void IfEqual()
        {
            FormResultRow("=");
            stack.Push(inputChain[0]);
            inputChain.RemoveAt(0);
            NextLexem();
        }

        

        private void CorrectBufferNotTerm()
        {
            for (int i = 0; i < bufferNotTerm.Count; i++)
            {
                foreach (OutputRow row in OutputTable)
                {
                    if ((bufferNotTerm[i] == row.Name) && (row.Id == 36))
                    {
                        bufferNotTerm[i] = "id";
                        return;
                    }
                    if ((bufferNotTerm[i] == row.Name) && (row.Id == 37))
                    {
                        bufferNotTerm[i] = "con";
                        return;
                    }
                }
            }
        }

        private string FindRule()
        {
            CorrectBufferNotTerm();
            Grammar = ParseGrammar.GetInstance().Grammar;

            List<string> rulesNameList = new List<string>();
            List<List<string>> rulesList = new List<List<string>>();

            rulesNameList = Grammar.GetRuleKeys();
            
            foreach(string ruleName in rulesNameList)
            {
                rulesList = Grammar.GetSuquence(ruleName);
                foreach(List<string> rule in rulesList)
                {
                    if (CompareRule(rule) != true)
                        continue;
                    else
                        return ruleName;
                }
            }
           
            return "";
        }

        private bool CompareRule(List<string> rule)
        {
            if (bufferNotTerm.Count == rule.Count)
            {
                for (int i = 0; i < rule.Count; i++)
                {
                    if (bufferNotTerm[i] != rule[i])
                        return false;
                }
                return true;
            }
            else
                return false;
        }

        #endregion

    }
}
