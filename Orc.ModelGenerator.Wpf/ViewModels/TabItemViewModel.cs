using System.Windows;
using Catel.MVVM;

namespace Orc.ModelGenerator.Wpf.ViewModels
{
    public class TabItemViewModel : ViewModelBase
    {
        public GeneratorResult GeneratorResult { get; set; }

        public TabItemViewModel(GeneratorResult generatorResult)
        {
            GeneratorResult = generatorResult;
            CopyActiveTabToBuffer = new Command(OnCopyActiveTabToBuffer);
        }

        public Command CopyActiveTabToBuffer { get; private set; }

        private void OnCopyActiveTabToBuffer()
        {
            Clipboard.SetData(DataFormats.Text, GeneratorResult.OutputString);
        }
    }
}