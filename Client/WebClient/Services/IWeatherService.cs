using System.Threading.Tasks;

namespace WebClient.Services
{
    public interface IWeatherService
    {
        Task<WeatherInfo> GetData();

        string GetCityCode();
    }
}