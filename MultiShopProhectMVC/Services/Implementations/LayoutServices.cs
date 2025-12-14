using Microsoft.EntityFrameworkCore;
using MultiShopProjectMVC.DAL;
using MultiShopProjectMVC.Services.Interfaces;

namespace MultiShopProjectMVC.Services.Implementations
{

    public class LayoutServices:ILayoutService
    {
        private readonly AppDbContext _context;

        public LayoutServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<string, string>> GetSettingAsync()
        {
            Dictionary<string,string> setting = await _context.Settings.ToDictionaryAsync(s=>s.Key,s=>s.Value);
            return setting;
        }
    }
}
