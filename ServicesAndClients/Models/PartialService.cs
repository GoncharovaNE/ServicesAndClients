using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesAndClients.ViewModels;

namespace ServicesAndClients.Models
{
    public partial class Service
    {
        /*public bool IsVisCostWithDis
        {
            get => isVisCostWithDis =;
            set
            {
                if (Discount != null)
                {
                    isVisCostWithDis = true;
                }
                else isVisCostWithDis = false;
            }
        }*/
        public string CostAndMin
        {
            get
            {
                int min = DurationInSecond / 60;
                if (Discount != null)
                {
                    float dif = Cost * (float)Discount;
                    float cost = Cost - dif;
                    return $"{cost} рублей за {min} минут";
                }
                return $"{Cost} рублей за {min} минут";
            }
        }

        public string DiscountFormat
        {
            get
            {
                if (Discount != null)
                {
                    decimal dis = Math.Round((decimal)Discount * 100, 0);
                    return $"* скидка {dis}%";
                }
                else return "";                
            }
        }
    }
}
