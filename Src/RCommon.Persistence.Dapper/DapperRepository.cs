﻿using Microsoft.Extensions.Logging;
using RCommon.DataServices;
using RCommon.DataServices.Sql;
using RCommon.DataServices.Transactions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Reflection;
using System.ComponentModel;
using System.Data.Common;
using RCommon.BusinessEntities;
using System.Threading;
using MediatR;
using Microsoft.Extensions.Options;
using Dommel;
using RCommon.Collections;

namespace RCommon.Persistence.Dapper
{
    public class DapperRepository<TEntity> : SqlRepositoryBase<TEntity>
        where TEntity : class, IBusinessEntity
    {
        private readonly IMediator _mediator;

        public DapperRepository(IDataStoreProvider dataStoreProvider, ILoggerFactory logger, IUnitOfWorkManager unitOfWorkManager, 
            IChangeTracker changeTracker, IMediator mediator, IOptions<DefaultDataStoreOptions> defaultDataStoreOptions)
            : base(dataStoreProvider, logger, unitOfWorkManager, changeTracker, defaultDataStoreOptions)
        {
            _mediator = mediator;
        }



        public override async Task AddAsync(TEntity entity, CancellationToken token = default)
        {

            await using (var db = this.DbConnection)
            {
                try
                {
                    if (db.State == ConnectionState.Closed)
                    {
                        await db.OpenAsync();
                    }

                    entity.AddLocalEvent(new EntityCreatedEvent<TEntity>(entity));
                    this.ChangeTracker.AddEntity(entity);
                    await db.InsertAsync(entity, cancellationToken: token);
                    this.SaveChanges();

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (db.State == ConnectionState.Open)
                    {
                        await db.CloseAsync();
                    }
                }

            }
        }


        public override async Task DeleteAsync(TEntity entity, CancellationToken token = default)
        {
            await using (var db = this.DbConnection)
            {
                try
                {
                    if (db.State == ConnectionState.Closed)
                    {
                        await db.OpenAsync();
                    }

                    entity.AddLocalEvent(new EntityDeletedEvent<TEntity>(entity));
                    this.ChangeTracker.AddEntity(entity);
                    await db.DeleteAsync(entity, cancellationToken: token);
                    this.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (db.State == ConnectionState.Open)
                    {
                        await db.CloseAsync();
                    }
                }

            }
        }



        public override async Task UpdateAsync(TEntity entity, CancellationToken token = default)
        {

            await using (var db = this.DbConnection)
            {
                try
                {
                    if (db.State == ConnectionState.Closed)
                    {
                        await db.OpenAsync();
                    }

                    entity.AddLocalEvent(new EntityUpdatedEvent<TEntity>(entity));
                    this.ChangeTracker.AddEntity(entity);
                    await db.UpdateAsync(entity, cancellationToken: token);
                    this.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (db.State == ConnectionState.Open)
                    {
                        await db.CloseAsync();
                    }
                }
            }
        }

        public override async Task<ICollection<TEntity>> FindAsync(ISpecification<TEntity> specification, CancellationToken token = default)
        {
            return await this.FindAsync(specification.Predicate, token);
        }

        public override async Task<ICollection<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression, CancellationToken token = default)
        {
            await using (var db = this.DbConnection)
            {
                try
                {
                    if (db.State == ConnectionState.Closed)
                    {
                        await db.OpenAsync();
                    }

                    var results = await db.SelectAsync(expression, cancellationToken: token);
                    return results.ToList();
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (db.State == ConnectionState.Open)
                    {
                        await db.CloseAsync();
                    }
                }
            }
        }

        public override async Task<TEntity> FindAsync(object primaryKey, CancellationToken token = default)
        {
            await using (var db = this.DbConnection)
            {
                try
                {
                    if (db.State == ConnectionState.Closed)
                    {
                        await db.OpenAsync();
                    }

                    var result = await db.GetAsync<TEntity>(primaryKey, cancellationToken: token);
                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (db.State == ConnectionState.Open)
                    {
                        await db.CloseAsync();
                    }
                }
            }
        }

        public override async Task<long> GetCountAsync(ISpecification<TEntity> selectSpec, CancellationToken token = default)
        {
            await using (var db = this.DbConnection)
            {
                try
                {
                    if (db.State == ConnectionState.Closed)
                    {
                        await db.OpenAsync();
                    }

                    var results = await db.CountAsync(selectSpec.Predicate);
                    return results;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (db.State == ConnectionState.Open)
                    {
                        await db.CloseAsync();
                    }
                }
            }
        }

        public override async Task<long> GetCountAsync(Expression<Func<TEntity, bool>> expression, CancellationToken token = default)
        {
            await using (var db = this.DbConnection)
            {
                try
                {
                    if (db.State == ConnectionState.Closed)
                    {
                        await db.OpenAsync();
                    }

                    var results = await db.CountAsync(expression);
                    return results;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (db.State == ConnectionState.Open)
                    {
                        await db.CloseAsync();
                    }
                }
            }
        }

        public override async Task<TEntity> FindSingleOrDefaultAsync(Expression<Func<TEntity, bool>> expression, CancellationToken token = default)
        {
            await using (var db = this.DbConnection)
            {
                try
                {
                    if (db.State == ConnectionState.Closed)
                    {
                        await db.OpenAsync();
                    }
;
                    var result = await db.FirstOrDefaultAsync(expression, cancellationToken: token);
                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (db.State == ConnectionState.Open)
                    {
                        await db.CloseAsync();
                    }
                }
            }
        }

        public override async Task<TEntity> FindSingleOrDefaultAsync(ISpecification<TEntity> specification, CancellationToken token = default)
        {
            return await FindSingleOrDefaultAsync(specification, token);
        }

        public override async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken token = default)
        {
            await using (var db = this.DbConnection)
            {
                try
                {
                    if (db.State == ConnectionState.Closed)
                    {
                        await db.OpenAsync();
                    }

                    var results = await db.AnyAsync(expression);
                    return results;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (db.State == ConnectionState.Open)
                    {
                        await db.CloseAsync();
                    }
                }
            }
        }

        public override async Task<bool> AnyAsync(ISpecification<TEntity> specification, CancellationToken token = default)
        {
            return await this.AnyAsync(specification.Predicate, token);
        }

        protected void SaveChanges()
        {
            // We are not actually persisting anything since that is handled by the client
            // , but we need to publish events.
            this.ChangeTracker.TrackedEntities.PublishLocalEvents(_mediator);
        }
    }
}
