using FamilyTree.Context;
using FamilyTree.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace FamilyTree.Controllers
{
    public class HalflingController(HalflingContext familyContext) : ControllerBase
    {
        private readonly HalflingContext _familyContext = familyContext;

        [Route("GetAll")]
        [HttpGet]
        public async Task<List<Halfling>> GetAll()
        {
            return await _familyContext.Halflings.ToListAsync();
        }
        [Route("Add")]
        [HttpPost]
        public async Task Add(string name, string family, DateTime date, int parentId, CancellationToken cancellationToken)
        {
            var parentPathFromPatriarch = _familyContext.Halflings.FirstOrDefault(x => x.Id == parentId)?.PathFromPatriarch;
            _familyContext.Halflings.Add(
                new Halfling(name, family, date, parentPathFromPatriarch is not null ? HierarchyId.Parse($"{parentPathFromPatriarch}{parentId}/") : HierarchyId.GetRoot()));
            await _familyContext.SaveChangesAsync(cancellationToken);
        }
        [Route("GetGrandpa")]
        [HttpGet]
        public async Task<Halfling?> GetGrandpa(int id, CancellationToken cancellationToken)
        {
            var idGrandpa = _familyContext.Halflings.FirstOrDefault(x => x.Id == id)?.PathFromPatriarch?.GetAncestor(1)?.ToString().Trim('/');
            return string.IsNullOrWhiteSpace(idGrandpa)
                ? throw new ArgumentException()
                : await _familyContext.Halflings.FirstOrDefaultAsync(x => x.Id == Convert.ToInt32(idGrandpa), cancellationToken: cancellationToken);
        }
        [Route("GetGreatGrandpa")]
        [HttpGet]
        public async Task<Halfling?> GetGreatGrandpaAsync(int id, CancellationToken cancellationToken)
        {
            var idGreatGrandpa = _familyContext.Halflings.FirstOrDefault(x => x.Id == id)?.PathFromPatriarch?.GetAncestor(2)?.ToString().Trim('/');
            return string.IsNullOrWhiteSpace(idGreatGrandpa)
                ? throw new ArgumentException()
                : await _familyContext.Halflings.FirstOrDefaultAsync(x => x.Id == Convert.ToInt32(idGreatGrandpa), cancellationToken: cancellationToken);
        }
        [Route("GetGreatGrandson")]
        [HttpGet]
        public async Task<IEnumerable<Halfling>> GetGreatGrandson(int id)
        {
            var hierarchyid = _familyContext.Halflings.FirstOrDefault(x => x.Id == id)?.PathFromPatriarch;
            return await _familyContext.Halflings.Where(x => x.PathFromPatriarch.IsDescendantOf(hierarchyid) && x.PathFromPatriarch.GetLevel() == 3).ToListAsync();
        }
    }
}
