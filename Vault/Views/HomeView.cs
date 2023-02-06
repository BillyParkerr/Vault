using Application.Models;

namespace Application.Views;

public partial class HomeView : Form, IHomeView
{
    public FileInformation? SelectedFile
    {
        get { return GetSelectedRow(); }
    }

    public HomeView()
    {
        InitializeComponent();
        AssociateAndRaiseViewEvents();
    }

    private void AssociateAndRaiseViewEvents()
    {
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

    public event EventHandler AddFileToVaultEvent;
    public event EventHandler DownloadFileFromVaultEvent;

    public void SetFilesInVaultListBindingSource(BindingSource filesInVaultList)
    {
        dataGridView.DataSource = filesInVaultList;
    }
}