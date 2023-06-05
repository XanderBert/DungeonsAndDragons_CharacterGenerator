using DungeonsAndDragonsApp.Model;
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
using System.Windows.Shapes;

namespace DungeonsAndDragonsApp.View
{
    /// <summary>
    /// Interaction logic for CharacterSheetWindow.xaml
    /// </summary>
    public partial class CharacterSheetWindow : Window
    {
        public CharacterSheetWindow(Character character)
        {
            InitializeComponent();
            Character = character;
        }
        public Character Character { get; set; }

    }
}
