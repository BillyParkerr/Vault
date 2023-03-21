using Application.Models;
using Application.Views.Interfaces;

namespace Application.Views.Forms;

public partial class HomeView : Form, IHomeView
{
    public event EventHandler AddFileToVaultEvent;
    public event EventHandler AddFolderToVaultEvent;
    public event EventHandler DownloadFileFromVaultEvent;
    public event EventHandler DeleteFileFromVaultEvent;
    public event EventHandler OpenFileFromVaultEvent;
    public event EventHandler ExportFileFromVaultEvent;
    public event EventHandler ImportFileToVaultEvent;
    public event EventHandler SearchFilterAppliedEvent;
    public event FormClosingEventHandler FormClosingEvent;

    public HomeView()
    {
        InitializeComponent();
        AssociateAndRaiseViewEvents();
    }

    public FileInformation SelectedFile
    {
        get { return GetSelectedRow(); }
    }

    public string SearchValue
    {
        get { return SearchBox.Text; }
    }

    private void AssociateAndRaiseViewEvents()
    {
        UploadButton.Click += delegate { AddFileToVaultEvent?.Invoke(this, EventArgs.Empty); };
        UploadFolderButton.Click += delegate { AddFolderToVaultEvent?.Invoke(this, EventArgs.Empty); };
        DownloadButton.Click += delegate { DownloadFileFromVaultEvent?.Invoke(this, EventArgs.Empty); };
        DeleteButton.Click += delegate { DeleteFileFromVaultEvent?.Invoke(this, EventArgs.Empty); };
        OpenFileButton.Click += delegate { OpenFileFromVaultEvent?.Invoke(this, EventArgs.Empty); };
        ImportButton.Click += delegate { ImportFileToVaultEvent?.Invoke(this, EventArgs.Empty); };
        ExportButton.Click += delegate { ExportFileFromVaultEvent?.Invoke(this, EventArgs.Empty); };
        SearchBox.TextChanged += delegate { SearchFilterAppliedEvent?.Invoke(this, EventArgs.Empty); };
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        FormClosingEvent?.Invoke(this, e);

        base.OnFormClosing(e);
    }

    private FileInformation? GetSelectedRow()
    {
        if (dataGridView.SelectedRows.Count != 0)
        {
            return (FileInformation?)dataGridView.SelectedRows[0].DataBoundItem;
        }
        else
        {
            return null;
        }
    }

    public void SetFilesInVaultListBindingSource(BindingSource filesInVaultList)
    {
        if (dataGridView.InvokeRequired)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                dataGridView.DataSource = filesInVaultList;
            }));
        }
        else
        {
            dataGridView.DataSource = filesInVaultList;
        }
    }

    public void ShowFailedToDeleteError()
    {
        MessageBox.Show("Failed to delete file from the Vault! Please try again.");
    }
}