using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using ServicesAndClients.Models;
using ServicesAndClients.Views;

namespace ServicesAndClients.ViewModels
{
    internal class PageAddEditServVM: ViewModelBase
    {
        string _headPage;

        public string HeadPage { get => _headPage; set => this.RaiseAndSetIfChanged(ref _headPage, value); }

        public PageAddEditServVM()
        {
            _headPage = "Добавление услуги";
        }
        public PageAddEditServVM(int id)
        {
            _headPage = "Редактирование услуги";
        }
        #region Переход по страницам

        public void ToMainPage()
        {
            MainWindowViewModel.Instance.PageContent = new MainPage();
        }

        #endregion

    }
}
