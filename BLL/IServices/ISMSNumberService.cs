using BLL.DTOs.SMSNumber;

namespace BLL.IServices
{
    public interface ISMSNumberService
    {
        IEnumerable<SMSNumberReadDTO>? GetAllNumbers();
        IEnumerable<SMSNumberReadDTO>? GetAllNumbersById(int id);
    }
}
