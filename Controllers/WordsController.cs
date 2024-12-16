using Microsoft.AspNetCore.Mvc;
using dictionary_app.Interfaces;
using dictionary_app.Services;

namespace dictionary_app.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WordsController : ControllerBase
    {
        private readonly IDatabase _database;

        // Внедрение зависимости для работы с базой данных
        public WordsController(IDatabase database)
        {
            _database = database;
        }

        // Метод для получения всех слов
        [HttpGet]
      //  метод возвращает результат HTTP-ответа
        public async Task<ActionResult<IEnumerable<Word>>> GetWords()
        {
            var words = await _database.GetAllWordsAsync();
            if (words == null || !words.Any())
            {
                return NotFound("Слова не найдены.");
            }
            return Ok(words);
        }

        // Метод для получения слова по ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Word>> GetWord(int id)
        {
            var word = await _database.GetWordByIdAsync(id);
            if (word == null)
            {
                return NotFound($"Слово с ID {id} не найдено.");
            }
            return Ok(word);
        }
        
        // Метод для фильтрации слов по уровню
        [HttpGet("level/{level}")]
        public async Task<ActionResult<IEnumerable<Word>>> GetWordsByLevel(int level)
        {
            if (!IsValidLevel(level))
            {
                return BadRequest("Уровень должен быть в пределах от 1 до 5.");
            }

            var words = await _database.GetWordsByLevelAsync(level);
            if (!words.Any())
            {
                return NotFound($"Нет слов с уровнем {level}.");
            }

            return Ok(words);
        }

        // Метод для подсчета количества слов
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetWordCount()
        {
            var count = await _database.CountWordsAsync();
            return Ok(count);
        }

        // Метод для добавления нового слова
        [HttpPost]
        public async Task<ActionResult<Word>> AddWord(Word word)
        {
            if (string.IsNullOrWhiteSpace(word.Original) || string.IsNullOrWhiteSpace(word.Meaning))
            {
                return BadRequest("Оригинал и/или перевод не могут быть пустыми.");
            }

            if (!IsValidLevel(word.Level))
            {
                return BadRequest("Неверный уровень запоминания.");
            }

            bool wordExists = await _database.ContainsAsync(word.Original);
            if (wordExists)
            {
                return Conflict("Такое слово уже существует.");
            }
            
            wordExists = await _database.ContainsByIdAsync(word.Id);
            if (wordExists)
            {
                return Conflict("Cлово с таким Айди уже существует.");
            }

            await _database.AddWordAsync(word);
            return CreatedAtAction(nameof(GetWord), new { id = word.Id }, word);
        }

        // Метод для обновления слова
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWord(int id, Word word)
        {
            if (id != word.Id)
            {
                return BadRequest("ID слова не совпадает.");
            }

            var existingWord = await _database.GetWordByIdAsync(id);
            if (existingWord == null)
            {
                return NotFound($"Слово с ID {id} не найдено.");
            }

            existingWord.Original = word.Original;
            existingWord.Meaning = word.Meaning;
            existingWord.Level = word.Level;

            await _database.SaveWordAsync(existingWord);
            return NoContent();
        }

        // Метод для удаления слова
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWord(int id)
        {
            var wordToDelete = await _database.GetWordByIdAsync(id);
            if (wordToDelete == null)
            {
                return NotFound($"Слово с ID {id} не найдено.");
            }

            await _database.RemoveWordAsync(wordToDelete.Original);
            return NoContent();
        }
        
        // Метод для удаления слова
        [HttpDelete("original/{original}")]
        public async Task<IActionResult> DeleteWord(string original)
        {
            var wordToDelete = await _database.GetWordByOriginalAsync(original);
            if (wordToDelete == null)
            {
                return NotFound($"Cлово {original} не найдено.");
            }

            await _database.RemoveWordAsync(wordToDelete.Original);
            return NoContent();
        }
        
        
        private bool IsValidLevel(int level)
        {
            return level >= 1 && level <= 5; // Уровни от 1 до 5.
        }
    }
}
