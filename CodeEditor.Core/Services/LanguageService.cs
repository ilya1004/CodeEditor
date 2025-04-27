using CodeEditor.Core.Abstractions.Data;
using CodeEditor.Core.Abstractions.Services;
using CodeEditor.Core.Entities;

namespace CodeEditor.Core.Services;

public class LanguageService(IUnitOfWork unitOfWork) : ILanguageService
{
    public async Task<List<Language>> GetAllLanguagesAsync()
    {
        return await unitOfWork.LanguagesRepository.ListAllAsync();
    }

    public async Task<Dictionary<string, string>> GetExtensionToLanguageMapAsync()
    {
        var languages = await GetAllLanguagesAsync();
        var map = new Dictionary<string, string>();

        foreach (var language in languages)
        {
            var extensions = language.Extensions.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(ext => ext.Trim());
            foreach (var ext in extensions)
            {
                map[ext] = language.Name;
            }
        }

        return map;
    }
}