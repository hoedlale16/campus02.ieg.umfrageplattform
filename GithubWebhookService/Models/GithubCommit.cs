

namespace GithubWebhookService.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class GithubCommit
    {
        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("sha")]
        public string Sha { get; set; }

        [JsonProperty("node_id")]
        public string NodeId { get; set; }

        [JsonProperty("html_url")]
        public Uri HtmlUrl { get; set; }

        [JsonProperty("comments_url")]
        public Uri CommentsUrl { get; set; }

        [JsonProperty("commit")]
        public Commit Commit { get; set; }

        [JsonProperty("author")]
        public GithubCommitAuthor Author { get; set; }

        [JsonProperty("committer")]
        public GithubCommitAuthor Committer { get; set; }

        [JsonProperty("parents")]
        public Tree[] Parents { get; set; }

        [JsonProperty("stats")]
        public Stats Stats { get; set; }

        [JsonProperty("files")]
        public File[] Files { get; set; }
    }

    public partial class GithubCommitAuthor
    {
        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("node_id")]
        public string NodeId { get; set; }

        [JsonProperty("avatar_url")]
        public Uri AvatarUrl { get; set; }

        [JsonProperty("gravatar_id")]
        public string GravatarId { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("html_url")]
        public Uri HtmlUrl { get; set; }

        [JsonProperty("followers_url")]
        public Uri FollowersUrl { get; set; }

        [JsonProperty("following_url")]
        public string FollowingUrl { get; set; }

        [JsonProperty("gists_url")]
        public string GistsUrl { get; set; }

        [JsonProperty("starred_url")]
        public string StarredUrl { get; set; }

        [JsonProperty("subscriptions_url")]
        public Uri SubscriptionsUrl { get; set; }

        [JsonProperty("organizations_url")]
        public Uri OrganizationsUrl { get; set; }

        [JsonProperty("repos_url")]
        public Uri ReposUrl { get; set; }

        [JsonProperty("events_url")]
        public string EventsUrl { get; set; }

        [JsonProperty("received_events_url")]
        public Uri ReceivedEventsUrl { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("site_admin")]
        public bool SiteAdmin { get; set; }
    }

    public partial class Commit
    {
        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("author")]
        public CommitAuthor Author { get; set; }

        [JsonProperty("committer")]
        public CommitAuthor Committer { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("tree")]
        public Tree Tree { get; set; }

        [JsonProperty("comment_count")]
        public long CommentCount { get; set; }

        [JsonProperty("verification")]
        public Verification Verification { get; set; }
    }

    public partial class CommitAuthor
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("date")]
        public DateTimeOffset Date { get; set; }
    }

    public partial class Tree
    {
        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("sha")]
        public string Sha { get; set; }
    }

    public partial class Verification
    {
        [JsonProperty("verified")]
        public bool Verified { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("signature")]
        public object Signature { get; set; }

        [JsonProperty("payload")]
        public object Payload { get; set; }
    }

    public partial class File
    {
        [JsonProperty("filename")]
        public string Filename { get; set; }

        [JsonProperty("additions")]
        public long Additions { get; set; }

        [JsonProperty("deletions")]
        public long Deletions { get; set; }

        [JsonProperty("changes")]
        public long Changes { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("raw_url")]
        public Uri RawUrl { get; set; }

        [JsonProperty("blob_url")]
        public Uri BlobUrl { get; set; }

        [JsonProperty("patch")]
        public string Patch { get; set; }
    }

    public partial class Stats
    {
        [JsonProperty("additions")]
        public long Additions { get; set; }

        [JsonProperty("deletions")]
        public long Deletions { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }
    }

    public partial class GithubCommit
    {
        public static GithubCommit FromJson(string json) => JsonConvert.DeserializeObject<GithubCommit>(json, GithubWebhookService.Models.Converter.Settings);
    }

    public static class SerializeCommit
    {
        public static string ToJson(this GithubCommit self) => JsonConvert.SerializeObject(self, GithubWebhookService.Models.CommitConverter.Settings);
    }

    internal static class CommitConverter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
