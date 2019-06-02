using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biavlere.Model;
using Prism.Mvvm;

namespace Biavlere.ViewModels
{
    class VarroaRecordDetailsViewModel : BindableBase
    {
        public string Comments { get; set; }
        public string Title;
        public VarroaRecordDetailsViewModel(ref VarroaCount varroaCount)
        {
            Title = "Bemærkninger for Bistad: " + varroaCount.BistadId;
            Comments = varroaCount.Comment;
        }

    }
}
