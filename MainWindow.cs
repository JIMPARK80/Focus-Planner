using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FocusPlanner
{
    public class MainWindow : Window
    {
        private List<TaskItem> tasks = new();
        private TextBox taskInput = null!;
        private Button addButton = null!;
        private Button deleteButton = null!;
        private Button refreshButton = null!;
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

            refreshButton = new Button
            {
                Content = "Refresh",
                Width = 80
            };
            refreshButton.Click += (_, __) => RenderTasks();

            inputPanel.Children.Add(taskInput);
            inputPanel.Children.Add(addButton);
            inputPanel.Children.Add(deleteButton);
            inputPanel.Children.Add(refreshButton);

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
                new TaskItem("Upload new YouTube video", false),
                new TaskItem("Study CELPIP reading", false),
                new TaskItem("Polish clicker game prototype", true)
            };
        }

        private void AddTask()
        {
            string title = taskInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(title)) return;
            tasks.Add(new TaskItem(title, false));
            taskInput.Clear();
            RenderTasks();
        }

        private void DeleteCompleted()
        {
            tasks = tasks.Where(t => !t.Done).ToList();
            RenderTasks();
        }

        private void RenderTasks()
        {
            pendingTaskList.Items.Clear();
            completedTaskList.Items.Clear();

            foreach (var t in tasks)
            {
                var cb = new CheckBox
                {
                    Content = t.Title,
                    IsChecked = t.Done,
                    FontSize = 14,
                    Margin = new Thickness(5, 3, 0, 3)
                };

                cb.Checked += (_, __) =>
                {
                    t.Done = true;
                    RenderTasks();
                };

                cb.Unchecked += (_, __) =>
                {
                    t.Done = false;
                    RenderTasks();
                };

                if (t.Done)
                {
                    completedTaskList.Items.Add(cb);
                }
                else
                {
                    pendingTaskList.Items.Add(cb);
                }
            }
        }
    }

    public class TaskItem
    {
        public string Title { get; set; }
        public bool Done { get; set; }

        public TaskItem(string title, bool done)
        {
            Title = title;
            Done = done;
        }
    }
}

