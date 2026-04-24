namespace GoodHamburger.Application.Repositories;

public interface IUnitOfWork
{
    Task<bool> SaveAsync();
}