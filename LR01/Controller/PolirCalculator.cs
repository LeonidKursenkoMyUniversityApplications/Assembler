using LR01.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR01.Controller
{
    public class PolirCalculator
    {
        #region Attributes
        private Stack<string> stack;
        private int currindex;
        #endregion

        #region Properties

        public List<OutputRow> Polir { get; set; }
        public List<IdentificatorRow> IdentificatorsTable { get; set; }
        public List<LabelRow> LabelsTable { get; set; }
        #endregion

        #region Methods

        public void Calculate()
        {
            if (Polir == null)
                throw new SyntaxException("Error, Неправильний ПОЛІЗ");
            if (IdentificatorsTable == null)
                throw new SyntaxException("Error, Неправильний ПОЛІЗ");
            // Clear Terminal
            Terminal.Clear();
            stack = new Stack<string>();
            for(currindex = 0; currindex < Polir.Count; currindex++)
            {
                if (IsOperand(Polir[currindex].Name) == false)
                    IsOperator(Polir[currindex].Name);
            }
        }

        private bool IsOperand(string element)
        {
            bool result = IdentificatorsTable.Exists(x => x.Name == element);
            if (result == true)
            {
                stack.Push(element);
                return true;
            }

            result = LabelsTable.Exists(x => x.Name == element);
            if (result == true)
            {
                stack.Push(element);
                return true;
            }

            double value;
            result = Double.TryParse(element, out value);
            if (result == true)
            {
                stack.Push(element);
                return true;
            }

            bool value2;
            result = Boolean.TryParse(element, out value2);

            if (result == true)
            {
                stack.Push(element);
                return true;
            }
            return false;
        }

        private bool IsOperator(string element)
        {
            string[] operatorsList = 
            {
                "+", "-", "*", "/", "=",
                "<", "<=", ">", ">=", "==", "!=",
                "and", "or", "not",
                "УПЛ", "БП",
                "read", "write"
            };
            bool result = operatorsList.Contains(element);
            if (result == false) return false;
            DoOperation(element);
            return true;

        }

        private void DoOperation(string element)
        {
            if (DoIfAriphmeticOperation(element) == true) return;
            if (DoIfLogicOperation(element) == true) return;
            if (DoIfTransitionOperation(element) == true) return;
            if (DoIfInOutOperation(element) == true) return;    
        }

        private bool DoIfAriphmeticOperation(string element)
        {
            string[] ariphmeticOperation =
            {
                "+", "-", "*", "/", "=",
                "<", "<=", ">", ">=", "==", "!="                
            };

            // If element isn`t ariphmetic operation returns false.
            bool result = ariphmeticOperation.Contains(element);
            if (result == false)
                return false;

            string second = stack.Pop();
            string first = stack.Pop();
            double firstOp;
            double secondOp;           

            firstOp = ConvertToDouble(first);
            secondOp = ConvertToDouble(second);
            switch (element)
            {
                case "+":
                    stack.Push((firstOp + secondOp).ToString());
                    break;
                case "-":
                    stack.Push((firstOp - secondOp).ToString());
                    break;
                case "*":
                    stack.Push((firstOp * secondOp).ToString());
                    break;
                case "/":
                    stack.Push((firstOp / secondOp).ToString());
                    break;
                case "=":
                    stack.Push(second);
                    Assignment(first, secondOp);
                    break;
                case "<":
                    stack.Push((firstOp < secondOp).ToString());
                    break;
                case "<=":
                    stack.Push((firstOp <= secondOp).ToString());
                    break;
                case ">":
                    stack.Push((firstOp > secondOp).ToString());
                    break;
                case ">=":
                    stack.Push((firstOp >= secondOp).ToString());
                    break;
                case "==":
                    stack.Push((firstOp == secondOp).ToString());
                    break;
                case "!=":
                    stack.Push((firstOp != secondOp).ToString());
                    break;                
            }

            // If element is ariphmetic operation returns true.
            return true;
        }

        private bool DoIfLogicOperation(string element)
        {
            string[] logicOperation =
            {
                "and", "or", "not"
            };

            // If element isn`t logic operation returns false.
            bool result = logicOperation.Contains(element);
            if (result == false)
                return false;

            string second = stack.Pop();
            bool secondOp;
            secondOp = Convert.ToBoolean(second);

            if (element == "not")
            {
                stack.Push((!secondOp).ToString());
                return true;
            }

            string first = stack.Pop();
            bool firstOp;
            
            firstOp = Convert.ToBoolean(first);
            
            switch (element)
            {
                case "and":
                    stack.Push((firstOp && secondOp).ToString());
                    break;
                case "or":
                    stack.Push((firstOp || secondOp).ToString());
                    break;                
            }

            // If element is logic operation returns true.
            return true;
        }

        private bool DoIfInOutOperation(string element)
        {
            string[] ioOperation =
            {
                "read", "write"
            };

            // If element isn`t io operation returns false.
            bool result = ioOperation.Contains(element);
            if (result == false)
                return false;

            string operandName = stack.Pop();
            IdentificatorRow operand = IdentificatorsTable.First(x => x.Name == operandName);
            
            switch (element)
            {
                case "read":
                    Terminal.Read(ref operand);
                    break;
                case "write":
                    Terminal.Write(operand);
                    break;
            }

            // If element is io operation returns true.
            return true;
        }

        private bool DoIfTransitionOperation(string element)
        {
            string[] transitionOperation =
            {
                "УПЛ", "БП"
            };

            // If element isn`t ariphmetic operation returns false.
            bool result = transitionOperation.Contains(element);
            if (result == false)
                return false;

            string second = stack.Pop();
            string first = stack.Pop();
            
            switch (element)
            {
                case "УПЛ":
                    ConditionTransition(first, second);
                    break;
                case "БП":
                    stack.Push(first);
                    UnconditionTransition(second);
                    break;
            }

            // If element is ariphmetic operation returns true.
            return true;
        }

        private double ConvertToDouble(string str)
        {
            double value;
            bool result;
            //string sPattern = "^[a-zE0-9\\.\\+\\-]*$";
            //if (System.Text.RegularExpressions.Regex.IsMatch(str, sPattern))
            //    result = true;
            //else
            //    result = false;
            
            result = Double.TryParse(str, out value);
            if (result == true) return value;

            str = IdentificatorsTable.Find(x => x.Name == str).Value;
            result = Double.TryParse(str, out value);
            return value;
        }

        private void Assignment(string str, double value)
        {
            int index = IdentificatorsTable.IndexOf(IdentificatorsTable.Find(x => x.Name == str));
            IdentificatorsTable[index].Value = value.ToString();
        }

        private void ConditionTransition(string conditions, string label)
        {
            bool condition = Convert.ToBoolean(conditions);
            if (condition == false)
            {
                label += ":";
                currindex = Polir.FindIndex(x => x.Name == label);
            }
        }

        private void UnconditionTransition(string label)
        {            
                label += ":";
                currindex = Polir.FindIndex(x => x.Name == label);            
        }

        #endregion

    }
}
