using DefenceDB.EL.Extensions;
using DefenceDB.EL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace DefenceDB.WebUI.Services;

public class ProductFormMapper : IProductFormMapper
{
    public DefenseProduct? MapFromFormForCreate(IFormCollection form)
    {
        string? modelTypeFullName = form["ModelTypeFullName"];
        if (string.IsNullOrEmpty(modelTypeFullName)) return null;

        Type? modelType = Type.GetType(modelTypeFullName);
        
        // Fallback arama
        if (modelType == null)
        {
            modelType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.FullName == modelTypeFullName);
        }
        
        if (modelType == null) return null;

        var instance = Activator.CreateInstance(modelType) as DefenseProduct;
        if (instance == null) return null;

        MapBaseProperties(form, instance);
        MapSpecificProperties(form, instance, modelType);

        return instance;
    }

    public void MapFromFormForEdit(IFormCollection form, DefenseProduct existingInstance)
    {
        MapBaseProperties(form, existingInstance);
        MapSpecificProperties(form, existingInstance, existingInstance.GetType());
    }

    private void MapBaseProperties(IFormCollection form, DefenseProduct instance)
    {
        instance.Name = form["Name"].ToString();
        instance.Country = form["Country"].ToString();
        instance.Manufacturer = form["Manufacturer"].ToString();
        instance.Status = form["Status"].ToString();
        instance.Description = form["Description"].ToString();
        
        instance.IsActive = form["IsActive"].ToString().Contains("true", StringComparison.OrdinalIgnoreCase);
        instance.IsShowcase = form["IsShowcase"].ToString().Contains("true", StringComparison.OrdinalIgnoreCase);
        
        if (int.TryParse(form["CategoryId"], out int catId))
        {
            instance.CategoryId = catId;
        }
        
        instance.VideoUrl = form["VideoUrl"].ToString();
        instance.Slug = instance.Name?.ToSlug() ?? "";
    }

    private void MapSpecificProperties(IFormCollection form, DefenseProduct instance, Type modelType)
    {
        var baseProperties = typeof(DefenseProduct).GetProperties().Select(p => p.Name).ToList();
        var specificProperties = modelType.GetProperties().Where(p => !baseProperties.Contains(p.Name)).ToList();

        foreach (var prop in specificProperties)
        {
            if (form.TryGetValue(prop.Name, out var values))
            {
                var valueStr = values.FirstOrDefault();
                if (string.IsNullOrEmpty(valueStr) && prop.PropertyType != typeof(bool)) continue;

                object? convertedValue = null;
                
                try 
                {
                    if (prop.PropertyType == typeof(bool))
                    {
                        convertedValue = values.ToString().Contains("true", StringComparison.OrdinalIgnoreCase);
                    }
                    else if (prop.PropertyType.IsEnum)
                    {
                        convertedValue = Enum.Parse(prop.PropertyType, valueStr!);
                    }
                    else if (Nullable.GetUnderlyingType(prop.PropertyType)?.IsEnum == true)
                    {
                        convertedValue = Enum.Parse(Nullable.GetUnderlyingType(prop.PropertyType)!, valueStr!);
                    }
                    else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
                    {
                        convertedValue = int.Parse(valueStr!);
                    }
                    else if (prop.PropertyType == typeof(double) || prop.PropertyType == typeof(double?))
                    {
                        convertedValue = double.Parse(valueStr!, System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        convertedValue = valueStr;
                    }
                    
                    prop.SetValue(instance, convertedValue);
                }
                catch { /* Dönüşüm hatasını yoksay */ }
            }
        }
    }
}
