using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using ServicesAndClients.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;

namespace ServicesAndClients.ViewModels
{
    internal class MainPageVM: ViewModelBase
    {
        #region Переход по страницам
        /// <summary>
        /// четыре метода для реализации переходов по страницам: добавление/редактирование услуги, 
        /// добавление записи на услугу, ближайщие записи на услуги
        /// </summary>
        public void ToPageAddServ()
        {
            MainWindowViewModel.Instance.PageContent = new PageAddEditServ();
        }

        public void ToPageEditServ(int id)
        {
            MainWindowViewModel.Instance.PageContent = new PageAddEditServ(id);
        }

        public void ToPageAddEntry(int id)
        {
            MainWindowViewModel.Instance.PageContent = new PageAddEntry(id);
        }
        
        public void ToPageUpcomingEntries()
        {
            MainWindowViewModel.Instance.PageContent = new PageUpcomingEntries();
        }

        #endregion

        #region Элементы для главной страницы

        /// <summary>
        /// реализованы поле и свойство для списка услуг
        /// </summary>
        List<Service> servicesList;

        public List<Service> ServicesList { get => servicesList; set => this.RaiseAndSetIfChanged(ref servicesList, value); }

        /// <summary>
        /// два конструктора для считывания данных об услугах в лист, 
        /// во втором реализовано свойство для запоминания состояния режима администратора
        /// </summary>
        public MainPageVM()
        {
            ServicesList = MainWindowViewModel.myConnection.Services.Include(x => x.ClientServices).ToList();
        }

        public MainPageVM(bool Admin)
        {
            ServicesList = MainWindowViewModel.myConnection.Services.Include(x => x.ClientServices).ToList();
            IsVisitableEditDelBut = Admin;
            IsVisitableAdmin = false;
        }

        /// <summary>
        /// поля и свойства для отображения функционала только в режиме администратора 
        /// </summary>
        bool isVisitableEditDelBut = false;
        
        bool isVisitableAdmin = true;

        string kodAdmin;

        public string KodAdmin { get => kodAdmin; set => this.RaiseAndSetIfChanged(ref kodAdmin, value); }
        public bool IsVisitableEditDelBut { get => isVisitableEditDelBut; set => this.RaiseAndSetIfChanged(ref isVisitableEditDelBut, value); }
        public bool IsVisitableAdmin { get => isVisitableAdmin; set => this.RaiseAndSetIfChanged(ref isVisitableAdmin, value); }
        /// <summary>
        /// метод для проверки коррекности введённого кода администратора
        /// </summary>
        public async void GetKodAdmin()
        {
            if (KodAdmin == null || KodAdmin != "0000")
            {
                IsVisitableAdmin = true;
                IsVisitableEditDelBut = false;
                string Messege = "Неверный код администратора! Доступ ограничен.";
                ButtonResult result = await MessageBoxManager.GetMessageBoxStandard("Сообщение с уведомлением о некорректных данных!", Messege, ButtonEnum.Ok).ShowAsync();
            }
            else if(KodAdmin == "0000")
            {
                IsVisitableAdmin = false;
                IsVisitableEditDelBut = true;
                string Messege = "Режим администратора активирован!";
                ButtonResult result = await MessageBoxManager.GetMessageBoxStandard("Сообщение с уведомлением!", Messege, ButtonEnum.Ok).ShowAsync();
            }
            KodAdmin = "";
        }
        /// <summary>
        /// метод для возврата в пользовательский режим
        /// </summary>
        public async void ClienеMode()
        {
            IsVisitableAdmin = true;
            IsVisitableEditDelBut = false;
            string Messege = "Режим пользователя активирован!";
            ButtonResult result = await MessageBoxManager.GetMessageBoxStandard("Сообщение с уведомлением!", Messege, ButtonEnum.Ok).ShowAsync();
            KodAdmin = "";
        }

        #endregion

        #region Сортировка, поиск и фильтрация
        /// <summary>
        /// поля и свойства для количества элементов в списке с услугами,
        /// поиска услуги по её названию и описанию,
        /// сортировки по стоимости,
        /// фильтрации по скидке
        /// и для отслеживания результата не найденых услуг
        /// </summary>
        int _countItemsList = MainWindowViewModel.myConnection.Services.ToList().Count;

        public int CountItemsList { get => _countItemsList; set => this.RaiseAndSetIfChanged(ref _countItemsList, value); }

        int _countItemsDB = MainWindowViewModel.myConnection.Services.ToList().Count;

        public int CountItemsDB { get => _countItemsDB; set => this.RaiseAndSetIfChanged(ref _countItemsDB, value); }

