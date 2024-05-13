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
            var parentPathFromPatriarch = _halflingContext.Halflings.
                FirstOrDefault(x => x.Id == parentId)?.PathFromPatriarch;
            _halflingContext.Halflings.Add(
                new Halfling(name, family, date, parentPathFromPatriarch is not null ? HierarchyId.Parse($"{parentPathFromPatriarch}{parentId}/") : HierarchyId.GetRoot()));
            await _halflingContext.SaveChangesAsync(cancellationToken);
        }
        public async Task<Halfling?> GetGrandpa(int id, CancellationToken cancellationToken)
        {
            var idGrandpa = _halflingContext.Halflings.
                FirstOrDefault(x => x.Id == id)?.PathFromPatriarch?.
                GetAncestor(1)?.ToString().Trim('/').Split('/').
                LastOrDefault();
            return string.IsNullOrWhiteSpace(idGrandpa)
                ? throw new ArgumentException()
                : await _halflingContext.Halflings.FirstOrDefaultAsync(x => x.Id == Convert.ToInt32(idGrandpa), cancellationToken: cancellationToken);
        }
        public async Task<Halfling?> GetGreatGrandpaAsync(int id, CancellationToken cancellationToken)
        {
            var idGreatGrandpa = _halflingContext.Halflings.
                FirstOrDefault(x => x.Id == id)?.PathFromPatriarch?.
                GetAncestor(2)?.ToString().Trim('/').Split('/').
                LastOrDefault(); ;
            return string.IsNullOrWhiteSpace(idGreatGrandpa)
                ? throw new ArgumentException()
                : await _halflingContext.Halflings.
                FirstOrDefaultAsync(x => x.Id == Convert.ToInt32(idGreatGrandpa), cancellationToken: cancellationToken);
        }
        public async Task<IEnumerable<Halfling>> GetGreatGrandson(int id)
        {
            var hierarchyid = _halflingContext.Halflings.FirstOrDefault(x => x.Id == id)?.PathFromPatriarch;
            return hierarchyid is null
                 ? throw new ArgumentException()
                 : await _halflingContext.Halflings.Where(x => x.PathFromPatriarch.IsDescendantOf(hierarchyid) &&
                 x.PathFromPatriarch.GetLevel() == hierarchyid.GetLevel() + 3).ToListAsync();
        }
    }
}
