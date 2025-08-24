using Application.Abstraction.Services;

namespace Infrastructure.Services;

public class FileEmailTemplateService : IEmailTemplateService
{
    public Task<string> RenderTemplateAsync(string content, Dictionary<string, string> replacements)
    {
        foreach (var kvp in replacements)
        {
            content = content.Replace($"{kvp.Key}", kvp.Value);
        }
        return Task.FromResult(content);
    }
}
