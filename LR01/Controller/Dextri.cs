using LR01.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR01.Controller
{
    public class Dextri : Data
    {
        #region Attributes
        int identificatorId;
        int constantId;
        int ariphmeticOperationIdFirst;
        int ariphmeticOperationIdLast;
        int logicOperationIdFirst;
        int logicOperationIdLast;
        int andId;
        int orId;
        int notId;
        int ifId;
        int elseId;
        int endifId;
        int forId;
        int toId;
        int stepId;
        int nextId;
        int assignId;
        int endLineId;
        int readId;
        int writeId;
        int leftRoundQouteId;
        int rightRoundQouteId;
        int leftSquareQouteId;
        int rightSquareQouteId;

        int currIndex;
        Stack<OutputRow> stack;

        Dictionary<string, int> priorTable;    
        PolirRow polirRow;
        // Table with rj and value.
        List<string> rTable;
        string loopParametr;
        int loopSign;
        #endregion

        #region Properties
        public List<OutputRow> Polir { get; set; }
        public List<PolirRow> PolirTable { get; set; }
        public List<LabelRow> LabelTable { get; set; }
        #endregion

        #region Additional methods
        public string PolirToString()
        {
            var list = Polir.Select(x => x.Name);
            return string.Join(" ", list.ToArray());
        }

        private void FixStackForDisplay()
        {
            if (stack.Count > 0)
            {
                polirRow.Stack.Clear();
                foreach (OutputRow row in stack)
                {
                    if (row.RelatedLabels != null)
                    {
                        string str = string.Join("", row.RelatedLabels);
                        polirRow.Stack.Add(row.Name + str);
                    }
                    else
                    {
                        polirRow.Stack.Add(row.Name);
                    }
                }
            }
        }

        private void DisplayParametrAndLoop()
        {
            polirRow.LoopSign = loopSign.ToString();
            polirRow.LoopParametr = loopParametr;
        }

        private void AddToPolir(string str)
        {
            Polir.Add(new OutputRow()
            {
                Name = str
            });
            polirRow.Polir.Add(str);
        }

        private void UnconditionalTransition()
        {
            // Adds Unconditional transition (БП) to POLIR.
            AddToPolir("БП");
        }

        private void JumpLabel(int index)
        {
            // Adds label mi if index 0.
            // Adds label mi+1 if index 1.
            // Adds label mi+2 if index 2.            
            string label = stack.Peek().RelatedLabels[index];
            label += ":";

            AddToPolir(label);
        }

        private void GenerateRjTable(int index)
        {
            rTable.Add("r" + index);
            string str = rTable[rTable.Count - 1];
            IdentificatorRow idRow = new IdentificatorRow
            {
                Name = str,
                Index = IdentificatorTable.Count - 1,
                Value = "0"
            };
            IdentificatorTable.Add(idRow);

            Polir.Add(new OutputRow()
            {
                Name = str,
                Id = identificatorId
            });
            polirRow.Polir.Add(str);
            //AddToPolir(rTable[rTable.Count - 1]);
        }

        private void AddLabel(bool addLabelToPolir = true)
        {
            // Adds a new label to table.
            int counter = LabelTable.Count;
            LabelRow row = new LabelRow()
            {
                Name = "m" + counter,
                Index = counter
            };
            LabelTable.Add(row);

            // Adds label mi to if in the stack.
            OutputRow outputRow = stack.Pop();
            if (outputRow.RelatedLabels == null)
                outputRow.RelatedLabels = new List<string>();
            outputRow.RelatedLabels.Add(row.Name);
            stack.Push(outputRow);

            // Adds label mi to POLIR if addLabelToPolir == true
            if (addLabelToPolir == true)
            {
                AddToPolir(row.Name);
            }
        }

        private void ConditionalTransition()
        {
            // Adds Conditional transition (УПЛ) to POLIR.
            AddToPolir("УПЛ");
        }

        private void ReadFromStack()
        {
            polirRow.Polir.Add(stack.Peek().Name);
            Polir.Add(stack.Pop());
        }
        #endregion

        #region Methods
        private void MakeInitializes()
        {
            LabelTable = new List<LabelRow>();
            stack = new Stack<OutputRow>();
            Polir = new List<OutputRow>();
            PolirTable = new List<PolirRow>();
            LabelTable = new List<LabelRow>();
            rTable = new List<string>();

            identificatorId = LexemTable.First(x => x.Name == "identificator").Id;
            constantId = LexemTable.First(x => x.Name == "constant").Id;
            ariphmeticOperationIdFirst = LexemTable.First(x => x.Name == "=").Id;
            ariphmeticOperationIdLast = LexemTable.First(x => x.Name == "/").Id;
            logicOperationIdFirst = LexemTable.First(x => x.Name == ">").Id;
            logicOperationIdLast = LexemTable.First(x => x.Name == "!=").Id;
            assignId = LexemTable.First(x => x.Name == "=").Id;
            endLineId = LexemTable.First(x => x.Name == "\n").Id;
            leftRoundQouteId = LexemTable.First(x => x.Name == "(").Id;
            rightRoundQouteId = LexemTable.First(x => x.Name == ")").Id;
            leftSquareQouteId = LexemTable.First(x => x.Name == "[").Id;
            rightSquareQouteId = LexemTable.First(x => x.Name == "]").Id;
            andId = LexemTable.First(x => x.Name == "and").Id;
            orId = LexemTable.First(x => x.Name == "or").Id;
            notId = LexemTable.First(x => x.Name == "not").Id;
            ifId = LexemTable.First(x => x.Name == "if").Id;
            elseId = LexemTable.First(x => x.Name == "else").Id;
            endifId = LexemTable.First(x => x.Name == "endif").Id;
            forId = LexemTable.First(x => x.Name == "for").Id;
            toId = LexemTable.First(x => x.Name == "to").Id;
            stepId = LexemTable.First(x => x.Name == "step").Id;
            nextId = LexemTable.First(x => x.Name == "next").Id;
            readId = LexemTable.First(x => x.Name == "read").Id;
            writeId = LexemTable.First(x => x.Name == "write").Id;
        }

        public void BuildPolir()
        {
            MakeInitializes();
            BuildPriorTable();
            loopSign = 0;
            loopParametr = "";
            currIndex = OutputTable.FindIndex(x => x.Name == "begin") + 2;
            for(; currIndex < OutputTable.Count - 1; currIndex++)
            {                
                IdentifyLexem();
            }
        }

        private void BuildPriorTable()
        {
            priorTable = new Dictionary<string, int>();
            priorTable.Add("(", 0);
            priorTable.Add("[", 0);
            priorTable.Add("if", 0);
            priorTable.Add("for", 0);
            priorTable.Add(")", 1);
            priorTable.Add("]", 1);    
            priorTable.Add("else", 1);
            priorTable.Add("endif", 1);
            priorTable.Add("to", 1);
            priorTable.Add("step", 1);
            priorTable.Add("next", 1);
            priorTable.Add("=", 2);
            priorTable.Add("or", 3);
            priorTable.Add("and", 4);
            priorTable.Add("not", 5);
            priorTable.Add(">", 6);
            priorTable.Add(">=", 6);
            priorTable.Add("<", 6);
            priorTable.Add("<=", 6);
            priorTable.Add("==", 6);
            priorTable.Add("!=", 6);
            priorTable.Add("+", 7);
            priorTable.Add("-", 7);
            priorTable.Add("*", 8);
            priorTable.Add("/", 8);
        }

        private void IdentifyLexem()
        {
            polirRow = new PolirRow();
            polirRow.InputChain.Add(OutputTable[currIndex].Name);

            if (IfReadWrite() == true)
            {
                ReadWrite();
                FixStackForDisplay();
                DisplayParametrAndLoop();
                PolirTable.Add(polirRow);
                return;
            }

            if (IfEndLine() == true)
            {
                EndLine();
                FixStackForDisplay();
                DisplayParametrAndLoop();                                                                
                PolirTable.Add(polirRow);
                return;
            }

            if (IfRightQoute() == true)
            {
                RightQoute();
                FixStackForDisplay();
                DisplayParametrAndLoop();
                PolirTable.Add(polirRow);
                return;
            }

            FixStackForDisplay();

            if (IfOperation() == true)
            {
                Operation();
                DisplayParametrAndLoop();
                PolirTable.Add(polirRow);
                return;
            }
            
            if (IfLeftQoute() == true)
            {
                LeftQoute();
                DisplayParametrAndLoop();
                PolirTable.Add(polirRow);
                return;
            }

            DisplayParametrAndLoop();

            if (IfConstantOrIdentificator() == true)
            {
                Polir.Add(OutputTable[currIndex]);
                polirRow.Polir.Add(OutputTable[currIndex].Name);
                
                if (stack.Count > 0)
                {
                    if ((stack.Peek().Id == readId) || (stack.Peek().Id == writeId))
                    {
                        Polir.Add(stack.Peek());
                        polirRow.Polir.Add(stack.Peek().Name);
                        // skip ',' or ')'.
                        currIndex++;
                    }
                }
                PolirTable.Add(polirRow);
                return;
            }
        }

        private bool IfReadWrite()
        {
            if (OutputTable[currIndex].Id == readId) return true;
            if (OutputTable[currIndex].Id == writeId) return true;
            return false;
        }

        private void ReadWrite()
        {
            stack.Push(OutputTable[currIndex]);
            polirRow.Stack.Insert(0, OutputTable[currIndex].Name);
            // Skip '(', ',' after 'read' or 'write'.
            currIndex += 2;
        }

        private bool IfConstantOrIdentificator()
        {
            if (OutputTable[currIndex].Id == identificatorId) return true;
            if (OutputTable[currIndex].Id == constantId) return true;
            return false;
        }

        private bool IfOperation()
        {
            int id = OutputTable[currIndex].Id;
            if ((id >= ariphmeticOperationIdFirst) && (id <= ariphmeticOperationIdLast)) return true;
            if ((id >= logicOperationIdFirst) && (id <= logicOperationIdLast)) return true;
            if (id == andId) return true;
            if (id == orId) return true;
            if (id == notId) return true;
            return false;
        }

        private void Operation()
        {
            if (stack.Count > 0)
            {
                if((OutputTable[currIndex].Id == assignId) && (loopSign == 1))
                {
                    loopParametr = Polir[Polir.Count - 1].Name;
                    loopSign = 0;
                }

                int priority = priorTable.First(x => x.Key == OutputTable[currIndex].Name).Value;
                int priority2 = priorTable.First(x => x.Key == stack.Peek().Name).Value;
                if (priority2 >= priority)
                {
                    polirRow.Polir.Add(stack.Peek().Name);
                    Polir.Add(stack.Pop());
                    polirRow.Stack.RemoveAt(0);
                    Operation();                    
                }
                else
                {
                    stack.Push(OutputTable[currIndex]);
                    polirRow.Stack.Insert(0, OutputTable[currIndex].Name);
                }
            }
            else
            {
                stack.Push(OutputTable[currIndex]);
                polirRow.Stack.Insert(0, OutputTable[currIndex].Name);
            }
            
        }

        private bool IfEndLine()
        {
            if (OutputTable[currIndex].Id == endLineId) return true;
            return false;
        }

        private void EndLine()
        {
            OutputRow lexem;
            while (stack.Count > 0)
            {
                lexem = stack.Peek();
                if (lexem.Id == assignId)
                {
                    ReadFromStack();
                    break;
                }

                if (lexem.Id == ifId)
                {
                    // mi => POLIR
                    AddLabel();
                    // UPL => POLIR
                    ConditionalTransition();                  
                    break;
                }

                if (lexem.Id == forId)
                {
                    EnterInTheFor();                    
                    break;
                }

                if ((lexem.Id == readId) || (lexem.Id == writeId))
                {
                    stack.Pop();
                    break;
                }

                ReadFromStack();
            }
        }

        private void EnterInTheFor()
        {
            AddToPolir("=");
            // rj
            AddToPolir(rTable[0]);
            AddToPolir("0");
            AddToPolir("==");
            // mi+1
            string label = stack.Peek().RelatedLabels[1];
            AddToPolir(label);
            ConditionalTransition();
            AddToPolir(loopParametr);
            AddToPolir(loopParametr);
            // rj+1
            AddToPolir(rTable[2]);
            AddToPolir("+");
            AddToPolir("=");
            // mi+1:
            JumpLabel(1);
            // rj
            AddToPolir(rTable[0]);
            AddToPolir("0");
            AddToPolir("=");
            AddToPolir(loopParametr);
            // rj+2
            AddToPolir(rTable[1]);
            AddToPolir("-");
            // rj+1
            AddToPolir(rTable[2]);
            AddToPolir("*");
            AddToPolir("0");
            AddToPolir("<=");
            // mi+2
            label = stack.Peek().RelatedLabels[2];
            AddToPolir(label);
            ConditionalTransition();
            // Deletes rj, rj+1, rj+2.
            rTable.Clear();
            // Sets loopParametr to default value.
            loopParametr = "";
        }
        
        private bool IfLeftQoute()
        {
            int id = OutputTable[currIndex].Id;
            if (id == leftRoundQouteId) return true;
            if (id == leftSquareQouteId) return true;
            if (id == ifId) return true;
            if (id == forId) return true;
            return false;
        }

        private void LeftQoute()
        {
            stack.Push(OutputTable[currIndex]);            
            if (OutputTable[currIndex].Id == forId)
            {
                AddLabel(false);
                AddLabel(false);
                AddLabel(false);
                FixStackForDisplay();
                loopSign = 1;
            }
            else
            {
                polirRow.Stack.Insert(0, OutputTable[currIndex].Name);
            }
            
        }

        private bool IfRightQoute()
        {
            int id = OutputTable[currIndex].Id;
            if (id == rightRoundQouteId) return true;
            if (id == rightSquareQouteId) return true;
            if (id == elseId) return true;
            if (id == endifId) return true;
            if (id == toId) return true;
            if (id == stepId) return true;
            if (id == nextId) return true;
            return false;
        }

        private void RightQoute()
        {
            OutputRow lexem;
            while (stack.Count > 0)
            {
                lexem = stack.Peek();
                if (lexem.Id == leftRoundQouteId)
                {
                    stack.Pop();
                    break;
                }

                if (lexem.Id == leftSquareQouteId)
                {
                    stack.Pop();
                    break;
                }

                if ((lexem.Id == ifId) && (OutputTable[currIndex].Id == elseId))
                {
                    AddLabel();
                    UnconditionalTransition();
                    // mi: => POLIR
                    JumpLabel(0);
                    // Skip enter after "else"
                    currIndex++;
                    break;
                }

                if ((lexem.Id == ifId) && (OutputTable[currIndex].Id == endifId))
                {                    
                    // mi+1: => POLIR
                    JumpLabel(1);
                    // Deletes "ifmimi+1" from stack
                    stack.Pop();
                    // Skip enter after "endif"
                    currIndex++;
                    break;
                }

                if ((lexem.Id == forId) && (OutputTable[currIndex].Id == toId))
                {
                    // Generates rj
                    GenerateRjTable(0);
                    AddToPolir("1");
                    AddToPolir("=");
                    // mi: => POLIR
                    JumpLabel(0);
                    // Generates rj+2
                    GenerateRjTable(2);
                    break;
                }

                if ((lexem.Id == forId) && (OutputTable[currIndex].Id == stepId))
                {
                    AddToPolir("=");
                    // Generates rj+1
                    GenerateRjTable(1);                    
                    break;
                }

                if ((lexem.Id == forId) && (OutputTable[currIndex].Id == nextId))
                {
                    // mi
                    string label = stack.Peek().RelatedLabels[0];
                    AddToPolir(label);
                    UnconditionalTransition();
                    // mi+2:
                    JumpLabel(2);
                    // Deletes "formimi+1mi+2" from stack
                    stack.Pop();
                    // Skip enter after "next"
                    currIndex++;
                    break;
                }

                ReadFromStack();
            }
        }

        
        #endregion
    }
}
