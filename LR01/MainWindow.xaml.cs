using LR01.Controller;
using LR01.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TableBuilder;

namespace LR01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public LexicAnalizator LA { get; set; }
        public SyntaxAnalyzator3 SA { get; set; }
        public StringBuilder ErrorMessages { get; set; }
        // Table for compare
        private Form1 form1;

        public MainWindow()
        {
            InitializeComponent();
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            LA = new LexicAnalizator();
            LA.DefaultInit();
            
            lexGrid.ItemsSource = LA.LexemTable;
            classGrid.ItemsSource = LA.ClassTable;

            // Table for compare
            form1 = new Form1();
            form1.Show();
        }

        private void runButton_Click(object sender, RoutedEventArgs e)
        {          
            TextRange textRange = new TextRange(workField.Document.ContentStart, workField.Document.ContentEnd);
            string s;
            s = textRange.Text;
            LA.Source = s;
            ErrorMessages = new StringBuilder();
            try
            {
                LA.Build();
            }
            catch(LexicException ex)
            {
                ErrorMessages.Append(ex.Message).Append('\n');
                ShowErrors();
                return;
            }
            outputGrid.ItemsSource = LA.OutputTable;
            identificatorGrid.ItemsSource = LA.IdentificatorTable;
            constantGrid.ItemsSource = LA.ConstantTable;
            
            // Initializes Syntax analyzator
            SA = new SyntaxAnalyzator3()
            {
                Source = LA.Source,
                LexemTable = LA.LexemTable,
                ClassTable = LA.ClassTable,
                IdentificatorTable = LA.IdentificatorTable,
                ConstantTable = LA.ConstantTable,
                OutputTable = LA.OutputTable,
                CompareTable = form1.GetCompareTable
            };
            //  Syntax analyzator start        
            try
            {
                SA.Start();
            }
            catch (SyntaxException ex)
            {
                ErrorMessages.Append(ex.Message).Append('\n');
                ShowErrors();
                return;
            }
            resultGrid.ItemsSource = SA.ResultTable;

            // Initializes Dextri
            Dextri D = new Dextri
            {
                LexemTable = SA.LexemTable,
                ClassTable = SA.ClassTable,
                IdentificatorTable = SA.IdentificatorTable,
                ConstantTable = SA.ConstantTable,
                OutputTable = SA.OutputTable
            };

            // Builds Polir.
            try
            {
                D.BuildPolir();                
            }
            catch
            {
                throw new Exception("Polir exception");
            }
            polirGrid.ItemsSource = D.PolirTable;
            TextRange polirtextRange = new TextRange(polirField.Document.ContentStart, polirField.Document.ContentEnd);
            polirtextRange.Text = D.PolirToString();
            labelGrid.ItemsSource = D.LabelTable;
            identificatorGrid.ItemsSource = D.IdentificatorTable;
            identificatorGrid.Items.Refresh();

            // Initializes PolirCalculator
            PolirCalculator polirCalculator = new PolirCalculator
            {
                Polir = D.Polir,
                IdentificatorsTable = D.IdentificatorTable,
                LabelsTable = D.LabelTable
            };

            // Calculates Polir.
            try
            {
                polirCalculator.Calculate();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message + "; Calculate exception");
            }
            identificatorGrid.ItemsSource = polirCalculator.IdentificatorsTable;

            ShowErrors();

        }

        public void ShowErrors()
        {
            TextRange textRange = new TextRange(errorField.Document.ContentStart, errorField.Document.ContentEnd);
            textRange.Text = "";
            if (ErrorMessages.Length > 0)
            {                
                textRange.Text = ErrorMessages.ToString();
            }
            else
            {
                textRange.Text = "no errors";
            }
        }
    }
}
