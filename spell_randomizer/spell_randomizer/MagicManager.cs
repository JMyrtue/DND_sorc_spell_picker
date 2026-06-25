using System.Text.Json;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace spell_randomizer;

public class SpellCacheData
{
    public int Version { get; set; } = 10; // Final final final fix for metadata styling
    public List<Spell> Cantrips { get; set; } = new();
    public List<Spell> Spells { get; set; } = new();
    public int[] SpellLevelBounds { get; set; } = new int[9];
}

public class MagicManager
{
    private List<Spell> _cantrips;
    private List<Spell> _spells;
    private int[] _spellLevelBounds = new int[9];
    private const string BaseUrl = "https://dnd5e.wikidot.com";
    private const string CacheFilePath = "spell_cache.json";
    private const int CurrentCacheVersion = 10; // Final final final fix for metadata styling

    public MagicManager()
    {
        _cantrips = new List<Spell>();
        _spells = new List<Spell>();
        LoadSpells();
    }

    private void LoadSpells()
    {
        if (File.Exists(CacheFilePath))
        {
            try
            {
                string json = File.ReadAllText(CacheFilePath);
                var cacheData = JsonSerializer.Deserialize<SpellCacheData>(json);
                if (cacheData != null && cacheData.Version == CurrentCacheVersion && cacheData.Cantrips.Count > 0)
                {
                    _cantrips = cacheData.Cantrips;
                    _spells = cacheData.Spells;
                    _spellLevelBounds = cacheData.SpellLevelBounds;
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load cache: {ex.Message}. Fetching from web instead...");
            }
        }

        HtmlWeb web = new HtmlWeb();
        web.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
        HtmlDocument doc = web.Load(BaseUrl + "/spells:sorcerer");

        var tables = doc.DocumentNode.SelectNodes("//div[@id='page-content']//table");
        if (tables == null)
        {
            throw new Exception("Could not find spell tables on the wikidot page.");
        }

        int currentSpellCount = 0;
        for (int i = 0; i < tables.Count; i++)
        {
            var links = tables[i].SelectNodes(".//a");
            if (links != null)
            {
                foreach (var link in links)
                {
                    string spellName = link.InnerText.Trim();
                    
                    if (spellName.Contains("(UA)"))
                    {
                        continue;
                    }

                    string href = link.GetAttributeValue("href", "");
                    string absoluteUrl = href.StartsWith("http") ? href : $"{BaseUrl}{href}";

                    if (i == 0)
                    {
                        _cantrips.Add(new Spell(spellName, 0, absoluteUrl));
                    }
                    else
                    {
                        _spells.Add(new Spell(spellName, i, absoluteUrl));
                        currentSpellCount++;
                    }
                }
            }
            if (i > 0 && i <= 9)
            {
                _spellLevelBounds[i - 1] = currentSpellCount;
            }
        }
        
        SaveCache();
    }

    private void SaveCache()
    {
        try
        {
            var cacheData = new SpellCacheData
            {
                Version = CurrentCacheVersion,
                Cantrips = _cantrips,
                Spells = _spells,
                SpellLevelBounds = _spellLevelBounds
            };

            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(cacheData, options);
            File.WriteAllText(CacheFilePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save spell cache: {ex.Message}");
        }
    }

    public Spell? GetUniqueSpell(int index)
    {
        if (index < 1 || index > _spells.Count)
        {
            throw new Exception("Invalid index provided for spell retrieval");
        }
        return _spells[index - 1];
    }

    public Spell? GetUniqueCantrip(int index)
    {
        if (index < 1 || index > _cantrips.Count)
        {
            throw new Exception("Invalid index provided for cantrip retrieval");
        }
        return _cantrips[index - 1];
    }

    public async Task<string> GetSpellDescription(Spell spell)
    {
        if (!string.IsNullOrEmpty(spell.Description))
        {
            return spell.Description;
        }

        if (string.IsNullOrEmpty(spell.Url))
        {
            return "Description not available for this spell.";
        }

        HtmlWeb web = new HtmlWeb();
        web.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
        HtmlDocument detailDoc = await web.LoadFromWebAsync(spell.Url);

        var contentNode = detailDoc.DocumentNode.SelectSingleNode("//div[@id='page-content']");
        string description = "Description not found.";
        if (contentNode != null)
        {
            string html = contentNode.InnerHtml;

            // This pattern finds the specific keywords and wraps them in the metadata span.
            string pattern = @"(<strong>Source:</strong>|<em>.*?cantrip.*?</em>|<strong>Casting Time:</strong>|<strong>Range:</strong>|<strong>Components:</strong>|<strong>Duration:</strong>)";
            
            html = Regex.Replace(html, pattern, match => $"<span class='spell-metadata'>{match.Value}</span>", RegexOptions.Singleline);

            description = html;
        }
        
        spell.Description = description;
        SaveCache();

        return description;
    }

    public int GetSpellsIndexUpperBound(int maxSpellLevel)
    {
        if (maxSpellLevel < 1) return 0;
        if (maxSpellLevel > 9) maxSpellLevel = 9;
        return _spellLevelBounds[maxSpellLevel - 1];
    }

    public int GetTotalCantripsCount()
    {
        return _cantrips.Count;
    }
}
