namespace Api.Repository.Interfaces;

public interface IUnitOfWork
{
    public Task SaveChangesAsync();
}