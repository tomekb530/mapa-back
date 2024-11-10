using mapa_back.Exceptions;
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
        public SchoolsController(DatabaseContext context, IRSPOApiService apiService)
        {
            rSPOApiService = apiService;
        }

        [HttpGet("GetDataFromRSPO")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetDataFromRSPO()
        {
            try
            {
                bool result = await rSPOApiService.SyncDataFromRSPOApi();
                if(result)
                {
                    return StatusCode(StatusCodes.Status200OK);
                }
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch(RSPOToDatabaseException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while trying to sync data from RSPO API");
            }
        }
    }
}
