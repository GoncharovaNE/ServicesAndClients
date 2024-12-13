using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAndClients.Models
{
    public partial class ClientService
    {
        public string ColorTime
        {
            get
            {
                DateTime now = DateTime.Now;
                TimeSpan remaining = StartTime - now;

                double totalMinutes = Math.Ceiling(remaining.TotalMinutes);
                int hours = (int)totalMinutes / 60;
                int minutes = (int)totalMinutes % 60;

                if (hours < 1 && minutes > 0) return "#f32a2a";
                else if (remaining.TotalSeconds < 0) return "#ffca19";
                return "";
            }
        }

        public string TimeRemaining
        {
            get
            {
                DateTime now = DateTime.Now;
                TimeSpan remaining = StartTime - now;

                if (remaining.TotalSeconds < 0)
                    return "время уже прошло";

                double totalMinutes = Math.Ceiling(remaining.TotalMinutes); 
                int hours = (int)totalMinutes / 60;
                int minutes = (int)totalMinutes % 60;

                return $"{hours} {GetHourWord(hours)} {minutes} {GetMinuteWord(minutes)}";
            }
        }

        private string GetHourWord(int hours)
        {
            if (hours % 10 == 1 && hours % 100 != 11)
                return "час";
            else if (hours % 10 >= 2 && hours % 10 <= 4 && (hours % 100 < 10 || hours % 100 >= 20))
                return "часа";
            else
                return "часов";
        }

        private string GetMinuteWord(int minutes)
        {
            if (minutes % 10 == 1 && minutes % 100 != 11)
                return "минута";
            else if (minutes % 10 >= 2 && minutes % 10 <= 4 && (minutes % 100 < 10 || minutes % 100 >= 20))
                return "минуты";
            else
                return "минут";
        }
    }
}
