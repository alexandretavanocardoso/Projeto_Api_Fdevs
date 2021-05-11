using System;

namespace FdevzQuiz.Models
{
    public class PerguntasModel
    {
        public Guid CodigoPergunta { get; set; }
        public Guid CodigoQuiz { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
    }
}
