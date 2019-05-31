using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Biavlere.Model;
using Prism.Commands;
using Prism.Mvvm;

namespace Biavlere.ViewModels
{
    public class AddCountViewModel : BindableBase
    {

        private VarroaCount _currentVarroaCount;


        public AddCountViewModel(VarroaCount varroaCount)
        {
            _currentVarroaCount = varroaCount;
            _currentVarroaCount.BistadId = "Angiv Id for Bistad";
        }

        /*public AddCountViewModel()
        {
            _currentVarroaCount = new VarroaCount("Angiv Id for Bistad");
        }*/


        public string BistadId
        {
            get => _currentVarroaCount.BistadId;
            set
            {
                if (_currentVarroaCount.BistadId != value)
                {
                    _currentVarroaCount.BistadId = value;
                }
            }
        }

        public DateTime DateTime
        {
            get => _currentVarroaCount.OptaellingsDato;
            set
            {
                if (_currentVarroaCount.OptaellingsDato != value)
                {
                    _currentVarroaCount.OptaellingsDato = value;
                }
            }
        }

        public int NumberOfMites {
            get => _currentVarroaCount.NumberOfVarroaMites;
            set
            {
                if (_currentVarroaCount.NumberOfVarroaMites != value)
                {
                    _currentVarroaCount.NumberOfVarroaMites = value;
                }
            }
        }
        public string Comments {
            get => _currentVarroaCount.Comment;
            set
            {
                if (_currentVarroaCount.Comment != value)
                {
                    _currentVarroaCount.Comment = value;
                }
            }
        }
        
        #region properties
    
        public VarroaCount CurrentvarroaCount
        {
            get => _currentVarroaCount;
            set => SetProperty(ref _currentVarroaCount, value);
        }
        
        #endregion


        #region Commands

        ICommand _saveBtnCommand;
        public ICommand SaveBtnCommand
        {
            get
            {
                return _saveBtnCommand ?? (_saveBtnCommand = new DelegateCommand(
                               OkBtnCommand_Execute, OkBtnCommand_CanExecute)
                           .ObservesProperty(() => CurrentvarroaCount.BistadId)
                           .ObservesProperty(() => CurrentvarroaCount.OptaellingsDato)
                           .ObservesProperty(() => CurrentvarroaCount.NumberOfVarroaMites)
                           .ObservesProperty(() => CurrentvarroaCount.Comment)
                       );
            }
        }

        private bool OkBtnCommand_CanExecute()
        {
            return !string.IsNullOrWhiteSpace(CurrentvarroaCount.BistadId);
        }

        private void OkBtnCommand_Execute()
        {
            DialogCloser.SetDialogResult(Application.Current.Windows[1],true);
        }

        #endregion

    }
}
