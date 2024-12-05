using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ServicesAndClients.ViewModels;

namespace ServicesAndClients;

public partial class PageAddEditServ : UserControl
{
    public PageAddEditServ()
    {
        InitializeComponent();
        DataContext = new PageAddEditServVM();
    }

    public PageAddEditServ(int id)
    {
        InitializeComponent();
        DataContext = new PageAddEditServVM(id);
    }
}