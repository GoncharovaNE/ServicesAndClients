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
        #region Переход по страницам и их заголовки

        bool _isAdmin = true;
        public bool IsAdmin { get => _isAdmin; set => _isAdmin = value; }

        public void ToMainPage()
        {
            MainWindowViewModel.Instance.PageContent = new MainPage(IsAdmin);
        }

        #endregion

        List<ClientService> _entries;
        public List<ClientService> Entries { get =>_entries; set => this.RaiseAndSetIfChanged(ref _entries, value); }

        DispatcherTimer timer = new DispatcherTimer();

        public PageUpcomingEntriesVM()
        {
            DateTime startOfDay = DateTime.Today.Date;
            DateTime endOfDay = startOfDay.AddDays(1).Date;

            _secondsToNextUpdate = 30; // Инициализация обратного отсчёта на 30 секунд

            timer.Interval = new TimeSpan(0, 0, 1); // Таймер срабатывает каждую секунду
            timer.Tick += TimerTick; // Обработчик для каждого тика
            StartTimer();

            Entries = MainWindowViewModel.myConnection.ClientServices
                .AsNoTracking()
                .Include(x => x.Client)
                .Include(x => x.Service)
                .Where(x => x.StartTime.Date >= startOfDay && x.StartTime.Date <= endOfDay)
                .OrderBy(x => x.StartTime).ToList();
        }   

        private void TimerTick(object? sender, EventArgs e)
        {
            _secondsToNextUpdate--;

            if (_secondsToNextUpdate <= 0)
            {
                _secondsToNextUpdate = 30; // Сброс обратного отсчёта
                RefreshPage(); // Обновление страницы
            }

            // Уведомляем интерфейс об изменении свойства
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
    }
}
