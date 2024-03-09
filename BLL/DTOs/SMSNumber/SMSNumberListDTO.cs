
namespace BLL.DTOs.SMSNumber
{
    public class SMSNumberListDTO
    {
        public int Id { get; set; }
        public virtual IEnumerable<SMSNumberDetailsDTO> SMSNumbers { get; set; } = new HashSet<SMSNumberDetailsDTO>();
    }
}
