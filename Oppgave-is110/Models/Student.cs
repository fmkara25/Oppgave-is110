using System;
using System.Collections.Generic;

namespace UniversitySystem.Models
{
    public class Student : User
    {
        public string StudentId { get; private set; } = string.Empty;

        private readonly List<string> _enrolledCourses = new();
        public IReadOnlyList<string> EnrolledCourses => _enrolledCourses;

        public Student(string studentId, string name, string email)
            : base(name, email)
        {
            SetStudentId(studentId);
        }

        public void SetStudentId(string studentId)
        {
            if (string.IsNullOrWhiteSpace(studentId))
                throw new ArgumentException("StudentId cannot be empty.");

            StudentId = studentId.Trim();
        }

        public void EnrollToCourse(string courseCode)
        {
            if (string.IsNullOrWhiteSpace(courseCode))
                throw new ArgumentException("Course code cannot be empty.");

            courseCode = courseCode.Trim().ToUpperInvariant();

            if (_enrolledCourses.Contains(courseCode))
                return;

            _enrolledCourses.Add(courseCode);
        }

        public override string ToString()
        {
            return $"Student {StudentId} - {base.ToString()}";
        }
    }
}
