using System;
using GitWatchdog.Presentation.ViewModel;

namespace Gitwatchdog.MacOS.Services
{
    public static class ViewModelProvider
    {
        private static MainViewModel _mainViewModel;

        public static MainViewModel ProvideMainViewModel()
        {
            if(_mainViewModel == null)
            {
                _mainViewModel = new MainViewModel()
                {
                    PlatformProvider = new PlatformProvider()
                };
            }

            return _mainViewModel;
        }

    }
}
