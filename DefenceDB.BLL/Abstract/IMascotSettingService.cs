using DefenceDB.EL.Models;

namespace DefenceDB.BLL.Abstract;

public interface IMascotSettingService
{
    Task<List<MascotSetting>> GetAllAsync();
    Task<MascotSetting?> GetByIdAsync(int id);
    Task<MascotSetting?> GetByPathAsync(string path);
    Task AddAsync(MascotSetting mascotSetting);
    Task UpdateAsync(MascotSetting mascotSetting);
    Task DeleteAsync(int id);
}
