﻿namespace Application.Views;

public interface ILoginView
{
    // Properties
    string GivenPassword { get; }

    // Event
    event EventHandler LoginEvent;

    // Methods
    void Show();
    void Close();
    void ShowBlankPasswordGivenError();
    void ShowIncorrectPasswordError();
}