using DefenceDB.EL.Extensions;
using DefenceDB.EL.Models;
using DefenceDB.BLL.Abstract;

namespace DefenceDB.WebUI.Services;

public class ProductFormMapper : IProductFormMapper
{
    private readonly ICategoryQueryService _categoryQueryService;

    public ProductFormMapper(ICategoryQueryService categoryQueryService)
    {
        _categoryQueryService = categoryQueryService;
    }

    public DefenseProduct? MapFromFormForCreate(IFormCollection form)
    {
        var instance = new DefenseProduct();
        MapBaseProperties(form, instance);
        MapSpecsFromForm(form, instance);
        return instance;
    }

    public void MapFromFormForEdit(IFormCollection form, DefenseProduct existingInstance)
    {
        MapBaseProperties(form, existingInstance);
        MapSpecsFromForm(form, existingInstance);
    }

    private void MapBaseProperties(IFormCollection form, DefenseProduct instance)
    {
        instance.Name = form["Name"].ToString();
        instance.Country = form["Country"].ToString();
        instance.Manufacturer = form["Manufacturer"].ToString();
        instance.Status = form["Status"].ToString();
        instance.Description = form["Description"].ToString();
        instance.NatoReportingName = form["NatoReportingName"].ToString();
        
        instance.IsActive = form["IsActive"].ToString().Contains("true", StringComparison.OrdinalIgnoreCase);
        instance.IsShowcase = form["IsShowcase"].ToString().Contains("true", StringComparison.OrdinalIgnoreCase);
        
        if (int.TryParse(form["CategoryId"], out int catId))
        {
            instance.CategoryId = catId;
        }

        if (int.TryParse(form["YearIntroduced"], out int year))
        {
            instance.YearIntroduced = year;
        }
        
        instance.VideoUrl = form["VideoUrl"].ToString();
        instance.Slug = instance.Name?.ToSlug() ?? "";
    }

    private void MapSpecsFromForm(IFormCollection form, DefenseProduct instance)
    {
        // Specs_ prefix ile gelen form alanlarını Specs dictionary'sine yaz
        var specs = new Dictionary<string, string>();
        
        foreach (var key in form.Keys)
        {
            if (key.StartsWith("Specs_", StringComparison.OrdinalIgnoreCase))
            {
                var specName = key.Substring("Specs_".Length);
                var value = form[key].ToString();
                
                if (!string.IsNullOrWhiteSpace(value))
                {
                    specs[specName] = value;
                }
            }
        }

        // Checkbox'lar için: Specs_Bool_ prefix
        foreach (var key in form.Keys)
        {
            if (key.StartsWith("Specs_Bool_", StringComparison.OrdinalIgnoreCase))
            {
                var specName = key.Substring("Specs_Bool_".Length);
                var value = form[key].ToString().Contains("true", StringComparison.OrdinalIgnoreCase);
                specs[specName] = value.ToString();
            }
        }

        instance.Specs = specs;
    }
}
