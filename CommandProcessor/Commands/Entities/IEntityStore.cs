using System.Threading.Tasks;
using CommandProcessor.Entities;

namespace CommandProcessor.Commands.Entities
{
    public interface IEntityStore
    {
        public Task Save<T>(T entity) where T : Entity;
    }
}