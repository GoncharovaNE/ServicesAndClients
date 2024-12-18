using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
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
        #region Переход по страницам, их заголовки и сохранение режима администратора

        bool _isAdmin = true;
        public bool IsAdmin { get => _isAdmin; set => _isAdmin = value; }

        string _headPage = "Запись клиента на услугу";

        public string HeadPage { get => _headPage; set => this.RaiseAndSetIfChanged(ref _headPage, value); }        

        public void ToMainPage()
        {
            MainWindowViewModel.Instance.PageContent = new MainPage(IsAdmin);
        }

        #endregion

        #region Создание новой записи на услугу, свойства и метод для полей ввода

        ClientService? _entry;
        public ClientService? Entry { get => _entry; set => this.RaiseAndSetIfChanged(ref _entry, value); }

        Service? _serviceToEntry;
        public Service? ServiceToEntry { get => _serviceToEntry; set => this.RaiseAndSetIfChanged(ref _serviceToEntry, value); }      

        string _durationInMin;
        public string DurationInMin
        {
            get
            {
                _durationInMin = _serviceToEntry.DurationInSecond / 60 + " минут - продолжительность услуги";
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
        public TimeOnly? TimeStart { get => _timeStart; set { this.RaiseAndSetIfChanged(ref _timeStart, value); getTimeEnd(); } }

        TimeOnly? _timeEnd;
        public TimeOnly? TimeEnd { get => _timeEnd; set => this.RaiseAndSetIfChanged(ref _timeEnd, value); }

        public void getTimeEnd()
        {
            if (TimeStart.HasValue)
            {
                TimeEnd = TimeStart.Value.Add(TimeSpan.FromSeconds(ServiceToEntry.DurationInSecond));
            }
            else
            {
                TimeEnd = null;
            }
        }

        public PageAddEntryVM()
        {
            _entry = new ClientService() { };
        }

        public PageAddEntryVM(int id)
        {
            _serviceToEntry = MainWindowViewModel.myConnection.Services.FirstOrDefault(x => x.Id == id);
            _entry = new ClientService() { };
        }

        #endregion

        #region Метод добавления записи на услугу в базу данных
        public async void SaveAddEntry()
        {
            try
            {
                if (DateOnly.HasValue && TimeStart.HasValue && _selectedClient != null)
                {
                    Entry.StartTime = new DateTime(_dateOnly.Value.Year, _dateOnly.Value.Month, _dateOnly.Value.Day, 
                        _timeStart.Value.Hour, _timeStart.Value.Minute, _timeStart.Value.Second);
                    Entry.ClientId = SelectedClient.Id;
                    Entry.ServiceId = ServiceToEntry.Id;
                    MainWindowViewModel.myConnection.ClientServices.Add(Entry);
                    MainWindowViewModel.myConnection.SaveChanges();
                    string Messege = "Запись на услугу добавлена!";
                    ButtonResult result = await MessageBoxManager.GetMessageBoxStandard("Сообщение с уведомлением о добавлении!", Messege, ButtonEnum.Ok).ShowAsync();
                    MainWindowViewModel.Instance.PageContent = new MainPage(IsAdmin);
                }
                else
                {
                    if (_selectedClient == null)
                    {
                        string Messege = "Не выбран клиент для записи на услуги!";
                        ButtonResult result = await MessageBoxManager.GetMessageBoxStandard("Сообщение с уведомлением о незаполненных полях!", Messege, ButtonEnum.Ok).ShowAsync();
                    }
                    if (!DateOnly.HasValue)
                    {
                        string Messege = "Не выбрана дата оказания услуги!";
                        ButtonResult result = await MessageBoxManager.GetMessageBoxStandard("Сообщение с уведомлением о незаполненных полях!", Messege, ButtonEnum.Ok).ShowAsync();
                    }
                    if (!TimeStart.HasValue)
                    {
                        string Messege = "Не выбрано время начала оказания услуги!";
                        ButtonResult result = await MessageBoxManager.GetMessageBoxStandard("Сообщение с уведомлением о незаполненных полях!", Messege, ButtonEnum.Ok).ShowAsync();
                    }                    
                }
            }
            catch(Exception ex)
            {
                MessageBoxManager.GetMessageBoxStandard("Внимание", ex.Message + "\n" + ex.StackTrace, MsBox.Avalonia.Enums.ButtonEnum.Ok).ShowAsync();
            }
        }

        #endregion
    }
}
