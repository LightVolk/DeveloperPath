﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeveloperPath.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DeveloperPath.Application.Paths.Commands.UpdatePath
{
  public class UpdatePathCommandValidator : AbstractValidator<UpdatePathCommand>
  {
    private readonly IApplicationDbContext _context;
    public UpdatePathCommandValidator(IApplicationDbContext context)
    {
      _context = context;

      RuleFor(v => v.Title)
        .NotEmpty().WithMessage("Title is required.")
        .MaximumLength(100).WithMessage("Title must not exceed 100 characters.")
        .MustAsync(BeUniqueTitle).WithMessage("The specified path already exists.");

      RuleFor(v => v.Description)
        .NotEmpty().WithMessage("Description is required.")
        .MaximumLength(3000).WithMessage("Description must not exceed 3000 characters.");
    }

    public async Task<bool> BeUniqueTitle(UpdatePathCommand model, string title, CancellationToken cancellationToken)
    {
      return await _context.Paths
        .Where(p => p.Id != model.Id)
        .AllAsync(l => l.Title != title, cancellationToken);
    }
  }
}
