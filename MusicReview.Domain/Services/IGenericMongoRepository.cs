using System.Linq.Expressions;
using MusicReview.Domain.Models.Base;

namespace MusicReview.Domain.Services;

public interface IGenericMongoRepository<TEntity> where TEntity : MongoEntity
{
    Task AddAsync(TEntity entity);
    Task<TEntity> GetByIdAsync(string id);
    Task<List<TEntity>> GetAllAsync();
    Task<List<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> predicate);
    Task<bool> RemoveAsync(string id);
    Task UpdateAsync(TEntity entity);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
}