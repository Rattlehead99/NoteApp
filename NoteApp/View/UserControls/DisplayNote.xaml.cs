using NoteApp.Model;
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

namespace NoteApp.View.UserControls
{
    /// <summary>
    /// Interaction logic for DisplayNote.xaml
    /// </summary>
    public partial class DisplayNote : UserControl
    {
        public DisplayNote()
        {
            InitializeComponent();
        }
        public Note Note
        {
            get { return (Note)GetValue(NoteDependencyProperty); }
            set { SetValue(NoteDependencyProperty, value); }
        }

        public static readonly DependencyProperty NoteDependencyProperty =
            DependencyProperty.Register(nameof(Note), typeof(Note), typeof(DisplayNote),
                new PropertyMetadata(null, SetValues));

        private static void SetValues(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DisplayNote userControl = d as DisplayNote;

            if (userControl == null) { return; }

            userControl.DataContext = userControl.Note;

        }
    }
}
