namespace Application.Abstraction.Services;

public interface IEmailTemplateService
{
    Task<string> RenderTemplateAsync(string content, Dictionary<string, string> replacements);
}
