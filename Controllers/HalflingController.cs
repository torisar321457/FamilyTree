using FamilyTree.Context;
using FamilyTree.Model;
using FamilyTree.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace FamilyTree.Controllers
{
    public class HalflingController(IHalflingService halflingService) : ControllerBase
    {
        private readonly IHalflingService _halflingService = halflingService;

        [Route("GetAll")]
        [HttpGet]
        public async Task<List<Halfling>> GetAll() => await _halflingService.GetAll();
        [Route("Add")]
        [HttpPost]
        public async Task Add(string name, string family, DateTime date, int parentId, CancellationToken cancellationToken)
        {
            await _halflingService.Add(name, family, date, parentId, cancellationToken);
        }
        [Route("GetGrandpa")]
        [HttpGet]
        public async Task<Halfling?> GetGrandpa(int id, CancellationToken cancellationToken) => await _halflingService.GetGrandpa(id, cancellationToken);
        [Route("GetGreatGrandpa")]
        [HttpGet]
        public async Task<Halfling?> GetGreatGrandpaAsync(int id, CancellationToken cancellationToken) => await _halflingService.GetGreatGrandpaAsync(id, cancellationToken);
        [Route("GetGreatGrandson")]
        [HttpGet]
        public async Task<IEnumerable<Halfling>> GetGreatGrandson(int id) => await _halflingService.GetGreatGrandson(id);
    }
}
