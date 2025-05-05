using Microsoft.EntityFrameworkCore;
using Youth_Innovation_System.Core;
using Youth_Innovation_System.Core.Entities;
using Youth_Innovation_System.Core.Specifications;
using Youth_Innovation_System.Repository.Data;

namespace Youth_Innovation_System.Repository
{
    internal class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(T entity)
        => await _dbContext.Set<T>().AddAsync(entity);
        public async Task AddRangeAsync(IEnumerable<T> entities)
        => await _dbContext.Set<T>().AddRangeAsync(entities);
        public void Delete(T entity)
        => _dbContext.Set<T>().Remove(entity);
        public async Task<IReadOnlyList<T>> GetAllAsync()
        => await _dbContext.Set<T>().AsNoTracking().ToListAsync();

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        => await GetQuery(spec).AsNoTracking().ToListAsync();
        public async Task<int> CountAsyncWithSpec(ISpecification<T> spec)
        => await GetQuery(spec).CountAsync();
        public async Task<T?> GetAsync(int id)
        => await _dbContext.Set<T>().FindAsync(id);
        public Task<T?> GetWithSpecAsync(ISpecification<T> spec)
         => GetQuery(spec).FirstOrDefaultAsync();

        public void Update(T entity)
            => _dbContext.Set<T>().Update(entity);
        private IQueryable<T> GetQuery(ISpecification<T> spec)
          => SpecificationEvaluator<T>.BuildQuery(_dbContext.Set<T>(), spec);
    }
}
