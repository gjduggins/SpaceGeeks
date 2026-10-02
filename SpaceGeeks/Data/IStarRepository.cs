using System.Collections.Generic;
using SpaceGeeks.Models;

namespace SpaceGeeks.Data
{
    public interface IStarRepository
    {
        IReadOnlyList<Star> GetAllOrderedByDistance();
    }
}