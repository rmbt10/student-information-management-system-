using System;

namespace StudentInfoManagementSystem.Models
{
    public class Course
    {
        public string CourseID { get; set; }   // Unique ID, e.g., "C001"
        public string CourseName { get; set; }
        public int Credits { get; set; }

        public Course(string id, string name, int credits)
        {
            CourseID = id;
            CourseName = name;
            Credits = credits;
        }

        public Course() { }
    }
}