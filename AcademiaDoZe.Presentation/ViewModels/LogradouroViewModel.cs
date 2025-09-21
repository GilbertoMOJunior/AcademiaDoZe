using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;

namespace AcademiaDoZe.Presentation.AppMaui.ViewModels
{
    public partial class LogradouroViewModel : BaseViewModel
    {
        // inicialmente só estamos incluindo o comando de cancelar

        [RelayCommand]
        private async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
