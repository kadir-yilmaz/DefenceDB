using Microsoft.Extensions.Configuration;
using DefenceDB.BLL.Abstract;

namespace DefenceDB.BLL.Concrete;

/// <summary>
/// appsettings.json → "Features" bölümünden feature toggle'ları okur.
/// Shared hosting'de false, local Docker'da true olacak şekilde ayarlanır.
/// </summary>
public class FeatureManager : IFeatureManager
{
    public bool UseElasticsearch { get; }

    public FeatureManager(IConfiguration configuration)
    {
        UseElasticsearch = configuration.GetSection("Features:UseElasticsearch").Get<bool>();
    }
}
