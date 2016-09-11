using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Gabriel.Cat.Extension;
namespace RomDiscover
{//zafiro 1.1 ,1.2 , fire red 1.1 y verde hoja 1.1
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RomViewer romActual;
        public MainWindow()
        {
            romActual = null;
            InitializeComponent();
        }

        private void btnAñadirArchivoGBA_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog opnRom = new Microsoft.Win32.OpenFileDialog();
            opnRom.Filter = "GBA|*.gba";
            opnRom.Multiselect = true;
            if (opnRom.ShowDialog().GetValueOrDefault())
            {
                AñadirRom(opnRom.FileNames);
            }
        }


        private void btnAñadirCarpetaRoms_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog opnRom = new FolderBrowserDialog();
            opnRom.Description = "Selecciona la carpeta con la roms\n\n[mira dentro de todas las subcarpetas]";
            if (opnRom.ShowDialog()==System.Windows.Forms.DialogResult.OK)
            {
                AñadirRom(new DirectoryInfo(opnRom.SelectedPath).GetAllFiles().Filtra((file)=> { return file.Extension == ".gba"; }));
            }
        }

        private void AñadirRom(IEnumerable<FileInfo> filesDir)
        {
            foreach (FileInfo file in filesDir)
                AñadirRom(file.FullName);
        }
        private void AñadirRom(string[] fileNames)
        {
            for (int i = 0; i < fileNames.Length; i++)
                AñadirRom(fileNames[i]);
        }
        private void AñadirRom(string fullName)
        {
            //añado la rom si no esta mirando su path
            RomViewer romViewer = new RomViewer(fullName);
            romViewer.Seleccionado += RomCambiada;
            ugRomsGba.Children.Add(romViewer);
        }

        private void RomCambiada(object sender, RomViewerSeleccionadoArgs e)
        {
            if (romActual != null)
                romActual.Deseleccionar();
            if(e.EstaSeleccionado)
               romActual = sender as RomViewer;
            else romActual = null;
        
        }
    }
}
