using System.ComponentModel;

namespace McDotNet.ViewModels
{
    public interface IViewModel<out T> : INotifyPropertyChanged
    {
        T Model { get; }
    }
}