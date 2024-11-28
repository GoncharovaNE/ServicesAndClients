using Avalonia.Controls;
using ReactiveUI;
using ServicesAndClients.Models;

namespace ServicesAndClients.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public static MainWindowViewModel Instance;

        public static _43pSchool4Context myConnection = new _43pSchool4Context();

        public MainWindowViewModel() 
        {
            Instance = this;
        }

        UserControl pageContent = new MainPage();

        public UserControl PageContent { get => pageContent; set => this.RaiseAndSetIfChanged(ref pageContent, value); }
    }
}
