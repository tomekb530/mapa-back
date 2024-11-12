using mapa_back.Exceptions;
using mapa_back.Models.DTO;
using mapa_back.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace mapa_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolsController : ControllerBase
    {
        private readonly IRSPOApiService rSPOApiService;
        private readonly ISchoolsService schoolsService;
        public SchoolsController(DatabaseContext context, IRSPOApiService apiService, ISchoolsService schoolsService)
        {
            rSPOApiService = apiService;
            this.schoolsService = schoolsService;
        }

        [HttpGet("GetDataFromRSPO")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult> GetDataFromRSPO()
        {
            try
            {
                await rSPOApiService.SyncDataFromRSPOApi();
                return StatusCode(StatusCodes.Status200OK);
            }
            catch(RSPOToDatabaseException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while trying to sync data from RSPO API");
            }
        }

        [HttpGet("GetSchoolsCount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<long>> GetSchoolsCount()
        {
            try
            {
                long productCount = await schoolsService.GetSchoolsCount();
                if(productCount > 0)
                {
                    return Ok(productCount);
                }
                return NotFound();
            }
            catch (DatabaseException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while trying to get schools count from database. Try again later");
            }
        }

        [HttpGet("GetSchoolPage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<List<SchoolDTO>>> GetSchoolPage(int size, int pageNumber)
        {
            try
            {
                List<SchoolDTO> schoolsPage = await schoolsService.GetSchoolsPage(size, pageNumber);
                if (schoolsPage.Count > 0)
                {
                    return Ok(schoolsPage);
                }
                return NotFound();
            }
            catch(ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while trying to get schools count from database. Try again later");
            }
        }
    }
}
