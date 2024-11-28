using System;
using System.Collections.Generic;

namespace ServicesAndClients.Models;

public partial class Service
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public float Cost { get; set; }

    public int DurationInSecond { get; set; }

    public string? Description { get; set; }

    public float? Discount { get; set; }

    public string? MainImagePath { get; set; }

    public virtual ICollection<ClientService> ClientServices { get; set; } = new List<ClientService>();

    public virtual ICollection<ServicePhoto> ServicePhotos { get; set; } = new List<ServicePhoto>();
}
