using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using StudentInfoManagementSystem.Models;

namespace StudentInfoManagementSystem
{
    public partial class CourseForm : Form
    {
        private List<Course> courses = new List<Course>();
        private TextBox txtCourseName, txtCredits;
        private DataGridView dgvCourses;

        public CourseForm()
        {
            InitializeUI();
            LoadSampleData();
            RefreshGrid();
        }

        private void InitializeUI()
        {
            this.Text = "Course Management";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lblName = new Label { Text = "Course Name:", Location = new Point(50, 50), AutoSize = true };
            txtCourseName = new TextBox { Location = new Point(150, 48), Width = 300 };

            Label lblCredits = new Label { Text = "Credits:", Location = new Point(50, 100), AutoSize = true };
            txtCredits = new TextBox { Location = new Point(150, 98), Width = 100 };

            Button btnAdd = new Button { Text = "Add", Location = new Point(150, 150), Width = 100 };
            btnAdd.Click += BtnAdd_Click;

            Button btnUpdate = new Button { Text = "Update", Location = new Point(270, 150), Width = 100 };
            btnUpdate.Click += BtnUpdate_Click;

            Button btnDelete = new Button { Text = "Delete", Location = new Point(390, 150), Width = 100 };
            btnDelete.Click += BtnDelete_Click;

            Button btnRefresh = new Button { Text = "Refresh", Location = new Point(510, 150), Width = 100 };
            btnRefresh.Click += (s, e) => RefreshGrid();

            dgvCourses = new DataGridView
            {
                Location = new Point(50, 200),
                Size = new Size(700, 350),
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            dgvCourses.SelectionChanged += DgvCourses_SelectionChanged;

            this.Controls.AddRange(new Control[] { lblName, txtCourseName, lblCredits, txtCredits, btnAdd, btnUpdate, btnDelete, btnRefresh, dgvCourses });
        }

        private void DgvCourses_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCourses.CurrentRow?.DataBoundItem is Course course)
            {
                txtCourseName.Text = course.CourseName;
                txtCredits.Text = course.Credits.ToString();
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCourseName.Text) || !int.TryParse(txtCredits.Text, out int credits))
            {
                MessageBox.Show("Please enter valid course name and credits.");
                return;
            }

            courses.Add(new Course(Guid.NewGuid().ToString().Substring(0, 8), txtCourseName.Text.Trim(), credits));
            RefreshGrid();
            ClearFields();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvCourses.CurrentRow?.DataBoundItem is Course selected)
            {
                if (string.IsNullOrWhiteSpace(txtCourseName.Text) || !int.TryParse(txtCredits.Text, out int credits))
                {
                    MessageBox.Show("Please enter valid data.");
                    return;
                }

                selected.CourseName = txtCourseName.Text.Trim();
                selected.Credits = credits;
                RefreshGrid();
            }
            else MessageBox.Show("Please select a course to update.");
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCourses.CurrentRow?.DataBoundItem is Course selected)
            {
                if (MessageBox.Show($"Delete {selected.CourseName}?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    courses.Remove(selected);
                    RefreshGrid();
                    ClearFields();
                }
            }
            else MessageBox.Show("Please select a course to delete.");
        }

        private void RefreshGrid()
        {
            dgvCourses.DataSource = null;
            dgvCourses.DataSource = courses;
        }

        private void ClearFields()
        {
            txtCourseName.Clear();
            txtCredits.Clear();
        }

        private void LoadSampleData()
        {
            courses.Add(new Course("C001", "Database Systems", 3));
            courses.Add(new Course("C002", ".Net Programming", 3));
            courses.Add(new Course("C003", "Computer Network", 3));
            courses.Add(new Course("C004", "HSK3", 5));
        }
    }
}