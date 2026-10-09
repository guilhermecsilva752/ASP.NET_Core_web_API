using System;



namespace NZWalks.API.Models.DTO;

public class RegionDto
{
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? ImageUrl { get; set; } // Nullable property to allow for regions without an image
}
