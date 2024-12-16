using dictionary_app.Services;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Word> Words { get; set; }

    // Конструктор с параметром DbContextOptions
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Метод для указания имени таблицы
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Указание имени таблицы для модели Word
        modelBuilder.Entity<Word>().ToTable("Words");
    }
    
}