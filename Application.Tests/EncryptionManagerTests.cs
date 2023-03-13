using Application.Managers;
using Application.Models;
using Application.Presenters;
using Application.Views;
using Moq;

namespace Application.Tests;

public class EncryptionManagerTests
{
    private PasswordEncryptionManager _encryptionManager;
    private Mock<IDatabaseManager> _mockDatabaseManager;
    private Mock<IRegisterView> _mockRegisterView;

    [SetUp]
    public void Setup()
    {
        _mockDatabaseManager = new Mock<IDatabaseManager>();
        _encryptionManager = new PasswordEncryptionManager(_mockDatabaseManager.Object);
    }

    [Test]
    public void VerifyPassword_CallWithValidPassword_ReturnsTrue()
    {
        // Arrange
        EncryptionKey encryptionKey = new EncryptionKey
        {
            Key = "Tl5pREXA8kCdYzn5Xwc1bXaHmnm1RIbStj1e0YCLnKNtq7Wc1TM_9ddVc5k4jueaJZ3pCjPGwEN2dC_pP+1R64l_Hp_veI1U"
        };
        _mockDatabaseManager.SetupGet(_ => _.GetEncryptionKey()).Returns(encryptionKey);
        string password = "Password";

        // Act
        bool result = _encryptionManager.VerifyPassword(password);

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// This test acts as a workflow test and commits actual database to the database as well as the file system. Please use with caution!
    /// </summary>
    [Ignore("Should only be used to test a specific flow when real life results are needed!")]
    [Test]
    public void RegistrationFlow()
    {
        // Arrange
        var databaseManager = new DatabaseManager();
        var encryptionManager = new PasswordEncryptionManager(databaseManager);

        var mockRegisterView = new Mock<IRegisterView>();
        mockRegisterView.SetupGet(_ => _.GivenPassword).Returns("Password");
        mockRegisterView.SetupGet(_ => _.GivenSecondPassword).Returns("Password");

        var mockLoginView = new Mock<ILoginView>();
        mockLoginView.SetupGet(_ => _.GivenPassword).Returns("Password");

        var regViewPresenter = new RegistrationViewPresenter(encryptionManager, mockRegisterView.Object);
        var loginViewPresenter = new LoginViewPresenter(mockLoginView.Object, encryptionManager);

        // Act / Assert

        // This simulates the user registering their password for first time setup.
        regViewPresenter.RegisterEventHandler(mockRegisterView.Object, EventArgs.Empty);

        Assert.IsTrue(regViewPresenter.UserSuccessfullyRegistered);

        // This simulates the user logging in after closing the application and reopening it.
        loginViewPresenter.LoginEventHandler(mockLoginView.Object, EventArgs.Empty);
            
        Assert.IsTrue(loginViewPresenter.UserSuccessfullyAuthenticated);
    }
}