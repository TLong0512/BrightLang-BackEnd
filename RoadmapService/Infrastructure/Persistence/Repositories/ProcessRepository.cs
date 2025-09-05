using Application.Abstraction.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories;

public class ProcessRepository : IProcessRepository
{
    protected readonly AppDbContext _context;
    public ProcessRepository(AppDbContext context)
    {
        _context = context;
    }



    public async Task<IEnumerable<Process>> FindAsync(Expression<Func<Process, bool>> predicate)
    {
        List<Process> matchEntities = await _context.Processes.Where(predicate).ToListAsync();
        return matchEntities;
    }

    public async Task<IEnumerable<Process>> GetAllAsync()
    {
        List<Process> entities = await _context.Processes.ToListAsync();
        return entities;
    }

    public async Task<Process?> GetByIdAsync(Guid userRoadmapId, Guid roadmapElementId)
    {
        Process? entity = await _context.Processes.FindAsync(userRoadmapId, roadmapElementId);
        return entity;
    }




    public async Task AddAsync(Process entity)
    {
        await _context.Processes.AddAsync(entity);
        return;
    }

    public Task UpdateAsync(Process entity)
    {
        //_context.Entry(entity).State = EntityState.Modified;
        _context.Processes.Update(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid userRoadmapId, Guid roadmapElementId)
    {
        var entity = await _context.Processes.FindAsync(userRoadmapId, roadmapElementId);
        if (entity == null)
        {
            return;
        }
        _context.Processes.Remove(entity);
        return;
    } 
}
