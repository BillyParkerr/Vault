using Application.Models;
using System.Windows.Forms;

namespace Application.Views;

public partial class HomeView : Form, IHomeView
{
    public event EventHandler AddFileToVaultEvent;
    public event EventHandler DownloadFileFromVaultEvent;

    public HomeView()
    {
        InitializeComponent();
        AssociateAndRaiseViewEvents();
    }

    public FileInformation? SelectedFile
    {
        get { return GetSelectedRow(); }
    }

    private void AssociateAndRaiseViewEvents()
    {
        UploadButton.Click += delegate { AddFileToVaultEvent?.Invoke(this, EventArgs.Empty); };
        //DownloadButton.Click += (sender, e) => AddFileToVaultEvent.Invoke(this, EventArgs.Empty);
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
        dataGridView.DataSource = filesInVaultList;
    }
}