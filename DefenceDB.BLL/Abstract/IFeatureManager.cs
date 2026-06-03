namespace DefenceDB.BLL.Abstract;

/// <summary>
/// Feature toggle yönetimi — Elasticsearch açık mı kapalı mı kontrol eder.
/// </summary>
public interface IFeatureManager
{
    bool UseElasticsearch { get; }
}
