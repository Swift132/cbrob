using System;


namespace CanSat_Terminal
{

    public class Model
    {
        public event EventHandler onNewString = null;

        private string[] _availableCOMs;
        private string _selectedCOM;
        private string _selectedBaudRate = " ";
        private string _newData;

        public string[] availableCOMs
        {
            get { return _availableCOMs; }
            set { _availableCOMs = value; }
        }

        public string SelectedCOM
        {
            get { return _selectedCOM; }
            set { _selectedCOM = value; }
        }

        public string selectedBaudRate
        {
            get { return _selectedBaudRate; }
            set { _selectedBaudRate = value; }
        }

        public string NewData
        {
            get { return _newData; }
            set { _newData = value; }
        }


    }

    public class ModelException : Exception
    {
        public ModelException()
        {
            //construtor
        }
    }
}
