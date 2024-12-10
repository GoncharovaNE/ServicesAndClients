using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAndClients.Models
{
    public partial class Client
    {
        public string fio => LastName + " " + FirstName + " " + Patronymic;
    }
}
