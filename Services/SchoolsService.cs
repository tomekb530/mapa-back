using mapa_back.Data;
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
       
        public async Task<ChangedSchoolsResponse> GetChangedSchoolsList(int size, int pageNumber)
        {
            try
            {
				const int pageSize = 1000;
				int pageIndex = 0;
				ChangedSchoolsResponse response = new ChangedSchoolsResponse();
				List<ChangedSchool> changedSchools = new List<ChangedSchool>();
                if (!ValidatePageParametres(pageNumber, size))
                {
                    throw new ArgumentException("Parametres not valid");
                }
                while (true)
                {
					List<SchoolFromRSPO> rspoChunk = await _dbContext.SchoolsFromRSPO
							   .OrderBy(x => x.Id)
							   .Skip(pageIndex * pageSize)
							   .Take(pageSize)
							   .ToListAsync();

					if (!rspoChunk.Any())
						break;

					List<int> keys = rspoChunk
                        .Select(r => r.NumerRspo)
                        .Distinct()
                        .ToList();
					List<School> dbSchools = await _dbContext.Schools
		                .Where(s => keys.Contains(s.NumerRspo))
		                .ToListAsync();

					foreach (var element in rspoChunk)
                    {
                        try
                        {
                            School school = dbSchools.FirstOrDefault(x => x.NumerRspo == element.NumerRspo);
                            if (school == null)
                            {
                                changedSchools.Add(new ChangedSchool(null, SchoolMapper.MapToDTO(element)));
                            }
                            else
                            {
                                bool isDifferent =
                                    school.Geography.X != element.Geography.X ||
                                    school.Geography.Y != element.Geography.Y ||
                                    school.Typ != element.Typ ||
                                    school.StatusPublicznoPrawny != element.StatusPublicznoPrawny ||
                                    school.Nazwa != element.Nazwa ||
                                    school.Wojewodztwo != element.Wojewodztwo ||
                                    school.Gmina != element.Gmina ||
                                    school.Powiat != element.Powiat ||
                                    school.Miejscowosc != element.Miejscowosc ||
                                    school.GminaRodzaj != element.GminaRodzaj ||
                                    school.KodPocztowy != element.KodPocztowy ||
                                    school.Ulica != element.Ulica ||
                                    school.NumerBudynku != element.NumerBudynku ||
                                    school.NumerLokalu != element.NumerLokalu ||
                                    school.Email != element.Email ||
                                    school.Telefon != element.Telefon ||
                                    school.StronaInternetowa != element.StronaInternetowa ||
                                    school.DyrektorImie != element.DyrektorImie ||
                                    school.DyrektorNazwisko != element.DyrektorNazwisko ||
                                    school.Nip != element.Nip ||
                                    school.Regon != element.Regon ||
                                    school.DataRozpoczecia != element.DataRozpoczecia ||
                                    school.DataZalozenia != element.DataZalozenia ||
                                    school.DataZakonczenia != element.DataZakonczenia ||
                                    school.DataLikwidacji != element.DataLikwidacji ||
                                    school.LiczbaUczniow != element.LiczbaUczniow ||
                                    school.KategoriaUczniow != element.KategoriaUczniow ||
                                    school.SpecyfikaSzkoly != element.SpecyfikaSzkoly ||
                                    school.PodmiotProwadzacy != element.PodmiotProwadzacy;

                                if (isDifferent)
                                {
                                    changedSchools.Add(new ChangedSchool(SchoolMapper.MapToDTO(school), SchoolMapper.MapToDTO(element)));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            response.CorruptedRSPO.Add(element.NumerRspo);
                        }
                    }
					pageIndex++;
				}
                response.schoolsCount = changedSchools.Count();
				response.ChangedSchools = changedSchools.Skip((pageNumber-1) * size).Take(size).ToList();
				return response;
			}
            catch(Exception)
            {
                throw new DatabaseException("An unexpected error occurred while trying to get data from database");
            }
            
        }

		public async Task<ChangedSchool> GetSingleChangedSchool(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentException("Id has to be higher than 0");
            }
    
            School? singleSchool = _dbContext.Schools.FirstOrDefault(s => s.Id == id);
            if(singleSchool == null)
            {
                throw new SchoolServiceException("Couldnt find school with given Id in database");
            }
            SchoolFromRSPO? singleSchoolFromRSPO = _dbContext.SchoolsFromRSPO.FirstOrDefault(s => s.NumerRspo == singleSchool.NumerRspo);
            if(singleSchoolFromRSPO == null)
            {
                throw new SchoolServiceException($"Couldn't find matching school in RSPO Database with given rspo number: {singleSchool.NumerRspo}");
            }

            ChangedSchool changedSchool = new ChangedSchool(SchoolMapper.MapToDTO(singleSchool), SchoolMapper.MapToDTO(singleSchoolFromRSPO));
            return changedSchool;

		}

        public async Task<bool> PostSingleSchool(School school)
        {
            if(school == null)
            {
                throw new SchoolServiceException("School cannot be null");
            }
            try
            {
                _dbContext.Add(school);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Couldn't add given school to database");
            }
        }

		public async Task<bool> PostManySchools(List<School> schools)
		{
			if (schools == null)
			{
				throw new SchoolServiceException("School cannot be null");
			}
            if(schools.Count <= 0)
            {
                throw new SchoolServiceException("Schools cannot be empty list");
            }
			try
			{
                foreach(var school in schools)
                {
					_dbContext.Add(school);
				}
                await _dbContext.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				throw new DatabaseException("Couldn't add given school to database");
			}
		}

		public async Task<bool> UpdateSingleSchool(School school)
		{
			if (school == null)
			{
				throw new SchoolServiceException("School cannot be null");
			}
			if (school.Id <= 0)
			{
				throw new ArgumentException("Id has to be higher than 0");
			}
			try
			{
                if (!_dbContext.Schools.Any(s => s.Id == school.Id))
                {
                    throw new SchoolServiceException("Couldn't find school with matching Id in database");
                }
				_dbContext.Attach(school);
				_dbContext.Entry(school).Property(s => s.Nazwa).IsModified = true;
                await _dbContext.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				throw new DatabaseException("Couldn't update given school in database");
			}
		}

		public async Task<bool> UpdateManySchools(List<School> schools)
		{
			if (schools == null)
			{
				throw new SchoolServiceException("School cannot be null");
			}
			if (schools.Count <= 0)
			{
				throw new SchoolServiceException("Schools cannot be empty list");
			}
			try
			{
				var schoolIds = schools.Select(s => s.Id).ToList();

				var existingSchools = await _dbContext.Schools
					.Where(s => schoolIds.Contains(s.Id))
					.ToListAsync();

				if (existingSchools.Count != schools.Count)
				{
					throw new ArgumentException("Some schools with given id do not exist in the database");
				}
				foreach (var school in schools)
                {
                    _dbContext.Attach(school);
                    _dbContext.Entry(school).Property(s => s.Nazwa).IsModified = true;
                }
				await _dbContext.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				throw new DatabaseException("Couldn't update given school in database");
			}
		}
	}
}
