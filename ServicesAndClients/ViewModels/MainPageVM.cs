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
    internal class MainPageVM: ViewModelBase
    {
        List<Service> servicesList;

        public List<Service> ServicesList { get => servicesList; set => this.RaiseAndSetIfChanged(ref servicesList, value); }        

        public MainPageVM() 
        {
            ServicesList = MainWindowViewModel.myConnection.Services.ToList();
        }

        bool isVisitableEditDelBut = false;

        string kodAdmin;

        public string KodAdmin { get => kodAdmin; set => this.RaiseAndSetIfChanged(ref kodAdmin, value); }
        public bool IsVisitableEditDelBut { get => isVisitableEditDelBut; set => this.RaiseAndSetIfChanged(ref isVisitableEditDelBut, value); }

        public void GetKodAdmin()
        {
            if (kodAdmin == null || kodAdmin != "0000")
            {
                MainWindowViewModel.Instance.PageContent = new MainPage();
            }
            else if(kodAdmin == "0000")
            {
                MainWindowViewModel.Instance.PageContent = new MainPage();
            }
        }
    }
}
