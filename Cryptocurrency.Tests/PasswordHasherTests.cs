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
        var password = "TestPassword123";
        var hash = _passwordHasher.HashPassword(password);

        Assert.NotNull(hash);
        Assert.NotEqual(password, hash);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnTrueForMatchingPassword()
    {
        var password = "TestPassword123";
        var hash = _passwordHasher.HashPassword(password);

        var isValid = _passwordHasher.VerifyPassword(password, hash);

        Assert.True(isValid);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnFalseForNonMatchingPassword()
    {
        var password = "TestPassword123";
        var hash = _passwordHasher.HashPassword(password);

        var isValid = _passwordHasher.VerifyPassword("WrongPassword", hash);

        Assert.False(isValid);
    }
}
