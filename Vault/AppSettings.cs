using Application.Enums;

namespace Application;

public class AppSettings
{
    public AuthenticationMethod AuthenticationMethod { get; set; }
    public ApplicationMode Mode { get; set; }
    public bool DeleteUnencryptedFileUponUpload { get; set; }
    public string DefaultDownloadLocation { get; set; }
}