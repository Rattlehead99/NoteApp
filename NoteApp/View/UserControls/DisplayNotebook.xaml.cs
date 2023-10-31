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
    /// Interaction logic for DisplayNotebook.xaml
    /// </summary>
    public partial class DisplayNotebook : UserControl
    {
        public DisplayNotebook()
        {
            InitializeComponent();
        }

        public NoteBook Notebook
        {
            get { return (NoteBook)GetValue(NotebookDependencyProperty);}
            set{ SetValue(NotebookDependencyProperty, value);}
        }

        public static readonly DependencyProperty NotebookDependencyProperty = 
            DependencyProperty.Register(nameof(Notebook), typeof(NoteBook), typeof(DisplayNotebook),
                new PropertyMetadata(null, SetValues));

        private static void SetValues(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DisplayNotebook notebookUserControl = d as DisplayNotebook;

            if (notebookUserControl == null)
            {
                return;
            }

            notebookUserControl.DataContext = notebookUserControl.Notebook;

        }
    }
}
