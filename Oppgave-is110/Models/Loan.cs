using System;

namespace UniversitySystem.Models
{
    public class Loan
    {
        public User User { get; private set; }
        public Book Book { get; private set; }
        public DateTime BorrowDate { get; private set; }
        public DateTime? ReturnDate { get; private set; }

        public Loan(User user, Book book, DateTime borrowDate)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
            Book = book ?? throw new ArgumentNullException(nameof(book));
            BorrowDate = borrowDate;
        }

        public bool IsActive()
        {
            return ReturnDate == null;
        }

        public void CloseLoan(DateTime returnDate)
        {
            if (ReturnDate != null)
                throw new InvalidOperationException("Loan is already closed.");

            if (returnDate < BorrowDate)
                throw new ArgumentException("Return date cannot be earlier than borrow date.");

            ReturnDate = returnDate;
        }

        public override string ToString()
        {
            string status = IsActive()
                ? "Active"
                : $"Returned: {ReturnDate:yyyy-MM-dd}";

            return $"{User} | {Book.Title} | Borrowed: {BorrowDate:yyyy-MM-dd} | {status}";
        }
    }
}
