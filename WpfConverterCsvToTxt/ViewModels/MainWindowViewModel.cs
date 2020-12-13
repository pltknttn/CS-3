using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfConverterCsvToTxt.Commands;
using WpfUserControlLibrary;

namespace WpfConverterCsvToTxt.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Title { get; set; } = "Конвертор CSV в TXT";
        public string FileFrom { get; set; }
        public string FileTo { get; set; }

        private string _resultLabel = String.Empty;
        public string ResultLabel { get => _resultLabel; set => Set(ref _resultLabel, value); }
         
        private bool _isFileToEnabled = true;
        public bool IsFileToEnabled { get => _isFileToEnabled; set => Set(ref _isFileToEnabled, value); }
        
        private bool _isFileFromEnabled = true;
        public bool IsFileFromEnabled { get => _isFileFromEnabled; set => Set(ref _isFileFromEnabled, value); } 
        
        private bool _isButtonConvertEnabled = true;
        public bool IsButtonConvertEnabled { get => _isButtonConvertEnabled; set => Set(ref _isButtonConvertEnabled, value); }
        
        private bool _isButtonStopEnabled = false;
        public bool IsButtonStopEnabled { get => _isButtonStopEnabled; set => Set(ref _isButtonStopEnabled, value); }

        private double _progressValue = 0;
        public double ProgressValue { get => _progressValue; set => Set(ref _progressValue, value); }
         
        private int highestPercentageReached = 0; 
        private BackgroundWorker backgroundWorker = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation =true};

        public MainWindowViewModel()
        {
            InitializeBackgroundWorker();
        }

        private void InitializeBackgroundWorker()
        {
            backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(  backgroundWorker_RunWorkerCompleted);
            backgroundWorker.ProgressChanged +=  new ProgressChangedEventHandler( backgroundWorker_ProgressChanged);
        }

        private ICommand _closeCommand;
        public ICommand CloseCommand => _closeCommand ?? (_closeCommand = new RelayCommand((object par) => {
            if(backgroundWorker.IsBusy) backgroundWorker.CancelAsync();
            App.Current.Shutdown();
        }));

        private ICommand _startCommand;
        public ICommand StartCommand => _startCommand ?? (_startCommand = new RelayCommand((object par) => 
        {             
            if (string.IsNullOrWhiteSpace(FileFrom) || !File.Exists(FileFrom))
            {
                Dialog.ShowWarning("Файл конвертации не выбран или не указан!!!");
                return;
            }

            if (string.IsNullOrWhiteSpace(FileTo) || !FileTo.EndsWith(".txt", StringComparison.CurrentCultureIgnoreCase))
            {
                Dialog.ShowWarning("Файл для сохранения результата не указан!!!");
                return;
            }

            ResultLabel = "Конвертируем...";
            IsFileFromEnabled = false;
            IsFileToEnabled = false;
            IsButtonConvertEnabled = false;
            IsButtonStopEnabled = true;
            highestPercentageReached = 0;

            backgroundWorker.RunWorkerAsync(new object[] { FileFrom, FileTo });
        }));

        private ICommand _stopCommand;
        public ICommand StopCommand => _stopCommand ?? (_stopCommand = new RelayCommand((object par) => 
        { 
            backgroundWorker.CancelAsync();

            IsFileFromEnabled = true;
            IsFileToEnabled = true;
            IsButtonConvertEnabled = true;
            IsButtonStopEnabled = false;
        }));

        private void SetReportProgress(BackgroundWorker worker, float numberToCompute, float current)
        {
            int percentComplete = (int)((current /numberToCompute) * 100);
            if (percentComplete > highestPercentageReached)
            {
                highestPercentageReached = percentComplete;
                worker.ReportProgress(percentComplete);
            }
        }

        private void StartReportProgress(BackgroundWorker worker)
        {
            highestPercentageReached = 0;
            worker.ReportProgress(0);
        }

        private void FinishReportProgress(BackgroundWorker worker)
        {
            SetReportProgress(worker, 100, 100);
        }

        private bool LoadCsv(string filename, BackgroundWorker worker, DoWorkEventArgs doWorkEvent, out List<string> list)
        {                      
            list = new List<string>();
            StreamReader sr = null;
            if (worker.CancellationPending) return false;

            int n = 0;
            ResultLabel = $"Считываем файл {filename}...";
            try
            {
                sr = new StreamReader(filename);
                while (!worker.CancellationPending && !sr.EndOfStream)
                {
                    try
                    {
                        Thread.Sleep(10);
                        n++;
                        list.Add(sr.ReadLine());                        
                    }
                    catch (Exception e)
                    {
                        ResultLabel = $"Ошибка {e.Message}";
                        doWorkEvent.Cancel = true;
                        return false; 
                    }
                    SetReportProgress(worker, (float)n + 100, n); 
                }
                FinishReportProgress(worker);
                return true;
            }
            catch
            {
                list.Clear();
                return false;
            }
            finally
            {
                sr?.Close();
            } 
        }

        private bool WriteCsv(string fileName, List<string> list, BackgroundWorker worker, DoWorkEventArgs doWorkEvent)
        {
            if (worker.CancellationPending) return false;

            Thread.Sleep(100);
            StartReportProgress(worker);
            ResultLabel = $"Записываем файл {fileName}...";
           
            StreamWriter sr = null;
            int numberToCompute = list.Count;
            int n = 0;    
            try
            {
                sr = new StreamWriter(fileName);
                while (!worker.CancellationPending && n < numberToCompute)
                {
                    Thread.Sleep(10);
                    sr.WriteLine(list[n]);
                    n++;
                    SetReportProgress(worker, numberToCompute, n); 
                }
                if (n >= numberToCompute)
                {
                    FinishReportProgress(worker);
                    doWorkEvent.Cancel = true;
                    return true;
                } 
            }
            finally
            {
                sr?.Close();
            }
            return false;
        }

        private bool ConverterCsvToTxt(string fileFrom, string fileTo, BackgroundWorker worker, DoWorkEventArgs e)
        {
            if(LoadCsv(fileFrom, worker, e, out var list))             
                return WriteCsv(fileTo, list, worker, e);
            return true;
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var filefrom = (e.Argument as object[])[0] as string;
            var fileTo = (e.Argument as object[])[1] as string;
            var worker = sender as BackgroundWorker;
            
            e.Result = ConverterCsvToTxt(filefrom, fileTo, worker, e);
        }
                         
        private void backgroundWorker_RunWorkerCompleted( object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Dialog.ShowException(e.Error.Message);
                ResultLabel = e.Error.Message;
            }
            else if (e.Cancelled)
            { 
                ResultLabel = "Выполнено...";
            } 
            else if (!(bool)e.Result)
            {
                ResultLabel = $"Остановлен...";
            }
             
            IsFileFromEnabled = true;
            IsFileToEnabled = true;
            IsButtonConvertEnabled = true;
            IsButtonStopEnabled = false;
        }
         
        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressValue = e.ProgressPercentage;
        }
    }
}
