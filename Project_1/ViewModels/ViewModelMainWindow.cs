using Project_1.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Module_19.Commands;

namespace Project_1.ViewModels;

public class ViewModelMainWindow : INotifyPropertyChanged
{
    #region Поля

    public event PropertyChangedEventHandler? PropertyChanged;
    
    private Temperature temperature;

    private string title = "Прогноз погоды";

    private string contentButton = "Загрузить данные";

    private string[] contentComboBox = new[]
    {
        "Moscow",
        "Saint-Petersburg", 
        "Saransk", 
        "Saratov",
        "Samara", 
        "Novosibirsk", 
        "Irkutsk"
    };

    private ObservableCollection<City> collectionCity;

    private string selectedCityComboBox;

    #endregion
    
    #region Свойства
    
    public string Title { get { return title; } }
    
    public string ContentButton { get { return contentButton; } }
    
    public string[] ContentComboBox { get { return contentComboBox; } }
    
    public ObservableCollection<City> CollectionCity
    {
        get { return collectionCity; }

        set
        {
            if (collectionCity == value)
                return;

            collectionCity = value;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.CollectionCity)));
        }
    }
    
    public string SelectedCityComboBox
    {
        get { return selectedCityComboBox; }
        
        set
        {
            if (selectedCityComboBox == value)
                return;
            
            selectedCityComboBox = value;
            
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.SelectedCityComboBox)));
        }
    }
    
    #endregion

    #region Команда

    public ICommand CommandGetTempCity { get; }
    private void onCommandGetTempCityExecuted(object p)
    {
        AddTemperatureInCollectionAsync();
    }
    private bool commandCommandGetTempCityExecute(object p)
    {
        return true;
    }

    #endregion

    #region Конструктор

    public ViewModelMainWindow()
    {
        temperature = new Temperature();

        CollectionCity = new ObservableCollection<City>();

        SelectedCityComboBox = contentComboBox[0];
        
        CommandGetTempCity = new Command(onCommandGetTempCityExecuted, commandCommandGetTempCityExecute);
    }

    #endregion

    #region Метод

    /// <summary>
    /// Метод добавление температуры в коллекцию
    /// </summary>
    private async Task AddTemperatureInCollectionAsync()
    {
        var city = await temperature.GetTemperatureCityAsync(SelectedCityComboBox);

        if (city != null)
        {
            CollectionCity.Add(city);
        }
    }

    #endregion
}