using System.ComponentModel.DataAnnotations;

namespace DefenceDB.EL.Models.Products;

public class TurboshaftEngine : DefenseProduct
{
    [Display(Name = "Şaft Beygir Gücü (SHP)")]
    public double? ShaftHorsePowerHp { get; set; }
}
