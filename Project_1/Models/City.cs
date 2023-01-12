using Notifications.Wpf.Core;
using System;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using Project_1.Logic;

namespace Project_1.Models;

public class City
{
    #region Поля

    private string _name;

    private string _currentTemp;

    private string _maxTemp;

    private string _minTemp;

    private string _urlCity;

    private string _weather;

    private NotificationManager _notificationManager;

    #endregion

    #region Свойства

    public string Name { get { return _name; } }
    
    public string CurrentTemp { get { return _currentTemp; } }

    public string MaxTemp { get { return _maxTemp; } }

    public string MinTemp { get { return _minTemp; ; } }

    #endregion

    #region Событие

    private event Func<string, Task> _eventError;

    #endregion

    #region Конструктор

    public City() 
    {
        _urlCity = string.Empty;

        _weather = string.Empty;

        _notificationManager = new NotificationManager();

        _eventError += ErrorAsync;
    }

    private City(string name, string currentTemp, string maxTemp, string minTemp)
    {
        _name = name;

        _currentTemp = currentTemp;

        _maxTemp = maxTemp;

        _minTemp = minTemp;
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
                        _urlCity = "http://api.openweathermap.org/data/2.5/weather?lat=59.8944&lon=30.2642&units=metric&appid=fce39138fbef5f8c5d15cb37f6bd1c17";

                        break;
                    }
                case "Samara":
                    {
                        _urlCity = "http://api.openweathermap.org/data/2.5/weather?lat=53.2&lon=50.15&units=metric&appid=fce39138fbef5f8c5d15cb37f6bd1c17";

                        break;
                    }
                default:
                    {
                        _urlCity = $"http://api.openweathermap.org/data/2.5/weather?q={nameCity}&units=metric&appid=fce39138fbef5f8c5d15cb37f6bd1c17";

                        break;
                    }
            }

            await ResponseSiteAsync();

            await GetCurrentValueAsync();

            return new City(_name, _currentTemp, _maxTemp, _minTemp);
        }
        catch(NullReferenceException)
        {
            return null;
        }
        catch(Exception ex) 
        {
            _eventError?.Invoke($"{ex.Message}");

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
                try
                {
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(_urlCity);

                    HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                    using (StreamReader tempfile = new StreamReader(httpWebResponse.GetResponseStream()))
                    {
                        try
                        {
                            _weather = tempfile.ReadToEnd();
                        }
                        catch
                        {
                            _eventError?.Invoke("Не удалось прочитать данные с сервера");

                            throw new NullReferenceException();
                        }
                        finally
                        {
                            tempfile.Dispose();
                        }

                    }
                }
                catch
                {
                    _eventError?.Invoke("Нет ответа от сервера");

                    throw new NullReferenceException();
                }
                finally
                {
                    client.Dispose();
                }
            }
        });
    }

    /// <summary>
    /// Метод записи данных
    /// </summary>
    private async Task GetCurrentValueAsync()
    {
        await Task.Run(() =>
        {
            try 
            {
                _name = JObject.Parse(_weather)["name"].ToString();

                _currentTemp = JObject.Parse(_weather)["main"]["temp"]
                    .ToString()
                    .TrimEnd(new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' })
                    .TrimEnd(',')
                    .CheckingString();

                _maxTemp = JObject.Parse(_weather)["main"]["temp_max"]
                    .ToString()
                    .TrimEnd(new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' })
                    .TrimEnd(',')
                    .CheckingString();

                _minTemp = JObject.Parse(_weather)["main"]["temp_min"]
                    .ToString()
                    .TrimEnd(new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' })
                    .TrimEnd(',')
                    .CheckingString();
            }
            catch 
            {
                throw new NullReferenceException();
            }
        });
    }

    /// <summary>
    /// Метод ошибки
    /// </summary>
    /// <param name="textError">Текст ошибки</param>
    private async Task ErrorAsync(string textError)
    {
        await _notificationManager.ShowAsync(new NotificationContent
        {
            Title = "Ошибка",
            Message = $"{textError}",
            Type = NotificationType.Error
        });
    }

    #endregion
}