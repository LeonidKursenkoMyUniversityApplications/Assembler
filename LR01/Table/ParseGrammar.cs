using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableBuilder
{
    public class ParseGrammar
    {
        private static ParseGrammar instance;

        private ParseGrammar()
        { }

        public static ParseGrammar GetInstance()
        {
            if(instance == null)
            {
                instance = new ParseGrammar();
            }
            return instance;
        }

        public List<string> Source { get; set; }

        public Grammar Grammar { get; set; }

        public void ReadFromFile(string fileName)
        {
            string line;
            Source = new List<string>();
            System.IO.StreamReader file = new System.IO.StreamReader(fileName, Encoding.Default);
            while ((line = file.ReadLine()) != null)
            {
                Source.Add(line);
            }

            file.Close();
        }

        public Dictionary<string, List<List<string>>> Parse()
        {            
            List<string> elementsList; // = new List<string>();

            List<string> left = new List<string>();
            List<List<string>> right = new List<List<string>>();

            string str;
            foreach(var it in Source)
            {
                str = it.Replace("\\n", "\n");
                elementsList = str.Split(' ').ToList<string>();
                left.Add(elementsList[0]);
                elementsList.RemoveAt(0);
                elementsList.RemoveAt(0);
                right.Add(elementsList);
            }

            List<string> notTermList; // = new List<string>();
            notTermList = left.Distinct().ToList();

            Dictionary<string, List<List<string>>> rulesList = new Dictionary<string, List<List<string>>>();
            
            foreach (var notTerm in notTermList)
            {
                List<List<string>> rules = new List<List<string>>();
                for(int i = 0; i < left.Count; i++)
                {
                    if(notTerm == left[i])
                    {
                        rules.Add(right[i]);
                    }
                }
                rulesList.Add(notTerm, rules);
            }

            Grammar = new Grammar();
            foreach(var rule in rulesList)
            {
                Grammar.AddRule(rule.Key, rule.Value);
            }

            return rulesList;
        }

    }
}
