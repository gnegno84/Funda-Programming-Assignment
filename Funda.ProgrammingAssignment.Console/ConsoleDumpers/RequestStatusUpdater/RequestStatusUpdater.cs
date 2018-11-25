using System.Drawing;
using Funda.ProgrammingAssignment.ServiceProxy.Services.RequestStatusUpdater;
using ShellProgressBar;

namespace Funda.ProgrammingAssignment.Console.ConsoleDumpers.RequestStatusUpdater
{
    public class ProgressBarRequestStatusUpdater : IRequestStatusUpdater
    {
        private ProgressBarOptions _options;
        private ProgressBar _progressbar;
        private string _progressBarText;

        public void Setup(string text)
        {
            _options = new ProgressBarOptions
            {
                ProgressCharacter = '─',
                ProgressBarOnBottom = true,
                CollapseWhenFinished = false,
                EnableTaskBarProgress = false
            };

            _progressBarText = text;
        }

        public void Initialize(int totalBatches, string text = null)
        {
            if (_options == null)
                Setup("");

            _progressbar = new ProgressBar(totalBatches, text ?? _progressBarText, _options);

        }

        public void Tick()
        {
            _progressbar?.Tick();
        }
    }

    public class SimpleDumperProgressBarRequestStatusUpdater : IRequestStatusUpdater
    {
        private string _progressBarText;
        private int _totalBatches;
        private int _currentBatch;

        public void Setup(string text)
        {
            _progressBarText = text;
        }

        public void Initialize(int totalBatches, string text = null)
        {
            _totalBatches = totalBatches;
            _currentBatch = 0;
            if (!string.IsNullOrEmpty(text))
                _progressBarText = text;
        }

        public void Tick()
        {
            _currentBatch += 1;

            Colorful.Console.WriteLine($"{_currentBatch}/{_totalBatches} - {_currentBatch / (float)_totalBatches * 100f:0}% - {_progressBarText}", Color.DarkGreen);
        }
    }
}