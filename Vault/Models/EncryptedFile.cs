namespace Application.Models;

public class EncryptedFile
{
    public int Id { get; set; }
    public string FilePath { get; set; }
    public bool UniquePassword { get; set; }
    public FileInformation? DecryptedFileInformation { get; set; } // Note that this property is not stored in the database.
}

public class FileInformation
{
    public string? FileName { get; set; }
    public string? FileExtension { get; set; }
    public string? FileSize { get; set; }
}