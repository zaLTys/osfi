namespace Vic.ZubStatistika.Entities
{
    public class User
    {
        public virtual int Id { get; set; }
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
        public virtual string Role { get; set; }
        public virtual string Vardas { get; set; }
        public virtual string Pavarde { get; set; }
    }
}