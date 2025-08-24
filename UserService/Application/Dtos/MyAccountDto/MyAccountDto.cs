namespace Application.Dtos.MyAccountDto;

public class MyAccountDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    //public string Password { get; set; } = string.Empty;
    public string[] Roles { get; set; } = [];
}
