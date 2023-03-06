public static class DirectoryPaths
{
    public static string ProgramDirectory { get; } = CreateDirectory("PersonalVaultApplication");

    public static string EncryptedFilesDirectory { get; } = CreateDirectory("PersonalVaultApplication", "EncryptedFiles");
    public static string EncryptedFilesCommonDirectory { get; } = CreateDirectory("PersonalVaultApplication", "EncryptedFiles", "Common");
    public static string EncryptedFilesCustomDirectory { get; } = CreateDirectory("PersonalVaultApplication", "EncryptedFiles", "Custom");

    public static string DecryptedFilesDirectory { get; } = CreateDirectory("PersonalVaultApplication", "DecryptedFiles");
    public static string DecryptedFilesCommonDirectory { get; } = CreateDirectory("PersonalVaultApplication", "DecryptedFiles", "Common");
    public static string DecryptedFilesTempDirectory { get; } = CreateDirectory("PersonalVaultApplication", "DecryptedFiles", "Temp");

    public static string ConfigDirectory { get; } = CreateDirectory("PersonalVaultApplication", "Config");

    private static string CreateDirectory(params string[] paths)
    {
        string directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Path.Combine(paths));
        Directory.CreateDirectory(directoryPath);
        return directoryPath;
    }
}