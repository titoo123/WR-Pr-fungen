﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WR_Prüfungen
{
    /// <summary>
    /// Interaktionslogik für Prüfer_Window.xaml
    /// </summary>
    public partial class Prüfer_Window : Window
    {
        static bool neu;
        public Prüfer_Window()
        {
            InitializeComponent();
            LoadDatagrid();
        }
        
        private void dataGrid_pruefer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((DataGrid)sender).SelectedIndex != -1)
            {
                button_löschen.IsEnabled = true;
                button_bearbeiten.IsEnabled = true;
            }
            else
            {
                button_löschen.IsEnabled = false;
                button_bearbeiten.IsEnabled = false;
            }


            if (dataGrid_pruefer.SelectedItem != null)
            {
                textBox_Pruefer.Text = "" + ((TextBlock)dataGrid_pruefer.Columns[1].GetCellContent(dataGrid_pruefer.SelectedItem)).Text;
            }
            neu = false;
            textBox_Pruefer.IsEnabled = false;
        }

        private void button_neu_Click(object sender, RoutedEventArgs e)
        {

            neu = true;
            button_Speichern.IsEnabled = true;
            textBox_Pruefer.IsEnabled = true;

            textBox_Pruefer.Text = String.Empty;
        }

        private void button_bearbeiten_Click(object sender, RoutedEventArgs e)
        {

            textBox_Pruefer.IsEnabled = true;
            button_Speichern.IsEnabled = true;
        }

        private void button_Speichern_Click(object sender, RoutedEventArgs e)
        {

            DatabaseConnectionDataContext db = new DatabaseConnectionDataContext();
            if (neu)
            {
                Prüfer m = new Prüfer() { Name = textBox_Pruefer.Text };
                db.Prüfer.InsertOnSubmit(m);
            }
            else
            {
                var mat = from m in db.Prüfer
                          where m.Id == Convert.ToInt32("" + ((TextBlock)dataGrid_pruefer.Columns[0].GetCellContent(dataGrid_pruefer.SelectedItem)).Text)
                          select m;

                mat.First().Name = textBox_Pruefer.Text;

            }
            db.SubmitChanges();
            textBox_Pruefer.Text = String.Empty;
            LoadDatagrid();
        }

        private void button_löschen_Click(object sender, RoutedEventArgs e)
        {
            string tmp = Helper.GetStringFromDataGrid(dataGrid_pruefer,0);
            if (tmp != "8")
            {
                if (MessageBox.Show("Wenn Sie diesen Prüfer löschen, löschen sie alle damit verbundenen Prüfungen! Sicher?"
                , "Achtung!",
                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    DatabaseConnectionDataContext db = new DatabaseConnectionDataContext();


                    var pru = from x in db.Prüfung
                              where x.Id_Kunde == Convert.ToInt32(tmp)
                              select x;

                    db.Prüfung.DeleteAllOnSubmit(pru);

                    try
                    {
                        db.SubmitChanges();
                    }
                    catch (Exception)
                    {
                    }


                    var usr = from u in db.Prüfer
                              where u.Id == Convert.ToInt32(tmp)
                              select u;

                    db.Prüfer.DeleteAllOnSubmit(usr);

                    try
                    {
                        db.SubmitChanges();
                    }
                    catch (Exception)
                    {
                    }

                    LoadDatagrid();
                }
            }
            else
            {
                MessageBox.Show("Dieser Prüfer ist nicht löschbar!","Achtung!");
            }


        }

        private void LoadDatagrid()
        {
            DatabaseConnectionDataContext db = new DatabaseConnectionDataContext();

            var mat = from u in db.Prüfer
                      select u;

            dataGrid_pruefer.ItemsSource = mat.ToList();
        }
        
        private void dataGrid_pruefer_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "Id" || e.Column.Header.ToString() == "Prüfung")
            {
                e.Column.MinWidth = 0;
                e.Column.Width = 0;
            }
        }
    }
}
