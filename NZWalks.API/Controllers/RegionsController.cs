using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Data;

namespace NZWalks.API.Controllers
{

    // https://localhost:porta/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext; // Dependency injection of the DbContext

        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        
        // GET ALL REGIONS
        // GET: api/Regions
        [HttpGet] 
        public IActionResult GetAll() // Pega todos os registros da tabela Regions do banco de dados
        {

            // Get data from the database - Domain model
            var regionsDomain = dbContext.Regions.ToList();

            // Map domain model to DTOs (Data Transfer Objects)
            var regionsDto = new List<RegionDto>();
            foreach (var regionDomain in regionsDomain)
            {
                regionsDto.Add(new RegionDto()
                {
                    Id = regionDomain.Id,
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    ImageUrl = regionDomain.ImageUrl
                });
            }

            // Return DTOs to the client

            return Ok(regionsDomain);
        }




        // GET SINGLE REGION BY ID
        // GET: api/Regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id) // Pega um registro da tabela Regions do banco de dados pelo ID
        {
            // var region = dbContext.Regions.Find(id); // Find the region by ID using the DbContext

            var regionDomain = dbContext.Regions.FirstOrDefault(x =>x.Id == id); // Find the region by ID using LINQ

            if (regionDomain == null)
            {
                return NotFound();
            }

            // Map region domain model to DTO
            var regionDto = new RegionDto()
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                ImageUrl = regionDomain.ImageUrl
            };

            // Return the region DTO to the client
            return Ok(regionDomain);
        }
        



        // Create a new region
        // POST: api/Regions
        [HttpPost]
        public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // Map the incoming DTO to a domain model
            var regionDomainModel = new Region()
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                ImageUrl = addRegionRequestDto.ImageUrl
            };

            // Save the new region to the database
            dbContext.Regions.Add(regionDomainModel);
            dbContext.SaveChanges();

            // Map the domain model to a DTO to return to the client
            var regionDto = new RegionDto()
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                ImageUrl = regionDomainModel.ImageUrl
            };

            return CreatedAtAction(nameof(GetById), new {id = regionDto.Id}, regionDto); // Return a 201 Created response with the location of the new resource 
        }
        



        // Update an existing region
        // PUT: api/Regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            // Check if the region exists in the database
            var regionDomainModel = dbContext.Regions.FirstOrDefault(x => x.Id == id); // Find the region by ID using LINQ
            
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Map the incoming DTO to the domain model
            regionDomainModel.Code = updateRegionRequestDto.Code;
            regionDomainModel.Name = updateRegionRequestDto.Name;
            regionDomainModel.ImageUrl = updateRegionRequestDto.ImageUrl; 

            dbContext.SaveChanges(); // Save the changes to the database

            // Convert the updated domain model to a DTO to return to the client
            var regionDto = new RegionDto()
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                ImageUrl = regionDomainModel.ImageUrl
            };

            return Ok(regionDto); // Return a 200 OK response with the updated region DTO
        }
        


        // Delete a region
        // DELETE: api/Regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            var regionDomainModel = dbContext.Regions.FirstOrDefault(x => x.Id == id); // Find the region by ID using LINQ

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Delete the region from the database
            dbContext.Regions.Remove(regionDomainModel);
            dbContext.SaveChanges(); // Save the changes to the database

            // return the deleted region DTO to the client
            // map the deleted domain model to a DTO
            var regionDto = new RegionDto()
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                ImageUrl = regionDomainModel.ImageUrl
            };

            return Ok(regionDto); // return a 200 OK response with the deleted region DTO
        }
    }
} 