using Catel.Data;

namespace Orc.ModelGenerator
{
    public class GeneratorResult:ModelBase
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                if (value == _title) return;
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        private string _outputString;
        public string OutputString
        {
            get { return _outputString; }
            set
            {
                if (value == _outputString) return;
                _outputString = value;
                RaisePropertyChanged(() => OutputString);
            }
        }

    }
}