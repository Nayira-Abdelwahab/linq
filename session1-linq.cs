using System;
using System.Linq;
using LibraryManagementSystem;
using LINQ_DATA;

namespace session1_linq
{
    class Program
    {
        static void Main(string[] args)
        {
            var books = LibraryData.Books;
            var authors = LibraryData.Authors;
            var members = LibraryData.Members;
            var loans = LibraryData.Loans;

            // 1. Find All Available Books
            books.Where(b => b.IsAvailable).ToConsoleTable("1. Available Books");

            // 2. Get All Book Titles
            books.Select(b => new { b.Title }).ToConsoleTable("2. Book Titles");

            // 3. Find Books by Genre (Programming)
            books.Where(b => b.Genre == "Programming").ToConsoleTable("3. Programming Books");

            // 4. Sort Books by Title
            books.OrderBy(b => b.Title).ToConsoleTable("4. Books Sorted by Title");

            // 5. Find Expensive Books (>30)
            books.Where(b => b.Price > 30).ToConsoleTable("5. Expensive Books");

            // 6. Get Unique Genres
            books.Select(b => new { b.Genre }).Distinct().ToConsoleTable("6. Unique Genres");

            // 7. Count Books by Genre
            books.GroupBy(b => b.Genre)
                 .Select(g => new { Genre = g.Key, Count = g.Count() })
                 .ToConsoleTable("7. Count by Genre");

            // 8. Find Recent Books (>2010)
            books.Where(b => b.PublishedYear > 2010).ToConsoleTable("8. Recent Books");

            // 9. Get First 5 Books
            books.Take(5).ToConsoleTable("9. First 5 Books");

            // 10. Check if Any Expensive Books Exist (>50)
            bool anyExpensive = books.Any(b => b.Price > 50);
            Console.WriteLine($"\n10. Any Expensive Books > $50? {anyExpensive}\n");

            // 11. Books with Author Information
            books.Join(authors,
                       b => b.AuthorId,
                       a => a.Id,
                       (b, a) => new { b.Title, AuthorName = a.Name, b.Genre })
                 .ToConsoleTable("11. Books with Authors");

            // 12. Average Price by Genre
            books.GroupBy(b => b.Genre)
                 .Select(g => new { Genre = g.Key, AveragePrice = g.Average(b => b.Price) })
                 .ToConsoleTable("12. Avg Price by Genre");

            // 13. Most Expensive Book
            var mostExpensiveBook = books.OrderByDescending(b => b.Price).FirstOrDefault();
            Console.WriteLine($"\n13. Most Expensive Book: {mostExpensiveBook?.Title} - ${mostExpensiveBook?.Price}\n");

            // 14. Group Books by Published Decade
            books.GroupBy(b => (b.PublishedYear / 10) * 10)
                 .Select(g => new { Decade = g.Key + "s", Books = string.Join(", ", g.Select(b => b.Title)) })
                 .ToConsoleTable("14. Books by Decade");

            // 15. Members with Active Loans
            loans.Where(l => l.ReturnDate == null)
                 .Join(members,
                       l => l.MemberId,
                       m => m.Id,
                       (l, m) => new { m.Name, l.BookId })
                 .Distinct()
                 .ToConsoleTable("15. Members with Active Loans");

            // 16. Books Borrowed More Than Once
            loans.GroupBy(l => l.BookId)
                 .Where(g => g.Count() > 1)
                 .Join(books,
                       g => g.Key,
                       b => b.Id,
                       (g, b) => new { b.Title, LoanCount = g.Count() })
                 .ToConsoleTable("16. Books Borrowed > Once");

            // 17. Overdue Books
            loans.Where(l => l.DueDate < DateTime.Now && l.ReturnDate == null)
                 .Join(books,
                       l => l.BookId,
                       b => b.Id,
                       (l, b) => new { b.Title, l.DueDate })
                 .ToConsoleTable("17. Overdue Books");

            // 18. Author Book Counts (desc)
            books.GroupBy(b => b.AuthorId)
                 .Join(authors,
                       g => g.Key,
                       a => a.Id,
                       (g, a) => new { Author = a.Name, Count = g.Count() })
                 .OrderByDescending(x => x.Count)
                 .ToConsoleTable("18. Author Book Counts");

            // 19. Price Range Analysis
            books.GroupBy(b =>
                b.Price < 20 ? "Cheap" :
                b.Price <= 40 ? "Medium" : "Expensive")
                 .Select(g => new { Range = g.Key, Count = g.Count() })
                 .ToConsoleTable("19. Price Ranges");

            // 20. Member Loan Statistics
            loans.GroupBy(l => l.MemberId)
                 .Join(members,
                       g => g.Key,
                       m => m.Id,
                       (g, m) => new
                       {
                           Member = m.Name,
                           TotalLoans = g.Count(),
                           ActiveLoans = g.Count(l => l.ReturnDate == null),
                           AverageDaysBorrowed = g.Average(l => ((l.ReturnDate ?? DateTime.Now) - l.LoanDate).TotalDays)
                       })
                 .ToConsoleTable("20. Member Loan Stats");
        }
    }
}
