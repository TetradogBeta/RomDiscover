using Gabriel.Cat.Extension;
using PokemonGBAFrameWork;
using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RomDiscover
{
    /// <summary>
    /// Interaction logic for ImagenConOffset.xaml
    /// </summary>
    public partial class ImagenConOffset : UserControl
    {
        BloqueImagen blImg;
        public event EventHandler Selected;
        public ImagenConOffset()
        {
            InitializeComponent();

        }
        public ImagenConOffset(BloqueImagen blImg):this()
        { BloqueImagen = blImg; }

        public BloqueImagen BloqueImagen
        {
            get
            {
                return blImg;
            }

            set
            {
                if (value == null) throw new ArgumentNullException();
                blImg = value;
                txtOffset.Text = blImg.OffsetInicio;
                img.SetImage(blImg);
            }
        }

        private void StackPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Selected != null)
                Selected(this, new EventArgs());
        }
    }
}
