using SpaceGeeks.Models;
using System.Collections.Concurrent;

namespace SpaceGeeks.Data
{
    public class InMemoryStarRepository : IStarRepository
    {
        private readonly ConcurrentDictionary<int, Star> _stars;
        private int _nextId = 1;

        public InMemoryStarRepository()
        {
            _stars = new ConcurrentDictionary<int, Star>();
            SeedData();
        }

        public IEnumerable<Star> GetAll()
        {
            return _stars.Values;
        }

        public Star GetById(int id)
        {
            _stars.TryGetValue(id, out var star);
            return star;
        }

        public void Add(Star star)
        {
            star.Id = _nextId++;
            _stars.TryAdd(star.Id, star);
        }

        public void Update(Star star)
        {
            _stars.TryUpdate(star.Id, star, star);
        }

        public void Delete(int id)
        {
            _stars.TryRemove(id, out _);
        }

        private void SeedData()
        {
            // Add some sample stars
            Add(new Star { Name = "Sirius", Description = "Brightest star in the night sky", ApparentMagnitude = -1.46, DistanceInLightYears = 8.6, Hemisphere = Hemisphere.Both, Constellation = "Canis Major" });
            Add(new Star { Name = "Canopus", Description = "Second brightest star in the night sky", ApparentMagnitude = -0.74, DistanceInLightYears = 310, Hemisphere = Hemisphere.Southern, Constellation = "Carina" });
            Add(new Star { Name = "Arcturus", Description = "Brightest star in the northern celestial hemisphere", ApparentMagnitude = -0.05, DistanceInLightYears = 36.7, Hemisphere = Hemisphere.Northern, Constellation = "Boötes" });
        }
    }
}