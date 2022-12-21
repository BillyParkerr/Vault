using Application.Helpers;

namespace Application.Tests.HelperTests;

internal class EncryptionHelperTests
{
    private static Random random = new();
    private string password = "";

    [SetUp]
    public void Setup()
    {
        password = RandomString(20);
    }

    private static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    [Test]
    public void EncryptFile()
    {

    }
}