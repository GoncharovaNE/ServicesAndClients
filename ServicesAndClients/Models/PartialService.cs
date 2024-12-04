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
        public bool IsVisCostWithDis
        {
            get
            {
                bool isVisCostWithDis;
                if (Discount != null && Discount != 0)
                {
                    return isVisCostWithDis = true;
                }
                return isVisCostWithDis = false;
            }
        }
        public string CostAndMin
        {
            get
            {
                int min = DurationInSecond / 60;
                if (Discount != null && Discount != 0)
                {
                    float dif = Cost * (float)Discount;
                    float cost = Cost - dif;                    
                    return $"{cost} рублей за {min} минут";
                }
                return $"{Cost} рублей за {min} минут";
            }
        }

        public string Color
        {
            get
            {
                string color;
                if (Discount != null && Discount != 0)
                {
                    return color = "#e7fabf";
                }
                return "";
            }
        }

        public string DiscountFormat
        {
            get
            {
                if (Discount != null && Discount != 0)
                {
                    decimal dis = Math.Round((decimal)Discount * 100, 0);
                    return $"* скидка {dis}%";
                }
                else return "";                
            }
        }        
    }
}
