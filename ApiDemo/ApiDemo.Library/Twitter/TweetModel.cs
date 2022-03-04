using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApiDemo.Library.Twitter;
public class TweetModel
{
    [JsonPropertyName("data")]
    public Data Data { get; set; }
    [JsonPropertyName("includes")]
    public Includes Includes { get; set; }
    public override string ToString()
    {
        return $"[{Data.CreatedAt}] {Data.Text}";
    }
}

public class Data
{
    [JsonPropertyName("author_id")]
    public string AuthorId { get; set; }
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
    [JsonPropertyName("entities")]
    public Entities Entities { get; set; }
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("text")]
    public string Text { get; set; }
}

public class Entities
{
    [JsonPropertyName("urls")]
    public Url[] Urls { get; set; }
}

public class Url
{
    [JsonPropertyName("start")]
    public int Start { get; set; }
    [JsonPropertyName("end")]
    public int End { get; set; }
    [JsonPropertyName("url")]
    public string Link { get; set; }
    [JsonPropertyName("expanded_url")]
    public string ExpandedUrl { get; set; }
    [JsonPropertyName("display_url")]
    public string DisplayUrl { get; set; }
    [JsonPropertyName("images")]
    public Image[] Images { get; set; }
    [JsonPropertyName("status")]
    public int Status { get; set; }
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("description")] 
    public string Description { get; set; }
    [JsonPropertyName("unwound_url")] 
    public string UnwoundUrl { get; set; }
}

public class Image
{
    [JsonPropertyName("url")]
    public string Url { get; set; }
    [JsonPropertyName("width")]
    public int Width { get; set; }
    [JsonPropertyName("height")]
    public int Height { get; set; }
}

public class Includes
{
    [JsonPropertyName("users")]
    public User[] Users { get; set; }
    [JsonPropertyName("tweets")]
    public Tweet[] Tweets { get; set; }
}

public class User
{
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
    [JsonPropertyName("description")]
    public string Description { get; set; }
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("profile_image_url")]
    public string ProfileImageUrl { get; set; }
    [JsonPropertyName("username")]
    public string Username { get; set; }
    [JsonPropertyName("verified")]
    public bool Verified { get; set; }
}

public class Tweet
{

    [JsonPropertyName("author_id")]
    public string AuthorId { get; set; }
    [JsonPropertyName("conversation_id")] 
    public string ConversationId { get; set; }
    [JsonPropertyName("created_at")] 
    public DateTime CreatedAt { get; set; }
    [JsonPropertyName("id")] 
    public string Id { get; set; }
    [JsonPropertyName("lang")] 
    public string Lang { get; set; }
    [JsonPropertyName("possibly_sensitive")] 
    public bool PossiblySensitive { get; set; }
    [JsonPropertyName("source")] 
    public string Source { get; set; }
    [JsonPropertyName("text")] 
    public string Text { get; set; }
}