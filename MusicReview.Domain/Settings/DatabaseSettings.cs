namespace MusicReview.Domain.Settings;

public class DatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string MusicCollectionName { get; set; } = null!;

    public string UserCollectionName { get; set; } = null!;
}