using System.Collections.ObjectModel;

namespace ICS_Project.BL.Mappers;

public static class IEnumerableExtension
{
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> values)
        => new(values);
}