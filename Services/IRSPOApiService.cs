namespace mapa_back.Services
{
    public interface IRSPOApiService
    {
        Task<bool> SyncDataFromRSPOApi();
    }
}
