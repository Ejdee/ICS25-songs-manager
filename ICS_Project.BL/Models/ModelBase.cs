using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ICS_Project.BL.Models;

public record ModelBase : INotifyPropertyChanged, IModel
{
    public Guid Id { get; set; }
    public event PropertyChangedEventHandler? PropertyChanged;
    
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}