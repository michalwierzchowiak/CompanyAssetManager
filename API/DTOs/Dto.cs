namespace API.DTOs
{
    public record UserRegisterDto(string Username, string Password);
    public record UserLoginDto(string Username, string Password);
}
