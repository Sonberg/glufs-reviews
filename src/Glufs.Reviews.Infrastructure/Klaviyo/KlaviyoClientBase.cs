using Glufs.Reviews.Infrastructure.Policies;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Glufs.Reviews.Infrastructure.Klaviyo;

public abstract class KlaviyoClientBase
{
    protected record Profile(string Email, string? Phone);

    protected record Event(Profile Profile, string Name, object Properties);

    private record ProfileCreated(ProfileCreatedData Data);

    private record ProfileCreatedData(string Id);

    private record ProfileCreationFailed(List<ProfileError> Errors);

    private record ProfileError
    {
        public record ProfileErrorMeta(string DuplicateProfileId);

        public required string Code { get; set; }

        public bool ProfileAlreadyExists => Code == "duplicate_profile";

        public required ProfileErrorMeta Meta { get; set; }
    }

    private readonly HttpClient _httpClient;

    public KlaviyoClientBase(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    protected async Task<string> UpsertProfile(Profile profile, object properties, CancellationToken cancellationToken = default)
    {
        var url = $"/api/profiles";
        var content = JsonContent.Create(new
        {
            data = new
            {
                type = "profile",
                attributes = new
                {
                    email = profile.Email,
                    phone_number = profile.Phone,
                    properties
                }
            }
        });

        var response = await _httpClient.PostAsync(url, content, cancellationToken);
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance
        };

        if (response.IsSuccessStatusCode)
        {
            var data = JsonSerializer.Deserialize<ProfileCreated>(json, jsonOptions);
            var profileId = data!.Data.Id!;

            return profileId;
        }

        if (response.StatusCode == HttpStatusCode.Conflict)
        {
            var data = JsonSerializer.Deserialize<ProfileCreationFailed>(json, jsonOptions);
            var profileId = data!.Errors.Single(x => x.ProfileAlreadyExists).Meta.DuplicateProfileId;

            await UpdateMember(profileId, properties, cancellationToken);

            return profileId;
        }

        throw new Exception("Failed to upsert profile");
    }

    protected async Task UpdateMember(string id, object properties, CancellationToken cancellationToken = default)
    {
        var url = $"/api/profiles/{id}";
        var content = JsonContent.Create(new
        {
            data = new
            {
                id,
                type = "profile",
                attributes = new
                {
                    properties
                }
            }
        });
        var response = await _httpClient.PatchAsync(url, content, cancellationToken);

        response.EnsureSuccessStatusCode();
    }

    protected async Task TrackEvent(string profileId, string name, object properties, CancellationToken cancellationToken = default)
    {
        var url = $"/api/events";
        var content = JsonContent.Create(new
        {
            data = new
            {
                type = "event",
                attributes = new
                {
                    metric = new
                    {
                        data = new
                        {
                            type = "metric",
                            attributes = new
                            {
                                name
                            }
                        }
                    },
                    properties,
                    profile = new
                    {
                        data = new
                        {
                            type = "profile",
                            id = profileId
                        }
                    }
                }
            }
        });

        var response = await _httpClient.PostAsync(url, content, cancellationToken);

        response.EnsureSuccessStatusCode();
    }

    protected async Task AddToList(string listId, string profileId, CancellationToken cancellationToken = default)
    {
        var url = $"/api/lists/{listId}/relationships/profiles/";
        var content = JsonContent.Create(new
        {
            data = new object[]
            {
                new
                {
                    type = "profile",
                    id = profileId
                }
            }
        });

        var response = await _httpClient.PostAsync(url, content, cancellationToken);

        response.EnsureSuccessStatusCode();
    }
}

