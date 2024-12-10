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

        string _headPage = "Запись клиента на услугу";

        public string HeadPage { get => _headPage; set => this.RaiseAndSetIfChanged(ref _headPage, value); }        

        public void ToMainPage()
        {
            MainWindowViewModel.Instance.PageContent = new MainPage(IsAdmin);
        }

        #endregion

        ClientService? _entry;
        public ClientService? Entry { get => _entry; set => this.RaiseAndSetIfChanged(ref _entry, value); }

        Service? _serviceToEntry;
        public Service? ServiceToEntry { get => _serviceToEntry; set => this.RaiseAndSetIfChanged(ref _serviceToEntry, value); }      

        string _durationInMin;
        public string DurationInMin
        {
            get
            {
                _durationInMin = _serviceToEntry.DurationInSecond / 60 + " минут";
                return _durationInMin;
            }
            set => this.RaiseAndSetIfChanged(ref _durationInMin, value);
        }

        List<Client> _clientList = [new Client() { Id = 0, FirstName = "Выберите клиента" }, .. MainWindowViewModel.myConnection.Clients.ToList()];

        public List<Client> ClientList { get => _clientList; set => this.RaiseAndSetIfChanged(ref _clientList, value); }        

        Client? _selectedClient = null;
        public Client? SelectedClient 
        { 
            get
            {
                if (_selectedClient == null) return _clientList[0];
                else return _selectedClient;
            }
            set => this.RaiseAndSetIfChanged(ref _selectedClient, value);
        }        

        DateOnly? _dateOnly;
        public DateOnly? DateOnly { get => _dateOnly; set => this.RaiseAndSetIfChanged(ref _dateOnly, value); }

        TimeOnly? _timeStart;
        public TimeOnly? TimeStart { get => _timeStart; set => this.RaiseAndSetIfChanged(ref _timeStart, value); }

        TimeOnly? _timeEnd;
        public TimeOnly? TimeEnd { get => _timeEnd; set => this.RaiseAndSetIfChanged(ref _timeEnd, value); }

        public PageAddEntryVM()
        {
            _entry = new ClientService() { };
        }

        public PageAddEntryVM(int id)
        {
            _serviceToEntry = MainWindowViewModel.myConnection.Services.FirstOrDefault(x => x.Id == id);
            _entry = new ClientService() { };
        }

    }
}
