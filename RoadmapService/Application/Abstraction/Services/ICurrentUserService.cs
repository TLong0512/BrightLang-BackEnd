namespace Application.Abstraction.Services;

public interface ICurrentUserService
{
    Guid? UserId { get; }
}
