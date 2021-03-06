﻿using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperPath.Application.Modules.Commands.DeleteModule
{
  public record DeleteModuleCommand : IRequest
  {
    public int Id { get; init; }
    public int PathId { get; init; }
  }

  public class DeleteModuleCommandHandler : IRequestHandler<DeleteModuleCommand>
  {
    private readonly IApplicationDbContext _context;

    public DeleteModuleCommandHandler(IApplicationDbContext context)
    {
      _context = context;
    }

    public async Task<Unit> Handle(DeleteModuleCommand request, CancellationToken cancellationToken)
    {
      var entity = await _context.Modules
        .Include(m => m.Paths)
        .Where(m => m.Id == request.Id)
        .FirstOrDefaultAsync(cancellationToken);

      if (entity == null)
        throw new NotFoundException(nameof(Module), request.Id);

      if (entity.Paths.Any(p => p.Id == request.PathId))
      {
        // remove Module from this path
        entity.Paths.Remove(entity.Paths.Single(p => p.Id == request.PathId));

        // If module is not tied to any path, delete the module
        if (entity.Paths.Count == 0)
          _context.Modules.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
      }
      else
        throw new NotFoundException(nameof(Module), request.Id);

      return Unit.Value;
    }
  }
}
