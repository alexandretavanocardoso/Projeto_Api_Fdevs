namespace Api.Domain.Security
{
    public class TokenConfiguration
    {
        //publico
        public string Audience { get; set; }
        // Emissor
        public string Issuer { get; set; }
        // Tempo de
        public int Seconds { get; set; }
    }
}
