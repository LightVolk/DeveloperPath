﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeveloperPath.Application.Paths.Queries.GetPaths
{
  public class GetPathDetailsQuery : IRequest<PathViewModel>
  {
    public int Id { get; set; }
  }

  public class GetPathDetailsQueryHandler : IRequestHandler<GetPathDetailsQuery, PathViewModel>
  {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPathDetailsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<PathViewModel> Handle(GetPathDetailsQuery request, CancellationToken cancellationToken)
    {
      var result = await _context.Paths
        .Include(p => p.Modules)
        .Where(c => c.Id == request.Id)
        .FirstOrDefaultAsync(cancellationToken);

      if (result == null)
      {
        throw new NotFoundException(nameof(Path), request.Id);
      }

      //TODO: is there another way to map single item?
      return _mapper.Map<PathViewModel>(result);
    }
  }
}
