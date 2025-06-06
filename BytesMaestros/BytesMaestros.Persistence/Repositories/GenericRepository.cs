using BytesMaestros.Domain.Entities;
using BytesMaestros.Domain.Repositories;
using BytesMaestros.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace BytesMaestros.Persistence.Repositories
{
	public class GenericRepository<TKey, TEntity> : IGenericRepository<TKey, TEntity> where TEntity : BaseEntity<TKey>
	{
		private readonly BytesMaestorsDbContext _context;
		private readonly DbSet<TEntity> _entities;
		public GenericRepository(BytesMaestorsDbContext context)
		{
			_context = context;
			_entities = context.Set<TEntity>();
		}
		public async Task AddAsync(TEntity entity)
		=> await _entities.AddAsync(entity);

		public async Task AddReangeAsync(IEnumerable<TEntity> entities)
		   => _entities.AddRange(entities);

		public async Task<IReadOnlyList<TEntity>> GetAllAsync(int pageSize, int pageIndex)
		=> await _entities.AsNoTracking().Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

		public async Task<TEntity?> GetByIdAsync(TKey id)
		=> await _entities.FindAsync(id)!;

		public async Task<IReadOnlyList<TEntity>?> GetWithPrdicateAsync(Expression<Func<TEntity, bool>> pridecate, int pageSize, int pageIndex)
		=> await _entities.AsNoTracking().Where(pridecate).Skip((pageIndex-1)*pageSize).Take(pageSize).ToListAsync();
		
	}

}
