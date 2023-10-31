using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NoteApp.ViewModel.Commands.NotesCommands
{
    public class NewNotebookCommand : ICommand
    {
        public NewNotebookCommand(NotesViewModel noteBooksVm)
        {
            NoteBookssVM = noteBooksVm;
        }

        public NotesViewModel NoteBookssVM { get; set; }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            NoteBookssVM.CreateNoteBook();
        }

        public event EventHandler? CanExecuteChanged;
    }
}
