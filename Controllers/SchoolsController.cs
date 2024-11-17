using mapa_back.Exceptions;
using mapa_back.Models;
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
                long schoolsCount = await schoolsService.GetSchoolsCount();
                if(schoolsCount > 0)
                {
                    return Ok(schoolsCount);
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
            catch(SchoolServiceException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while trying to get schools count from database. Try again later");
            }
        }

        [HttpDelete("DeleteSchool")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> DeleteSingleSchool(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid school id");
                }
                await schoolsService.DeleteSingleSchool(id);
                return Ok($"school with id: {id} deleted");           
            }
            catch(DatabaseException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while trying to delete single school. Try again later");
            }

        }

        [HttpDelete("DeleteManySchools")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> DeleteManySchools(List<int> ids)
        {
            try
            {
                if (ids.Count <= 0)
                {
                    return BadRequest("ID list empty");
                }
                var invalidIds = ids.Where(id => id <= 0).ToList();
                if (invalidIds.Any())
                {
                    return BadRequest($"The following IDs are invalid: {string.Join(", ", invalidIds)}");
                }
                await schoolsService.DeleteManySchools(ids);
                return Ok($"school with given ids deleted");
            }
            catch (DatabaseException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while trying to delete single school. Try again later");
            }
        }
        [HttpGet("GetChanges")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ChangedSchool>>> GetChanges(int size, int page)
        {
            try
            {
                List<ChangedSchool> changedSchools = await schoolsService.GetChangedSchoolsList(size, page);
                if (changedSchools.Any())
                {
                    return Ok(changedSchools);
                }
                return NoContent();
            }
            catch(ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (SchoolServiceException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            catch(DatabaseException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while trying to get changes. Try again later");
            }
        }

        [HttpGet("GetChangesCount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<long>> GetChangesCount()
        {
            try
            {
                long changesCount = await schoolsService.GetChangesCount();
                if (changesCount > 0)
                {
                    return Ok(changesCount);
                }
                return NotFound();
            }
            catch (DatabaseException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while trying to get changes count from database. Try again later");
            }
        }


    }
}
