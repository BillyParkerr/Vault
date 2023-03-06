using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Views;

public interface IImportEncryptedFileView
{
    // Properties
    string GivenPassword { get; }

    // Event
    event EventHandler ConfirmEvent;

    // Methods
    void Show();
    void Close();
    void ShowBlankPasswordError();
}