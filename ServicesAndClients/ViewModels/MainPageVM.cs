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
        
        bool isVisitableAdmin = true;

        string kodAdmin;

        public string KodAdmin { get => kodAdmin; set => this.RaiseAndSetIfChanged(ref kodAdmin, value); }
        public bool IsVisitableEditDelBut { get => isVisitableEditDelBut; set => this.RaiseAndSetIfChanged(ref isVisitableEditDelBut, value); }
        public bool IsVisitableAdmin { get => isVisitableAdmin; set => this.RaiseAndSetIfChanged(ref isVisitableAdmin, value); }

        public void GetKodAdmin()
        {
            if (KodAdmin == null || KodAdmin != "0000")
            {
                IsVisitableAdmin = true;
                IsVisitableEditDelBut = false;
            }
            else if(KodAdmin == "0000")
            {
                IsVisitableAdmin = false;
                IsVisitableEditDelBut = true;
            }
        }
    }
}
