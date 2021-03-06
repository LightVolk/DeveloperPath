﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Application.Common.Models;
using DeveloperPath.WebApi.Paging;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeveloperPath.Application.Modules.Queries.GetModules
{
    public class GetModuleListQueryPaging : IRequest<PagedResponse<IEnumerable<ModuleDto>>>
    {
        public int PathId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

    }


    public class GetModulesQueryPagingHandler : IRequestHandler<GetModuleListQueryPaging, PagedResponse<IEnumerable<ModuleDto>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetModulesQueryPagingHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<ModuleDto>>> Handle(GetModuleListQueryPaging request, CancellationToken cancellationToken)
        {
            //TODO: check if requested module is in requested path (???)
            var path = await _context.Paths.FindAsync(new object[] { request.PathId }, cancellationToken);
            if (path == null)
                throw new NotFoundException(nameof(Path), request.PathId);
            IEnumerable<ModuleDto> modules = null;
            PagedResponse<IEnumerable<ModuleDto>> pagedResponse = null;
            if (request.PageNumber > 0 || request.PageSize > 0)
            {
                 modules = await _context.Paths
                    .Where(p => p.Id == request.PathId)
                    .SelectMany(p => p.Modules)
                    .Include(m => m.Paths)
                    .Include(m => m.Prerequisites)
                    .ProjectTo<ModuleDto>(_mapper.ConfigurationProvider).Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);
                 pagedResponse = new PagedResponse<IEnumerable<ModuleDto>>(modules, request.PageNumber, request.PageSize);
                return pagedResponse;
            }

            // TODO: Order modules (from PathModules.Order)
            modules = await _context.Paths
                .Where(p => p.Id == request.PathId)
                .SelectMany(p => p.Modules)
                .Include(m => m.Paths)
                .Include(m => m.Prerequisites)
                .ProjectTo<ModuleDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            var modulesResult = new PagedResponse<IEnumerable<ModuleDto>>(modules, request.PageNumber, request.PageSize);
            return modulesResult;

        }
    }

}
