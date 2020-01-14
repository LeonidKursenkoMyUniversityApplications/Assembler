using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TableBuilder;

namespace LR01
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public DataGridView GetCompareTable { get { return dataGridView1; } }

        private void FillTable(TableMaker table)
        {
            for (int i = 0; i < table.GetLength(); i++)
            {
                dataGridView1.Columns.Add("", "");
                dataGridView1.Columns[dataGridView1.Columns.Count - 1].Width = 70;
                dataGridView1.Rows.Add();
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Height = 30;
            }    
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;

            for (int i = 0; i < dataGridView1.Columns.Count - 1; i++)
            {
                dataGridView1.Columns[i].HeaderCell.Value = table.GetTable()[0, i + 1];
                dataGridView1.Rows[i].HeaderCell.Value = table.GetTable()[i + 1, 0];
            }
            dataGridView1.RowHeadersWidth = 100;
            dataGridView1.ColumnHeadersHeight = 50;
            for (int i = 0; i < dataGridView1.Columns.Count - 1; i++)
            {
                for (int j = 0; j < dataGridView1.Columns.Count - 1; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value = table.GetTable()[i + 1, j + 1];
                    
                }
            }
            for (int i = 0; i < dataGridView1.Columns.Count - 1; i++)
            {
                dataGridView1.Rows[dataGridView1.Columns.Count - 1].Cells[i].Value = "<";
                dataGridView1.Rows[i].Cells[dataGridView1.Columns.Count - 1].Value = ">";
            }
            dataGridView1.Rows[dataGridView1.Columns.Count - 1].HeaderCell.Value = "#";
            dataGridView1.Columns[dataGridView1.Columns.Count - 1].HeaderCell.Value = "#";

        }

        private void InitGrammar(Grammar grammar)
        {

            grammar.UpdateKeys();
            grammar.UpdateAllItems();
        }

        private void ToCsV(DataGridView dGV, string filename)
        {
            string stOutput = "";
            // Export titles:
            string sHeaders = "";

            sHeaders = "\t";
            for (int j = 0; j < dGV.Columns.Count; j++)
                sHeaders = sHeaders.ToString() + Convert.ToString(dGV.Columns[j].HeaderText) + "\t";
            stOutput += sHeaders.Replace("\n", "\\n") + "\r\n";
            // Export data.
            for (int i = 0; i < dGV.RowCount; i++)
            {
                string stLine = Convert.ToString(dGV.Columns[i].HeaderText) + "\t";
                for (int j = 0; j < dGV.Rows[i].Cells.Count; j++)
                    stLine = stLine.ToString() + Convert.ToString(dGV.Rows[i].Cells[j].Value) + "\t";
                stOutput += stLine.Replace("\n", "\\n") + "\r\n";
            }
            Encoding utf8 = UTF8Encoding.Default;  //Encoding.GetEncoding(1254);
            byte[] output = utf8.GetBytes(stOutput);
            FileStream fs = new FileStream(filename, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(output, 0, output.Length); //write the encoded file
            bw.Flush();
            bw.Close();
            fs.Close();
        }

        //public Grammar Grammar { get; set; }

        private void Form1_Load(object sender, EventArgs e)
        {
            ParseGrammar parser = ParseGrammar.GetInstance();
            parser.ReadFromFile(@"grammar.txt");
            parser.Parse();

            Grammar grammar = parser.Grammar;
            InitGrammar(grammar);
            TableMaker maker = new TableMaker(grammar);
            maker.MakeTable();
            FillTable(maker);
            //Grammar = grammar;
            // to excel
            ToCsV(dataGridView1, @"table.xls");
        }
    }
}
