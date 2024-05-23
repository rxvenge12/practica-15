using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace practica_15
{
    public partial class MainWindow : Window
    {
        private List<Note> notes = new List<Note>();
        private string filePath = "notes.json";

        public MainWindow()
        {
            InitializeComponent();
            LoadNotes();
            DisplayNotesForToday();
            Calendar.SelectedDate = DateTime.Today; // Установка текущей даты в календаре
            Calendar.DisplayDate = DateTime.Today; // Отображение текущей даты в календаре
        }

        private void LoadNotes()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                notes = JsonConvert.DeserializeObject<List<Note>>(json);
            }
        }

        private void SaveNotes()
        {
            string json = JsonConvert.SerializeObject(notes);
            File.WriteAllText(filePath, json);
        }

        private void DisplayNotesForToday()
        {
            DateTime today = DateTime.Today;
            Note noteForToday = notes.Find(note => note.Date.Date == today);

            if (noteForToday == null)
            {
                MessageBox.Show("Сегодня нет заметок. Создайте новую.");
                // Дополнительный код для создания новой заметки
            }
            else
            {
                MessageBox.Show($"Заметка для сегодня:\n{noteForToday.Text}");
                // Дополнительный код для изменения или удаления существующей заметки
            }
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime selectedDate = Calendar.SelectedDate.GetValueOrDefault();
            DisplayNotesForDate(selectedDate);
        }

        private void DisplayNotesForDate(DateTime date)
        {
            Note noteForDate = notes.Find(note => note.Date.Date == date);

            if (noteForDate == null)
            {
                MessageBox.Show($"На {date.ToShortDateString()} нет заметок. Создайте новую.");
                // Дополнительный код для создания новой заметки для выбранной даты
            }
            else
            {
                MessageBox.Show($"Заметка на {date.ToShortDateString()}:\n{noteForDate.Text}");
                // Дополнительный код для изменения или удаления существующей заметки для выбранной даты
            }
        }

        private void AddOrUpdateNoteForDate(DateTime date, string text)
        {
            Note existingNote = notes.Find(note => note.Date.Date == date);

            if (existingNote == null)
            {
                notes.Add(new Note { Date = date, Text = text });
                MessageBox.Show("Заметка успешно добавлена.");
            }
            else
            {
                existingNote.Text = text;
                MessageBox.Show("Заметка успешно обновлена.");
            }

            SaveNotes();
        }

        private void DeleteNoteForDate(DateTime date)
        {
            Note existingNote = notes.Find(note => note.Date.Date == date);

            if (existingNote != null)
            {
                notes.Remove(existingNote);
                MessageBox.Show("Заметка успешно удалена.");
                SaveNotes();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string noteText = NoteTextBox.Text;
            DateTime selectedDate = Calendar.SelectedDate.GetValueOrDefault();
            AddOrUpdateNoteForDate(selectedDate, noteText);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime selectedDate = Calendar.SelectedDate.GetValueOrDefault();
            DeleteNoteForDate(selectedDate);
        }
    }

    public class Note
    {
        public DateTime Date { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
