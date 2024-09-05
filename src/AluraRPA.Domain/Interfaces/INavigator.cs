namespace AluraRPA.Domain.Interfaces;
public interface INavigator
{
    Task<NavigationResult> StartNavigationAsync(Credential credential);
}