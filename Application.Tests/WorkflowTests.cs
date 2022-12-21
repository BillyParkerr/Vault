using NPOI.HPSF;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Forms;
using Application.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;

namespace Application.Tests
{
    internal class WorkflowTests
    {
        private static Random random = new();
        private static string EncryptedFilesPath = @"C:\Encryption\Encrypted\"; // TODO Move this into a base class
        private static string Password = "PASSWORD"; // TODO Move this into a base class

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static string CreateTextFile(string fileName)
        {
            string testPath = @"C:\Test\WorkflowTestFiles\";
            Directory.CreateDirectory(testPath);

            string filePath = Path.Combine(testPath, fileName);
            filePath += ".txt";
            // Check if file already exists. If yes, delete it.     
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            // Create a new file     
            using FileStream fs = File.Create(filePath);

            // Add some text to file    
            Byte[] title = new UTF8Encoding(true).GetBytes(RandomString(1000000));
            fs.Write(title, 0, title.Length);

            return filePath;
        }

        private static int GenerateRandomInt(int maxSize)
        {
            Random rnd = new Random();
            return rnd.Next(maxSize);
        }

        [Test]
        public void EncryptFileThenDecryptFileFlow() // TODO Improve name
        {
            // Arrange
            string textFileName = $"Test File {GenerateRandomInt(100000000)}";
            string textFilePath = CreateTextFile(textFileName);
            
            // Act
            // Encrypt the file
           var encryptedFilePath = EncryptionHelper.EncryptFile(textFilePath, Password);

           Assert.That(File.Exists(encryptedFilePath));

            // Decrypt the file
            var decryptedFilePath = EncryptionHelper.DecryptFile(encryptedFilePath, Password);

            // Assert
            string originalTextFileContents = File.ReadAllText(textFilePath);
            string decryptedTextFileCOntents = File.ReadAllText(decryptedFilePath);

            // Check that the decrypted contents are the same.
            Assert.AreEqual(originalTextFileContents, decryptedTextFileCOntents);
        }

        [Test]
        public void SetupTestData()
        {
            int n = 50; // number of times to loop

            for (int i = 0; i < n; i++)
            {
                string textFileName = $"Test File {GenerateRandomInt(100000000)}";
                string textFilePath = CreateTextFile(textFileName);
                var encryptedFilePath = EncryptionHelper.EncryptFile(textFilePath, Password);
            }
        }
    }
}
