using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NoteApp.Model;
using NoteApp.ViewModel.Commands.NotesCommands;
using NoteApp.ViewModel.Helpers;

namespace NoteApp.ViewModel
{
    public class NotesViewModel : INotifyPropertyChanged
    {
        private NoteBook _selectedNotebook;
        private Visibility _isVisible;
        private Note _selectedNote;

        public NotesViewModel()
        {
            NewNoteCommand = new NewNoteCommand(this);
            NewNotebookCommand = new NewNotebookCommand(this);
            EditCommand = new EditCommand(this);
            EndEditingCommand = new EndEditingCommand(this);

            NoteBooks = new ObservableCollection<NoteBook>();
            Notes = new ObservableCollection<Note>();

            IsVisible = Visibility.Collapsed;

            GetNotebooks();
        }

        public ObservableCollection<NoteBook> NoteBooks { get; set; }

        public ObservableCollection<Note> Notes { get; set; }

        public NoteBook SelectedNotebook
        {
            get { return _selectedNotebook; }
            set
            {
                if (Equals(value, _selectedNotebook)) return;
                _selectedNotebook = value;
                OnPropertyChanged(nameof(SelectedNotebook));
                this.GetNotes();
            }
        }

        public Note SelectedNote
        {
            get { return _selectedNote; }
            set
            {
                _selectedNote = value;
                OnPropertyChanged(nameof(SelectedNote));
                SelectedNoteChanged?.Invoke(this, new EventArgs());
            }
        }

        public Visibility IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                OnPropertyChanged(nameof(IsVisible));
            }
        }

        public NewNotebookCommand NewNotebookCommand { get; set; }

        public NewNoteCommand NewNoteCommand { get; set; }

        public EditCommand EditCommand { get; set; }

        public EndEditingCommand EndEditingCommand { get; set; }

        public async void CreateNote(string notebookId)
        {
            Note newNote = new Note()
            {
                NotebookId = notebookId,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
                Title = "New Note"
            };
            await DatabaseHelper.Insert<Note>(newNote);

            this.GetNotes();
        }

        public async void CreateNoteBook()
        {
            NoteBook newNoteBook = new NoteBook()
            {
                Name = "New Notebook",
                UserId = App.UserId
            };
            await DatabaseHelper.Insert<NoteBook>(newNoteBook);

            this.GetNotebooks();
        }

        public async void GetNotebooks()
        {
            List<NoteBook> notebooks = (await DatabaseHelper.ReadAll<NoteBook>()).Where(notebook => notebook.UserId == App.UserId).ToList();
            NoteBooks.Clear();
            notebooks.ForEach(notebook => NoteBooks.Add(notebook));
        }

        private async void GetNotes()
        {
            if (SelectedNotebook == null)
            {
                return;
            }

            List<Note> notes =  (await DatabaseHelper.ReadAll<Note>()).Where(note => note.Id == SelectedNotebook.Id)
                    .ToList();
                Notes.Clear();
                notes.ForEach(note => Notes.Add(note));
        }

        public void StartEditing()
        {
            //TODO: Change a property bound to the TextBox visibility. - done
            IsVisible = Visibility.Visible;
        }
        public void StopEditing(NoteBook notebook)
        {
            //TODO: Change a property bound to the TextBox visibility. - done
            IsVisible = Visibility.Collapsed;
            DatabaseHelper.Update(notebook);
            GetNotebooks();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? SelectedNoteChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
