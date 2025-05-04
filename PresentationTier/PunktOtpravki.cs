using System;
using System.Collections.Generic;

namespace PresentationTier;

public partial class PunktOtpravki
{
    public int IdPunktOtpravki { get; set; }

    public string NazvaniePunktO { get; set; } = null!;

    public virtual ICollection<Itog> Itogs { get; set; } = new List<Itog>();
}
