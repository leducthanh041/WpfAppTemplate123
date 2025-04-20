using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppTemplate.Models;

namespace WpfAppTemplate.Services
{
    public interface IDaiLyService
    {
        Task<DaiLy> GetDaiLyById(int id);
        Task<IEnumerable<DaiLy>> GetAllDaiLy();
        Task AddDaiLy(DaiLy daiLy);
        Task UpdateDaiLy(DaiLy daiLy);
        Task DeleteDaiLy(int id);
        //Task<DaiLy> GetDaiLyByTenDaiLy(string tenDaiLy);
        Task<int> GenerateAvailableId();
    }
}
