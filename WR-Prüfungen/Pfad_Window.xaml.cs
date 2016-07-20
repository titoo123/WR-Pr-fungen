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
using System.Xml;

namespace WR_Prüfungen
{
    /// <summary>
    /// Interaktionslogik für Pfad_Window.xaml
    /// </summary>
    public partial class Pfad_Window : Window
    {
        
        private XmlDocument xml = new XmlDocument();
        public Pfad_Window()
        {
            InitializeComponent();

            try
            {
                xml.Load(XML_Manager.option_pfad);

                textBox_Pfad.Text = xml.SelectSingleNode("Einstellungen").SelectSingleNode("Importpfad").InnerText;
            }
            catch (Exception)
            {
                MessageBox.Show("Einstellungen nicht gefunden!", "Fehler!");
            }

        }

        private void button_ExportierenUndSchließen_Click(object sender, RoutedEventArgs e)
        {
            xml.SelectSingleNode("Einstellungen").SelectSingleNode("Importpfad").InnerText = textBox_Pfad.Text;

            try
            {
                xml.Save(XML_Manager.option_pfad);
            }
            catch (Exception)
            {
                MessageBox.Show("Einstellungen konnten nicht gespeichert werden!", "Fehler!");
            }

            this.Close();
        }

        private void button_Pfad_auswählen_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog f = new System.Windows.Forms.FolderBrowserDialog();
            if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox_Pfad.Text = f.SelectedPath;
            }
        }
    }
}
