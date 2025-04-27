using CodeEditor.Core.Entities;

namespace CodeEditor.Core.Abstractions.Services;

public interface ILanguageService
{
    Task<List<Language>> GetAllLanguagesAsync();
    Task<Dictionary<string, string>> GetExtensionToLanguageMapAsync();
}