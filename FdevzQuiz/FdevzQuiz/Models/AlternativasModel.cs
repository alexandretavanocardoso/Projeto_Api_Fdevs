namespace FdevzQuiz.Models
{
    public class AlternativasModel
    {
        public int CodigoAlternativa { get; set; }
        public int CodigoQuiz { get; set; }
        public int CodigoPergunta { get; set; }
        public string Titulo { get; set; }
        public bool Correta { get; set; }
    }
}
