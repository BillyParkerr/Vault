using Application.Models;

namespace Application.Managers;

public interface IFileManager
{
    /// <summary>
    /// Gets a list of all the files currently in the vault that do not use a unique password.
    /// </summary>
    /// <returns>List of all files in the Vault.</returns>
    List<EncryptedFile> GetAllFilesInVault();

    /// <summary>
    /// Adds a file to the vault by encrypting the file then adding the information to the database.
    /// Uses the default password unless a password is given.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns>True if success, False if failure.</returns>
    bool AddFileToVault(string filePath);

    /// <summary>
    /// Adds a fodler to the vault by zipping then encrypting the folder then adding the information to the database.
    /// </summary>
    /// <param name="folderPath"></param>
    /// <returns>True if success, False if failure.</returns>
    bool ZipFolderAndAddToVault(string folderPath);

    /// <summary>
    /// Decrypts and downloads an encrypted file from the vault to a given destination.
    /// </summary>
    /// <param name="encryptedFilePath"></param>
    /// <param name="destinationFilePath"></param>
    /// <param name="password"></param>
    /// <returns>True upon success, False upon failure</returns>
    bool DownloadFileFromVault(string encryptedFilePath, string destinationFilePath, string password = null);

    /// <summary>
    /// Deletes the file from the system and removes the EncryptedFile from the database.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns>True upon success, False upon failure</returns>
    bool DeleteFileFromVault(string filePath);

    /// <summary>
    /// Decrypts a file within the vault to a temporary location and initilises the file monitering process.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns>True upon success, False upon failure</returns>
    bool OpenFileFromVault(string filePath);

    /// <summary>
    /// Downloads a encrypted file to a specified destination.
    /// The file is first decrypted then reencrypted with the given password.
    /// The decrypted copy is then deleted.
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="destinationFilePath"></param>
    /// <param name="newEncryptionPassword"></param>
    /// <returns>True upon success or False upon failure</returns>
    bool DownloadEncryptedFileFromVault(string filePath, string destinationFilePath, string newEncryptionPassword);

    /// <summary>
    /// Imports an existing EncryptedFile. This follows the following process:
    /// 
    /// 1 - Decrpyt the encrypted file to a temp location.
    /// 2 - Encrypted the decrypted file from the temp location.
    /// 3 - Add the encrypted file to the database.
    /// 4 - Delete the temp decrypted file.
    /// </summary>
    /// <param name="filePath">File path for the encrypted file.</param>
    /// <param name="encryptionPassword">Password which the given file was encrypted with.</param>
    /// <returns>True upon success or False upon failure</returns>
    bool ImportEncryptedFileToVault(string filePath, string encryptionPassword);

    /// <summary>
    /// Creates a protected file containing the given password. If a protected file already exists this is deleted.
    /// </summary>
    /// <param name="password"></param>
    void ProtectAndSavePassword(string password);

    /// <summary>
    /// Decrypts and reads the protected file.
    /// </summary>
    /// <returns>Password from the protected file.</returns>
    string ReadAndReturnProtectedPassword();

    /// <summary>
    /// Checks if an protected password file exists.
    /// </summary>
    /// <returns>True if exists, False if not</returns>
    bool ProtectedPasswordExists();

    /// <summary>
    /// Deletes any Files that were created for temporary purposes suchas opening a file within the vault.
    /// </summary>
    void CleanupTempFiles();

    /// <summary>
    /// Displays a file picker dialog to the user and returns the selected file path.
    /// This method ensures that the file picker dialog is run on an STA thread, as required.
    /// </summary>
    /// <param name="filter">Optional file filter string to show only specific file types in the dialog.</param>
    /// <returns>The file path of the selected file, or null if no file was selected.</returns>
    string GetFilePathFromExplorer(string filter = null);

    /// <summary>
    /// Displays a file picker dialog to the user and returns the selected file paths.
    /// This method ensures that the file picker dialog is run on an STA thread, as required.
    /// </summary>
    /// <param name="filter">Optional file filter string to show only specific file types in the dialog.</param>
    /// <returns>A list of paths of all selected files</returns>
    List<string> GetFilePathsFromExplorer(string filter = null);

    /// <summary>
    /// Displays a folder picker dialog to the user and returns the selected folder path.
    /// This method ensures that the folder picker dialog is run on an STA thread, as required.
    /// </summary>
    /// <returns>The folder path of the selected folder, or null if no folder was selected.</returns>
    string GetFolderPathFromExplorer(string description = null);

    /// <summary>
    /// Opens Windows Explorer and selects the specified file or folder.
    /// </summary>
    /// <param name="path">The path of the file or folder to be selected in Windows Explorer.</param>
    void OpenFolderInExplorer(string path);
}