using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ServicesAndClients.ViewModels;

namespace ServicesAndClients;

public partial class MainPage : UserControl
{
    public MainPage()
    {
        InitializeComponent();
        DataContext = new MainPageVM();
    }

    public MainPage(bool Admin)
    {
        InitializeComponent();
        DataContext = new MainPageVM(Admin);
    }
}