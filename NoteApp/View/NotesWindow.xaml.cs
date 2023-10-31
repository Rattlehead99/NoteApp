using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Printing.Interop;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using NoteApp.ViewModel;
using NoteApp.ViewModel.Helpers;

namespace NoteApp.View
{
    /// <summary>
    /// Interaction logic for NotesWindow.xaml
    /// </summary>
    public partial class NotesWindow : Window
    {
        private NotesViewModel notesVM;
        public NotesWindow()
        {
            InitializeComponent();

            notesVM = Resources["NotesVM"] as NotesViewModel;
            notesVM.SelectedNoteChanged += NotesVM_SelectedNoteChanged;

            List<FontFamily> fontFamilies = Fonts.SystemFontFamilies.OrderBy(font => font.Source).ToList();
            FontFamilyComboBox.ItemsSource = fontFamilies;

            List<double> fontSizes = new List<double>() { 8, 9, 10, 12, 14, 16, 18, 20, 24, 28, 30 };
            FontSizeComboBox.ItemsSource = fontSizes;
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            if (string.IsNullOrEmpty(App.UserId))
            {
                Login loginWindow = new Login();
                loginWindow.ShowDialog();

                notesVM.GetNotebooks();
            }
        }

        private void NotesVM_SelectedNoteChanged(object sender, EventArgs e)
        {
            ContentRichTextBox.Document.Blocks.Clear();

            if (string.IsNullOrEmpty(notesVM.SelectedNote.FileLocation) && notesVM.SelectedNote == null) { return; }

            FileStream fileStream = new FileStream(notesVM.SelectedNote.FileLocation, FileMode.Open);
            TextRange contents = new TextRange(ContentRichTextBox.Document.ContentStart,
                ContentRichTextBox.Document.ContentEnd);
            contents.Load(fileStream, DataFormats.Rtf);
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); 
        }
        
        private async void Speech_OnClick(object sender, RoutedEventArgs e)
        {
            string region = "westeurope";
            string apiKey = "9b74d31d4ee348e7b7f6904f60888f2b";

            SpeechConfig? speechConfig = SpeechConfig.FromSubscription(apiKey, region);

            using (var audioConfig = AudioConfig.FromDefaultMicrophoneInput())
            {
                using (SpeechRecognizer recognizer = new SpeechRecognizer(speechConfig, audioConfig))
                {
                    SpeechRecognitionResult? result = await recognizer.RecognizeOnceAsync();

                    ContentRichTextBox.Document.Blocks.Add(new Paragraph(new Run(result.Text)));
                }
            }
        }

        private void ContentRichTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            int characterCount = new TextRange(ContentRichTextBox.Document.ContentStart,
                ContentRichTextBox.Document.ContentEnd).Text.Length;
            StatusTextBlock.Text = $"Document length: {characterCount} characters";
        }

        private void BoldButton_OnClick(object sender, RoutedEventArgs e)
        {
            //var textToBold = new TextRange(ContentRichTextBox.Selection.Start, ContentRichTextBox.Selection.End);
            bool isChecked = (sender as ToggleButton).IsChecked ?? false;

            if (isChecked)
            {
                ContentRichTextBox.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Bold);
                return;
            }

            ContentRichTextBox.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Normal);

        }

        private void ContentRichTextBox_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            object? selectedWeight = ContentRichTextBox.Selection.GetPropertyValue(Inline.FontWeightProperty);
            object? selectedStyle = ContentRichTextBox.Selection.GetPropertyValue(Inline.FontStyleProperty);
            object? selectedDecoration = ContentRichTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty);

            BoldButton.IsChecked = (selectedWeight != DependencyProperty.UnsetValue) && selectedWeight.Equals(FontWeights.Bold);
            ItalicButton.IsChecked = (selectedWeight != DependencyProperty.UnsetValue) && selectedWeight.Equals(FontStyles.Italic);
            UnderlineButton.IsChecked = (selectedWeight != DependencyProperty.UnsetValue) && selectedWeight.Equals(TextDecorations.Underline);

            FontFamilyComboBox.SelectedItem = ContentRichTextBox.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            FontSizeComboBox.Text = (ContentRichTextBox.Selection.GetPropertyValue(Inline.FontSizeProperty)).ToString();
        }

        private void ItalicButton_OnClick(object sender, RoutedEventArgs e)
        {
            bool isChecked = (sender as ToggleButton).IsChecked ?? false;

            if (isChecked)
            {
                ContentRichTextBox.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Italic);
                return;
            }
            ContentRichTextBox.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Normal);
        }

        private void UnderlineButton_OnClick(object sender, RoutedEventArgs e)
        {
            bool isChecked = (sender as ToggleButton).IsChecked ?? false;

            if (isChecked)
            {
                ContentRichTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
                return;
            }
            ContentRichTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
        }

        private void FontFamilyComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontFamilyComboBox.SelectedItem == null) { return; }

            ContentRichTextBox.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, FontFamilyComboBox.SelectedItem);
        }

        private void FontSizeComboBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (FontSizeComboBox.SelectedItem == null) { return; }

            ContentRichTextBox.Selection.ApplyPropertyValue(Inline.FontSizeProperty, FontSizeComboBox.Text);
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            string rtfFile = System.IO.Path.Combine(Environment.CurrentDirectory, $"{notesVM.SelectedNote.Id}.rtf");
            notesVM.SelectedNote.FileLocation = rtfFile;
            DatabaseHelper.Update(notesVM.SelectedNote);

            FileStream fileStream = new FileStream(rtfFile, FileMode.Create);
            TextRange contents = new TextRange(ContentRichTextBox.Document.ContentStart,
                ContentRichTextBox.Document.ContentEnd);
            contents.Save(fileStream,DataFormats.Rtf);
        }
    }
}
