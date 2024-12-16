using System.Collections.Generic;
using System.Threading.Tasks;
using dictionary_app.Services;

namespace dictionary_app.Interfaces
{
    public interface IDatabase
    {
        // Метод для добавления нового слова
        Task AddWordAsync(Word word);

        // Метод для удаления слова по его оригиналу
        Task RemoveWordAsync(string term);
        
        // Метод для получения всех слов из базы данных
        Task<List<Word>> GetAllWordsAsync();

        // Метод для подсчета слов с учетом их уровня
        Task<int> CountWordsAsync();
        
        // Метод для проверки наличия слова по его оригиналу
        Task<bool> ContainsAsync(string term);

        // Метод для получения слов по уровню
        Task<List<Word>> GetWordsByLevelAsync(int level);

        // Метод для получения слова по ID
        Task<Word> GetWordByIdAsync(int id);

        // Метод для сохранения обновленного слова
        Task SaveWordAsync(Word existingWord);
        Task<Word> GetWordByOriginalAsync(string original);
        Task<bool> ContainsByIdAsync(int wordId);
    }
}