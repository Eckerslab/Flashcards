namespace Flashcards.Infrastructure.Repositories;

public interface IBaseRepository<TEntity> where TEntity : class
{
    public Task Add(TEntity value);
    public Task Delete(TEntity value);
    public Task Update(TEntity value);
    public Task<TEntity> Get(int id);
}