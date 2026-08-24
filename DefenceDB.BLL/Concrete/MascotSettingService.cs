using DefenceDB.BLL.Abstract;
using DefenceDB.DAL;
using DefenceDB.EL.Models;
using Microsoft.EntityFrameworkCore;

namespace DefenceDB.BLL.Concrete;

public class MascotSettingService : IMascotSettingService
{
    private readonly AppDbContext _context;
    private readonly ICacheService _cacheService;
    private const string CACHE_KEY = "mascot:settings:all";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);

    public MascotSettingService(AppDbContext context, ICacheService cacheService)
    {
        _context = context;
        _cacheService = cacheService;
    }

    public async Task<List<MascotSetting>> GetAllAsync()
    {
        var cached = await _cacheService.GetAsync<List<MascotSetting>>(CACHE_KEY);
        if (cached != null)
            return cached;

        var list = await _context.MascotSettings
            .AsNoTracking()
            .OrderBy(m => m.TargetPath)
            .ToListAsync();

        await _cacheService.SetAsync(CACHE_KEY, list, CacheDuration);
        return list;
    }

    public async Task<MascotSetting?> GetByIdAsync(int id)
    {
        return await _context.MascotSettings.FindAsync(id);
    }

    public async Task<MascotSetting?> GetByPathAsync(string path)
    {
        var all = await GetAllAsync();
        return all.FirstOrDefault(m => m.IsActive && string.Equals(m.TargetPath, path, StringComparison.OrdinalIgnoreCase));
    }

    public async Task AddAsync(MascotSetting mascotSetting)
    {
        _context.MascotSettings.Add(mascotSetting);
        await _context.SaveChangesAsync();
        await _cacheService.RemoveAsync(CACHE_KEY);
    }

    public async Task UpdateAsync(MascotSetting mascotSetting)
    {
        _context.MascotSettings.Update(mascotSetting);
        await _context.SaveChangesAsync();
        await _cacheService.RemoveAsync(CACHE_KEY);
    }

    public async Task DeleteAsync(int id)
    {
        var setting = await _context.MascotSettings.FindAsync(id);
        if (setting != null)
        {
            _context.MascotSettings.Remove(setting);
            await _context.SaveChangesAsync();
            await _cacheService.RemoveAsync(CACHE_KEY);
        }
    }
}
