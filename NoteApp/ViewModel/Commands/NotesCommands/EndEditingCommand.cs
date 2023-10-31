using System;
using System.Windows.Input;
using NoteApp.Model;

namespace NoteApp.ViewModel.Commands.NotesCommands
{
    public class EndEditingCommand : ICommand
    {
        public EndEditingCommand(NotesViewModel noteVm)
        {
            NoteVM = noteVm;
        }

        public NotesViewModel NoteVM { get; set; }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            NoteBook notebook = parameter as NoteBook;

            if (notebook == null) { return; }

            NoteVM.StopEditing(notebook);


        }

        public event EventHandler? CanExecuteChanged;
    }
}
