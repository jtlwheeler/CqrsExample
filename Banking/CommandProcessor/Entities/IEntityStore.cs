using System;
using System.Threading.Tasks;

namespace Banking.CommandProcessor.Entities
{
    public interface IEntityStore
    {
        public Task Save<T>(T entity) where T : AggregateRoot;
        public Task<T> Load<T>(Guid entityId) where T : AggregateRoot, new();
    }
}
