using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ShoppingList.List.Infrastructure.Data.Config;

public static class JsonConverters
{
    public static readonly ValueComparer<List<Dictionary<string, object>>> OpsComparer =
        new(
            (a, b) =>
                JsonSerializer.Serialize(a, (JsonSerializerOptions?)null)
                == JsonSerializer.Serialize(b, (JsonSerializerOptions?)null),
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null).GetHashCode(),
            v =>
                JsonSerializer.Deserialize<List<Dictionary<string, object>>>(
                    JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    (JsonSerializerOptions?)null
                )!
        );
}

