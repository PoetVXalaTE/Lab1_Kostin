using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading;
using WinFormsApp1;

namespace TestProject2
{
    [TestClass]
    public class NoteManagerTests
    {
        private const string FileName = "notes.txt";

        [TestInitialize]
        public void TestInitialize()
        {
            ForceDeleteFile();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            ForceDeleteFile();
        }
        private void ForceDeleteFile()
        {
            int maxRetries = 10;
            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    if (File.Exists(FileName))
                    {
                        File.Delete(FileName);
                    }
                    break;
                }
                catch (IOException)
                {
                    Thread.Sleep(500);
                }
            }
        }

        //Тест 1: Добавление заметки работает
        [TestMethod]
        public void AddNote_AddsNote()
        {
            var manager = new NoteManager();
            var note = new Note("Тест", "Контент", DateTime.Now);

            manager.AddNote(note);

            Assert.AreEqual(1, manager.Notes.Count);
            Assert.AreEqual("Тест", manager.Notes[0].Title);
        }

        //Тест 2: Добавление null выбрасывает ошибку
        [TestMethod]
        public void AddNote_Null_ThrowsError()
        {
            var manager = new NoteManager();

            Assert.ThrowsException<ArgumentNullException>(() => manager.AddNote(null));
        }

        //Тест 3: Удаление заметки работает
        [TestMethod]
        public void RemoveNote_RemovesNote()
        {
            var manager = new NoteManager();
            var note = new Note("Удалить", "Тест", DateTime.Now);
            manager.AddNote(note);

            manager.RemoveNote(note);

            Assert.AreEqual(0, manager.Notes.Count);
        }

        //Тест 4: Удаление null выбрасывает ошибку
        [TestMethod]
        public void RemoveNote_Null_ThrowsError()
        {
            var manager = new NoteManager();

            Assert.ThrowsException<ArgumentNullException>(() => manager.RemoveNote(null));
        }

        //Тест 5: Можно добавить несколько заметок
        [TestMethod]
        public void AddNote_MultipleNotes_AddsAll()
        {
            var manager = new NoteManager();

            manager.AddNote(new Note("Первая", "Контент 1", DateTime.Now));
            manager.AddNote(new Note("Вторая", "Контент 2", DateTime.Now));
            manager.AddNote(new Note("Третья", "Контент 3", DateTime.Now));

            Assert.AreEqual(3, manager.Notes.Count);
        }
    }
}