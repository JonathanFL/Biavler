using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using Biavlere.Model;
using Biavlere.Views;
using Prism.Commands;
using Prism.Mvvm;

namespace Biavlere.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        private ObservableCollection<VarroaCount> _varroaRecords;
        private VarroaCount _currentVarroaCount = null;
        private List<VarroaCount> temp;

        private string filename;

        public ObservableCollection<VarroaCount> VarroaRecords
        { 
            get => _varroaRecords;
            set => SetProperty(ref _varroaRecords, value);
        }


        public MainWindowViewModel()
        {
            _varroaRecords = new ObservableCollection<VarroaCount>();

            _varroaRecords.Add(new VarroaCount("ASDFGHJKLÆØQWERTYU", DateTime.Now, 12, "Det var lige godt var"));
            _varroaRecords.Add(new VarroaCount("AX432", DateTime.Now, 12, "Det var lige godt var"));
            _varroaRecords.Add(new VarroaCount("ASD", DateTime.Now, 12, "Det hej hej hej ehj var lige godt var"));
            _varroaRecords.Add(new VarroaCount("AX432", DateTime.Now, 12, "Det var lige godt var"));
            _varroaRecords.Add(new VarroaCount("ASD", DateTime.Now, 12, "Det var lige godt var"));
            _varroaRecords.Add(new VarroaCount("AX432", DateTime.Now, 12, "Det var lige godt var"));
            _varroaRecords.Add(new VarroaCount("AX432", DateTime.Now, 12, "Det var lige godt var"));
        }


        private ICommand _addNewRecordCommand;
        private int currentIndex = 0;

        public ICommand AddNewRecord
        {
            get
            {
                return _addNewRecordCommand ?? (_addNewRecordCommand = new DelegateCommand(() =>
                {
                    var newRecord = new VarroaCount();
                    var vm = new AddCountViewModel(newRecord);
                    var dlg = new AddCountWindow
                    {
                        DataContext = vm
                    };
                    if (dlg.ShowDialog() == true)
                    {

                        /*Logger.addDebtor(newDebtor);*/
                        _varroaRecords.Add(newRecord);
                    }
                }));
            }
        }

        

        public VarroaCount CurrentVarroaCount
        {
            get => _currentVarroaCount;
            set => SetProperty(ref _currentVarroaCount, value);
        }

        public int CurrentIndex
        {
            get => currentIndex;
            set => SetProperty(ref currentIndex, value);
        }

        private bool _canReset = false;

        public bool CanReset
        {
            get => _canReset;
            set => SetProperty(ref _canReset, value);
        }

        private ICommand _selectedRecordDetailsCommand;
        public ICommand SelectedRecordDetailsCommand
        {
            get
            {
                return _selectedRecordDetailsCommand ?? (_selectedRecordDetailsCommand = new DelegateCommand(() =>
                {
                    var vm = new VarroaRecordDetailsViewModel(ref _currentVarroaCount);

                    var dlg = new VarroaRecordDetails()
                    {
                        DataContext = vm
                    };

                    if (dlg.ShowDialog() == true)
                    {

                    }
                }));
            }
        }


        private ICommand _closeApplication;

        public ICommand CloseApplicationCommand =>
            _closeApplication ?? (_closeApplication =
                new DelegateCommand(OnClose, CanExecuteMethod));

        private bool CanExecuteMethod()
        {
            return true;
        }

        public void OnClose()
        {
            Application.Current.MainWindow?.Close();
        }


        private ICommand _searchCommand;

        public ICommand SearchCommand =>
            _searchCommand ?? (_searchCommand = new
                DelegateCommand<String>(SearchCommand_Execute).ObservesProperty(() => CanReset));

        private void SearchCommand_Execute(string bistadId)
        {
            CanReset = true;

            temp = new List<VarroaCount>(_varroaRecords);

            _varroaRecords.Clear();

            foreach (var varroaCount in temp)
            {
                if (varroaCount.BistadId == bistadId)
                {
                    _varroaRecords.Add(varroaCount);
                }
            }
        }

        private ICommand _resetDataGrid;

        public ICommand ResetDataGrid =>
            _resetDataGrid ?? (_resetDataGrid = new
                DelegateCommand(ResetDataGridCommand_Execute,CanExecuteReset).ObservesProperty(() => CanReset));

        private bool CanExecuteReset()
        {
            return CanReset;
        }

        private void ResetDataGridCommand_Execute()
        {
            _varroaRecords.Clear();
            foreach (var varroaCount in temp)
            {
                _varroaRecords.Add(varroaCount);
            }

            CanReset = false;
        }


        #region SaveAsFileCommand

        
        ICommand _saveAsFileCommand;
        

        public ICommand SaveAsFileCommand => 
            _saveAsFileCommand ?? (_saveAsFileCommand = new DelegateCommand(SaveAsFileCommand_Execute));

        private void SaveAsFileCommand_Execute()
        {
            var vm = new InputFileWindowViewModel(ref _varroaRecords, "Gem");

            var dlg = new InputFileWindow
            {
                DataContext = vm
            };

            if (dlg.ShowDialog() == true)
            {

            }
        }

        #endregion


        #region SaveFileCommand
            


        ICommand _SaveCommand;
        public ICommand SaveCommand
        {
            get
            {
                return _SaveCommand ?? (_SaveCommand = new DelegateCommand(SaveFileCommand_Execute, SaveFileCommand_CanExecute)
                           .ObservesProperty(() => VarroaRecords.Count));
            }
        }

        private void SaveFileCommand_Execute()
        {
            // Create an instance of the XmlSerializer class and specify the type of object to serialize.
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<VarroaCount>));
            TextWriter writer = new StreamWriter("test.txt");
            // Serialize all the agents.
            serializer.Serialize(writer, VarroaRecords);
            writer.Close();
        }

        private bool SaveFileCommand_CanExecute()
        {
            return true; //(filename != "") && (VarroaRecords.Count > 0);
        }

        #endregion


        #region OpenFileCommand

        ICommand _OpenFileCommand;
        public ICommand OpenFileCommand
        {
            get { return _OpenFileCommand ?? (_OpenFileCommand = new DelegateCommand<string>(OpenFileCommand_Execute)); }
        }

        private void OpenFileCommand_Execute(string argFilename)
        {
            var vm = new InputFileWindowViewModel(ref _varroaRecords, "Åben");

            var dlg = new InputFileWindow
            {
                DataContext = vm
            };

            if (dlg.ShowDialog() == true)
            {

            }
        }

        #endregion

    }
}
