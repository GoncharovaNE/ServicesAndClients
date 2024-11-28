using System;
using System.Collections.Generic;

namespace ServicesAndClients.Models;

public partial class ClientService
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    public int ServiceId { get; set; }

    public DateTime StartTime { get; set; }

    public string? Comment { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}
