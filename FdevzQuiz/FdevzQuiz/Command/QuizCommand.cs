using System;

namespace FdevzQuiz.Command
{
    public class QuizCommand
    {
        public Guid CodigoQuiz { get; set; }
        public string Titulo { get; set; }
        public string Nivel { get; set; }
        public int Respostas { get; set; }
        public string ImagemUrl { get; set; }
    }
}
