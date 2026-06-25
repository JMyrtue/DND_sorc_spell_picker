using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace spell_randomizer.Controllers
{
    [ApiController]
    [Route("api")]
    public class CharacterController : ControllerBase
    {
        // Now _player can be null initially.
        private static Player? _player = null;

        [HttpGet("characters")]
        public IActionResult GetCharacters()
        {
            var savedCharactersPath = Path.Combine(Directory.GetCurrentDirectory(), "saved_characters");

            if (!Directory.Exists(savedCharactersPath))
            {
                return Ok(new string[0]);
            }
            
            var files = Directory.GetFiles(savedCharactersPath, "*.json");
            var characterNames = files.Select(f => Path.GetFileNameWithoutExtension(f)).ToArray();
            return Ok(characterNames);
        }

        [HttpPost("character/new")]
        public IActionResult CreateCharacter([FromBody] CreateCharacterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Name))
            {
                return BadRequest("Character name is required.");
            }

            _player = new Player(request.Name);
            _player.Save(); // Save immediately so it appears in lists
            return Ok();
        }

        [HttpPost("character/load")]
        public IActionResult LoadCharacter([FromBody] LoadCharacterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Name))
            {
                return BadRequest("Character name is required.");
            }

            try
            {
                _player = Player.Load(request.Name);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to load character: {ex.Message}");
            }
        }
        
        [HttpPost("character/save")]
        public IActionResult SaveCharacter()
        {
            if (_player == null)
            {
                return BadRequest("No active character to save.");
            }
            _player.Save();
            return Ok();
        }


        [HttpGet("character")]
        public IActionResult GetCharacter()
        {
            if (_player == null)
            {
                // Return 404 if no character is currently loaded, prompting frontend to go to selection screen
                return NotFound("Character not loaded.");
            }
            return Ok(new
            {
                _player.Name,
                _player.Level,
                RemainingSorceryPoints = _player.MaxSorcPoints - _player.SorcPointsUsed,
                _player.WildMagicCounter, // Added WildMagicCounter
                Cantrips = _player.Cantrips.OrderBy(c => c.Name).ToList(),
                Spells = _player.Spells.OrderBy(s => s.Level).ThenBy(s => s.Name).ToList(),
                SpellSlots = GetSpellSlotsInfo()
            });
        }

        [HttpGet("spell/{spellName}")]
        public async Task<IActionResult> GetSpellDescription(string spellName)
        {
            if (_player == null) return BadRequest("No active character.");
            var spell = _player.GetSpell(spellName);
            if (spell == null)
            {
                return NotFound($"Spell '{spellName}' not found.");
            }

            var description = await new MagicManager().GetSpellDescription(spell);
            return Ok(new { spell.Name, Description = description });
        }

        [HttpPost("cast")]
        public IActionResult CastSpell([FromBody] CastSpellRequest request)
        {
            if (_player == null) return BadRequest("No active character.");
            if (request == null || request.Level <= 0)
            {
                return BadRequest("Invalid spell level.");
            }

            var result = _player.CastSpell(request.Level, request.WildMagicSurgeOccurred);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok();
        }

        [HttpPost("metamagic")]
        public IActionResult UseMetamagic([FromBody] MetamagicRequest request)
        {
            if (_player == null) return BadRequest("No active character.");
            if (request == null || request.Cost <= 0)
            {
                return BadRequest("Invalid sorcery point cost.");
            }

            var result = _player.MetaMagic(request.Cost);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok();
        }

        [HttpPost("rest")]
        public IActionResult Rest()
        {
            if (_player == null) return BadRequest("No active character.");
            _player.LongRest();
            return Ok();
        }

        [HttpPost("levelup")]
        public IActionResult LevelUp()
        {
            if (_player == null) return BadRequest("No active character.");
            _player.LevelUp();
            return Ok();
        }

        [HttpPost("leveldown")]
        public IActionResult LevelDown()
        {
            if (_player == null) return BadRequest("No active character.");
            _player.LevelDown();
            return Ok();
        }

        [HttpPost("convert/pointstoslots")]
        public IActionResult ConvertPointsToSlots([FromBody] ConversionRequest request)
        {
            if (_player == null) return BadRequest("No active character.");
            if (request == null || request.Level <= 0 || request.Level > 5)
            {
                return BadRequest("Invalid spell level for conversion. Can only convert to slots 1-5.");
            }

            var result = _player.FlexCast_PointsToSlots(request.Level);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok();
        }

        [HttpPost("convert/slotstopoints")]
        public IActionResult ConvertSlotsToPoints([FromBody] ConversionRequest request)
        {
            if (_player == null) return BadRequest("No active character.");
            if (request == null || request.Level <= 0 || request.Level > 5)
            {
                return BadRequest("Invalid spell level for conversion. Can only convert from slots 1-5.");
            }

            var result = _player.FlexCast_SlotsToPoints(request.Level);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok();
        }

        private object GetSpellSlotsInfo()
        {
            var spellSlots = new System.Collections.Generic.Dictionary<string, object>();
            Debug.Assert(_player != null, nameof(_player) + " != null");
            for (int i = 0; i < _player.SpellSlotsTotal.Length; i++)
            {
                if (_player.SpellSlotsTotal[i] > 0)
                {
                    spellSlots[$"Level {i + 1}"] = new 
                    { 
                        available = _player.SpellSlotsTotal[i] - _player.SpellSlotsUsed[i],
                        total = _player.SpellSlotsTotal[i]
                    };
                }
            }
            return spellSlots;
        }
    }
    
    public class CreateCharacterRequest
    {
        public CreateCharacterRequest(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }

    public class LoadCharacterRequest
    {
        public LoadCharacterRequest(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }

    public class CastSpellRequest
    {
        public int Level { get; set; }
        public bool WildMagicSurgeOccurred { get; set; } // Added this flag
    }

    public class MetamagicRequest
    {
        public int Cost { get; set; }
    }

    public class ConversionRequest
    {
        public int Level { get; set; }
    }
}
