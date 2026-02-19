using System;
using System.Collections.Generic;
using System.Linq;
using UniversitySystem.Models;

namespace UniversitySystem.Services
{
    public class CourseService
    {
        private readonly List<Course> _courses = new();
        public IReadOnlyList<Course> Courses => _courses;

        public Course CreateCourse(string code, string name, int credits, int maxCapacity)
        {
            code = (code ?? "").Trim().ToUpperInvariant();

            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Course code cannot be empty.");

            if (FindCourse(code) != null)
                throw new InvalidOperationException("A course with this code already exists.");

            Course course = new Course(code, name, credits, maxCapacity);
            _courses.Add(course);
            return course;
        }

        public Course? FindCourse(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return null;

            code = code.Trim().ToUpperInvariant();
            return _courses.FirstOrDefault(c => c.Code == code);
        }

        public List<Course> SearchCourses(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<Course>();

            keyword = keyword.Trim();

            return _courses
                .Where(c =>
                    c.Code.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    c.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public void EnrollStudentToCourse(string courseCode, Student student)
        {
            Course? course = FindCourse(courseCode);
            if (course == null)
                throw new InvalidOperationException("Course not found.");

            course.EnrollStudent(student);
        }
    }
}
