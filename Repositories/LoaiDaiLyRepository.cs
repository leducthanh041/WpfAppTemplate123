using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WpfAppTemplate.Services;
using WpfAppTemplate.Models;
using WpfAppTemplate.Data;
using WpfAppTemplate.Configs;


namespace WpfAppTemplate.Repositories
{
    public class LoaiDaiLyRepository : ILoaiDaiLyService
    {
        private readonly DataContext _context;

        public LoaiDaiLyRepository(DatabaseConfig databaseConfig)
        {
            _context = databaseConfig.DataContext;
            if (_context == null)
            {
                throw new ArgumentNullException(nameof(databaseConfig), "Database not initialized!");
            }
        }

        //public async Task<LoaiDaiLy> GetLoaiDaiLyById(int id)
        //{
        //    LoaiDaiLy? loaiDaiLy = await _context.DsLoaiDaiLy.FindAsync(id);
        //    return loaiDaiLy ?? throw new Exception("LoaiDaiLy not found!");
        //}


        //public async Task<LoaiDaiLy> GetLoaiDaiLyByTenLoaiDaiLy(string tenLoaiDaiLy)
        //{
        //    LoaiDaiLy? loaiDaiLy = await _context.DsLoaiDaiLy.FirstOrDefaultAsync(l => l.TenLoaiDaiLy == tenLoaiDaiLy);
        //    return loaiDaiLy ?? throw new Exception("LoaiDaiLy not found!");
        //}


        async Task<IEnumerable<LoaiDaiLy>> ILoaiDaiLyService.GetAllLoaiDaiLy()
        {
            return await _context.DsLoaiDaiLy
                .Include(l => l.DsDaiLy)
                .ToListAsync();
        }

        public async Task AddLoaiDaiLy(LoaiDaiLy loaiDaiLy)
        {
            _context.DsLoaiDaiLy.Add(loaiDaiLy);
            await _context.SaveChangesAsync();
        }


        //public async Task UpdateLoaiDaiLy(LoaiDaiLy loaiDaiLy)
        //{
        //    _context.Entry(loaiDaiLy).State = EntityState.Modified;
        //    await _context.SaveChangesAsync();
        //}

        //public async Task DeleteLoaiDaiLy(int id)
        //{
        //    var loaiDaiLy = await _context.DsLoaiDaiLy.FindAsync(id);
        //    if (loaiDaiLy != null)
        //    {
        //        _context.DsLoaiDaiLy.Remove(loaiDaiLy);
        //        await _context.SaveChangesAsync();
        //    }
        //}
        
        public async Task<int> GenerateAvailableId()
        {
            int maxId = await _context.DsLoaiDaiLy.MaxAsync(d => d.MaLoaiDaiLy);
            return maxId + 1;
        }
    }
}
