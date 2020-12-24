using System;
using System.Threading.Tasks;

namespace Banking.CommandProcessor.Entities
{
    public interface IEntityStore
    {
        public Task Save<T>(T entity) where T : Entity;
        public Task<T> Load<T>(Guid entityId) where T : Entity, new();
    }
}
