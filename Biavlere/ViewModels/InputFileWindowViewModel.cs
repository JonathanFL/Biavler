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
using Prism.Commands;
using Prism.Mvvm;

namespace Biavlere.ViewModels
{
    public class InputFileWindowViewModel : BindableBase
    {

        public string Filename { get; set; }
        private string _type = "Åben";
        public string Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        private ObservableCollection<VarroaCount> VarroaRecords
        {
            get => _varroaRecords;
            set => SetProperty(ref _varroaRecords, value);
        }

        private ObservableCollection<VarroaCount> _varroaRecords;

        ICommand _saveAsCommand;
        private string _filename;

        public InputFileWindowViewModel(ref ObservableCollection<VarroaCount> varroaRecords, string type)
        {
            _varroaRecords = new ObservableCollection<VarroaCount>(varroaRecords);
            _type = type;
        }

        public ICommand SaveAsCommand
        {
            get { return _saveAsCommand ?? (_saveAsCommand = new DelegateCommand<string>(SaveAsCommand_Execute)); }
        }

        private void SaveAsCommand_Execute(string argFilename)
        {
            if (argFilename == "")
            {
                MessageBox.Show("You must enter a file name in the File Name textbox!", "Unable to save file",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                _filename = argFilename;
                SaveFileCommand_Execute();
            }
        }

        private void SaveFileCommand_Execute()
        {

            if (Type == "Gem")
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<VarroaCount>));
                TextWriter writer = new StreamWriter(_filename);
                // Serialize all the agents.
                serializer.Serialize(writer, VarroaRecords);
                writer.Close();
            }
            else
            {
                if (_filename == "")
                {

                    MessageBox.Show("You must enter a file name in the File Name textbox!", "Unable to save file",
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    var tempAgents = new ObservableCollection<VarroaCount>();

                    // Create an instance of the XmlSerializer class and specify the type of object to deserialize.
                    XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<VarroaCount>));
                    try
                    {
                        TextReader reader = new StreamReader(_filename);
                        // Deserialize all the agents.
                        tempAgents = (ObservableCollection<VarroaCount>)serializer.Deserialize(reader);
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Unable to open file", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    VarroaRecords = tempAgents;
                }
            }
        }



        


    }
}
