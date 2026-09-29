using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using StudentInfoManagementSystem.Models;

namespace StudentInfoManagementSystem
{
    public partial class StudentForm : Form
    {
        private List<Student> students = new List<Student>();

        private TextBox txtStudentID, txtName, txtAge, txtClass, txtMajor;
        private ComboBox cmbGender;
        private DataGridView dgvStudents;

        public StudentForm()
        {
            InitializeUI();
            LoadSampleData();
            RefreshGrid();
        }

        private void InitializeUI()
        {
            this.Text = "Student Management";
            this.Size = new Size(900, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 10);

            int y = 40;

            // Labels and Controls
            AddLabelAndControl("Student ID:", out txtStudentID, y); y += 50;
            AddLabelAndControl("Name:", out txtName, y); y += 50;

            Label lblGender = new Label { Text = "Gender:", Location = new Point(50, y + 5), AutoSize = true };
            cmbGender = new ComboBox { Location = new Point(160, y), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbGender.Items.AddRange(new[] { "Male", "Female", "Other" });
            y += 50;

            AddLabelAndControl("Age:", out txtAge, y); y += 50;
            AddLabelAndControl("Class:", out txtClass, y); y += 50;
            AddLabelAndControl("Major:", out txtMajor, y); y += 80;

            // Buttons
            Button btnAdd = new Button { Text = "Add", Location = new Point(160, y), Width = 100, Height = 35 };
            btnAdd.Click += BtnAdd_Click;

            Button btnUpdate = new Button { Text = "Update", Location = new Point(280, y), Width = 100, Height = 35 };
            btnUpdate.Click += BtnUpdate_Click;

            Button btnDelete = new Button { Text = "Delete", Location = new Point(400, y), Width = 100, Height = 35 };
            btnDelete.Click += BtnDelete_Click;

            Button btnRefresh = new Button { Text = "Refresh", Location = new Point(520, y), Width = 100, Height = 35 };
            btnRefresh.Click += (s, e) => RefreshGrid();

            // DataGridView
            dgvStudents = new DataGridView
            {
                Location = new Point(50, y + 80),
                Size = new Size(800, 350),
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false
            };
            dgvStudents.SelectionChanged += DgvStudents_SelectionChanged;

            // Add all controls
            this.Controls.AddRange(new Control[] {
                lblGender, cmbGender,
                btnAdd, btnUpdate, btnDelete, btnRefresh,
                dgvStudents
            });
        }

        private void AddLabelAndControl(string labelText, out TextBox textBox, int y)
        {
            Label label = new Label { Text = labelText, Location = new Point(50, y + 5), AutoSize = true };
            textBox = new TextBox { Location = new Point(160, y), Width = 300 };
            this.Controls.Add(label);
            this.Controls.Add(textBox);
        }

        private void DgvStudents_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvStudents.CurrentRow?.DataBoundItem is Student student)
            {
                txtStudentID.Text = student.StudentID;
                txtName.Text = student.Name;
                cmbGender.SelectedItem = student.Gender;
                txtAge.Text = student.Age.ToString();
                txtClass.Text = student.Class;
                txtMajor.Text = student.Major;
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStudentID.Text) || string.IsNullOrWhiteSpace(txtName.Text) ||
                cmbGender.SelectedItem == null || !int.TryParse(txtAge.Text, out int age))
            {
                MessageBox.Show("Please fill all fields correctly.", "Invalid Input");
                return;
            }

            students.Add(new Student(
                txtStudentID.Text.Trim(),
                txtName.Text.Trim(),
                cmbGender.SelectedItem.ToString(),
                age,
                txtClass.Text.Trim(),
                txtMajor.Text.Trim()
            ));

            RefreshGrid();
            ClearFields();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvStudents.CurrentRow?.DataBoundItem is Student selected)
            {
                if (string.IsNullOrWhiteSpace(txtName.Text) || cmbGender.SelectedItem == null || !int.TryParse(txtAge.Text, out int age))
                {
                    MessageBox.Show("Please fill all fields correctly.");
                    return;
                }

                selected.Name = txtName.Text.Trim();
                selected.Gender = cmbGender.SelectedItem.ToString();
                selected.Age = age;
                selected.Class = txtClass.Text.Trim();
                selected.Major = txtMajor.Text.Trim();

                RefreshGrid();
            }
            else
            {
                MessageBox.Show("Please select a student to update.");
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvStudents.CurrentRow?.DataBoundItem is Student selected)
            {
                if (MessageBox.Show($"Delete student {selected.Name} ({selected.StudentID})?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    students.Remove(selected);
                    RefreshGrid();
                    ClearFields();
                }
            }
            else
            {
                MessageBox.Show("Please select a student to delete.");
            }
        }

        private void RefreshGrid()
        {
            dgvStudents.DataSource = null;
            dgvStudents.DataSource = students;
        }

        private void ClearFields()
        {
            txtStudentID.Clear();
            txtName.Clear();
            cmbGender.SelectedIndex = -1;
            txtAge.Clear();
            txtClass.Clear();
            txtMajor.Clear();
        }

        private void LoadSampleData()
        {
            students.Add(new Student("S001", "Alice Johnson", "Female", 20, "Class A", "Computer Science"));
            students.Add(new Student("S002", "Bob Smith", "Male", 21, "Class A", "Mathematics"));
            students.Add(new Student("S003", "Carol Lee", "Female", 19, "Class B", "Physics"));
            students.Add(new Student("S004", "David Kim", "Male", 20, "Class B", "Computer Science"));
            students.Add(new Student("S005", "Zakaria", "Male", 23, "22lq", "Computer Science"));
        }
    }
}