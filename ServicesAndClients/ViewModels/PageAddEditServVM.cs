using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using ReactiveUI;
using ServicesAndClients.Models;
using ServicesAndClients.Views;
using Avalonia;
using System.Collections.ObjectModel;
using System.IO;
using Avalonia.Controls;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia.Base;

namespace ServicesAndClients.ViewModels
{
    internal class PageAddEditServVM: ViewModelBase
    {
        #region Переход по страницам, их заголовки и сохранение режима администратора

        bool _isAdmin = true;
        public bool IsAdmin { get => _isAdmin; set => _isAdmin = value; }

        string _headPage;

        public string HeadPage { get => _headPage; set => this.RaiseAndSetIfChanged(ref _headPage, value); }

        public void ToMainPage()
        {
            MainWindowViewModel.Instance.PageContent = new MainPage(IsAdmin);
        }

        #endregion

        #region Создание новой услуги и свойства для полей ввода

        Service? _newService;
        public Service? NewService { get => _newService; set => this.RaiseAndSetIfChanged(ref _newService, value); }

        string _nameBT;
        public string NameBT { get => _nameBT; set => this.RaiseAndSetIfChanged(ref _nameBT, value); }

        bool _isVisibleBT = true;
        public bool IsVisibleBT { get => _isVisibleBT; set => this.RaiseAndSetIfChanged(ref _isVisibleBT, value); }

        bool _isEnableBT = true;
        public bool IsEnableBT { get => _isEnableBT; set => this.RaiseAndSetIfChanged(ref _isEnableBT, value); }

        List<Service>? _servicesList;
        public List<Service>? ServiceList { get => _servicesList; set => this.RaiseAndSetIfChanged(ref _servicesList, value); }

        string _cost;
        public string Cost { get => _cost; set => this.RaiseAndSetIfChanged(ref _cost, value); }

        string _durationInMin;
        public string DurationInMin { get => _durationInMin; set => this.RaiseAndSetIfChanged(ref _durationInMin, value); }

        string _discount;
        public string Discount { get => _discount; set => this.RaiseAndSetIfChanged(ref _discount, value); }

        public PageAddEditServVM()
        {
            _headPage = "Добавление услуги";
            _nameBT = "Добавить услугу";
            IsVisibleBT = false;
            IsEnableBT = false;

            _newService = new Service() ;           

            ServiceList = MainWindowViewModel.myConnection.Services.ToList();
        }
        public PageAddEditServVM(int id)
        {
            _headPage = "Редактирование услуги";
            _nameBT = "Сохранить изменения";
            IsVisibleBT = true;
            IsEnableBT = false;

            _newService = MainWindowViewModel.myConnection.Services.FirstOrDefault(x => x.Id == id);

            Cost = _newService.Cost.ToString();
            Discount = (_newService.Discount*100).ToString();
            DurationInMin = (_newService.DurationInSecond/60).ToString();

            ServiceList = MainWindowViewModel.myConnection.Services.ToList();
        }

        #endregion

        #region Метод добавления и сохранения изменений услуги в базе данных

        bool flagCost = false;
        bool flagDis = false;
        bool flagDur = false;

        public async void SaveAddEditServ()
        {
            try
            {
                if (NewService.Title == null || Cost == null || DurationInMin == null)
                {
                    string Messege = "Все поля, кроме описания и скидки, должны быть заполнены!";
                    ButtonResult result1 = await MessageBoxManager.GetMessageBoxStandard("Сообщение с уведомлением о некорректных данных!", Messege, ButtonEnum.Ok).ShowAsync();
                }
                else
                {                    
                    if (!string.IsNullOrEmpty(_cost) && int.TryParse(_cost, out int res1) && res1 > 0)
                    { 
                        NewService.Cost = res1;
                        flagCost = true;
                    }
                    else
                    {
                        flagCost = false;
                        string Messege = "Некорректное значение у стоимости услуги!";
                        ButtonResult result = await MessageBoxManager.GetMessageBoxStandard("Сообщение с уведомлением о некорректных данных!", Messege, ButtonEnum.Ok).ShowAsync();
                    }

                    if (!string.IsNullOrEmpty(_discount) && int.TryParse(_discount, out int res2) && res2 <= 100 && res2 >= 0)
                    {
                        float dis = res2/100f;
                        NewService.Discount = dis;
                        flagDis = true;
                    }
                    else if(string.IsNullOrEmpty(_discount))
                    {
                        NewService.Discount = 0;
                        flagDis = true;
                    }
                    else
                    {
                        flagDis = false;
                        string Messege = "Некорректное значение у скидки услуги!";
                        ButtonResult result = await MessageBoxManager.GetMessageBoxStandard("Сообщение с уведомлением о некорректных данных!", Messege, ButtonEnum.Ok).ShowAsync();
                    }

                    if (!string.IsNullOrEmpty(_durationInMin) && int.TryParse(_durationInMin, out int res3) && res3 <= 240 && res3 > 0)
                    {
                        res3 = res3 * 60;
                        NewService.DurationInSecond = res3;
                        flagDur = true;
                    }
                    else
                    {
                        flagDur = false;
                        string Messege = "Некорректное значение у длительности услуги!";
                        ButtonResult result = await MessageBoxManager.GetMessageBoxStandard("Сообщение с уведомлением о некорректных данных!", Messege, ButtonEnum.Ok).ShowAsync();
                    }
                    if (flagCost == true && flagDis == true && flagDur == true)
                    {
                        if (NewService.Id == 0)
                        {
                            MainWindowViewModel.myConnection.Services.Add(NewService);
                        }
                        if (ServiceList.Any(x => x.Title == NewService.Title) && NewService.Id == 0)
                        {
                            string Messege = "Услуга с таким названием уже еcть в базе данных!";
                            ButtonResult result1 = await MessageBoxManager.GetMessageBoxStandard("Сообщение с уведомлением о некорректных данных!", Messege, ButtonEnum.Ok).ShowAsync();
                        }
                        else
                        {
                            MainWindowViewModel.myConnection.SaveChanges();
                            MainWindowViewModel.Instance.PageContent = new MainPage(IsAdmin);
                            if (NameBT == "Добавить услугу")
                            {
                                string Messege = "Услуга добавлена!";
                                ButtonResult result = await MessageBoxManager.GetMessageBoxStandard("Сообщение с уведомлением о добавлении!", Messege, ButtonEnum.Ok).ShowAsync();
                            }
                            else if (NameBT == "Сохранить изменения")
                            {
                                string Messege = "Изменения об услуге сохранены!";
                                ButtonResult result = await MessageBoxManager.GetMessageBoxStandard("Сообщение с уведомлением об изменениях!", Messege, ButtonEnum.Ok).ShowAsync();
                            }
                        }   
                    }
                    else
                    {
                        string Messege = "Ошибка записи в базу данных! Возможно присутствуют некорректные значения.";
                        ButtonResult result = await MessageBoxManager.GetMessageBoxStandard("Сообщение с уведомлением об ошибке!", Messege, ButtonEnum.Ok).ShowAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxManager.GetMessageBoxStandard("Внимание", ex.Message + "\n" + ex.StackTrace, MsBox.Avalonia.Enums.ButtonEnum.Ok).ShowAsync();
            }
        }

        #endregion
    }
}
