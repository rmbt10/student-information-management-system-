using System;

namespace StudentInfoManagementSystem.Models
{
    public class Student
    {
        public string StudentID { get; set; }  // Unique ID, e.g., "S001"
        public string Name { get; set; }
        public string Gender { get; set; }     // "Male", "Female", etc.
        public int Age { get; set; }
        public string Class { get; set; }      // e.g., "Class 101"
        public string Major { get; set; }      // e.g., "Computer Science"

        // Constructor for easy creation
        public Student(string id, string name, string gender, int age, string className, string major)
        {
            StudentID = id;
            Name = name;
            Gender = gender;
            Age = age;
            Class = className;
            Major = major;
        }

        // Parameterless constructor for serialization
        public Student() { }
    }
}