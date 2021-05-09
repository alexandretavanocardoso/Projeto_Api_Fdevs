namespace FdevzQuiz.Models
{
    public class PerguntasModel
    {
        public int CodigoPergunta { get; set; }
        public int CodigoQuiz { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public AlternativasModel Alternativas { get; set; }
    }
}
