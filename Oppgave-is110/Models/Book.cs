using System;

namespace UniversitySystem.Models
{
    public class Book
    {
        public string Id { get; private set; } = string.Empty;
        public string Title { get; private set; } = string.Empty;
        public string Author { get; private set; } = string.Empty;
        public int Year { get; private set; }
        public int Copies { get; private set; }

        public Book(string id, string title, string author, int year, int copies)
        {
            SetId(id);
            SetTitle(title);
            SetAuthor(author);
            SetYear(year);
            SetCopies(copies);
        }

        public void SetId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Book id cannot be empty.");

            Id = id.Trim();
        }

        public void SetTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty.");

            Title = title.Trim();
        }

        public void SetAuthor(string author)
        {
            if (string.IsNullOrWhiteSpace(author))
                throw new ArgumentException("Author cannot be empty.");

            Author = author.Trim();
        }

        public void SetYear(int year)
        {
            int currentYear = DateTime.Now.Year;

            if (year < 1400 || year > currentYear)
                throw new ArgumentException("Year is not valid.");

            Year = year;
        }

        public void SetCopies(int copies)
        {
            if (copies < 0)
                throw new ArgumentException("Copies cannot be negative.");

            Copies = copies;
        }

        public bool HasAvailableCopy()
        {
            return Copies > 0;
        }

        public void DecreaseCopies()
        {
            if (Copies <= 0)
                throw new InvalidOperationException("No copies available.");

            Copies--;
        }

        public void IncreaseCopies()
        {
            Copies++;
        }

        public override string ToString()
        {
            return $"{Id} - {Title} by {Author} ({Year}) | Copies: {Copies}";
        }
    }
}
