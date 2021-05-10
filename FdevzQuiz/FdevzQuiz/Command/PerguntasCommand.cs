using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FdevzQuiz.Command
{
    public class PerguntasCommand
    {
        public Guid CodigoPergunta { get; set; }
        public Guid CodigoQuiz { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public ICollection<AlternativasCommand> Alternativas { get; set; }
    }
}
