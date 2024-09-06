using AluraRPA.Domain.Interfaces;
namespace AluraRPA.Domain.Interfaces;
public interface INavigator
{
    Task<ResultProcess> NavigationAlura(string url, string searchWord);
   
}