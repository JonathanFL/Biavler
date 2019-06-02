using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using Biavlere.Model;

namespace Biavlere
{
    public static class Persistence
    {

        private static ObservableCollection<VarroaCount> _varroaCounts;
        private static string _filename = Directory.GetCurrentDirectory() + "\\test.txt";

        public static void LoadCollection(ref ObservableCollection<VarroaCount> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            var tempAgents = new ObservableCollection<VarroaCount>();
            
            var serializer = new XmlSerializer(typeof(ObservableCollection<VarroaCount>));
            try
            {

                /*Debug.WriteLine(Directory.GetCurrentDirectory() + "\\test.txt TEST");*/
                if (!File.Exists(_filename))
                {
                    _filename = Directory.GetCurrentDirectory() + "\\test.txt";
                    TextWriter writer = new StreamWriter(_filename);
                    serializer.Serialize(writer, _varroaCounts);
                    writer.Close();
                }
                
                TextReader reader = new StreamReader(_filename);
                tempAgents = (ObservableCollection<VarroaCount>)serializer.Deserialize(reader);
                reader.Close();
            }
            catch (Exception ex)
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("dk-DK");
                string msg2 = new FileNotFoundException().Message;
                MessageBox.Show(ex.Message, "Ikke i stand til at loade data - kontakt IT-teknikker", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            collection = tempAgents;
        }
    }
}
