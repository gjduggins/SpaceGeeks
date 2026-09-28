using SpaceGeeks.Models;
using System.Collections.Concurrent;

namespace SpaceGeeks.Data
{
    public class InMemoryConstellationRepository : IConstellationRepository
    {
        private readonly ConcurrentDictionary<int, Constellation> _constellations;
        private int _nextId = 1;

        public InMemoryConstellationRepository()
        {
            _constellations = new ConcurrentDictionary<int, Constellation>();
            SeedData();
        }

        public IEnumerable<Constellation> GetAll()
        {
            return _constellations.Values;
        }

        public Constellation GetById(int id)
        {
            _constellations.TryGetValue(id, out var constellation);
            return constellation;
        }

        public void Add(Constellation constellation)
        {
            constellation.Id = _nextId++;
            _constellations.TryAdd(constellation.Id, constellation);
        }

        public void Update(Constellation constellation)
        {
            _constellations.TryUpdate(constellation.Id, constellation, constellation);
        }

        public void Delete(int id)
        {
            _constellations.TryRemove(id, out _);
        }

        private void SeedData()
        {
            // Add some sample constellations
            Add(new Constellation { Name = "Orion", Description = "The Hunter", Hemisphere = Hemisphere.Both, Stars = new List<string> { "Betelgeuse", "Rigel", "Bellatrix" } });
            Add(new Constellation { Name = "Ursa Major", Description = "The Great Bear", Hemisphere = Hemisphere.Northern, Stars = new List<string> { "Dubhe", "Merak", "Phecda" } });
            Add(new Constellation { Name = "Crux", Description = "The Southern Cross", Hemisphere = Hemisphere.Southern, Stars = new List<string> { "Acrux", "Mimosa", "Gacrux" } });
        }
    }
}