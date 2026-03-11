using System;
using System.Collections.Generic;
using System.Linq;

namespace UniversitySystem.Models
{
    public class Course
    {
        public string Code { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public int Credits { get; private set; }
        public int MaxCapacity { get; private set; }

        private readonly List<Student> _enrolledStudents = new();
        public IReadOnlyList<Student> EnrolledStudents => _enrolledStudents;

        public Course(string code, string name, int credits, int maxCapacity)
        {
            SetCode(code);
            SetName(name);
            SetCredits(credits);
            SetMaxCapacity(maxCapacity);
        }

        public void SetCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Course code cannot be empty.");

            Code = code.Trim().ToUpperInvariant();
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Course name cannot be empty.");

            Name = name.Trim();
        }

        public void SetCredits(int credits)
        {
            if (credits <= 0)
                throw new ArgumentException("Credits must be greater than 0.");

            Credits = credits;
        }

        public void SetMaxCapacity(int maxCapacity)
        {
            if (maxCapacity <= 0)
                throw new ArgumentException("MaxCapacity must be greater than 0.");

            MaxCapacity = maxCapacity;
        }

        public bool IsFull()
        {
            return _enrolledStudents.Count >= MaxCapacity;
        }

        public bool IsStudentEnrolled(string studentId)
        {
            if (string.IsNullOrWhiteSpace(studentId))
                return false;

            studentId = studentId.Trim();
            return _enrolledStudents.Any(s => s.StudentId.Equals(studentId, StringComparison.OrdinalIgnoreCase));
        }

        public void EnrollStudent(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            if (IsFull())
                throw new InvalidOperationException("Course is full.");

            if (IsStudentEnrolled(student.StudentId))
                return; // already enrolled

            _enrolledStudents.Add(student);

            // keep Student side consistent (stores course codes)
            student.EnrollToCourse(Code);
        }

        public void RemoveStudent(string studentId)
        {
            if (string.IsNullOrWhiteSpace(studentId))
                throw new ArgumentException("Student id cannot be empty.");

            Student? student = _enrolledStudents.FirstOrDefault(s =>
                s.StudentId.Equals(studentId.Trim(), StringComparison.OrdinalIgnoreCase));

            if (student == null)
                throw new InvalidOperationException("Student is not enrolled in this course.");

            _enrolledStudents.Remove(student);
            student.RemoveCourse(Code);
        }

        public override string ToString()
        {
            return $"{Code} - {Name} ({Credits} credits) [{_enrolledStudents.Count}/{MaxCapacity}]";
        }
    }
}
