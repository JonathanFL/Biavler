using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Biavlere.Model;
using OxyPlot;
using Prism.Mvvm;

namespace Biavlere.ViewModels
{
    public class VisualizationViewModel : BindableBase
    {
        private ObservableCollection<VarroaCount> _varroaCounts;
        public VisualizationViewModel(ref ObservableCollection<VarroaCount> varroaCounts)
        {
            _varroaCounts = varroaCounts;

            var i = 0.0;
            foreach (var varroaCount in _varroaCounts)
            {
                points.Add(new DataPoint(varroaCount.NumberOfVarroaMites,i));
                i++;
            }

        }

        List<DataPoint> points = new List<DataPoint>();



        public IEnumerable<DataPoint> Points
        {
            get { return points; }
        }


    }
}
