using CommandProcessor.Entities;

namespace CommandProcessor.Commands.Entities
{
    public interface IEntityStore
    {
        public void Save<T>(T entity) where T : Entity;
    }
}