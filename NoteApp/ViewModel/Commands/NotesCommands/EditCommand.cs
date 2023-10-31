using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NoteApp.ViewModel.Commands.NotesCommands
{
    public class EditCommand : ICommand
    {
        public EditCommand(NotesViewModel notesVm)
        {
            NotesVM = notesVm;
        }

        public NotesViewModel NotesVM { get; set; }


        public bool CanExecute(object? parameter) { return true; }

        public void Execute(object? parameter)
        {
            NotesVM.StartEditing();
        }

        public event EventHandler? CanExecuteChanged;
    }
}
