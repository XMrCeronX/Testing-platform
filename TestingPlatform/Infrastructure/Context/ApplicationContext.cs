using System.Windows.Controls;
using TestingPlatform.ViewModels.Base;

namespace TestingPlatform.Infrastructure.Context
{
    internal class ApplicationContext
    {
        public Page CurrentPage { get; set; }
        public ViewModel CurrentViewModel { get; set; }


        public ApplicationContext(Page currentPage, ViewModel currentViewModel)
        {
            CurrentPage = currentPage;
            CurrentViewModel = currentViewModel;
        }
    }
}
