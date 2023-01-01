using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Notifications.Wpf.Core;

namespace Project_1.Models;

public class Temperature
{
    #region Поля

    private string urlCity;

    private string weather;

    private City returnCity;
    
    private NotificationManager notificationManager;

    #endregion

    #region Событие

    private event Action<string> EventError;

    #endregion

    #region Конструктор

    public Temperature()
    {
        urlCity = string.Empty;
        
        weather = string.Empty;

        notificationManager = new NotificationManager();
        
        EventError += ErrorAsync;
    }

    #endregion

    #region Методы

    /// <summary>
    /// Метод получения температуры
    /// </summary>
    /// <param name="nameCity">Город, у которого нужно получить температуру</param>
    /// <returns>Экземпляр класса City с данными</returns>
    public async Task<City> GetTemperatureCityAsync(string nameCity) 
    {
        try
        {
            switch (nameCity)
            {
                case "Saint-Petersburg":
                {
                    urlCity = "http://api.openweathermap.org/data/2.5/weather?lat=59.8944&lon=30.2642&units=metric&appid=fce39138fbef5f8c5d15cb37f6bd1c17";

                    break;
                }
                case "Samara":
                {
                    urlCity = "http://api.openweathermap.org/data/2.5/weather?lat=53.2&lon=50.15&units=metric&appid=fce39138fbef5f8c5d15cb37f6bd1c17";

                    break;
                }
                default:
                {
                    urlCity = $"http://api.openweathermap.org/data/2.5/weather?q={nameCity}&units=metric&appid=fce39138fbef5f8c5d15cb37f6bd1c17";

                    break;
                }
            }

            await ResponseSiteAsync();

            await GetCurrentValueAsync();

            return returnCity;
        }
        catch
        {
            EventError?.Invoke("Не удалось получить данные");

            return null;
        }
    }

    /// <summary>
    /// Метод получения ответа от сайта
    /// </summary>
    private async Task ResponseSiteAsync()
    {
        await Task.Run(() =>
        {
            using (var client = new WebClient())
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(urlCity);

                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (StreamReader tempfile = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    weather = tempfile.ReadToEnd();
                }
            }
        });
    }
    
    /// <summary>
    /// Метод получения конкретных переменных
    /// </summary>
     private async Task GetCurrentValueAsync()
    {
        await Task.Run(() =>
        {
            var nameCity = JObject.Parse(weather)["name"].ToString();

            var currentTemp = JObject.Parse(weather)["main"]["temp"]
                .ToString()
                .TrimEnd(new []{'0','1','2','3','4','5','6','7','8','9'})
                .TrimEnd(',');

            var maxTemp = JObject.Parse(weather)["main"]["temp_max"]
                .ToString()
                .TrimEnd(new []{'0','1','2','3','4','5','6','7','8','9'})
                .TrimEnd(',');

            var minTemp = JObject.Parse(weather)["main"]["temp_min"]
                .ToString()
                .TrimEnd(new []{'0','1','2','3','4','5','6','7','8','9'})
                .TrimEnd(',');

            returnCity = new City { Name = nameCity, CurrentTemp = currentTemp, MaxTemp = maxTemp, MinTemp = minTemp };
        });
    }
    
    private async void ErrorAsync(string textError)
    {
        await Task.Run(() => notificationManager.ShowAsync(new NotificationContent
        {
            Title = "Ошибка",
            Message = $"{textError}",
            Type = NotificationType.Error
        }));
    }

    #endregion
}