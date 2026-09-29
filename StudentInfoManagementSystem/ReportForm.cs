using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using StudentInfoManagementSystem.Models;

namespace StudentInfoManagementSystem
{
    public partial class ReportForm : Form
    {
        private List<Score> scores = new List<Score>();
        private List<Student> students = new List<Student>();
        private List<Course> courses = new List<Course>();

        private DataGridView dgvReports;
        private Button btnRefresh;

        public ReportForm()
        {
            InitializeUI();
            LoadSampleData(); // Same sample data as other forms for consistency
            GenerateReports();
        }

        private void InitializeUI()
        {
            this.Text = "Reports & Statistics";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 10);

            Label lblTitle = new Label
            {
                Text = "Statistical Reports",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Location = new Point(50, 30),
                AutoSize = true,
                ForeColor = Color.DarkBlue
            };

            btnRefresh = new Button
            {
                Text = "Refresh Reports",
                Location = new Point(50, 100),
                Width = 180,
                Height = 40,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White
            };
            btnRefresh.Click += (s, e) => GenerateReports();

            dgvReports = new DataGridView
            {
                Location = new Point(50, 160),
                Size = new Size(900, 480),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                GridColor = Color.LightGray,
                Font = new Font("Segoe UI", 10)
            };

            this.Controls.AddRange(new Control[] { lblTitle, btnRefresh, dgvReports });
        }

        private void GenerateReports()
        {
            dgvReports.Rows.Clear();
            dgvReports.Columns.Clear();

            if (scores.Count == 0)
            {
                dgvReports.Columns.Add("Info", "Information");
                dgvReports.Rows.Add("No grades recorded yet. Please add grades in Grade Management to see reports.");
                return;
            }

            // Columns
            dgvReports.Columns.Add("Type", "Report Type");
            dgvReports.Columns.Add("Name", "Name / Detail");
            dgvReports.Columns.Add("Average", "Average Grade");
            dgvReports.Columns.Add("Details", "Additional Info");

            // === 1. Average by Class ===
            dgvReports.Rows.Add("AVERAGE BY CLASS", "", "", "");
            var classStats = scores
                .GroupBy(s => s.Student.Class)
                .Select(g => new
                {
                    Class = g.Key,
                    Avg = g.Average(s => s.Grade),
                    StudentCount = g.Select(s => s.Student.StudentID).Distinct().Count()
                })
                .OrderByDescending(x => x.Avg);

            foreach (var stat in classStats)
            {
                dgvReports.Rows.Add("", stat.Class, stat.Avg.ToString("F2"), $"{stat.StudentCount} students");
            }

            // === 2. Average by Course ===
            dgvReports.Rows.Add("AVERAGE BY COURSE", "", "", "");
            var courseStats = scores
                .GroupBy(s => s.Course.CourseName)
                .Select(g => new
                {
                    Course = g.Key,
                    Avg = g.Average(s => s.Grade),
                    GradeCount = g.Count()
                })
                .OrderByDescending(x => x.Avg);

            foreach (var stat in courseStats)
            {
                dgvReports.Rows.Add("", stat.Course, stat.Avg.ToString("F2"), $"{stat.GradeCount} grades");
            }

            // === 3. Top 5 Students ===
            dgvReports.Rows.Add("TOP 5 STUDENTS (Overall Average)", "", "", "");
            var topStudents = scores
                .GroupBy(s => s.Student)
                .Select(g => new
                {
                    Student = g.Key,
                    Avg = g.Average(s => s.Grade),
                    CourseCount = g.Count()
                })
                .OrderByDescending(x => x.Avg)
                .Take(5);

            int rank = 1;
            foreach (var stu in topStudents)
            {
                dgvReports.Rows.Add(
                    $"#{rank}",
                    $"{stu.Student.Name} ({stu.Student.StudentID})",
                    stu.Avg.ToString("F2"),
                    $"{stu.CourseCount} courses"
                );
                rank++;
            }

            // Style header rows
            foreach (DataGridViewRow row in dgvReports.Rows)
            {
                if (row.Cells[0].Value?.ToString().Contains("AVERAGE") == true ||
                    row.Cells[0].Value?.ToString().Contains("TOP") == true)
                {
                    row.DefaultCellStyle.Font = new Font(dgvReports.Font, FontStyle.Bold);
                    row.DefaultCellStyle.BackColor = Color.LightBlue;
                    row.DefaultCellStyle.ForeColor = Color.DarkBlue;
                }
            }
        }

        private void LoadSampleData()
        {
            // Same sample data as in GradeForm for consistency
            students.Add(new Student("S001", "Alice Johnson", "Female", 20, "Class A", "Computer Science"));
            students.Add(new Student("S002", "Bob Smith", "Male", 21, "Class A", "Mathematics"));
            students.Add(new Student("S003", "Carol Lee", "Female", 19, "Class B", "Physics"));
            students.Add(new Student("S004", "David Kim", "Male", 20, "Class B", "Computer Science"));

            courses.Add(new Course("C001", "Database Systems", 3));
            courses.Add(new Course("C002", ".Net Programming", 3));
            courses.Add(new Course("C003", "Computer Network", 3));
            courses.Add(new Course("C004", "Data Structures", 4));

            scores.Add(new Score(students[0], courses[0], 92.5));
            scores.Add(new Score(students[0], courses[1], 88.0));
            scores.Add(new Score(students[1], courses[0], 85.5));
            scores.Add(new Score(students[2], courses[2], 95.0));
            scores.Add(new Score(students[3], courses[0], 90.0));
            scores.Add(new Score(students[3], courses[3], 87.5));
        }
    }
}