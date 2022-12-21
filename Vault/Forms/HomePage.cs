using Application.Helpers;
using Application.Models;
using Application.UserInformation;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Application.Forms;

public partial class HomePage : Form
{
    private List<FileInformation> _displayedFiles = new List<FileInformation>();

    public HomePage()
    {
        InitializeComponent();
    }

    private void HomePage_Load(object sender, EventArgs e)
    {
        this.Shown += HomePage_Shown1;
    }

    private void HomePage_Shown1(object? sender, EventArgs e)
    {
        Login loginForm = new Login();
        if (loginForm.ShowDialog() == DialogResult.OK)
        {
            PopulateListOfFiles();
        }
    }

    private void HomepageList_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void PopulateListOfFiles()
    {
        HomepageList.Items.Clear();
        _displayedFiles.Clear();
        var encryptedFileNames = FilesHelper.GetEncryptedFilesWithDecryptedFileNames(LoginInfomation.Password);
        foreach (FileInformation file in encryptedFileNames.Item1)
        {
            ListViewItem item = new ListViewItem(file.FileName);
            item.SubItems.Add(file.FileExtension);
            item.SubItems.Add(FormatFileSize(file.FileSize));

            HomepageList.Items.Add(item);
            _displayedFiles.Add(file);
        }
    }

    private static string FormatFileSize(string fileLength)
    {
        if (string.IsNullOrEmpty(fileLength))
        {
            return fileLength;
        }

        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        double len = Convert.ToDouble(fileLength);
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len /= 1024;
        }

        // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
        // show a single decimal place, and no space.
        return $"{len:0.##} {sizes[order]}";
    }

    private void HomepageList_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
    {
        e.Cancel = true;
        e.NewWidth = HomepageList.Columns[e.ColumnIndex].Width;
    }

    private void label1_Click(object sender, EventArgs e)
    {

    }

    private void UploadButton_Click(object sender, EventArgs e)
    {
        OpenFileDialog openFileDialog1 = new OpenFileDialog();
        openFileDialog1.Title = "Select File";
        openFileDialog1.InitialDirectory = @"C:\"; //--"C:\\";
        openFileDialog1.FilterIndex = 2;
        openFileDialog1.Multiselect = false;
        openFileDialog1.CheckFileExists = true;
        openFileDialog1.ShowDialog();

        if (!string.IsNullOrWhiteSpace(openFileDialog1.FileName))
        {
            try
            {
                EncryptionHelper.EncryptFile(openFileDialog1.FileName, LoginInfomation.Password);
                PopulateListOfFiles(); // Refresh the homepage list.
            }
            catch
            {
                MessageBox.Show($"An Error Occurred While Attempting to Encrypt {openFileDialog1.SafeFileName}");
            }
        }
    }

    private void DownloadButton_Click(object sender, EventArgs e)
    {
        try
        {
            string decryptedFilePath = "";
            var decryptedFileName = HomepageList.FocusedItem.SubItems[0].Text;
            var file = _displayedFiles.First(_ => _.FileName == decryptedFileName);
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:\\Users";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                decryptedFilePath = EncryptionHelper.DecryptFile(file.EncryptedFilePath, LoginInfomation.Password, dialog.FileName);
            }

            if (!File.Exists(decryptedFilePath))
            {
                MessageBox.Show($"An Error Occured and we cannot find the decrypted file. Check it exists at {decryptedFilePath}");
                return;
            }

            string argument = "/select, \"" + decryptedFilePath + "\"";
            System.Diagnostics.Process.Start("explorer.exe", argument);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An Error Occured while trying to decrypt the file");
        }
    }
}