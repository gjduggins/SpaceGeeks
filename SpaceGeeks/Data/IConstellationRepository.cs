using SpaceGeeks.Models;

namespace SpaceGeeks.Data
{
    public interface IConstellationRepository
    {
        IEnumerable<Constellation> GetAll();
        Constellation GetById(int id);
        void Add(Constellation constellation);
        void Update(Constellation constellation);
        void Delete(int id);
    }
}