using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using StudentInfoManagementSystem.Models;

namespace StudentInfoManagementSystem
{
    public partial class GradeForm : Form
    {
        private List<Score> scores = new List<Score>();
        private List<Student> students = new List<Student>();
        private List<Course> courses = new List<Course>();

        private ComboBox cmbStudent, cmbCourse;
        private TextBox txtGrade;
        private DataGridView dgvGrades;

        public GradeForm()
        {
            InitializeUI();
            LoadSampleData();
            RefreshGrid();
        }

        private void InitializeUI()
        {
            this.Text = "Grade Management";
            this.Size = new Size(900, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 10);

            int y = 40;

            Label lblStudent = new Label { Text = "Student:", Location = new Point(50, y + 5), AutoSize = true };
            cmbStudent = new ComboBox { Location = new Point(160, y), Width = 350, DropDownStyle = ComboBoxStyle.DropDownList };
            y += 60;

            Label lblCourse = new Label { Text = "Course:", Location = new Point(50, y + 5), AutoSize = true };
            cmbCourse = new ComboBox { Location = new Point(160, y), Width = 350, DropDownStyle = ComboBoxStyle.DropDownList };
            y += 60;

            Label lblGrade = new Label { Text = "Grade:", Location = new Point(50, y + 5), AutoSize = true };
            txtGrade = new TextBox { Location = new Point(160, y), Width = 150 };
            y += 100;

            Button btnAdd = new Button { Text = "Add", Location = new Point(160, y), Width = 100, Height = 35 };
            btnAdd.Click += BtnAdd_Click;

            Button btnUpdate = new Button { Text = "Update", Location = new Point(280, y), Width = 100, Height = 35 };
            btnUpdate.Click += BtnUpdate_Click;

            Button btnDelete = new Button { Text = "Delete", Location = new Point(400, y), Width = 100, Height = 35 };
            btnDelete.Click += BtnDelete_Click;

            Button btnRefresh = new Button { Text = "Refresh", Location = new Point(520, y), Width = 100, Height = 35 };
            btnRefresh.Click += (s, e) => RefreshGrid();

            dgvGrades = new DataGridView
            {
                Location = new Point(50, y + 80),
                Size = new Size(800, 350),
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false
            };
            dgvGrades.Columns.Add("Student", "Student");
            dgvGrades.Columns.Add("Course", "Course");
            dgvGrades.Columns.Add("Grade", "Grade");
            dgvGrades.SelectionChanged += DgvGrades_SelectionChanged;

            this.Controls.AddRange(new Control[] {
                lblStudent, cmbStudent, lblCourse, cmbCourse, lblGrade, txtGrade,
                btnAdd, btnUpdate, btnDelete, btnRefresh, dgvGrades
            });
        }

        private void DgvGrades_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvGrades.CurrentRow?.Tag is Score score)
            {
                cmbStudent.SelectedItem = score.Student;
                cmbCourse.SelectedItem = score.Course;
                txtGrade.Text = score.Grade.ToString("F1");
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (cmbStudent.SelectedItem is Student student && cmbCourse.SelectedItem is Course course &&
                double.TryParse(txtGrade.Text, out double grade) && grade >= 0 && grade <= 100)
            {
                scores.Add(new Score(student, course, grade));
                RefreshGrid();
                ClearFields();
            }
            else
            {
                MessageBox.Show("Please select student/course and enter valid grade (0-100).");
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvGrades.CurrentRow?.Tag is Score selected &&
                cmbStudent.SelectedItem is Student student && cmbCourse.SelectedItem is Course course &&
                double.TryParse(txtGrade.Text, out double grade) && grade >= 0 && grade <= 100)
            {
                selected.Student = student;
                selected.Course = course;
                selected.Grade = grade;
                RefreshGrid();
            }
            else
            {
                MessageBox.Show("Please select a grade and enter valid data.");
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvGrades.CurrentRow?.Tag is Score selected)
            {
                if (MessageBox.Show($"Delete grade for {selected.Student.Name} in {selected.Course.CourseName}?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    scores.Remove(selected);
                    RefreshGrid();
                    ClearFields();
                }
            }
            else
            {
                MessageBox.Show("Please select a grade to delete.");
            }
        }

        private void RefreshGrid()
        {
            dgvGrades.Rows.Clear();
            foreach (var score in scores)
            {
                int row = dgvGrades.Rows.Add(score.Student.Name, score.Course.CourseName, score.Grade.ToString("F1"));
                dgvGrades.Rows[row].Tag = score;
            }

            cmbStudent.DataSource = null;
            cmbStudent.DataSource = students;
            cmbStudent.DisplayMember = "Name";

            cmbCourse.DataSource = null;
            cmbCourse.DataSource = courses;
            cmbCourse.DisplayMember = "CourseName";
        }

        private void ClearFields()
        {
            cmbStudent.SelectedIndex = -1;
            cmbCourse.SelectedIndex = -1;
            txtGrade.Clear();
        }

        private void LoadSampleData()
        {
            // Load students and courses (you can share data from other forms if needed)
            students.Add(new Student("S001", "Alice Johnson", "Female", 20, "Class A", "Computer Science"));
            students.Add(new Student("S002", "Bob Smith", "Male", 21, "Class A", "Mathematics"));
            students.Add(new Student("S003", "Carol Lee", "Female", 19, "Class B", "Physics"));

            courses.Add(new Course("C001", "Database Systems", 3));
            courses.Add(new Course("C002", ".Net Programming", 3));
            courses.Add(new Course("C003", "Computer Network", 3));

            scores.Add(new Score(students[0], courses[0], 92.5));
            scores.Add(new Score(students[0], courses[1], 88.0));
            scores.Add(new Score(students[1], courses[0], 85.0));
            scores.Add(new Score(students[2], courses[2], 95.0));
        }
    }
}