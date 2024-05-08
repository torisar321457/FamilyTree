using FamilyTree.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FamilyTree.Service
{
    public interface IHalflingService
    {
        public Task<List<Halfling>> GetAll();
        public Task Add(string name, string family, DateTime date, int parentId, CancellationToken cancellationToken);
        public Task<Halfling?> GetGrandpa(int id, CancellationToken cancellationToken);
        public Task<Halfling?> GetGreatGrandpaAsync(int id, CancellationToken cancellationToken);
        public Task<IEnumerable<Halfling>> GetGreatGrandson(int id);
    }
}
