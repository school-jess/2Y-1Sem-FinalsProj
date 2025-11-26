using SmartLibraryManagementSystemClassLibrary.Model;
using Microsoft.EntityFrameworkCore;

public class DatabaseContext : DbContext
{
    public DbSet<Book> Book { get; set; }
    public DbSet<Catalog> Catalog { get; set; }
    public DbSet<Fine> Fine { get; set; }
    public DbSet<Faculty> Faculty { get; set; }
    public DbSet<Loan> Loan { get; set; }
    public DbSet<Reservation> Reservation { get; set; }
    public DbSet<Student> Student { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<SessionData> SessionData { get; set; }
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
}
