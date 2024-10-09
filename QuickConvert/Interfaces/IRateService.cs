using QuickConvert.Models;

namespace QuickConvert.Interfaces
{
    public interface IRateService
    {
        Task<Rate> GetRate();
    }
}