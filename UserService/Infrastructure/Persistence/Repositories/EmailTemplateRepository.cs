using Application.Abstraction.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class EmailTemplateRepository : GenericRepository<EmailTemplate, Guid>, IEmailTemplateRepository
{
    public EmailTemplateRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<EmailTemplate?> GetByNameAsync(string name)
    {
        return await _context.EmailTemplates.SingleOrDefaultAsync(et => et.Name == name);
    }
}
