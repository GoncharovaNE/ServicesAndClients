using Avalonia.Threading;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using ServicesAndClients.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace ServicesAndClients.ViewModels
{
    internal class PageUpcomingEntriesVM: ViewModelBase
    {
        #region Переход по страницам, их заголовки и сохранение режима администратора

        bool _isAdmin = true;
        public bool IsAdmin { get => _isAdmin; set => _isAdmin = value; }

        public void ToMainPage()
        {
            MainWindowViewModel.Instance.PageContent = new MainPage(IsAdmin);
        }

        #endregion

        #region Поле, свойство и заполнение листа со списком ближайших записей, а также создание таймера в 30 секунд

        List<ClientService> _entries;
        public List<ClientService> Entries { get =>_entries; set => this.RaiseAndSetIfChanged(ref _entries, value); }

        DispatcherTimer timer = new DispatcherTimer();

        bool _noResults;
        public bool NoResults { get => _noResults; set => this.RaiseAndSetIfChanged(ref _noResults, value); }
        public PageUpcomingEntriesVM()
        {
            DateTime startOfDay = DateTime.Today.Date;
            DateTime endOfDay = startOfDay.AddDays(1).Date;

            _secondsToNextUpdate = 30; 

            timer.Interval = new TimeSpan(0, 0, 1); 
            timer.Tick += TimerTick; 
            StartTimer();

            Entries = MainWindowViewModel.myConnection.ClientServices
                .AsNoTracking()
                .Include(x => x.Client)
                .Include(x => x.Service)
                .Where(x => x.StartTime.Date >= startOfDay && x.StartTime.Date <= endOfDay)
                .OrderBy(x => x.StartTime).ToList();

            NoResults = Entries.Count == 0;
        }

        #endregion

        #region Поле, свойство, методы для отслеживания и вывода таймера, а также обновление списка ближайших записей по его истечению
        private void TimerTick(object? sender, EventArgs e)
        {
            _secondsToNextUpdate--;

            if (_secondsToNextUpdate <= 0)
            {
                _secondsToNextUpdate = 30; 
                RefreshPage(); 
            }
            
            this.RaisePropertyChanged(nameof(CountdownToNextUpdate));
        }
        private void RefreshPage()
        {
            DateTime startOfDay = DateTime.Today.Date;
            DateTime endOfDay = startOfDay.AddDays(1).Date;
            Entries = MainWindowViewModel.myConnection.ClientServices
                 .AsNoTracking()
                 .Include(x => x.Client)
                 .Include(x => x.Service)
                 .Where(x => x.StartTime.Date >= startOfDay && x.StartTime.Date <= endOfDay)
                 .OrderBy(x => x.StartTime).ToList();

            NoResults = Entries.Count == 0;
        }

        public void StartTimer()
        {
            timer.Start();
        }

        private int _secondsToNextUpdate;
        public string CountdownToNextUpdate
        {
            get => $"Обновление через: {_secondsToNextUpdate} сек.";
        }       

        #endregion
    }
}
