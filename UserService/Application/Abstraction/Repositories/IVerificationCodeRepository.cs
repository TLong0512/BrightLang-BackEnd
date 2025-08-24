using Domain.Entities;

namespace Application.Abstraction.Repositories;

public interface IVerificationCodeRepository : IGenericRepository<VerificationCode, Guid>
{
}
