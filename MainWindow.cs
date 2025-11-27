using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FocusPlanner
{
    public enum TaskCategory
    {
        Livelihood,
        FutureGrowth,
        PermanentResidency
    }

    public class MainWindow : Window
    {
        private List<TaskItem> tasks = new();
        private TextBox taskInput = null!;
        private ComboBox categoryComboBox = null!;
        private Button addButton = null!;
        private Button deleteButton = null!;
        private ListBox pendingTaskList = null!;
        private ListBox completedTaskList = null!;

        public MainWindow()
        {
            InitializeComponent();
            LoadDummyData();
            RenderTasks();
        }

        private void InitializeComponent()
        {
            Title = "Focus Planner";
            Height = 500;
            Width = 1000;

            var grid = new Grid { Margin = new Thickness(16) };
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var inputPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 0, 0, 8)
            };
            Grid.SetRow(inputPanel, 0);
            Grid.SetColumnSpan(inputPanel, 2);

            taskInput = new TextBox
            {
                Width = 400,
                Margin = new Thickness(0, 0, 8, 0),
                FontSize = 14,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            taskInput.KeyDown += (_, e) =>
            {
                if (e.Key == System.Windows.Input.Key.Enter)
                    AddTask();
            };

            categoryComboBox = new ComboBox
            {
                Width = 140,
                Margin = new Thickness(0, 0, 8, 0),
                FontSize = 14,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            categoryComboBox.Items.Add(TaskCategory.Livelihood);
            categoryComboBox.Items.Add(TaskCategory.FutureGrowth);
            categoryComboBox.Items.Add(TaskCategory.PermanentResidency);
            categoryComboBox.SelectedItem = TaskCategory.Livelihood;

            addButton = new Button
            {
                Content = "Add",
                Width = 80,
                Margin = new Thickness(0, 0, 8, 0)
            };
            addButton.Click += (_, __) => AddTask();

            deleteButton = new Button
            {
                Content = "Delete Done",
                Width = 100,
                Margin = new Thickness(0, 0, 8, 0)
            };
            deleteButton.Click += (_, __) => DeleteCompleted();

            inputPanel.Children.Add(taskInput);
            inputPanel.Children.Add(categoryComboBox);
            inputPanel.Children.Add(addButton);
            inputPanel.Children.Add(deleteButton);

            var pendingLabel = new TextBlock
            {
                Text = "미완료 / Pending",
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 8)
            };
            Grid.SetRow(pendingLabel, 1);
            Grid.SetColumn(pendingLabel, 0);

            var completedLabel = new TextBlock
            {
                Text = "완료됨 / Completed",
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 8)
            };
            Grid.SetRow(completedLabel, 1);
            Grid.SetColumn(completedLabel, 1);

            pendingTaskList = new ListBox
            {
                Margin = new Thickness(0, 0, 8, 0),
                BorderThickness = new Thickness(1),
                FontSize = 14
            };
            Grid.SetRow(pendingTaskList, 2);
            Grid.SetColumn(pendingTaskList, 0);

            completedTaskList = new ListBox
            {
                Margin = new Thickness(8, 0, 0, 0),
                BorderThickness = new Thickness(1),
                FontSize = 14
            };
            Grid.SetRow(completedTaskList, 2);
            Grid.SetColumn(completedTaskList, 1);

            grid.Children.Add(inputPanel);
            grid.Children.Add(pendingLabel);
            grid.Children.Add(completedLabel);
            grid.Children.Add(pendingTaskList);
            grid.Children.Add(completedTaskList);

            Content = grid;
        }

        private void LoadDummyData()
        {
            tasks = new List<TaskItem>
            {
                new TaskItem("Upload new YouTube video", false, TaskCategory.Livelihood),
                new TaskItem("Study CELPIP reading", false, TaskCategory.Livelihood),
                new TaskItem("Polish clicker game prototype", true, TaskCategory.Livelihood)
            };
        }

        private void AddTask()
        {
            string title = taskInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(title)) return;
            
            var selectedCategory = categoryComboBox.SelectedItem is TaskCategory category 
                ? category 
                : TaskCategory.Livelihood;
            
            tasks.Add(new TaskItem(title, false, selectedCategory));
            taskInput.Clear();
            RenderTasks();
        }

        private void DeleteCompleted()
        {
            tasks = tasks.Where(t => !t.IsCompleted).ToList();
            RenderTasks();
        }

        private void RenderTasks()
        {
            pendingTaskList.Items.Clear();
            completedTaskList.Items.Clear();

            foreach (var t in tasks)
            {
                var itemPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(5, 3, 0, 3)
                };

                var cb = new CheckBox
                {
                    Content = $"[{t.Category}] {t.Title}",
                    IsChecked = t.IsCompleted,
                    FontSize = 14,
                    VerticalAlignment = VerticalAlignment.Center
                };

                cb.Checked += (_, __) =>
                {
                    t.IsCompleted = true;
                    RenderTasks();
                };

                cb.Unchecked += (_, __) =>
                {
                    t.IsCompleted = false;
                    RenderTasks();
                };

                cb.MouseDoubleClick += (_, __) =>
                {
                    EditTask(t, itemPanel);
                };

                itemPanel.Children.Add(cb);
                var editButton = new Button
                {
                    Content = "✎",
                    Width = 25,
                    Height = 25,
                    Margin = new Thickness(5, 0, 0, 0),
                    FontSize = 12,
                    VerticalAlignment = VerticalAlignment.Center
                };
                editButton.Click += (_, __) => EditTask(t, itemPanel);
                itemPanel.Children.Add(editButton);

                if (t.IsCompleted)
                {
                    completedTaskList.Items.Add(itemPanel);
                }
                else
                {
                    pendingTaskList.Items.Add(itemPanel);
                }
            }
        }

        private void EditTask(TaskItem task, StackPanel itemPanel)
        {
            itemPanel.Children.Clear();

            var editTextBox = new TextBox
            {
                Text = task.Title,
                Width = 300,
                FontSize = 14,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 5, 0)
            };

            var editComboBox = new ComboBox
            {
                Width = 120,
                FontSize = 14,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 5, 0)
            };
            editComboBox.Items.Add(TaskCategory.Livelihood);
            editComboBox.Items.Add(TaskCategory.FutureGrowth);
            editComboBox.Items.Add(TaskCategory.PermanentResidency);
            editComboBox.SelectedItem = task.Category;

            var saveButton = new Button
            {
                Content = "✓",
                Width = 30,
                Height = 25,
                Margin = new Thickness(0, 0, 5, 0),
                FontSize = 14,
                VerticalAlignment = VerticalAlignment.Center
            };

            var cancelButton = new Button
            {
                Content = "✕",
                Width = 30,
                Height = 25,
                FontSize = 14,
                VerticalAlignment = VerticalAlignment.Center
            };

            saveButton.Click += (_, __) =>
            {
                string newTitle = editTextBox.Text.Trim();
                if (!string.IsNullOrWhiteSpace(newTitle))
                {
                    task.Title = newTitle;
                    if (editComboBox.SelectedItem is TaskCategory selectedCategory)
                    {
                        task.Category = selectedCategory;
                    }
                }
                RenderTasks();
            };

            cancelButton.Click += (_, __) =>
            {
                RenderTasks();
            };

            editTextBox.KeyDown += (_, e) =>
            {
                if (e.Key == System.Windows.Input.Key.Enter)
                {
                    saveButton.RaiseEvent(new System.Windows.RoutedEventArgs(Button.ClickEvent));
                }
                else if (e.Key == System.Windows.Input.Key.Escape)
                {
                    cancelButton.RaiseEvent(new System.Windows.RoutedEventArgs(Button.ClickEvent));
                }
            };

            itemPanel.Children.Add(editTextBox);
            itemPanel.Children.Add(editComboBox);
            itemPanel.Children.Add(saveButton);
            itemPanel.Children.Add(cancelButton);

            editTextBox.Focus();
            editTextBox.SelectAll();
        }
    }

    public class TaskItem
    {
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
        public TaskCategory Category { get; set; }

        public TaskItem(string title, bool isCompleted, TaskCategory category = TaskCategory.Livelihood)
        {
            Title = title;
            IsCompleted = isCompleted;
            Category = category;
        }
    }
}

