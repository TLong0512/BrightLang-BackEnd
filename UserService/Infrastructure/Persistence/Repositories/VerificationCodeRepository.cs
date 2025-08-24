using Application.Abstraction.Repositories;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public class VerificationCodeRepository : GenericRepository<VerificationCode, Guid>, IVerificationCodeRepository
{
    public VerificationCodeRepository(AppDbContext context) : base(context)
    {
    }
}
