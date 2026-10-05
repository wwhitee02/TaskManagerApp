namespace TaskManagerApp
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            descriptionLabel = new Label();
            descriptionTextBox = new TextBox();
            addTaskButton = new Button();
            editTaskButton = new Button();
            removeTaskButton = new Button();
            toggleCompletionButton = new Button();
            tasksLabel = new Label();
            tasksListBox = new ListBox();
            SuspendLayout();
            // 
            // descriptionLabel
            // 
            descriptionLabel.AutoSize = true;
            descriptionLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            descriptionLabel.Location = new Point(12, 12);
            descriptionLabel.Name = "descriptionLabel";
            descriptionLabel.Size = new Size(196, 30);
            descriptionLabel.TabIndex = 0;
            descriptionLabel.Text = "Описание задачи:";
            // 
            // descriptionTextBox
            // 
            descriptionTextBox.Location = new Point(12, 60);
            descriptionTextBox.Name = "descriptionTextBox";
            descriptionTextBox.Size = new Size(440, 35);
            descriptionTextBox.TabIndex = 1;
            // 
            // addTaskButton
            // 
            addTaskButton.BackColor = SystemColors.InactiveBorder;
            addTaskButton.Cursor = Cursors.Hand;
            addTaskButton.FlatStyle = FlatStyle.Flat;
            addTaskButton.ForeColor = SystemColors.Desktop;
            addTaskButton.Location = new Point(12, 130);
            addTaskButton.Name = "addTaskButton";
            addTaskButton.Size = new Size(131, 40);
            addTaskButton.TabIndex = 2;
            addTaskButton.Text = "Добавить";
            addTaskButton.UseVisualStyleBackColor = false;
            addTaskButton.Click += addTaskButton_Click;
            // 
            // editTaskButton
            // 
            editTaskButton.BackColor = SystemColors.InactiveBorder;
            editTaskButton.Cursor = Cursors.Hand;
            editTaskButton.FlatStyle = FlatStyle.Flat;
            editTaskButton.ForeColor = SystemColors.Desktop;
            editTaskButton.Location = new Point(166, 130);
            editTaskButton.Name = "editTaskButton";
            editTaskButton.Size = new Size(131, 40);
            editTaskButton.TabIndex = 3;
            editTaskButton.Text = "Изменить";
            editTaskButton.UseVisualStyleBackColor = false;
            editTaskButton.Click += editTaskButton_Click;
            // 
            // removeTaskButton
            // 
            removeTaskButton.BackColor = SystemColors.InactiveBorder;
            removeTaskButton.Cursor = Cursors.Hand;
            removeTaskButton.FlatStyle = FlatStyle.Flat;
            removeTaskButton.ForeColor = SystemColors.Desktop;
            removeTaskButton.Location = new Point(321, 130);
            removeTaskButton.Name = "removeTaskButton";
            removeTaskButton.Size = new Size(131, 40);
            removeTaskButton.TabIndex = 4;
            removeTaskButton.Text = "Удалить";
            removeTaskButton.UseVisualStyleBackColor = false;
            removeTaskButton.Click += removeTaskButton_Click;
            // 
            // toggleCompletionButton
            // 
            toggleCompletionButton.BackColor = SystemColors.InactiveBorder;
            toggleCompletionButton.Cursor = Cursors.Hand;
            toggleCompletionButton.FlatStyle = FlatStyle.Flat;
            toggleCompletionButton.ForeColor = SystemColors.Desktop;
            toggleCompletionButton.Location = new Point(91, 207);
            toggleCompletionButton.Name = "toggleCompletionButton";
            toggleCompletionButton.Size = new Size(308, 44);
            toggleCompletionButton.TabIndex = 5;
            toggleCompletionButton.Text = "Отметить выполненной";
            toggleCompletionButton.UseVisualStyleBackColor = false;
            toggleCompletionButton.Click += toggleCompletionButton_Click;
            // 
            // tasksLabel
            // 
            tasksLabel.AutoSize = true;
            tasksLabel.Location = new Point(464, 9);
            tasksLabel.Name = "tasksLabel";
            tasksLabel.Size = new Size(147, 30);
            tasksLabel.TabIndex = 6;
            tasksLabel.Text = "Список задач:";
            // 
            // tasksListBox
            // 
            tasksListBox.FormattingEnabled = true;
            tasksListBox.Location = new Point(464, 60);
            tasksListBox.Name = "tasksListBox";
            tasksListBox.Size = new Size(390, 274);
            tasksListBox.TabIndex = 7;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.PaleTurquoise;
            ClientSize = new Size(886, 426);
            Controls.Add(tasksListBox);
            Controls.Add(tasksLabel);
            Controls.Add(toggleCompletionButton);
            Controls.Add(removeTaskButton);
            Controls.Add(editTaskButton);
            Controls.Add(addTaskButton);
            Controls.Add(descriptionTextBox);
            Controls.Add(descriptionLabel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Управление задачами v1.0";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label descriptionLabel;
        private TextBox descriptionTextBox;
        private Button addTaskButton;
        private Button editTaskButton;
        private Button removeTaskButton;
        private Button toggleCompletionButton;
        private Label tasksLabel;
        private ListBox tasksListBox;
    }
}
