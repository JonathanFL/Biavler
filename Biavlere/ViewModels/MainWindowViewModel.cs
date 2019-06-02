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

        private bool _alreadySearched = false;
        private bool _justOpened = true;

        private string filename = "test.txt";

        public ObservableCollection<VarroaCount> VarroaRecords
        { 
            get => _varroaRecords;
            set => SetProperty(ref _varroaRecords, value);
        }


        public MainWindowViewModel()
        {
            _varroaRecords = new ObservableCollection<VarroaCount>();


            Persistence.LoadCollection(ref _varroaRecords);


            //_varroaRecords.Add(new VarroaCount("ASD123",DateTime.Now, 12,"WHAT UP"));

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
                },CanAdd).ObservesProperty(() => VarroaRecords.Count));
            }
        }

        private bool CanAdd()
        {
            return ((filename != "") && (VarroaRecords.Count > 0) || _justOpened && VarroaRecords.Count > 0);
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
            if (!_alreadySearched)
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

            _alreadySearched = true;
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
            if (!_isSaved)
            {
                foreach (var varroaCount in temp)
                {
                    _varroaRecords.Add(varroaCount);
                }
            }
            else
            {
                var tempAgents = new ObservableCollection<VarroaCount>();

                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<VarroaCount>));
                try
                {
                    TextReader reader = new StreamReader(filename);

                    tempAgents = (ObservableCollection<VarroaCount>)serializer.Deserialize(reader);
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ikke i stand til at loade data - kontakt IT-teknikker", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                VarroaRecords = tempAgents;
            }

            _alreadySearched = false;
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
                _isSaved = true;
            }
        }

        #endregion


        #region SaveFileCommand

        private bool _isSaved = false;

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
            _isSaved = true;
        }

        private bool SaveFileCommand_CanExecute()
        {
            return (filename != "") && (VarroaRecords.Count > 0);
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


        #region Visualization

        private ICommand _visualizationCommand;

        public ICommand VisualizationCommand
        {
            get
            {
                return _visualizationCommand ?? (_visualizationCommand = new DelegateCommand(() =>
                {
                    var vm = new VisualizationViewModel(ref _varroaRecords);
                    var dlg = new visualisering()
                    {
                        DataContext = vm
                    };
                    if (dlg.ShowDialog() == true)
                    {

                    }
                }, VisualizeCommand_CanExecute).ObservesProperty(() => VarroaRecords.Count));
            }
        }

        private bool VisualizeCommand_CanExecute()
        {
            return (VarroaRecords.Count > 0);
        }
        

        #endregion

    }
}
