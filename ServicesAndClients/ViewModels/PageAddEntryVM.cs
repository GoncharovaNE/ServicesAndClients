using ReactiveUI;
using ServicesAndClients.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAndClients.ViewModels
{
    internal class PageAddEntryVM:ViewModelBase
    {
        #region Переход по страницам и их заголовки

        bool _isAdmin = true;
        public bool IsAdmin { get => _isAdmin; set => _isAdmin = value; }

        string _headPage = "Запись на услугу";

        public string HeadPage { get => _headPage; set => this.RaiseAndSetIfChanged(ref _headPage, value); }        

        public void ToMainPage()
        {
            MainWindowViewModel.Instance.PageContent = new MainPage(IsAdmin);
        }

        #endregion

        ClientService? _entry;

        public ClientService? Entry { get => _entry; set => this.RaiseAndSetIfChanged(ref _entry, value); }

        public PageAddEntryVM(int id)
        {
            _entry = MainWindowViewModel.myConnection.ClientServices.FirstOrDefault(x => x.ServiceId == id);
        }

    }
}
