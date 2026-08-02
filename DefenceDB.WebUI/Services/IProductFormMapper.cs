using DefenceDB.EL.Models;
using Microsoft.AspNetCore.Http;

namespace DefenceDB.WebUI.Services;

public interface IProductFormMapper
{
    /// <summary>
    /// Gelen IFormCollection verisini kullanarak yeni bir DefenseProduct oluşturur.
    /// Kategoriye özel alanlar Specs dictionary'sine yazılır.
    /// </summary>
    DefenseProduct? MapFromFormForCreate(IFormCollection form);

    /// <summary>
    /// Var olan bir DefenseProduct nesnesini formdan gelen verilerle günceller.
    /// </summary>
    void MapFromFormForEdit(IFormCollection form, DefenseProduct existingInstance);
}
