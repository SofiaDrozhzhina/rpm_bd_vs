using System;
using System.Collections.Generic;

namespace PresentationTier;

public partial class PunktNaznach
{
    public int IdPunktNaznach { get; set; }

    public string NazvaniePunktN { get; set; } = null!;

    public virtual ICollection<Itog> Itogs { get; set; } = new List<Itog>();
}
