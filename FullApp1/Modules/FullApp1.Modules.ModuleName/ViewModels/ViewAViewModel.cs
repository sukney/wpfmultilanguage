using FullApp1.Core.Mvvm;
using FullApp1.Services.Interfaces;
using Prism.Regions;

namespace FullApp1.Modules.ModuleName.ViewModels
{
    public class ViewAViewModel : RegionViewModelBase
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }


        private string _message1;
        public string Message1
        {
            get { return _message1; }
            set { SetProperty(ref _message1, value); }
        }

        public ViewAViewModel(IRegionManager regionManager, IMessageService messageService) :
            base(regionManager)
        {
            Message = messageService.GetMessage();
            Core.LocalizedLangExtension.ChangeLange += () =>
            {
                Message = Core.LocalizedLangExtension.GetString("TB_KEY_A");
                Message1 = Core.LocalizedLangExtension.GetString("TB_KEY_B");
            };
          //  Message = Core.LocalizedLangExtension.GetString("TB_KEY_A");
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            //do something
        }
    }
}
