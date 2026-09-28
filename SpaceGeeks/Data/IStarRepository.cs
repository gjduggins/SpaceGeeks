using SpaceGeeks.Models;

namespace SpaceGeeks.Data
{
    public interface IStarRepository
    {
        IEnumerable<Star> GetAll();
        Star GetById(int id);
        void Add(Star star);
        void Update(Star star);
        void Delete(int id);
    }
}