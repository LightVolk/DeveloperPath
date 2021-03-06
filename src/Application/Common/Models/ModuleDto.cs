﻿using System.Collections.Generic;
using DeveloperPath.Application.Common.Mappings;
using DeveloperPath.Domain.Entities;
using DeveloperPath.Domain.Enums;

namespace DeveloperPath.Application.Common.Models
{
  public class ModuleDto : IMapFrom<Module>
  {
    /// <summary>
    /// Path ID
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Path name
    /// </summary>
    public string Title { get; init; }

    /// <summary>
    /// Path short summary
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// Paths module attached to
    /// </summary>
    public ICollection<PathTitle> Paths { get; init; }

    /// <summary>
    /// Necessity level
    /// </summary>
    public NecessityLevel Necessity { get; init; }

    /// <summary>
    /// Modules required to know before studying this module
    /// </summary>
    public ICollection<ModuleTitle> Prerequisites { get; init; }

    /// <summary>
    /// List of tags related to module
    /// </summary>
    public ICollection<string> Tags { get; set; }
  }
}
