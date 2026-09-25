using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Player
    {
        [Key]
        public Guid PlayerId { get; set; }

        [MaxLength(64)]
        public string PlayerName { get; set; } = string.Empty;

        [MaxLength(128)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(10)]
        public string Age { get; set; } = string.Empty;

        public int Level { get; set; }

        [MaxLength(64)]
        public string Email { get; set; } = string.Empty;

        public ICollection<PlayerAsset> PlayerAssets { get; set; } = new List<PlayerAsset>();
    }
}
