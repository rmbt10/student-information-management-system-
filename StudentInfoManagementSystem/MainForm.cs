using System;
using System.Drawing;
using System.Windows.Forms;

namespace StudentInfoManagementSystem
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Student Information Management System";
            this.Size = new Size(800, 600);
            this.IsMdiContainer = true; // Optional: for MDI style
            this.BackColor = Color.LightGray;

            MenuStrip menuStrip = new MenuStrip();
            ToolStripMenuItem fileMenu = new ToolStripMenuItem("File");
            fileMenu.DropDownItems.Add("Exit", null, (s, e) => Application.Exit());

            ToolStripMenuItem studentMenu = new ToolStripMenuItem("Student Management");
            studentMenu.Click += (s, e) => OpenForm(new StudentForm());

            ToolStripMenuItem courseMenu = new ToolStripMenuItem("Course Management");
            courseMenu.Click += (s, e) => OpenForm(new CourseForm());

            ToolStripMenuItem gradeMenu = new ToolStripMenuItem("Grade Management");
            gradeMenu.Click += (s, e) => OpenForm(new GradeForm());

            ToolStripMenuItem reportMenu = new ToolStripMenuItem("Reports");
            reportMenu.Click += (s, e) => OpenForm(new ReportForm());

            menuStrip.Items.Add(fileMenu);
            menuStrip.Items.Add(studentMenu);
            menuStrip.Items.Add(courseMenu);
            menuStrip.Items.Add(gradeMenu);
            menuStrip.Items.Add(reportMenu);

            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);
        }

        private void OpenForm(Form form)
        {
            form.MdiParent = this; // Optional: makes it child window
            form.WindowState = FormWindowState.Maximized;
            form.Show();
        }
    }
}