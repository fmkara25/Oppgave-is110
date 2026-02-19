using System;
using System.Collections.Generic;
using System.Linq;
using UniversitySystem.Models;

namespace UniversitySystem.Services
{
    public class LibraryService
    {
        private readonly List<Book> _books = new();
        private readonly List<Loan> _loans = new();

        public IReadOnlyList<Book> Books => _books;
        public IReadOnlyList<Loan> Loans => _loans;

        public Book RegisterBook(string id, string title, string author, int year, int copies)
        {
            id = (id ?? "").Trim();

            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Book id cannot be empty.");

            if (FindBookById(id) != null)
                throw new InvalidOperationException("A book with this id already exists.");

            Book book = new Book(id, title, author, year, copies);
            _books.Add(book);
            return book;
        }

        public Book? FindBookById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            id = id.Trim();
            return _books.FirstOrDefault(b => b.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
        }

        public List<Book> SearchBooks(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<Book>();

            keyword = keyword.Trim();

            return _books
                .Where(b =>
                    b.Id.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    b.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    b.Author.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public Loan BorrowBook(User user, string bookId)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            Book? book = FindBookById(bookId);
            if (book == null)
                throw new InvalidOperationException("Book not found.");

            if (!book.HasAvailableCopy())
                throw new InvalidOperationException("No copies available.");

            bool alreadyBorrowedSameBook = _loans.Any(l =>
                l.IsActive() &&
                l.User.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase) &&
                l.Book.Id.Equals(book.Id, StringComparison.OrdinalIgnoreCase));

            if (alreadyBorrowedSameBook)
                throw new InvalidOperationException("User already has an active loan for this book.");

            book.DecreaseCopies();

            Loan loan = new Loan(user, book, DateTime.Now);
            _loans.Add(loan);
            return loan;
        }

        public void ReturnBook(User user, string bookId)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            Book? book = FindBookById(bookId);
            if (book == null)
                throw new InvalidOperationException("Book not found.");

            Loan? activeLoan = _loans.FirstOrDefault(l =>
                l.IsActive() &&
                l.User.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase) &&
                l.Book.Id.Equals(book.Id, StringComparison.OrdinalIgnoreCase));

            if (activeLoan == null)
                throw new InvalidOperationException("Active loan not found for this user and book.");

            activeLoan.CloseLoan(DateTime.Now);
            book.IncreaseCopies();
        }
    }
}
