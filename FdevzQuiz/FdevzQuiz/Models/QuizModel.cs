using System;
using System.Collections.Generic;

namespace FdevzQuiz.Models
{
    public class QuizModel
    {
        public Guid CodigoQuiz { get; set; }
        public string Titulo { get; set; }
        public int Nivel { get; set; }
        public int Respostas { get; set; }
        public string ImagemUrl { get; set; }
        public ICollection<PerguntasModel> Perguntas { get; set; }
    }
}
