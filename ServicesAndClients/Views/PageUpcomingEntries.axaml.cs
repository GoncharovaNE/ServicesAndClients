using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ServicesAndClients.ViewModels;

namespace ServicesAndClients;

public partial class PageUpcomingEntries : UserControl
{
    public PageUpcomingEntries()
    {
        InitializeComponent();
        DataContext = new PageUpcomingEntriesVM();
    }
}