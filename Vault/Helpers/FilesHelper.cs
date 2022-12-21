using Application.Models;

namespace Application.Helpers;

public static class FilesHelper
{
    private static string EncryptedFilesPath = @"C:\Users\Billy\AppData\Roaming\PersonalVaultApplication\EncryptedFiles\Common"; // TODO Move this into a base class

    /// <summary>
    /// Gets the file names for all encrypted files in the encrypted files folder.
    /// </summary>
    /// <returns>Decrypted File Names, Path of files that failed decryption</returns>
    public static (List<FileInformation>, string[]) GetEncryptedFilesWithDecryptedFileNames(string password)
    {
        List<FileInformation> filesInPath = new List<FileInformation>();
        List<string> failedFilesInPath = new List<string>();

        var filePathsInDirectory = Directory.GetFiles(EncryptedFilesPath);
        foreach (var filePath in filePathsInDirectory)
        {
            FileInformation fileInfo = new FileInformation();
            // TODO Change this into async tasks
            // Remove the path from the filename.
            try
            {
                var encryptedFileName = Path.GetFileNameWithoutExtension(filePath);
                var decryptedFileName = EncryptionHelper.DecryptFileName(encryptedFileName, password);
                fileInfo.FileName = Path.GetFileNameWithoutExtension(decryptedFileName);
                fileInfo.FileExtension = Path.GetExtension(decryptedFileName);
                fileInfo.FileSize = new FileInfo(filePath).Length.ToString();
                fileInfo.EncryptedFilePath = filePath;
                filesInPath.Add(fileInfo);
            }
            catch (Exception ex)
            {
                // TODO Add logging here
                failedFilesInPath.Add(filePath);
            }
        }

        return (filesInPath, failedFilesInPath.ToArray());
    }
}