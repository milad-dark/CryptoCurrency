using Cryptocurrency.Application.Helper;

public class PasswordHasherTests
{
    private readonly PasswordHasher _passwordHasher;

    public PasswordHasherTests()
    {
        _passwordHasher = new PasswordHasher();
    }

    [Fact]
    public void HashPassword_ShouldReturnHashedPassword()
    {
        string password = "TestPassword123";
        string hash = _passwordHasher.HashPassword(password);

        Assert.NotNull(hash);
        Assert.NotEqual(password, hash);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnTrueForMatchingPassword()
    {
        string password = "TestPassword123";
        string hash = _passwordHasher.HashPassword(password);

        bool isValid = _passwordHasher.VerifyPassword(password, hash);

        Assert.True(isValid);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnFalseForNonMatchingPassword()
    {
        string password = "TestPassword123";
        string hash = _passwordHasher.HashPassword(password);

        bool isValid = _passwordHasher.VerifyPassword("WrongPassword", hash);

        Assert.False(isValid);
    }
}
