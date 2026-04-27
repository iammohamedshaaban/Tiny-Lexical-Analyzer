using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string inputString = textBox1.Text;

            string keywords = @"\b(num|text|check|otherwise|until|repeat|then)\b";
            string identifiers = @"\b[a-zA-Z][a-zA-Z0-9]*\b";
            string numbers = @"\b[0-9]+\b";
            string operators = @":=|==|[+\-*/<>]";
            string symbols = @"[;{}()]";
            string masterPattern = $"{keywords}|{numbers}|{operators}|{symbols}|{identifiers}";

            DataTable dt = new DataTable();
            dt.Columns.Add("lexeme");
            dt.Columns.Add("Token type");

            MatchCollection matches = Regex.Matches(inputString, masterPattern);
            List<Token> allTokens = new List<Token>();
            foreach (Match m in matches)
            {

                string lex = m.Value.Trim();
                string type = "";

                if (Regex.IsMatch(lex, keywords))
                    type = "keyword";
                else if (Regex.IsMatch(lex, numbers))
                    type = "number";
                else if (Regex.IsMatch(lex, identifiers))
                    type = "Identifier";
                else if (lex == ":=") type = "Assignment_Op";
                else if (lex == "==") type = "Equal_Op";
                else if (lex == "+") type = "Plus_Op";
                else if (lex == "-") type = "Minus_Op";
                else if (lex == "*") type = "Multiply_Op";
                else if (lex == "/") type = "Divide_Op";
                else if (lex == ">") type = "Greater_Than_Op";
                else if (lex == "<") type = "Less_Than_Op";

                else if (lex == ";") type = "Semicolon";
                else if (lex == "(") type = "Left_Paren";
                else if (lex == ")") type = "Right_Paren";
                else if (lex == "{") type = "Left_Brace";
                else if (lex == "}") type = "Right_Brace";
                else type = "unknown";

                allTokens.Add(new Token { Value = lex, Type = type });
                dt.Rows.Add(lex, type);
            }
            dataGridView1.DataSource = dt;

            try
            {
                if (allTokens.Count == 0) return;
                TinyParser parser = new TinyParser(allTokens);
                parser.ParseProgram();
                MessageBox.Show("success: Your Tiny code is correct!", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($" Syntax Error: {ex.Message}", "Parsing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
            private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

    }
}
