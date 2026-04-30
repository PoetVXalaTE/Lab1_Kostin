using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WinFormsApp1;

namespace TestProject2
{
    [TestClass]
    public class NoteTests
    {
        [TestMethod]
        public void Constructor_SetsAllPropertiesCorrectly()
        {
            var expectedDate = new DateTime(2024, 6, 15, 14, 30, 0);

            var note = new Note("Мой заголовок", "Мой текст", expectedDate);

            Assert.AreEqual("Мой заголовок", note.Title);
            Assert.AreEqual("Мой текст", note.Content);
            Assert.AreEqual(expectedDate, note.Date);
        }

        [TestMethod]
        public void Constructor_UsesPassedDateParameter()
        {
            var specificDate = new DateTime(2000, 1, 1, 12, 0, 0);

            var note = new Note("Тест", "Контент", specificDate);

            Assert.AreEqual(specificDate, note.Date);
        }

        [TestMethod]
        public void Properties_CanBeChangedAfterCreation()
        {
            var note = new Note("Старый", "Старый", DateTime.Now);

            note.Title = "Новый заголовок";
            note.Content = "Новый текст";
            note.Date = new DateTime(2025, 12, 31);

            Assert.AreEqual("Новый заголовок", note.Title);
            Assert.AreEqual("Новый текст", note.Content);
            Assert.AreEqual(new DateTime(2025, 12, 31), note.Date);
        }
        
        [TestMethod]
        public void File_CorrectCreating()
        {
            var manager = new NoteManager();
            var note = new Note("Удалить", "Тест", DateTime.Now);
            
            manager.AddNote(note);
            
            Assert.IsTrue(File.Exists(FileName))
        }

        [TestMethod]
        public void File_AbleToRead()
        {
            var manager = new NoteManager();
            manager.AddNote(new Note("Title", "Content", DateTime.Now));
            
            string text = File.ReadAllText(FileName);
            
            Assert.IsTrue(text.Length > 0);
        }

        [[TestMethod]
        public void File_SaveAndLoad_CorrectStructure()
        {
            var manager = new NoteManager();
            var originalNote = new Note("Test", "Hello, world!", DateTime.Now);

            manager.AddNote(originalNote);

            var loadedManager = new NoteManager();
            loadedManager.LoadFromFile(FileName);

            Assert.AreEqual(1, loadedManager.Notes.Count);

            var loadedNote = loadedManager.Notes[0];

            Assert.AreEqual(originalNote.Title, loadedNote.Title);
            Assert.AreEqual(originalNote.Text, loadedNote.Text);
        }
    }
}
