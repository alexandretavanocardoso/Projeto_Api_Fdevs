using System;

namespace FdevzQuiz.Command
{
    public class PerguntasCommand
    {
        public Guid CodigoPergunta { get; set; }
        public Guid CodigoQuiz { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
    }
}
