namespace localLib.Models
{
    public class User
    {
        public long UserId { get; set; }
        public string NumeUtilizator { get; set; }
        public string Email { get; set; }
        public string Parola { get; set; }
        public string NumePrenume { get; set; }
        public Rol Rol { get; set; }
    }
}
