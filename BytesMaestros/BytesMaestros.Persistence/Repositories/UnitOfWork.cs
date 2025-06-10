using BytesMaestros.Domain.Entities;
using BytesMaestros.Domain.Repositories;
using BytesMaestros.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BytesMaestros.Persistence.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly BytesMaestorsDbContext _context;
		private readonly Dictionary<string, object> _repositories = new();

		public UnitOfWork(BytesMaestorsDbContext context)
		{
			_context = context;
		}

		public IGenericRepository<TKey, TEntity> Repository<TKey, TEntity>() where TEntity : BaseEntity<TKey>
		{
			var typeName = typeof(TEntity).Name;

			if (!_repositories.ContainsKey(typeName))
			{
				var repositoryInstance = new GenericRepository<TKey, TEntity>(_context);
				_repositories[typeName] = repositoryInstance;
			}

			return (IGenericRepository<TKey, TEntity>)_repositories[typeName];
		}

		public async ValueTask DisposeAsync()
		{
			await _context.DisposeAsync();
		}

		public async Task<int> SaveChangesAsync()
		{
			return await _context.SaveChangesAsync();
		}
	}
}
