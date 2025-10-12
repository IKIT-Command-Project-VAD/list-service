using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ListService.Infrastructure.Persistence.Converters;

public static class JsonConverters
{
    public static readonly ValueComparer<List<Dictionary<string, object>>> OpsComparer =
        new ValueComparer<List<Dictionary<string, object>>>(
            (a, b) => JsonSerializer.Serialize(a, (JsonSerializerOptions?)null) == JsonSerializer.Serialize(b, (JsonSerializerOptions?)null),
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null).GetHashCode(),
            v => JsonSerializer.Deserialize<List<Dictionary<string, object>>>(
                JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                (JsonSerializerOptions?)null
            )!
        );

    public static readonly ValueComparer<Dictionary<string, decimal>> DictDecComparer =
        new ValueComparer<Dictionary<string, decimal>>(
            (a, b) => JsonSerializer.Serialize(a, (JsonSerializerOptions?)null) == JsonSerializer.Serialize(b, (JsonSerializerOptions?)null),
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null).GetHashCode(),
            v => JsonSerializer.Deserialize<Dictionary<string, decimal>>(
                JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                (JsonSerializerOptions?)null
            )!
        );
}