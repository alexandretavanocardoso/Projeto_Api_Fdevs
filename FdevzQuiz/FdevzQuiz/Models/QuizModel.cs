﻿namespace FdevzQuiz.Models
{
    public class QuizModel
    {
        public int CodigoQuiz { get; set; }
        public string Titulo { get; set; }
        public int Nivel { get; set; }
        public int Respostas { get; set; }
        public string ImagemUrl { get; set; }
        public PerguntasModel Perguntas { get; set; }
    }
}
