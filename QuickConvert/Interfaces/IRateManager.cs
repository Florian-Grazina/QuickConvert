using QuickConvert.Models;

namespace QuickConvert.Interfaces
{
    public interface IRateManager
    {
        Task<Rate> GetRate();
    }
}