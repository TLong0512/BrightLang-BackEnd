using Domain.Entities;

namespace Application.Abstraction.Repositories;

public interface IEmailTemplateRepository : IGenericRepository<EmailTemplate, Guid>
{
    Task<EmailTemplate?> GetByNameAsync(string name);
}
