namespace Data.Entities
{
    public class MailSetting
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Host { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
    }
}
