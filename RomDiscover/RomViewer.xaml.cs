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
using PokemonGBAFrameWork;
using Gabriel.Cat.Extension;

namespace RomDiscover
{
    /// <summary>
    /// Lógica de interacción para RomViewer.xaml
    /// </summary>
    public partial class RomViewer : UserControl
    {
        RomGBA rom;
        Edicion edicionPokemon;
        CompilacionRom.Compilacion? compilacionPokemon;
        bool seleccionado;

        public event EventHandler<RomViewerSeleccionadoArgs> Seleccionado;
        public RomViewer()
        {
            seleccionado = false;
            InitializeComponent();
            grid.Background = Brushes.Transparent;
        }
        public RomViewer(string pathRom)
            : this()
        {
            Rom = new RomGBA(new System.IO.FileInfo(pathRom));
        }
        public RomGBA Rom
        {
            get
            {
                return rom;
            }

            set
            {

                if (value == null) throw new ArgumentNullException();
                rom = value;
                txtRomName.Text = rom.NombreRom;

                Edicion = Edicion.GetEdicion(rom);
                CompilacionPokemon = CompilacionRom.GetCompilacion(rom, Edicion);
                switch (Edicion.AbreviacionRom)
                {
                    case Edicion.ABREVIACIONRUBI: imgRom.SetImage(recursos.PokeballRuby); break;
                    case Edicion.ABREVIACIONZAFIRO: imgRom.SetImage(recursos.PokeballZafiro); break;
                    case Edicion.ABREVIACIONROJOFUEGO: imgRom.SetImage(recursos.PokeballRojoFuego); break;
                    case Edicion.ABREVIACIONVERDEHOJA: imgRom.SetImage(recursos.PokeballVerdeHoja); break;
                    case Edicion.ABREVIACIONESMERALDA: imgRom.SetImage(recursos.PokeballEsmeralda); break;
                    default: imgRom.SetImage(recursos.GbaRom); CompilacionPokemon = null; break;
                }

            }
        }

        public bool EsUnaRomPokemon
        {
            get
            {
                return compilacionPokemon != null;
            }

        }

        public bool EstaSeleccionado
        {
            get
            {
                return seleccionado;
            }

            set
            {
                seleccionado = value;
                if (seleccionado)
                    Seleccionar();
                else Deseleccionar();
                if (Seleccionado != null)
                    Seleccionado(this, new RomViewerSeleccionadoArgs(seleccionado));
            }
        }


        public Edicion Edicion
        {
            get
            {
                return edicionPokemon;
            }

            private set
            {
                edicionPokemon = value;
            }
        }

        public CompilacionRom.Compilacion? CompilacionPokemon
        {
            get
            {
                return compilacionPokemon;
            }

            private set
            {
                compilacionPokemon = value;
            }
        }
        /// <summary>
        /// Selecciona sin lanzar el evento
        /// </summary>
        public void Seleccionar()
        {
            grid.Background = Brushes.Green;
            seleccionado = true;
        }
        /// <summary>
        /// Deselecciona sin lanzar el evento
        /// </summary>
        public void Deseleccionar()
        {
            grid.Background = Brushes.Transparent;
            seleccionado = false;
        }
        private void SeleccionadoClick(object sender, MouseButtonEventArgs e)
        {
            EstaSeleccionado = !EstaSeleccionado;
        }
    }
    public class RomViewerSeleccionadoArgs : EventArgs
    {
       public bool EstaSeleccionado { get; private set; }
        public RomViewerSeleccionadoArgs(bool seleccionado)
        { EstaSeleccionado = seleccionado; }
    }
}
