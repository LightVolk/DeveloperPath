﻿using System.Collections.Generic;
using DeveloperPath.Application.Common.Mappings;
using DeveloperPath.Domain.Entities;
using DeveloperPath.Domain.Enums;

namespace DeveloperPath.Application.Common.Models
{
  public class ThemeDto : IMapFrom<Theme>
  {
    /// <summary>
    /// Theme ID
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Theme Title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Theme short summary
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Id of module that theme is in
    /// </summary>
    public int ModuleId { get; set; }

    /// <summary>
    /// Section that theme is in (can be null)
    /// </summary>
    public SectionDto Section { get; init; }

    /// <summary>
    /// Complexity level
    /// </summary>
    public ComplexityLevel Complexity { get; set; }

    /// <summary>
    /// Necessity level
    /// </summary>
    public NecessityLevel Necessity { get; set; }

    /// <summary>
    /// Position of theme in module (0-based). 
    /// Multiple themes can have same order number (meaning they can be studied simultaneously)
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// List of tags related to theme
    /// </summary>
    public ICollection<string> Tags { get; set; }
  }
}