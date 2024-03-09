namespace BLL.Authentication
{
    public class AuthenDto
    {
        public string Message { get; set; } = string.Empty;
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public IEnumerable<string> Roles { get; set; } = new HashSet<string>();
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresOn { get; set; }
    }
}
