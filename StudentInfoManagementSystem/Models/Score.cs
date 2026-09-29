using System;

namespace StudentInfoManagementSystem.Models
{
    public class Score
    {
        public Student Student { get; set; }
        public Course Course { get; set; }
        public double Grade { get; set; }      // e.g., 85.5

        public Score(Student student, Course course, double grade)
        {
            Student = student;
            Course = course;
            Grade = grade;
        }

        public Score() { }
    }
}