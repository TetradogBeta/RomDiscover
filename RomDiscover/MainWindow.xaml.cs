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
using Gabriel.Cat;
using PokemonGBAFrameWork;

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
            if (e.EstaSeleccionado)
            {
                romActual = sender as RomViewer;
                CargaLasImagenes();//de momento 
            }
            else romActual = null;
        
        }

        private void txtNumImgACargar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (romActual != null)
                if (Hex.ValidaString(txtNumImgACargar.Text))
            {
                    CargaLasImagenes();
                }
        }

        private void cmbFormatosImgs_Selected(object sender, RoutedEventArgs e)
        {
            if (romActual != null) {
            }
        }

        private void txtOffsetImg_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            if (romActual!=null)
            if (Hex.ValidaString(txtOffsetImg.Text))
            {//si me han puesto las paletas cargo sino espero a que lo hagan
                    CargaLasImagenes();
            }
        }
        private void CargaLasImagenes()
        {
            string txtImg = txtOffsetImg.Text;
            string txtPaleta = txtOffsetPaleta.Text;
            CargaImagenes(txtImg,txtPaleta);
            if (ugImgs.Children.Count == 0)
            {
                try
                {
                    CargaImagenes(Offset.GetOffset(romActual.Rom, txtImg), txtPaleta);
                }
                catch { }
                finally
                {
                    if (ugImgs.Children.Count == 0)
                    {
                        try
                        {
                            CargaImagenes(txtImg, Offset.GetOffset(romActual.Rom, txtPaleta));
                        }
                        catch { }
                        finally
                        {
                            if (ugImgs.Children.Count == 0)
                            {
                                try
                                {
                                    CargaImagenes(Offset.GetOffset(romActual.Rom, txtImg), Offset.GetOffset(romActual.Rom, txtPaleta));
                                }
                                catch { }
                            }
                        }
                    }
                }
            }
        }
        private void CargaImagenes(Hex offsetImg,Hex offsetPaleta)
        {

            ImagenConOffset img;
            int total = int.MaxValue;
            ugImgs.Children.Clear();
           /* try
            {
                
                    img = new ImagenConOffset(BloqueImagen.GetBloqueImagen(romActual.Rom, BloqueImagen.GetOffsetPointerImg(romActual.Rom, offsetImg, 0), Paleta.GetPaleta(romActual.Rom, BloqueImagen.GetOffsetImg(romActual.Rom, offsetPaleta, 0))));
                    img.Selected += PonImagen;
                    ugImgs.Children.Add(img);

                if (Hex.ValidaString(txtNumImgACargar.Text))
                    total = (Hex)txtNumImgACargar.Text;
                for (int i = 1; i < total; i++)
                {
                    
                        //cargo las siguientes y si peta pues dejo de añadir :D
                        img = new ImagenConOffset(BloqueImagen.GetBloqueImagen(romActual.Rom, BloqueImagen.GetOffsetImg(romActual.Rom, offsetImg, i + 1), Paleta.GetPaleta(romActual.Rom, BloqueImagen.GetOffsetImg(romActual.Rom, offsetPaleta, i + 1))));
                        img.Selected += PonImagen;
                        ugImgs.Children.Add(img);
                }

            }
            catch { }*/
        }

        private void PonImagen(object sender, EventArgs e)
        {
            ImagenConOffset img = sender as ImagenConOffset;
            this.imgCargada.SetImage(img.BloqueImagen[0]);
            this.pltImgCargada.Colors = img.BloqueImagen.Paletas[0].Colores;
        }

        private void txtOffsetPaleta_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (romActual != null)
                if (Hex.ValidaString(txtOffsetPaleta.Text))
            {//si me han puesto las img cargo sino espero a que lo hagan.
                    CargaLasImagenes();
                }
        }

        private void lstImgs_Selected(object sender, RoutedEventArgs e)
        {

        }
    }
}
