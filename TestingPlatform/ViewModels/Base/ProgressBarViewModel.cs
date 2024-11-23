using System.Windows;

namespace TestingPlatform.ViewModels.Base
{
    internal class ProgressBarViewModel : ViewModel
    {
        #region ProgressBarValue
        private int _progressBarValue = 0;
        public int ProgressBarValue
        {
            get => _progressBarValue;
            set => Set(ref _progressBarValue, value);
        }
        #endregion

        #region ProgressBarVisibility
        private Visibility _progressBarVisibility = Visibility.Collapsed; // Visibility.Collapsed
        public Visibility ProgressBarVisibility
        {
            get => _progressBarVisibility;
            set => Set(ref _progressBarVisibility, value);
        }
        #endregion

        protected void ChangeProgressBarVisibility()
        {
            switch (ProgressBarVisibility)
            {
                case Visibility.Visible:
                    ProgressBarVisibility = Visibility.Collapsed; // Не отображайте элемент и не резервируйте для него место в макете.
                    break;
                case Visibility.Collapsed:
                    ProgressBarVisibility = Visibility.Visible; // Отображай элемент.
                    break;
                default:
                    ProgressBarVisibility = Visibility.Collapsed;
                    break;
            }
        }
    }
}
