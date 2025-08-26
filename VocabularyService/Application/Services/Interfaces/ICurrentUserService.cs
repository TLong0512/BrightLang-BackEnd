namespace Application.Services.Interfaces;

public interface ICurrentUserService
{
    Guid? UserId { get; }
}
