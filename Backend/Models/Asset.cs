using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Asset
    {
        [Key]
        public Guid AssetId { get; set; }

        [MaxLength(64)]
        public string AssetName { get; set; } = string.Empty;

        public int LevelRequire { get; set; }

        public ICollection<PlayerAsset> PlayerAssets { get; set; } = new List<PlayerAsset>();
    }
}