        string _search;
        public string Search { get => _search; set { _search = this.RaiseAndSetIfChanged(ref _search, value); filtersService(); } }

        int _selectedSort = 0;
        public int SelectedSort { get => _selectedSort; set { _selectedSort = value; filtersService(); } }

        int _selectedFilter = 0;
        public int SelectedFilter { get => _selectedFilter; set { _selectedFilter = value; filtersService(); } }

        private bool _noResults;
        public bool NoResults
        {
            get => _noResults;
            set => this.RaiseAndSetIfChanged(ref _noResults, value);
        }
        /// <summary>
        /// метод с для одновременного применения поиска, сортировки и фильтрации
        /// </summary>
        public void filtersService()
        {
            ServicesList = MainWindowViewModel.myConnection.Services.Include(x => x.ClientServices).ToList();

            if (!string.IsNullOrEmpty(_search))
            {
                ServicesList = ServicesList.Where(x => x.Title.ToLower().Contains(_search.ToLower()) ||
                x.Description.ToLower().Contains(_search.ToLower())).ToList();                
                CountItemsList = ServicesList.Count;
            }            

            float dis1 = 0.05F;
            float dis2 = 0.15F;
            float dis3 = 0.3F;
            float dis4 = 0.7F;

            switch (_selectedSort)
            {
                case 0:
                    ServicesList = ServicesList.ToList();
                    CountItemsList = ServicesList.Count;
                    break;
                case 1:
                    ServicesList = ServicesList.OrderBy(x => x.Cost).ToList();
                    CountItemsList = ServicesList.Count;
                    break;
                case 2:
                    ServicesList = ServicesList.OrderByDescending(x => x.Cost).ToList();
                    CountItemsList = ServicesList.Count;
                    break;                
            }

            switch (_selectedFilter)
            {
                case 0:
                    ServicesList = ServicesList.ToList();
                    CountItemsList = ServicesList.Count;
                    break;
                case 1:
                    ServicesList = ServicesList.Where(x => x.Discount < dis1).ToList();
                    CountItemsList = ServicesList.Count;
                    break;
                case 2:
                    ServicesList = ServicesList.Where(x => x.Discount >= dis1 && x.Discount < dis2).ToList();
                    CountItemsList = ServicesList.Count;
                    break;
                case 3:
                    ServicesList = ServicesList.Where(x => x.Discount >= dis2 && x.Discount < dis3).ToList();
                    CountItemsList = ServicesList.Count;
                    break;
                case 4:
                    ServicesList = ServicesList.Where(x => x.Discount >= dis3 && x.Discount < dis4).ToList();
                    CountItemsList = ServicesList.Count;
                    break;
                case 5:
                    ServicesList = ServicesList.Where(x => x.Discount >= dis4).ToList();
                    CountItemsList = ServicesList.Count;
                    break;
            }

            if (CountItemsList == 0) NoResults = true;
            else NoResults = false;
        }

        #endregion

        #region Удаление услуги
        /// <summary>
        /// метод удаления услуги по её индификатору
        /// </summary>
        /// <param name="id"></param>
        public async void DeleteService(int id)
        {            
            string Messege = "Вы уверенны, что хотите удалить данную услугу?";
            ButtonResult result = await MessageBoxManager.GetMessageBoxStandard("Сообщение с уведомлением об удалении!", Messege, ButtonEnum.YesNo).ShowAsync();
            Service deleteService = MainWindowViewModel.myConnection.Services.First(x => x.Id == id);

            switch (result)
            {
                case ButtonResult.Yes:
                    {
                        if (ServicesList.Any(x => x.ClientServices.Any(x => x.ServiceId == id)))
                        {
                            Messege = "Услуга не может быть удалёна!";
                            ButtonResult result1 = await MessageBoxManager.GetMessageBoxStandard("Сообщение с уведомлением об удалении!", Messege, ButtonEnum.Ok).ShowAsync();
                        }
                        else
                        {
                            MainWindowViewModel.myConnection.Services.Remove(deleteService);
                            MainWindowViewModel.myConnection.SaveChanges();
                            MainWindowViewModel.Instance.PageContent = new MainPage();
                            Messege = "Услуга удалёна!";
                            ButtonResult result2 = await MessageBoxManager.GetMessageBoxStandard("Сообщение с уведомлением об удалении!", Messege, ButtonEnum.Ok).ShowAsync();
                        }
                        break;
                    }
                case ButtonResult.No:
                    {
                        Messege = "Удаление отменено!";
                        ButtonResult result1 = await MessageBoxManager.GetMessageBoxStandard("Сообщение с уведомлением об удалении!", Messege, ButtonEnum.Ok).ShowAsync();
                        break;
                    }
            }
        }

        #endregion
    }
}
