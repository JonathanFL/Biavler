using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;


namespace Biavlere.Model
{
    public class VarroaCount : BindableBase
    {
        public string BistadId { get; set; }
        public DateTime OptaellingsDato { get; set; }
        public int NumberOfVarroaMites { get; set; }
        public string Comment { get; set; }

        public VarroaCount(string bistadId, DateTime optaellingsDato, int numberOfVarroaMites, string comment)
        {
            BistadId = bistadId;
            OptaellingsDato = optaellingsDato;
            NumberOfVarroaMites = numberOfVarroaMites;
            Comment = comment;
        }

        public VarroaCount()
        {
            BistadId = "";
            OptaellingsDato = DateTime.Now;
            NumberOfVarroaMites = 0;
            Comment = "";
        }

        public VarroaCount(string bistadId)
        {
            BistadId = bistadId;
            OptaellingsDato = DateTime.Now;
            NumberOfVarroaMites = 0;
            Comment = "";
        }



    }
}
