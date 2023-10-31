using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NoteApp.Model;

namespace NoteApp.ViewModel.Commands.NotesCommands
{
    public class NewNoteCommand : ICommand
    {
        public NewNoteCommand(NotesViewModel notesVm)
        {
            NotesVM = notesVm;
        }

        public NotesViewModel NotesVM { get; set; }

        public bool CanExecute(object? parameter)
        {
            NoteBook selectedNotebook = parameter as NoteBook;
            if (selectedNotebook != null)
            {
                return true;
            }
            return false;
        }

        public void Execute(object? parameter)
        {
            NoteBook selectedNotebook = parameter as NoteBook;

            NotesVM.CreateNote(selectedNotebook.Id);
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
