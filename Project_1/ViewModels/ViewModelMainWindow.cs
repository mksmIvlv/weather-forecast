using Project_1.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Project_1.Commands;

namespace Project_1.ViewModels;

public class ViewModelMainWindow : INotifyPropertyChanged
{
    #region Поля

    public event PropertyChangedEventHandler? PropertyChanged;

    private City _city;

    private string _title = "Прогноз погоды";

    private string _contentButton = "Загрузить данные";

    private string[] _contentComboBox = new[]
    {
        "Moscow",
        "Saint-Petersburg", 
        "Saransk", 
        "Saratov",
        "Samara", 
        "Novosibirsk", 
        "Irkutsk"
    };

    private ObservableCollection<City> _collectionCity;

    private string _selectedCityComboBox;

    #endregion
    
    #region Свойства
    
    public string Title { get { return _title; } }
    
    public string ContentButton { get { return _contentButton; } }
    
    public string[] ContentComboBox { get { return _contentComboBox; } }
    
    public ObservableCollection<City> CollectionCity
    {
        get { return _collectionCity; }

        set
        {
            if (_collectionCity == value)
                return;

            _collectionCity = value;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.CollectionCity)));
        }
    }
    
    public string SelectedCityComboBox
    {
        get { return _selectedCityComboBox; }
        
        set
        {
            if (_selectedCityComboBox == value)
                return;
            
            _selectedCityComboBox = value;
            
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.SelectedCityComboBox)));
        }
    }
    
    #endregion

    #region Команда

    public ICommand CommandGetTempCity { get; }
    private void onCommandGetTempCityExecuted(object p)
    {
        AddTemperatureInCollectionAsync(SelectedCityComboBox);
    }
    private bool commandGetTempCityExecute(object p)
    {
        return true;
    }

    #endregion

    #region Конструктор

    public ViewModelMainWindow()
    {
        _city = new City();

        CollectionCity = new ObservableCollection<City>();

        SelectedCityComboBox = _contentComboBox[0];
        
        CommandGetTempCity = new Command(onCommandGetTempCityExecuted, commandGetTempCityExecute);
    }

    #endregion

    #region Метод

    /// <summary>
    /// Метод добавление температуры в коллекцию
    /// </summary>
    private async Task AddTemperatureInCollectionAsync(string nameCity)
    {
        var city = await _city.GetTemperatureCityAsync(nameCity);

        if (city != null)
        {
            CollectionCity.Add(city);
        }
    }

    #endregion
}