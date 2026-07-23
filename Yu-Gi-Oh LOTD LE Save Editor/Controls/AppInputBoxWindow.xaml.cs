using System.Windows;
using System.Windows.Input;

namespace YuGiOhSaveEditor.Controls;

/// <summary>
/// The actual dialog behind AppInputBox.Show - everything on screen
/// (TitleText, PromptText, InputBox, OkButton, CancelButton) is declared
/// once in AppInputBoxWindow.xaml; this class only ever sets text and reads
/// back InputBox.Text - no controls are built here.
/// </summary>
public partial class AppInputBoxWindow : Window
{
    public string? Result { get; private set; }

    public AppInputBoxWindow(string title, string prompt, string defaultValue)
    {
        InitializeComponent();
        TitleText.Text = title;
        PromptText.Text = prompt;
        InputBox.Text = defaultValue;
    }

    private void OkButton_Click(object sender, RoutedEventArgs e) => Accept();

    private void CancelButton_Click(object sender, RoutedEventArgs e) => Close();

    private void Accept()
    {
        Result = InputBox.Text;
        Close();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        InputBox.Focus();
        InputBox.SelectAll();
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape) Close();
        else if (e.Key == Key.Enter) Accept();
    }
}
