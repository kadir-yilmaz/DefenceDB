using DefenceDB.BLL.Abstract;
using DefenceDB.DAL;
using DefenceDB.EL.Models;
using Microsoft.EntityFrameworkCore;

namespace DefenceDB.BLL.Concrete;

public class MascotSettingService : IMascotSettingService
{
    private readonly AppDbContext _context;

    public MascotSettingService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<MascotSetting>> GetAllAsync()
    {
        return await _context.MascotSettings.OrderBy(m => m.TargetPath).ToListAsync();
    }

    public async Task<MascotSetting?> GetByIdAsync(int id)
    {
        return await _context.MascotSettings.FindAsync(id);
    }

    public async Task<MascotSetting?> GetByPathAsync(string path)
    {
        return await _context.MascotSettings
            .Where(m => m.IsActive && m.TargetPath.ToLower() == path.ToLower())
            .FirstOrDefaultAsync();
    }

    public async Task AddAsync(MascotSetting mascotSetting)
    {
        _context.MascotSettings.Add(mascotSetting);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(MascotSetting mascotSetting)
    {
        _context.MascotSettings.Update(mascotSetting);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var setting = await _context.MascotSettings.FindAsync(id);
        if (setting != null)
        {
            _context.MascotSettings.Remove(setting);
            await _context.SaveChangesAsync();
        }
    }
}
