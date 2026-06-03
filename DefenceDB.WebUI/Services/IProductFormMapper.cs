using DefenceDB.EL.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DefenceDB.WebUI.Services;

public interface IProductFormMapper
{
    /// <summary>
    /// Gelen IFormCollection verisini kullanarak yeni bir DefenseProduct türetir.
    /// Create işlemleri için kullanılır.
    /// </summary>
    DefenseProduct? MapFromFormForCreate(IFormCollection form);

    /// <summary>
    /// Var olan bir DefenseProduct nesnesini formdan gelen verilerle günceller.
    /// Edit işlemleri için kullanılır.
    /// </summary>
    void MapFromFormForEdit(IFormCollection form, DefenseProduct existingInstance);
}
