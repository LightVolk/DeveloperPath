﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeveloperPath.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DeveloperPath.Application.Modules.Commands.CreateModule
{
  public class CreateModuleCommandValidator : AbstractValidator<CreateModuleCommand>
  {
    private readonly IApplicationDbContext _context;

    public CreateModuleCommandValidator(IApplicationDbContext context)
    {
      _context = context;

      RuleFor(v => v.PathId)
          .NotEmpty().WithMessage("Path Id is required.");

      RuleFor(v => v.Title)
        .NotEmpty().WithMessage("Title is required.")
        .MaximumLength(100).WithMessage("Title must not exceed 100 characters.")
        .MustAsync(BeUniqueTitle).WithMessage("The specified module already exists in this path.");

      RuleFor(v => v.Description)
        .NotEmpty().WithMessage("Description is required.")
        .MaximumLength(3000).WithMessage("Description must not exceed 3000 characters.");
    }

    public async Task<bool> BeUniqueTitle(CreateModuleCommand model, string title, CancellationToken cancellationToken)
    {
      //Verify that all modules in path have titles different than title
      var pathModules = await _context.Paths
        .Where(p => p.Id == model.PathId)
        .Include(p => p.Modules)
        .SelectMany(m => m.Modules).ToListAsync(cancellationToken);
        
      return pathModules.All(m => m.Title != title);
    }
  }
}
