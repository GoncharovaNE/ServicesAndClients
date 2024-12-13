using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using ServicesAndClients.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public PageUpcomingEntriesVM()
        {
            Entries = MainWindowViewModel.myConnection.ClientServices.Include(x => x.Client).Include(x => x.Service)
                .Where(x => x.StartTime == DateTime.Today /*|| x.StartTime == x.StartTime.AddDays(1)*/)
                .OrderBy(x => x.StartTime).ToList();
        }

    }
}
