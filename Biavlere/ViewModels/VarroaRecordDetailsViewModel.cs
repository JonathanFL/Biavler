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
        public VarroaRecordDetailsViewModel(ref VarroaCount varroaCount)
        {
            Comments = varroaCount.Comment;
        }

    }
}
