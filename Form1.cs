using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Teste_COVID
{
    public partial class Form1 : Form
    {
        const string DATA_FILE = "data-covid-19.xml";
        List<Test> teste = new List<Test>();
        List<Test> teste2Display = new List<Test>();

        public Form1()
        {
            InitializeComponent();

            //grdTeste.Columns
            grdTeste.Columns["Nume"].DataPropertyName = "Nume";
            grdTeste.Columns["Prenume"].DataPropertyName = "Prenume";
            grdTeste.Columns["CNP"].DataPropertyName = "CNP";
            grdTeste.Columns["DataTest"].DataPropertyName = "DataTest";
            grdTeste.Columns["Rezultat"].DataPropertyName = "Rezultat";

            //load tests
            if (System.IO.File.Exists(DATA_FILE))
            {
                teste.AddRange(Helper.ReadFromXmlFile<List<Test>>(DATA_FILE));
            }

            RefreshSearch();

            selRezultat.SelectedIndex = 0;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //save
            Helper.WriteToXmlFile<List<Test>>(DATA_FILE, teste, false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            teste.Add(new Test() { 
                Nume = txtNume.Text,
                Prenume = txtPrenume.Text,
                CNP = txtCNP.Text,
                DataTest = dtDataTest.Value.Date,
                Rezultat = selRezultat.SelectedItem.ToString() == "Pozitiv" ? RezultatTest.Pozitiv : (selRezultat.SelectedItem.ToString() == "Negativ" ? RezultatTest.Negativ : RezultatTest.Indecis)
            });

            RefreshSearch();
        }

        void BindGrid() {

            grdTeste.DataSource = new BindingList<Test>(teste2Display);
            grdTeste.Update();
            grdTeste.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtNume.Text = "";
            txtPrenume.Text = "";
            txtCNP.Text = "";
            dtDataTest.Value = DateTime.Now;
            selRezultat.SelectedIndex = 0;

            BindGrid();
        }

        private void txtNume_TextChanged(object sender, EventArgs e)
        {
            RefreshSearch();
        }

        private void RefreshSearch()
        {
            teste2Display.Clear();

            if (string.IsNullOrEmpty(txtNume.Text) && string.IsNullOrEmpty(txtPrenume.Text) && string.IsNullOrEmpty(txtCNP.Text))
            {

                teste2Display.AddRange(teste.OrderByDescending(t => t.DataTest).Take(100));
            }
            else
            {
                teste2Display.AddRange(teste.Where(
                    t =>
                        (string.IsNullOrWhiteSpace(txtNume.Text) || t.Nume.StartsWith(txtNume.Text)) &&
                        (string.IsNullOrWhiteSpace(txtPrenume.Text) || t.Prenume.StartsWith(txtPrenume.Text)) &&
                        (string.IsNullOrWhiteSpace(txtCNP.Text) || t.CNP.StartsWith(txtCNP.Text)))
                    .OrderByDescending(t => t.DataTest)
                    .Take(100)
                    .ToList());
            }

            BindGrid();
        }

        private void txtPrenume_TextChanged(object sender, EventArgs e)
        {
            RefreshSearch();
        }

        private void txtCNP_TextChanged(object sender, EventArgs e)
        {
            RefreshSearch();
        }

        private void grdTeste_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }
    }
}
