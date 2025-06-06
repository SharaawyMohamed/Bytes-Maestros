using BytesMaestros.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BytesMaestros.Domain.Repositories
{
	public interface IGenericRepository<TKey, TEntity> where TEntity : BaseEntity<TKey>
	{
		Task AddAsync(TEntity entity);
		Task AddReangeAsync(IEnumerable<TEntity> entities);
		Task<TEntity?> GetByIdAsync(TKey id);
		Task<IReadOnlyList<TEntity>?> GetAllAsync(int pageSize, int pageIndex);
		Task<IReadOnlyList<TEntity>?> GetWithPrdicateAsync(Expression<Func<TEntity, bool>> pridecate, int pageSize, int pageIndex);
		
	}
}
