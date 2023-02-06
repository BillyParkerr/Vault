using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;

namespace Application.Views;

public interface IHomeView
{
    // Properties
    FileInformation? SelectedFile { get; }

    // Events
    event EventHandler AddFileToVaultEvent;
    event EventHandler DownloadFileFromVaultEvent;

    // Methods
    void SetFilesInVaultListBindingSource(BindingSource filesInVaultList);
    void Show();
}