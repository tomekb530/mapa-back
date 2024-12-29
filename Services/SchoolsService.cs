using mapa_back.Exceptions;
using mapa_back.Mappers;
using mapa_back.Models;
using mapa_back.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace mapa_back.Services
{
    public class SchoolsService : ISchoolsService
    {
        private readonly DatabaseContext _dbContext;

        public SchoolsService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<long> GetSchoolsCount()
        {
            try
            {
                long schoolsNumber = await _dbContext.Schools.CountAsync();
                return schoolsNumber;
            }
            catch (Exception)
            {
                throw new DatabaseException("Unexpected error occurred while trying to count elements in database");
            }
        }
        private bool ValidatePageParametres(int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return false;
            }
            return true;
        }
        public async Task<List<SchoolDTO>> GetSchoolsPage(int size, int pageNumber)
        {
            if (!ValidatePageParametres(pageNumber, size))
            {
                throw new ArgumentException("Parametres not valid");
            }
            try
            {
                List<School> schoolsPage = await _dbContext.Schools.Where(p => true).Skip((pageNumber -1) * size).Take(size).ToListAsync();
                List<SchoolDTO> schoolsDTO = new List<SchoolDTO>();
                foreach(var element in schoolsPage)
                {
                    schoolsDTO.Add(SchoolMapper.MapToDTO(element));
                }
				return schoolsPage.Select(SchoolMapper.MapToDTO).ToList();
			}
			catch (SchoolServiceException)
            {
                throw;
            }
            catch(Exception)
            {
                throw new Exception("Unexpected error occurred while trying to get school page from database");
            }
        }
        public async Task DeleteSingleSchool(int id)
        {
            try
            {
                School school = await _dbContext.Schools.FirstOrDefaultAsync(p => p.Id == id) ?? throw new DatabaseException($"Couldnt find school with given Id: {id}");
                _dbContext.Schools.Remove(school);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new DatabaseException($"Unexpected eror occurred while trying to delete school with given Id: {id} from database");
            }
        }
        public async Task DeleteManySchools(List<int> ids)
        {
            try
            {
                List<School> schools = await _dbContext.Schools.Where(school => ids.Contains(school.Id)).ToListAsync();
                if (!schools.Any())
                {
                    throw new DatabaseException("No schools found with the provided IDs.");
                }
                _dbContext.Schools.RemoveRange(schools);
                await _dbContext.SaveChangesAsync();
            }
            catch(Exception)
            {
                throw new DatabaseException("An unexpected error occurred while deleting schools from the database.");
            }
        }
       
        public async Task<ChangedSchools> GetChangedSchoolsList(int size, int pageNumber)
        {
            try
            {
                if (!ValidatePageParametres(pageNumber, size))
                {
                    throw new ArgumentException("Parametres not valid");
                }
                List<SchoolDTO> schoolsAfterChanges = await _dbContext.SchoolsFromRSPO
                    .Where(schoolFromRSPO => !_dbContext.Schools.Any(school =>
                    school.NumerRspo == schoolFromRSPO.NumerRspo &&
                    school.Geography.Equals(schoolFromRSPO.Geography) &&
                    school.Typ == schoolFromRSPO.Typ &&
                    school.StatusPublicznoPrawny == schoolFromRSPO.StatusPublicznoPrawny &&
                    school.Nazwa == schoolFromRSPO.Nazwa &&
                    school.Wojewodztwo == schoolFromRSPO.Wojewodztwo &&
                    school.Gmina == schoolFromRSPO.Gmina &&
                    school.Powiat == schoolFromRSPO.Powiat &&
                    school.Miejscowosc == schoolFromRSPO.Miejscowosc &&
                    school.GminaRodzaj == schoolFromRSPO.GminaRodzaj &&
                    school.KodPocztowy == schoolFromRSPO.KodPocztowy &&
                    school.Ulica == schoolFromRSPO.Ulica &&
                    school.NumerBudynku == schoolFromRSPO.NumerBudynku &&
                    school.NumerLokalu == schoolFromRSPO.NumerLokalu &&
                    school.Email == schoolFromRSPO.Email &&
                    school.Telefon == schoolFromRSPO.Telefon &&
                    school.StronaInternetowa == schoolFromRSPO.StronaInternetowa &&
                    school.DyrektorImie == schoolFromRSPO.DyrektorImie &&
                    school.DyrektorNazwisko == schoolFromRSPO.DyrektorNazwisko &&
                    school.Nip == schoolFromRSPO.Nip &&
                    school.Regon == schoolFromRSPO.Regon &&
                    school.DataRozpoczecia == schoolFromRSPO.DataRozpoczecia &&
                    school.DataZalozenia == schoolFromRSPO.DataZalozenia &&
                    school.DataZakonczenia == schoolFromRSPO.DataZakonczenia &&
                    school.DataLikwidacji == schoolFromRSPO.DataLikwidacji &&
                    school.LiczbaUczniow == schoolFromRSPO.LiczbaUczniow &&
                    school.KategoriaUczniow == schoolFromRSPO.KategoriaUczniow &&
                    school.SpecyfikaSzkoly == schoolFromRSPO.SpecyfikaSzkoly &&
                    school.PodmiotProwadzacy.ToHashSet().SetEquals(schoolFromRSPO.PodmiotProwadzacy)))
                    .Skip((pageNumber - 1) * size).Take(size)
                    .Select(x => SchoolMapper.MapToDTO(x)).OrderBy(x => x.NumerRspo)
                    .ToListAsync();
                List<SchoolDTO> schoolsBeforeChanges = await _dbContext.Schools
                    .Where(school => schoolsAfterChanges.Any(element => element.NumerRspo == school.NumerRspo))
                    .Select(x => SchoolMapper.MapToDTO(x)).OrderBy(element => element.NumerRspo).ToListAsync();

                ChangedSchools changedSchools = new ChangedSchools();
                changedSchools.SchoolsAfterChanges.AddRange(schoolsAfterChanges);
                changedSchools.SchoolsBeforeChanges.AddRange(schoolsBeforeChanges);
                return changedSchools;
			}
            catch(SchoolServiceException)
            {
                throw;
            }
            catch(Exception)
            {
                throw new DatabaseException("An unexpected error occurred while trying to get data from database");
            }
            
        }
    }
}
