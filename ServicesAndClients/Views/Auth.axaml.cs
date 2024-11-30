using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ServicesAndClients.ViewModels;

namespace ServicesAndClients;

public partial class Auth : UserControl
{
    public Auth()
    {
        InitializeComponent();
        DataContext = new MainPageVM();
    }
}