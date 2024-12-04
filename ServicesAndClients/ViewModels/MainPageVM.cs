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

        #region Сортировка,поиск и фильтрация

        int _selectedSort = 0;
        public int SelectedSort { get => _selectedSort; set { _selectedSort = value; filtersService(); } }

        bool _sortUp = true;
        public bool SortUp { get => _sortUp; set { this.RaiseAndSetIfChanged(ref _sortUp, value); if (value) SortDown = false; filtersService(); } }

        bool _sortDown = false;
        public bool SortDown { get => _sortDown; set { this.RaiseAndSetIfChanged(ref _sortDown, value); if (value) SortUp = false; filtersService(); } }

        public void filtersService()
        {
            if (SortUp) ServicesList = ServicesList.OrderBy(x => x.Cost).ToList();
            else if (SortDown) ServicesList = ServicesList.OrderByDescending(x => x.Cost).ToList();

            float dis1 = 0.05F;
            float dis2 = 0.15F;
            float dis3 = 0.3F;
            float dis4 = 0.7F;            

            switch (_selectedSort)
            {
                case 0:
                    ServicesList = ServicesList.OrderBy(x => x.Title).ToList();
                    break;
                case 1:
                    ServicesList = ServicesList.Where(x => x.Discount >= 0F && x.Discount < dis1).OrderBy(x => x.Discount).ThenBy(x => x.Title).ToList();
                    break;
                case 2:
                    ServicesList = ServicesList.Where(x => x.Discount >= dis1 && x.Discount < dis2).OrderBy(x => x.Discount).ThenBy(x => x.Title).ToList();
                    break;
                case 3:
                    ServicesList = ServicesList.Where(x => x.Discount >= dis2 && x.Discount < dis3).OrderBy(x => x.Discount).ThenBy(x => x.Title).ToList();
                    break;
                case 4:
                    ServicesList = ServicesList.Where(x => x.Discount >= dis3 && x.Discount < dis4).OrderBy(x => x.Discount).ThenBy(x => x.Title).ToList();
                    break;
                case 5:
                    ServicesList = ServicesList.Where(x => x.Discount >= dis4 && x.Discount < 1F).OrderBy(x => x.Discount).ThenBy(x => x.Title).ToList();
                    break;
            }
        }

        #endregion
    }
}
