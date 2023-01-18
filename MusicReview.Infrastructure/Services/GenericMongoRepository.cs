using System.Linq.Expressions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MusicReview.Domain.Models.Base;
using MusicReview.Domain.Services;
using MusicReview.Domain.Settings;

namespace MusicReview.Infrastructure.Services.ModelServices;

public class GenericMongoRepository<TEntity> : IGenericMongoRepository<TEntity> where TEntity : MongoEntity
{
    private readonly IMongoCollection<TEntity> _collection;

    public GenericMongoRepository(IOptions<DatabaseSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

        _collection = mongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name);
    }

    public async Task AddAsync(TEntity entity)
    {
        await _collection.InsertOneAsync(entity);
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _collection.Find(predicate).AnyAsync();
    }

    public async Task<List<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _collection.Find(predicate).ToListAsync();
    }

    public async Task<List<TEntity>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public async Task<TEntity> GetByIdAsync(string id)
    {
        return await _collection.Find(entity => entity.Id == id).FirstOrDefaultAsync();
    }

    public async Task<bool> RemoveAsync(string id)
    {
        var filter = await _collection.DeleteOneAsync(entity => entity.Id == id);
        return filter.DeletedCount > 0;
    }
    


    public async Task UpdateAsync(TEntity entity)
    {
        await _collection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
    }
}