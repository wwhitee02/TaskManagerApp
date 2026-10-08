using System.Diagnostics;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.Core.Tools;
using FlaUI.Core.WindowsAPI;
using FlaUI.UIA3;

namespace TaskManagerApp.UITests
{
    // Автоматизация тест-кейсов из лабораторных работ №3 и №5 (библиотека FlaUI)
    [TestClass]
    public class TaskManagerUITests
    {
        private Application _app = null!;
        private UIA3Automation _automation = null!;
        private Window _mainWindow = null!;

        // Папка, где лежит собранная программа TaskManagerApp.exe
        private static readonly string AppFolder = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory, "..", "..", "..", "..",
            "TaskManagerApp", "bin", "Debug", "net10.0-windows"));

        private static readonly string AppPath = Path.Combine(AppFolder, "TaskManagerApp.exe");
        private static readonly string DataFile = Path.Combine(AppFolder, "tasks.txt");

        // Порядок пунктов в выпадающих списках
        private const int CategoryNone = 0;     // categoryComboBox: «Без категории»
        private const int CategoryWork = 1;     // categoryComboBox: «Работа» (первая созданная)
        private const int FilterAll = 0;        // filterComboBox: «Все категории»
        private const int FilterNone = 1;       // filterComboBox: «Без категории»
        private const int FilterWork = 2;       // filterComboBox: «Работа» (первая созданная)

        // Перед каждым тестом: удаляем файл с задачами и запускаем программу
        [TestInitialize]
        public void TestInitialize()
        {
            Assert.IsTrue(File.Exists(AppPath), $"Программа не найдена: {AppPath}. Сначала соберите решение (Ctrl+Shift+B).");
            File.Delete(DataFile);
            LaunchApp();
        }

        // После каждого теста: закрываем программу и удаляем файл с задачами
        [TestCleanup]
        public void TestCleanup()
        {
            CloseApp();
            File.Delete(DataFile);
        }

        // ---------- Вспомогательные методы ----------

        private void LaunchApp()
        {
            var startInfo = new ProcessStartInfo(AppPath) { WorkingDirectory = AppFolder };
            _app = Application.Launch(startInfo);
            _automation = new UIA3Automation();
            _mainWindow = _app.GetMainWindow(_automation, TimeSpan.FromSeconds(10));
        }

        private void CloseApp()
        {
            _automation?.Dispose();
            if (_app != null)
            {
                _app.Kill();
                _app.Dispose();
            }
            Thread.Sleep(300);
        }

        // Перезапуск программы (имитация закрытия и повторного открытия)
        private void RestartApp()
        {
            CloseApp();
            LaunchApp();
        }

        // Поиск элемента формы по имени (AutomationId = свойство Name в Windows Forms)
        private AutomationElement Find(string automationId)
        {
            var element = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId(automationId));
            Assert.IsNotNull(element, $"Элемент {automationId} не найден на форме");
            return element;
        }

        private void ClickButton(string automationId)
        {
            Find(automationId).AsButton().Click();
            Thread.Sleep(300);
        }

        private void SetText(string automationId, string text)
        {
            Find(automationId).AsTextBox().Text = text;
        }

        // Выбор пункта выпадающего списка с клавиатуры: Home — первый пункт, ↓ — следующий
        private void SelectInComboBox(string automationId, int index)
        {
            Find(automationId).Focus();
            Thread.Sleep(200);
            Keyboard.Press(VirtualKeyShort.HOME);
            Thread.Sleep(100);
            for (int i = 0; i < index; i++)
            {
                Keyboard.Press(VirtualKeyShort.DOWN);
                Thread.Sleep(100);
            }
            Thread.Sleep(300);
        }

        private string[] GetTaskItems()
        {
            return Find("tasksListBox").AsListBox().Items.Select(i => i.Text).ToArray();
        }

        private void SelectTask(int index)
        {
            Find("tasksListBox").AsListBox().Items[index].Select();
        }

        private void AddTask(string description)
        {
            SetText("descriptionTextBox", description);
            ClickButton("addTaskButton");
        }

        private void AddCategory(string name)
        {
            SetText("newCategoryTextBox", name);
            ClickButton("addCategoryButton");
        }

        // Ждёт окно сообщения, возвращает его текст и закрывает кнопкой OK
        private string ReadAndCloseMessageBox()
        {
            var messageBox = Retry.WhileNull(
                () => _mainWindow.ModalWindows.FirstOrDefault(),
                TimeSpan.FromSeconds(5)).Result;
            Assert.IsNotNull(messageBox, "Окно сообщения не появилось");

            string text = messageBox.FindFirstDescendant(cf => cf.ByAutomationId("65535"))?.Name ?? "";
            messageBox.FindFirstDescendant(cf => cf.ByAutomationId("2"))!.AsButton().Click();
            Thread.Sleep(300);
            return text;
        }

        // ---------- Тест-кейсы лабораторной работы №3 ----------

        [TestMethod]
        public void TC001_AddTask_ValidDescription()
        {
            AddTask("Купить хлеб");

            var items = GetTaskItems();
            Assert.AreEqual(1, items.Length);
            Assert.AreEqual("[ ] Купить хлеб", items[0]);
            Assert.AreEqual("", Find("descriptionTextBox").AsTextBox().Text);
        }

        [TestMethod]
        public void TC002_AddTask_EmptyDescription_ShowsError()
        {
            SetText("descriptionTextBox", "");
            ClickButton("addTaskButton");

            string message = ReadAndCloseMessageBox();
            Assert.IsTrue(message.Contains("Описание задачи не может быть пустым"), $"Сообщение: {message}");
            Assert.AreEqual(0, GetTaskItems().Length);
        }

        [TestMethod]
        public void TC003_AddTask_WhitespaceDescription_ShowsError()
        {
            SetText("descriptionTextBox", "   ");
            ClickButton("addTaskButton");

            string message = ReadAndCloseMessageBox();
            Assert.IsTrue(message.Contains("Описание задачи не может быть пустым"), $"Сообщение: {message}");
            Assert.AreEqual(0, GetTaskItems().Length);
        }

        [TestMethod]
        public void TC004_ToggleCompletion_MarksTaskCompleted()
        {
            AddTask("Купить хлеб");
            SelectTask(0);

            ClickButton("toggleCompletionButton");

            Assert.AreEqual("[X] Купить хлеб", GetTaskItems()[0]);
        }

        [TestMethod]
        public void TC006_ToggleCompletion_WithoutSelection_ShowsError()
        {
            AddTask("Купить хлеб");

            ClickButton("toggleCompletionButton");

            string message = ReadAndCloseMessageBox();
            Assert.IsTrue(message.Contains("Выберите задачу для изменения статуса"), $"Сообщение: {message}");
        }

        [TestMethod]
        public void TC007_EditTask_ChangesDescription()
        {
            AddTask("Купить хлеб");
            SelectTask(0);
            SetText("descriptionTextBox", "Купить молоко");

            ClickButton("editTaskButton");

            Assert.AreEqual("[ ] Купить молоко", GetTaskItems()[0]);
        }

        [TestMethod]
        public void TC010_RemoveTask_WithoutSelection_ShowsError()
        {
            AddTask("Купить хлеб");

            ClickButton("removeTaskButton");

            string message = ReadAndCloseMessageBox();
            Assert.IsTrue(message.Contains("Выберите задачу для удаления"), $"Сообщение: {message}");
            Assert.AreEqual(1, GetTaskItems().Length);
        }

        [TestMethod]
        public void TC011_RemoveTask_RemovesSelectedTask()
        {
            AddTask("Купить хлеб");
            SelectTask(0);

            ClickButton("removeTaskButton");

            Assert.AreEqual(0, GetTaskItems().Length);
        }

        [TestMethod]
        public void TC013_TasksRestoredAfterRestart()
        {
            AddTask("Купить хлеб");
            SelectTask(0);
            ClickButton("toggleCompletionButton");

            RestartApp();

            Assert.AreEqual("[X] Купить хлеб", GetTaskItems()[0]);
        }

        [TestMethod]
        public void TC015_TaskWithPipeRestoredAfterRestart()
        {
            AddTask("Хлеб|молоко");

            RestartApp();

            Assert.AreEqual("[ ] Хлеб|молоко", GetTaskItems()[0]);
        }

        // ---------- Тест-кейсы лабораторной работы №5 (категории) ----------

        [TestMethod]
        public void TC5_001_006_CreateCategoryAndAddTask()
        {
            AddCategory("Работа");
            AddTask("Сделать отчёт");

            // Созданная категория выбирается автоматически, поэтому задача попадает в неё
            Assert.AreEqual("[ ] Сделать отчёт (Работа)", GetTaskItems()[0]);
            Assert.AreEqual("", Find("newCategoryTextBox").AsTextBox().Text);
        }

        [TestMethod]
        public void TC5_003_DuplicateCategory_ShowsError()
        {
            AddCategory("Работа");

            AddCategory("Работа");

            string message = ReadAndCloseMessageBox();
            Assert.IsTrue(message.Contains("уже существует"), $"Сообщение: {message}");
        }

        [TestMethod]
        public void TC5_008_FilterByCategory_ShowsOnlyThatCategory()
        {
            AddCategory("Работа");
            AddTask("Сделать отчёт");
            SelectInComboBox("categoryComboBox", CategoryNone);
            AddTask("Купить хлеб");

            SelectInComboBox("filterComboBox", FilterWork);

            var items = GetTaskItems();
            Assert.AreEqual(1, items.Length);
            Assert.AreEqual("[ ] Сделать отчёт (Работа)", items[0]);
        }

        [TestMethod]
        public void TC5_009_FilterWithoutCategory_ShowsOnlyTasksWithoutCategory()
        {
            AddCategory("Работа");
            AddTask("Сделать отчёт");
            SelectInComboBox("categoryComboBox", CategoryNone);
            AddTask("Купить хлеб");

            SelectInComboBox("filterComboBox", FilterNone);

            var items = GetTaskItems();
            Assert.AreEqual(1, items.Length);
            Assert.AreEqual("[ ] Купить хлеб", items[0]);
        }

        [TestMethod]
        public void TC5_011_ToggleTaskWithFilter_ChangesCorrectTask()
        {
            AddCategory("Работа");
            AddTask("Сделать отчёт");
            SelectInComboBox("categoryComboBox", CategoryNone);
            AddTask("Купить хлеб");
            SelectInComboBox("filterComboBox", FilterWork);
            SelectTask(0);

            ClickButton("toggleCompletionButton");
            SelectInComboBox("filterComboBox", FilterAll);

            var items = GetTaskItems();
            CollectionAssert.Contains(items, "[X] Сделать отчёт (Работа)");
            CollectionAssert.Contains(items, "[ ] Купить хлеб");
        }

        [TestMethod]
        public void TC5_012_CategoriesRestoredAfterRestart()
        {
            AddCategory("Работа");
            AddTask("Сделать отчёт");

            RestartApp();

            Assert.AreEqual("[ ] Сделать отчёт (Работа)", GetTaskItems()[0]);
            SelectInComboBox("filterComboBox", FilterWork);
            Assert.AreEqual(1, GetTaskItems().Length);
        }
    }
}