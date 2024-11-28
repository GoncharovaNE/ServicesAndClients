using System;
using System.Collections.Generic;

namespace ServicesAndClients.Models;

public partial class Client
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public DateOnly? Birthday { get; set; }

    public DateOnly RegistrationDate { get; set; }

    public string? Email { get; set; }

    public string Phone { get; set; } = null!;

    public int Gendercode { get; set; }

    public string? PhotoPath { get; set; }

    public virtual ICollection<ClientService> ClientServices { get; set; } = new List<ClientService>();

    public virtual Gender GendercodeNavigation { get; set; } = null!;
}
