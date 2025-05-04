using System;
using System.Collections.Generic;

namespace PresentationTier;

public partial class Itog
{
    public int IdItog { get; set; }

    public int IdPunktOtpravki { get; set; }

    public int IdPunktNaznach { get; set; }

    public int IdTransport { get; set; }

    public virtual PunktNaznach IdPunktNaznachNavigation { get; set; } = null!;

    public virtual PunktOtpravki IdPunktOtpravkiNavigation { get; set; } = null!;

    public virtual Transport IdTransportNavigation { get; set; } = null!;
}
