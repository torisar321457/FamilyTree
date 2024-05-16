using FamilyTree.Context;
using FamilyTree.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FamilyTree.Service
{
    public class HalflingService(HalflingContext halflingContext) : IHalflingService
    {
        private readonly HalflingContext _halflingContext = halflingContext;
        public async Task<List<Halfling>> GetAll()
        {
            return await _halflingContext.Halflings.ToListAsync();
        }
        public async Task Add(string name, string family, DateTime date, int parentId, CancellationToken cancellationToken)
        {
            var parent = _halflingContext.Halflings.
                FirstOrDefault(x => x.Id == parentId);
            _halflingContext.Add(string.IsNullOrWhiteSpace(parent?.PathFromPatriarch) ? new Halfling(name, family, date, "/", 0) :
                new Halfling(name, family, date, $"{parent.PathFromPatriarch}{parentId}/", parent.Level + 1));
            await _halflingContext.SaveChangesAsync(cancellationToken);
        }
        public async Task<Halfling?> GetGrandpa(int id, CancellationToken cancellationToken)
        { 
            var idGrandpa = _halflingContext.Halflings.
                FirstOrDefault(x => x.Id == id)?.PathFromPatriarch?.
                Trim('/').
                Split('/').
                SkipLast(1).
                TakeLast(1).
                FirstOrDefault();
            return string.IsNullOrWhiteSpace(idGrandpa)
                ? throw new ArgumentException()
                : await _halflingContext.Halflings.FirstOrDefaultAsync(x => x.Id == Convert.ToInt32(idGrandpa), cancellationToken: cancellationToken);
        }


        public async Task<Halfling?> GetGreatGrandpaAsync(int id, CancellationToken cancellationToken)
        {
            var idGreatGrandpa = _halflingContext.Halflings.
                FirstOrDefault(x => x.Id == id)?.
                PathFromPatriarch?.Trim('/').
                Split('/').
                SkipLast(2).
                TakeLast(1).
                FirstOrDefault();
            return string.IsNullOrWhiteSpace(idGreatGrandpa)
                ? throw new ArgumentException()
                : await _halflingContext.Halflings.
                FirstOrDefaultAsync(x => x.Id == Convert.ToInt32(idGreatGrandpa), cancellationToken: cancellationToken);

        }
        public async Task<IEnumerable<Halfling>> GetGreatGrandson(int id)
        {
            var hierarchyid = _halflingContext.Halflings.FirstOrDefault(x => x.Id == id)?.Level;
            return hierarchyid is null
                 ? throw new ArgumentException()
                 : await _halflingContext.Halflings.Where(x => x.PathFromPatriarch.Contains(id.ToString()) &&
                 x.Level == hierarchyid + 3).ToListAsync(); // 3 - разница между текущим поколением и его прадедом
        }
    }
}
