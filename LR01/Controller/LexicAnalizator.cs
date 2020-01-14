using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR01.Model
{
    public class LexicAnalizator : Data
    {
        
        private int line;

        public void DefaultInit()
        {
            DefaultInitLexemTable();
            DefaultInitClassTable();
        }
        public void DefaultInitLexemTable()
        {
            LexemTable = new List<LexemRow>();
            LexemTable.Add(new LexemRow { Name = "program", Id = 1, IsSplitter = false });
            LexemTable.Add(new LexemRow { Name = "var", Id = 2, IsSplitter = false });
            LexemTable.Add(new LexemRow { Name = "begin", Id = 3, IsSplitter = false });
            LexemTable.Add(new LexemRow { Name = "end", Id = 4, IsSplitter = false });
            LexemTable.Add(new LexemRow { Name = "real", Id = 5, IsSplitter = false });
            LexemTable.Add(new LexemRow { Name = "for", Id = 6, IsSplitter = false });
            LexemTable.Add(new LexemRow { Name = "to", Id = 7, IsSplitter = false });
            LexemTable.Add(new LexemRow { Name = "step", Id = 8, IsSplitter = false });
            LexemTable.Add(new LexemRow { Name = "next", Id = 9, IsSplitter = false });
            LexemTable.Add(new LexemRow { Name = "if", Id = 10, IsSplitter = false });
            LexemTable.Add(new LexemRow { Name = "else", Id = 11, IsSplitter = false });
            LexemTable.Add(new LexemRow { Name = "endif", Id = 12, IsSplitter = false });
            LexemTable.Add(new LexemRow { Name = "read", Id = 13, IsSplitter = false });
            LexemTable.Add(new LexemRow { Name = "write", Id = 14, IsSplitter = false });
            LexemTable.Add(new LexemRow { Name = "or", Id = 15, IsSplitter = false });
            LexemTable.Add(new LexemRow { Name = "and", Id = 16, IsSplitter = false });
            LexemTable.Add(new LexemRow { Name = "not", Id = 17, IsSplitter = false });
            LexemTable.Add(new LexemRow { Name = ":", Id = 18, IsSplitter = true });
            LexemTable.Add(new LexemRow { Name = ",", Id = 19, IsSplitter = true });
            LexemTable.Add(new LexemRow { Name = "=", Id = 20, IsSplitter = true });
            LexemTable.Add(new LexemRow { Name = "+", Id = 21, IsSplitter = true });
            LexemTable.Add(new LexemRow { Name = "-", Id = 22, IsSplitter = true });
            LexemTable.Add(new LexemRow { Name = "*", Id = 23, IsSplitter = true });
            LexemTable.Add(new LexemRow { Name = "/", Id = 24, IsSplitter = true });
            LexemTable.Add(new LexemRow { Name = "(", Id = 25, IsSplitter = true });
            LexemTable.Add(new LexemRow { Name = ")", Id = 26, IsSplitter = true });
            LexemTable.Add(new LexemRow { Name = "[", Id = 27, IsSplitter = true });
            LexemTable.Add(new LexemRow { Name = "]", Id = 28, IsSplitter = true });
            LexemTable.Add(new LexemRow { Name = ">", Id = 29, IsSplitter = true });
            LexemTable.Add(new LexemRow { Name = "<", Id = 30, IsSplitter = true });
            LexemTable.Add(new LexemRow { Name = ">=", Id = 31, IsSplitter = true });
            LexemTable.Add(new LexemRow { Name = "<=", Id = 32, IsSplitter = true });
            LexemTable.Add(new LexemRow { Name = "==", Id = 33, IsSplitter = true });
            LexemTable.Add(new LexemRow { Name = "!=", Id = 34, IsSplitter = true });
            LexemTable.Add(new LexemRow { Name = "\n", Id = 35, IsSplitter = true });            
            LexemTable.Add(new LexemRow { Name = "identificator", Id = 36, IsSplitter = false });
            LexemTable.Add(new LexemRow { Name = "constant", Id = 37, IsSplitter = false });            
            LexemTable.Add(new LexemRow { Name = " ", Id = -1, IsSplitter = true });
            
        }

        public void DefaultInitClassTable()
        {
            ClassTable = new List<ClassRow>();           

            ClassTable.Add(new ClassRow() { Symbol = "a-z", Class = "Б" });
            ClassTable.Add(new ClassRow() { Symbol = "0-9", Class = "Ц" });
            ClassTable.Add(new ClassRow() { Symbol = "()[],:*/ enter space", Class = "ОРО" });
            ClassTable.Add(new ClassRow() { Symbol = "+-", Class = "-" });
            ClassTable.Add(new ClassRow() { Symbol = ".", Class = "." });
            ClassTable.Add(new ClassRow() { Symbol = "E", Class = "E" });
            ClassTable.Add(new ClassRow() { Symbol = ">", Class = ">" });
            ClassTable.Add(new ClassRow() { Symbol = "<", Class = "<" });
            ClassTable.Add(new ClassRow() { Symbol = "!", Class = "!" });
            ClassTable.Add(new ClassRow() { Symbol = "=", Class = "=" });
            
        }

        public void Build()
        {
            line = 1;
            //int position = 1;
            string lex = "";
            OutputTable = new List<OutputRow>();
            IdentificatorTable = new List<IdentificatorRow>();
            ConstantTable = new List<ConstantRow>();
            OutputRow oRow;
            for (int i = 0; i < Source.Length; i++)
            {
                if (IsSplitter(i) != true)
                {
                    
                    if (IsSymbole(i) == true) lex += Source[i];
                    else throw new LexicException("Error: недопустимий символ '" + Source[i] + "', рядок " + line);
                }
                else
                {
                    if (lex != "")
                    {
                        oRow = new OutputRow() { Line = line, Name = lex };
                        Recognise(oRow);
                        OutputTable.Add(oRow);                        
                    }
                    lex = Source[i].ToString();
                    if (IsSpace(i) == true)
                    {
                        lex = "";
                        continue;
                    }
                    if ((i + 1 < Source.Length) && (IsEqualSplitter(i + 1) == true) && (IsLogicSplitter(i + 1) == true))
                    {
                        oRow = new OutputRow() { Line = line, Name = lex + Source[i + 1] };
                        Recognise(oRow);
                        OutputTable.Add(oRow);                        
                        lex = "";
                        i++;
                    }
                    else
                    {
                        if (lex.CompareTo("!") == 0) throw new LexicException("Error: недопустимий оператор '!'");
                        
                        oRow = new OutputRow() { Line = line, Name = lex };
                        Recognise(oRow);
                        OutputTable.Add(oRow);
                        if (Source[i] == '\n') line++;
                        lex = "";
                    }
                    
                }
            }
            OutputTable.RemoveAt(OutputTable.Count - 1);
        }

        public bool IsSymbole(int index)
        {            
            string sPattern = "^[a-zE0-9\\.\\+\\-]*$";
            if (System.Text.RegularExpressions.Regex.IsMatch(Source[index].ToString(), sPattern))
            {                
                return true;
            }
            return false;
        }
        
        public bool IsSplitter(int index)
        {
            if ((index > 0) && ((Source[index] == '+') || (Source[index] == '-')) && (Source[index - 1] == 'E'))
                return false;

            char[] spliters = { '(', ')', '[', ']', ',', ':', '*', '/', ' ', '\r', '\n', '+', '-', '=', '>', '<', '!' };
            if(Array.IndexOf(spliters, Source[index]) >= 0) return true;

            return false;
        }

        public bool IsSpace(int index)
        {
            char[] spliters = {' ', '\r'};
            if (Array.IndexOf(spliters, Source[index]) >= 0) return true;
            return false;
        }

        public bool IsEqualSplitter(int index)
        {      
            if (Source[index] == '=') return true;
            return false;
        }

        public bool IsLogicSplitter(int index)
        {
            char[] spliters = { '=', '>', '<', '!' };
            if ((index > 0) && (Array.IndexOf(spliters, Source[index - 1]) >= 0)) return true;
            return false;
        }

        public void Recognise(OutputRow row)
        {
            string lex = row.Name;
            LexemRow lRow;
            for (int i = 0; i < LexemTable.Count - 3; i++)
            {
                lRow = LexemTable[i];
                if (lex.CompareTo(lRow.Name) == 0)
                {
                    row.Id = lRow.Id;
                    break;
                }
            }

            if (row.Id == 0)
            {
                row.Id = RecogniseId(row);
            }
        }

        public int RecogniseId(OutputRow row)
        {
            string lex = row.Name;
            string identificatorPattern = "^[a-z][a-z0-9]*$";
            if (System.Text.RegularExpressions.Regex.IsMatch(lex, identificatorPattern))
            {
                BuildIdentificator(row);
                return LexemTable[35].Id;
            }
            string constantPattern = "^(?(\\d+)\\d+\\.?\\d*|\\.\\d+)(E[-+]?\\d+)?$";
            if (System.Text.RegularExpressions.Regex.IsMatch(lex, constantPattern))
            {
                BuildConstant(row);
                return LexemTable[36].Id;
            }
            throw new LexicException("Error: неправльнй формат константи '" + lex + "', рядок " + line);

            //return 0;
        }

        public void BuildIdentificator(OutputRow row)
        {
            string lex = row.Name;
            foreach(IdentificatorRow idRow in IdentificatorTable)
            {
                if(idRow.Name.CompareTo(lex) == 0)
                {
                    row.Index = idRow.Index;
                    return;
                }
            }
            
            foreach (OutputRow oRow in OutputTable)
            {
                if (oRow.Name.CompareTo("begin") == 0)
                {
                    throw new LexicException("Error: неоголошений ідентифікатор '" + lex + "', рядок " + line);
                }
            }
            IdentificatorRow idRow2 = new IdentificatorRow()
            {
                Name = lex,
                Index = IdentificatorTable.Count + 1,
                Value = "0"
            };
            IdentificatorTable.Add(idRow2);
            row.Index = idRow2.Index;           
        }

        public void BuildConstant(OutputRow row)
        {
            string lex = row.Name;
            foreach (ConstantRow idRow in ConstantTable)
            {
                if (idRow.Name.CompareTo(lex) == 0)
                {
                    row.Index = idRow.Index;
                    return;
                }
            }
            ConstantRow idRow2 = new ConstantRow() { Name = lex, Index = ConstantTable.Count + 1 };
            ConstantTable.Add(idRow2);
            row.Index = idRow2.Index;
        }

    }
}
