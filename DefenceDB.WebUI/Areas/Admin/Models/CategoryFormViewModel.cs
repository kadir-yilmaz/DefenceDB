using System.Collections.Generic;
using DefenceDB.EL.Models;

namespace DefenceDB.WebUI.Areas.Admin.Models;

public class CategoryFormViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? ParentCategoryId { get; set; }
    public bool IsShowcase { get; set; }

    public List<CategoryAttributeViewModel> Attributes { get; set; } = new();
}

public class CategoryAttributeViewModel
{
    public int? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public AttributeType Type { get; set; }
    public string? OptionsJson { get; set; }
}
