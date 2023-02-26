using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Views;

public interface ILoginView
{
    // Properties
    string GivenPassword { get; }

    // Event
    event EventHandler LoginEvent;

    // Methods
    void Show();
    void Close();
}