namespace TDCLibrary.GtsfModel.Enums;

/// <summary>
/// Typ prestupu medzi zastávkami
/// </summary>
public enum TransferType
{
    /// <summary>
    /// Odporúčaný prestupový bod - Recommended transfer point
    /// Toto je odporúčané miesto pre prestup medzi trasami
    /// </summary>
    Recommended = 0,
    
    /// <summary>
    /// Časovaný prestup - Timed transfer
    /// Odchádzajúce vozidlo čaká na prichádzajúce vozidlo, s dostatočným časom na prestup
    /// </summary>
    Timed = 1,
    
    /// <summary>
    /// Je potrebný minimálny čas - Minimum time required
    /// Vyžaduje sa minimálny čas medzi príchodom a odchodom pre zaručený prestup
    /// </summary>
    MinimumTimeRequired = 2,
    
    /// <summary>
    /// Prestup nie je možný - Transfer not possible
    /// Prestupy medzi trasami nie sú na tomto mieste možné
    /// </summary>
    NotPossible = 3
}
