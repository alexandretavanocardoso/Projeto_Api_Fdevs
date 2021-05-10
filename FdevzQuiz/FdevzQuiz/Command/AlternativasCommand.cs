using System;

namespace FdevzQuiz.Command
{
    public class AlternativasCommand
    {
        public Guid CodigoAlternativa { get; set; }
        public Guid CodigoQuiz { get; set; }
        public Guid CodigoPergunta { get; set; }
        public string Titulo { get; set; }
        public bool Correta { get; set; }
    }
}
