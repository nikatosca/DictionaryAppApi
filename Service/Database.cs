using Microsoft.EntityFrameworkCore;
using dictionary_app.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dictionary_app.Services
{
    public class Database : IDatabase
    {
        private readonly AppDbContext _context;

        public Database(AppDbContext context)
        {
            _context = context;
        }

        // Метод для добавления слова
        public async Task AddWordAsync(Word word)
        {
            try
            {
                await _context.Words.AddAsync(word);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ошибка при добавлении слова в базу данных", ex);
            }
        }

        // Метод для получения всех слов
        public async Task<List<Word>> GetAllWordsAsync()
        {
            return await _context.Words.ToListAsync();
        }

        // Метод для получения слова по ID
        public async Task<Word> GetWordByIdAsync(int id)
        {
            return await _context.Words.FirstOrDefaultAsync(w => w.Id == id);
        }

        // Метод для проверки существования слова
        public async Task<bool> ContainsAsync(string term)
        {
            return await _context.Words
                .AnyAsync(word => word.Original.ToLower() == term.ToLower());
        }
        
        public async Task<bool> ContainsByIdAsync(int id)
        {
            return await _context.Words
                .FirstOrDefaultAsync(w => w.Id == id) != null;
        }
        

        // Метод для сохранения обновленных слов
        public async Task SaveWordAsync(Word word)
        {
            _context.Entry(word).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        // Метод для удаления слова
        public async Task RemoveWordAsync(string term)
        {
            var wordToDelete = await _context.Words
                .FirstOrDefaultAsync(w => w.Original.ToLower() == term.ToLower());
            if (wordToDelete != null)
            {
                _context.Words.Remove(wordToDelete);
                await _context.SaveChangesAsync();
            }
        }

        // Метод для получения слов по уровню
        public async Task<List<Word>> GetWordsByLevelAsync(int level)
        {
            return await _context.Words
                .Where(w => w.Level == level)
                .ToListAsync();
        }

        public async Task<Word> GetWordByOriginalAsync(string original)
        {
            return await _context.Words.FirstOrDefaultAsync(w => w.Original.ToLower() == original.ToLower());
        }
        
        
        // Метод для подсчета слов
        public async Task<int> CountWordsAsync()
        {
            var words = await _context.Words.ToListAsync();
            return words.Count;
        }
    }
}
