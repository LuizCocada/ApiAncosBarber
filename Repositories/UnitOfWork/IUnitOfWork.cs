namespace AncosBarber.Repositories.UnitOfWork;

public interface IUnitOfWork
{
    Task Commit();
}