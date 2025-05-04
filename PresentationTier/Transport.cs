using System;
using System.Collections.Generic;

namespace PresentationTier;

public partial class Transport
{
    public int IdTransport { get; set; }

    public string Nazvanie { get; set; } = null!;

    public virtual ICollection<Itog> Itogs { get; set; } = new List<Itog>();
}
