namespace GoodHamburger.Application.Interfaces;

public interface IUnitOfWork
{
    Task<bool> SaveAsync();
}