using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Catel.Collections;
using Orc.ModelGenerator.DataProviders;
using Orc.ModelGenerator.EntityCodeGenerators;

namespace Orc.ModelGenerator.Wpf.ViewModels
{
    using Catel.MVVM;

    /// <summary>
    /// MainWindow view model.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        private string _output;
        private ObservableCollection<TabItemViewModel> _outputItems = new ObservableCollection<TabItemViewModel>();
        private ClassGenerator _classGenerator = new ClassGenerator();
        private CsvMapGenerator _csvMapGenerator = new CsvMapGenerator();
        private RepositoryReaderGenerator _repositoryReaderGenerator = new RepositoryReaderGenerator();
        private RepositoryUnitTestGenerator _repositoryUnitTestGenerator = new RepositoryUnitTestGenerator();
        private BaseGeneratorEntity _activeEntity;

        #region Fields
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel()
            : base()
        {
            InputFiles = new ObservableCollection<DataFile>();
            Entities = new ObservableCollection<Entity>();

            ActiveEntityChanged = new Command<BaseGeneratorEntity>(OnActiveEntityChanged);
            FileDragDrop = new Command<DragEventArgs>(OnFileDragDrop);
            FileDragEnter = new Command<DragEventArgs>(OnFileDragEnter);
            Run = new Command(OnRun);
        }

        #endregion

        #region Propertiess
        /// <summary>
        /// Gets the title of the view model.
        /// </summary>
        /// <value>The title.</value>
        public override string Title { get { return "View model title"; } }

        public ObservableCollection<DataFile> InputFiles { get; private set; }
        public ObservableCollection<Entity> Entities { get; private set; }

        public BaseGeneratorEntity ActiveEntity
        {
            get { return _activeEntity; }
            set
            {
                if (_activeEntity == value) return;
                _activeEntity = value;
                RaisePropertyChanged(() => ActiveEntity);
            }
        }

        public string Output
        {
            get { return _output; }
            set
            {
                if (value == _output) return;
                _output = value;
                RaisePropertyChanged(() => Output);
            }
        }

        public ObservableCollection<TabItemViewModel> OutputItems
        {
            get { return _outputItems; }
        }

        // TODO: Register models with the vmpropmodel codesnippet
        // TODO: Register view model properties with the vmprop or vmpropviewmodeltomodel codesnippets

        #endregion

        #region Commands

        public Command<BaseGeneratorEntity> ActiveEntityChanged { get; private set; }
        public Command<DragEventArgs> FileDragDrop { get; private set; }
        public Command<DragEventArgs> FileDragEnter { get; private set; }
        public Command Run { get; private set; }

        #endregion

        #region Methods
        private void OnActiveEntityChanged(BaseGeneratorEntity entity)
        {
            ActiveEntity = entity;
        }

        private void OnFileDragDrop(DragEventArgs e)
        {
            foreach (var path in (string[])e.Data.GetData(DataFormats.FileDrop, false))
            {
                if (Directory.Exists(path))
                {
                    var files = Directory.GetFiles(path).Select(x => new DataFile(x));
                    InputFiles.AddRange(files);
                }
                else
                {
                    var dataFile = new DataFile(path);
                    var dataProvider = DataProvider.CreateDataProvider(dataFile);
                    InputFiles.Add(dataFile);
                    Entities.AddRange(dataProvider.Generate());
                }
            }
        }

        private void OnFileDragEnter(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
                e.Effects = DragDropEffects.All;
            else
                e.Effects = DragDropEffects.None;
            e.Handled = true;
        }

        private void OnRun()
        {
            OutputItems.Clear();

            var entities = Entities.Where(x => x.IsEnabled);

            OutputItems.Add(new TabItemViewModel(new GeneratorResult
            {
                Title = "Class",
                OutputString = string.Join("\n", entities.Select(x => _classGenerator.Generate(x))),
            }));
            OutputItems.Add(new TabItemViewModel(new GeneratorResult
            {
                Title = "CsvMaps",
                OutputString = string.Join("\n", entities.Select(x => _csvMapGenerator.Generate(x))),
            }));
            OutputItems.Add(new TabItemViewModel(new GeneratorResult
            {
                Title = "Repository",
                OutputString = string.Join("\n", entities.Select(x => _repositoryReaderGenerator.Generate(x))),
            }));
            OutputItems.Add(new TabItemViewModel(new GeneratorResult
            {
                Title = "Unit tests",
                OutputString = string.Join("\n", entities.Select(x => _repositoryUnitTestGenerator.Generate(x))),
            }));
        }



        #endregion
    }
}
