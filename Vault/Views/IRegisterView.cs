using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Views;

public interface IRegisterView
{
    // Properties
    string GivenPassword { get; }
    string GivenSecondPassword { get; }

    // Events
    event EventHandler RegisterEvent;

    // Methods
    public void ShowBlankPasswordError();
    public void ShowPasswordMismatchError();
    public void ShowPasswordTooShortError();

    void Show();
    void Close();
}