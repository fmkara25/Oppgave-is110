using System;
using System.Collections.Generic;
using System.Linq;
using UniversitySystem.Models;
using UniversitySystem.Services;

namespace UniversitySystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CourseService courseService = new CourseService();
            LibraryService libraryService = new LibraryService();

            List<Student> students = new List<Student>();
            List<Staff> staffMembers = new List<Staff>();

            SeedUsers(students, staffMembers);

            bool isRunning = true;

            while (isRunning)
            {
                PrintMenu();
                int choice = ReadMenuChoice();

                try
                {
                    switch (choice)
                    {
                        case 1:
                            CreateCourseFlow(courseService);
                            break;

                        case 2:
                            CourseEnrollmentMenuFlow(courseService, students);
                            break;

                        case 3:
                            PrintCoursesAndParticipantsFlow(courseService);
                            break;

                        case 4:
                            SearchCourseFlow(courseService);
                            break;

                        case 5:
                            BookMenuFlow(libraryService);
                            break;

                        case 6:
                            BorrowBookFlow(libraryService, students, staffMembers);
                            break;

                        case 7:
                            ReturnBookFlow(libraryService, students, staffMembers);
                            break;

                        case 8:
                            RegisterBookFlow(libraryService);
                            break;

                        case 0:
                            isRunning = false;
                            Console.WriteLine("Goodbye!");
                            break;

                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            Pause();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Error: {ex.Message}");
                    Pause();
                }
            }
        }

        static void PrintMenu()
        {
            Console.Clear();
            Console.WriteLine("=== University System ===");
            Console.WriteLine("[1] Create course");
            Console.WriteLine("[2] Enroll student to course");
            Console.WriteLine("[3] Print courses and participants");
            Console.WriteLine("[4] Search course");
            Console.WriteLine("[5] Search book");
            Console.WriteLine("[6] Borrow book");
            Console.WriteLine("[7] Return book");
            Console.WriteLine("[8] Register book");
            Console.WriteLine("[0] Exit");
            Console.WriteLine();
        }

        static int ReadMenuChoice()
        {
            while (true)
            {
                Console.Write("Select an option: ");
                string? input = Console.ReadLine();

                if (int.TryParse(input, out int choice))
                    return choice;

                Console.WriteLine("Please enter a valid number.");
            }
        }

        // -------- MAIN FLOWS --------

        static void CreateCourseFlow(CourseService courseService)
        {
            Console.WriteLine("--- Create Course ---");

            Console.Write("Course code: ");
            string code = Console.ReadLine() ?? "";

            Console.Write("Course name: ");
            string name = Console.ReadLine() ?? "";

            int credits = ReadInt("Credits: ");
            int maxCapacity = ReadInt("Max capacity: ");

            Course course = courseService.CreateCourse(code, name, credits, maxCapacity);

            Console.WriteLine();
            Console.WriteLine("Course created:");
            Console.WriteLine(course);

            Pause();
        }

        static void CourseEnrollmentMenuFlow(CourseService courseService, List<Student> students)
        {
            Console.WriteLine("--- Course Enrollment Menu ---");
            Console.WriteLine("[1] Enroll student");
            Console.WriteLine("[2] Unenroll student");
            Console.WriteLine("[0] Back");

            int choice = ReadInt("Select an option: ");
            Console.WriteLine();

            switch (choice)
            {
                case 1:
                    EnrollStudentToCourseFlow(courseService, students);
                    break;

                case 2:
                    UnenrollStudentFromCourseFlow(courseService, students);
                    break;

                case 0:
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    Pause();
                    break;
            }
        }

        static void SearchCourseFlow(CourseService courseService)
        {
            Console.WriteLine("--- Search Course ---");
            Console.Write("Keyword (code or name): ");
            string keyword = Console.ReadLine() ?? "";

            List<Course> results = courseService.SearchCourses(keyword);

            Console.WriteLine();
            if (results.Count == 0)
            {
                Console.WriteLine("No courses found.");
                Pause();
                return;
            }

            Console.WriteLine($"Found {results.Count} course(s):");
            foreach (Course c in results)
                Console.WriteLine(c);

            Pause();
        }

        static void BookMenuFlow(LibraryService libraryService)
        {
            Console.WriteLine("--- Book Menu ---");
            Console.WriteLine("[1] Search books");
            Console.WriteLine("[2] Show active loans");
            Console.WriteLine("[3] Show loan history");
            Console.WriteLine("[0] Back");

            int choice = ReadInt("Select an option: ");
            Console.WriteLine();

            switch (choice)
            {
                case 1:
                    SearchBookFlow(libraryService);
                    break;

                case 2:
                    ShowActiveLoansFlow(libraryService);
                    break;

                case 3:
                    ShowLoanHistoryFlow(libraryService);
                    break;

                case 0:
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    Pause();
                    break;
            }
        }

        static void PrintCoursesAndParticipantsFlow(CourseService courseService)
        {
            Console.WriteLine("--- Courses and Participants ---");

            if (courseService.Courses.Count == 0)
            {
                Console.WriteLine("No courses created.");
                Pause();
                return;
            }

            Console.WriteLine();
            foreach (Course course in courseService.Courses)
            {
                Console.WriteLine(course);

                if (course.EnrolledStudents.Count == 0)
                {
                    Console.WriteLine("  Participants: (none)");
                }
                else
                {
                    Console.WriteLine("  Participants:");
                    foreach (Student s in course.EnrolledStudents)
                        Console.WriteLine($"   - {s.StudentId}: {s.Name} ({s.Email})");
                }

                Console.WriteLine();
            }

            Pause();
        }

        static void EnrollStudentToCourseFlow(CourseService courseService, List<Student> students)
        {
            Console.WriteLine("--- Enroll Student To Course ---");

            if (students.Count == 0)
            {
                Console.WriteLine("No students available.");
                Pause();
                return;
            }

            Console.Write("Course code: ");
            string courseCode = Console.ReadLine() ?? "";

            Course? course = courseService.FindCourse(courseCode);
            if (course == null)
            {
                Console.WriteLine("Course not found.");
                Pause();
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Available students:");
            foreach (Student s in students)
                Console.WriteLine($"- {s.StudentId}: {s.Name} ({s.Email})");

            Console.Write("Student id: ");
            string studentId = Console.ReadLine() ?? "";

            Student? student = students.FirstOrDefault(s =>
                s.StudentId.Equals(studentId.Trim(), StringComparison.OrdinalIgnoreCase));

            if (student == null)
            {
                Console.WriteLine("Student not found.");
                Pause();
                return;
            }

            courseService.EnrollStudentToCourse(course.Code, student);

            Console.WriteLine();
            Console.WriteLine("Enrollment completed.");
            Console.WriteLine($"{student.Name} enrolled to {course.Code}.");

            Pause();
        }

        static void UnenrollStudentFromCourseFlow(CourseService courseService, List<Student> students)
        {
            Console.WriteLine("--- Unenroll Student From Course ---");

            if (students.Count == 0)
            {
                Console.WriteLine("No students available.");
                Pause();
                return;
            }

            Console.Write("Course code: ");
            string courseCode = Console.ReadLine() ?? "";

            Course? course = courseService.FindCourse(courseCode);
            if (course == null)
            {
                Console.WriteLine("Course not found.");
                Pause();
                return;
            }

            if (course.EnrolledStudents.Count == 0)
            {
                Console.WriteLine("No students are enrolled in this course.");
                Pause();
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Enrolled students:");
            foreach (Student s in course.EnrolledStudents)
                Console.WriteLine($"- {s.StudentId}: {s.Name} ({s.Email})");

            Console.Write("Student id: ");
            string studentId = Console.ReadLine() ?? "";

            Student? student = students.FirstOrDefault(s =>
                s.StudentId.Equals(studentId.Trim(), StringComparison.OrdinalIgnoreCase));

            if (student == null)
            {
                Console.WriteLine("Student not found.");
                Pause();
                return;
            }

            courseService.UnenrollStudentFromCourse(course.Code, student.StudentId);

            Console.WriteLine();
            Console.WriteLine("Unenrollment completed.");
            Console.WriteLine($"{student.Name} removed from {course.Code}.");

            Pause();
        }

        static void RegisterBookFlow(LibraryService libraryService)
        {
            Console.WriteLine("--- Register Book ---");

            Console.Write("Book id: ");
            string id = Console.ReadLine() ?? "";

            Console.Write("Title: ");
            string title = Console.ReadLine() ?? "";

            Console.Write("Author: ");
            string author = Console.ReadLine() ?? "";

            int year = ReadInt("Year: ");
            int copies = ReadInt("Copies: ");

            Book book = libraryService.RegisterBook(id, title, author, year, copies);

            Console.WriteLine();
            Console.WriteLine("Book registered:");
            Console.WriteLine(book);

            Pause();
        }

        static void SearchBookFlow(LibraryService libraryService)
        {
            Console.WriteLine("--- Search Book ---");
            Console.Write("Keyword (id, title, author): ");
            string keyword = Console.ReadLine() ?? "";

            List<Book> results = libraryService.SearchBooks(keyword);

            Console.WriteLine();
            if (results.Count == 0)
            {
                Console.WriteLine("No books found.");
                Pause();
                return;
            }

            Console.WriteLine($"Found {results.Count} book(s):");
            foreach (Book b in results)
                Console.WriteLine(b);

            Pause();
        }

        static void ShowActiveLoansFlow(LibraryService libraryService)
        {
            Console.WriteLine("--- Active Loans ---");

            List<Loan> activeLoans = libraryService.GetActiveLoans();

            Console.WriteLine();
            if (activeLoans.Count == 0)
            {
                Console.WriteLine("No active loans found.");
                Pause();
                return;
            }

            foreach (Loan loan in activeLoans)
                Console.WriteLine(loan);

            Pause();
        }

        static void ShowLoanHistoryFlow(LibraryService libraryService)
        {
            Console.WriteLine("--- Loan History ---");

            List<Loan> loanHistory = libraryService.GetLoanHistory();

            Console.WriteLine();
            if (loanHistory.Count == 0)
            {
                Console.WriteLine("No returned loans found.");
                Pause();
                return;
            }

            foreach (Loan loan in loanHistory)
                Console.WriteLine(loan);

            Pause();
        }

        static void BorrowBookFlow(LibraryService libraryService, List<Student> students, List<Staff> staffMembers)
        {
            Console.WriteLine("--- Borrow Book ---");

            User? user = SelectUserFlow(students, staffMembers);
            if (user == null)
            {
                Pause();
                return;
            }

            Console.Write("Book id: ");
            string bookId = Console.ReadLine() ?? "";

            Loan loan = libraryService.BorrowBook(user, bookId);

            Console.WriteLine();
            Console.WriteLine("Book borrowed successfully:");
            Console.WriteLine(loan);

            Pause();
        }

        static void ReturnBookFlow(LibraryService libraryService, List<Student> students, List<Staff> staffMembers)
        {
            Console.WriteLine("--- Return Book ---");

            User? user = SelectUserFlow(students, staffMembers);
            if (user == null)
            {
                Pause();
                return;
            }

            Console.Write("Book id: ");
            string bookId = Console.ReadLine() ?? "";

            libraryService.ReturnBook(user, bookId);

            Console.WriteLine();
            Console.WriteLine("Book returned successfully.");

            Pause();
        }

        static User? SelectUserFlow(List<Student> students, List<Staff> staffMembers)
        {
            Console.WriteLine("Select user type:");
            Console.WriteLine("[1] Student");
            Console.WriteLine("[2] Staff");

            int type = ReadInt("Type: ");

            if (type == 1)
            {
                if (students.Count == 0)
                {
                    Console.WriteLine("No students available.");
                    return null;
                }

                Console.WriteLine();
                Console.WriteLine("Students:");
                foreach (Student s in students)
                    Console.WriteLine($"- {s.StudentId}: {s.Name} ({s.Email})");

                Console.Write("Student email: ");
                string email = Console.ReadLine() ?? "";

                Student? student = students.FirstOrDefault(s =>
                    s.Email.Equals(email.Trim(), StringComparison.OrdinalIgnoreCase));

                if (student == null)
                {
                    Console.WriteLine("Student not found.");
                    return null;
                }

                return student;
            }

            if (type == 2)
            {
                if (staffMembers.Count == 0)
                {
                    Console.WriteLine("No staff available.");
                    return null;
                }

                Console.WriteLine();
                Console.WriteLine("Staff:");
                foreach (Staff st in staffMembers)
                    Console.WriteLine($"- {st.StaffId}: {st.Name} ({st.Email}) - {st.Position}, {st.Department}");

                Console.Write("Staff email: ");
                string email = Console.ReadLine() ?? "";

                Staff? staff = staffMembers.FirstOrDefault(s =>
                    s.Email.Equals(email.Trim(), StringComparison.OrdinalIgnoreCase));

                if (staff == null)
                {
                    Console.WriteLine("Staff not found.");
                    return null;
                }

                return staff;
            }

            Console.WriteLine("Invalid type.");
            return null;
        }

        // -------- HELPERS --------

        static int ReadInt(string label)
        {
            while (true)
            {
                Console.Write(label);
                string? input = Console.ReadLine();

                if (int.TryParse(input, out int value))
                    return value;

                Console.WriteLine("Please enter a valid number.");
            }
        }

        static void Pause()
        {
            Console.WriteLine();
            Console.Write("Press Enter to continue...");
            Console.ReadLine();
        }

        static void SeedUsers(List<Student> students, List<Staff> staffMembers)
        {
            students.Add(new Student("S1001", "Alice Johnson", "alice@uni.no"));
            students.Add(new Student("S1002", "Bob Smith", "bob@uni.no"));

            ExchangePeriod period = new ExchangePeriod(
                new DateTime(DateTime.Now.Year, 1, 10),
                new DateTime(DateTime.Now.Year, 6, 15));

            students.Add(new ExchangeStudent(
                "S2001",
                "Carlos Diaz",
                "carlos@uni.no",
                "University of Madrid",
                "Spain",
                period));

            staffMembers.Add(new Staff("T3001", "Dr. Emma Reed", "emma@uni.no", "Lecturer", "Computer Science"));
        }
    }
}