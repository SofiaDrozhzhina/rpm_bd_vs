using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataTier;
using LogicTier;
using Microsoft.Win32;
using Microsoft.VisualBasic;

namespace PresentationTier;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private Магазин магазин;
    public MainWindow()
    {
        InitializeComponent();
        магазин = new Магазин(new List<ТоварнаяПозиация>());
        this.DataContext = магазин;

    }

    private void btn_open_file_Click(object sender, RoutedEventArgs e)
    {
        // Создаем диалоговое окно для выбора файла
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "JSON Files (*.json)|*.json";  // Фильтр для файлов с расширением .json

        // Ожидаем, что пользователь выберет файл и нажмет "Открыть"
        if (openFileDialog.ShowDialog() == true)
        {
            string путь = openFileDialog.FileName;

            // Читаем данные из выбранного файла
            string json = File.ReadAllText(путь, Encoding.UTF8);  // Указываем кодировку UTF-8 для правильной обработки символов

            // Десериализуем JSON в список товаров
            var товары = JsonSerializer.Deserialize<List<DataTier.Товар>>(json);

            if (товары == null || товары.Count == 0)
            {
                MessageBox.Show("Нет товаров в файле!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Преобразуем данные товаров в товарные позиции и обновляем магазин
            List<ТоварнаяПозиация> позиции = товары.Select(t => new ТоварнаяПозиация(t)).ToList();
            var магазин = new Магазин(позиции);
            магазин.ОбновитьАналитику();
            this.DataContext = магазин;

            // Сообщение о успешном открытии
            MessageBox.Show("Файл успешно открыт!", "Открытие файла", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
   
    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (myComboBox.SelectedItem != null)
        {
            var selectedItem = (System.Windows.Controls.ComboBoxItem)myComboBox.SelectedItem;
            MessageBox.Show($"Выбрано: {selectedItem.Content}");
        }
    }

    private void btn_add_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            string откуда = PunktOtpravki.Text.Trim();
            string куда = PunktNaznachenia.Text.Trim();
            string транспорт = myComboBox.Text;
            string стоимостьStr = StoimostBileta.Text.Trim();

            // Проверка стоимости
            if (string.IsNullOrWhiteSpace(стоимостьStr))
            {
                MessageBox.Show("Введите стоимость билета.", "Ошибка");
                return;
            }

            if (!float.TryParse(стоимостьStr, out float стоимость) || стоимость < 0)
            {
                MessageBox.Show("Стоимость должна быть числом и не может быть отрицательной.", "Ошибка");
                return;
            }

            // Проверка пунктов: только буквы (русские и латинские), пробелы разрешены
            Regex толькоБуквы = new Regex(@"^[\p{L}\s\-]+$"); // \p{L} — любая буква

            if (string.IsNullOrWhiteSpace(откуда) || !толькоБуквы.IsMatch(откуда))
            {
                MessageBox.Show("Пункт отправки должен содержать только буквы.", "Ошибка");
                return;
            }

            if (string.IsNullOrWhiteSpace(куда) || !толькоБуквы.IsMatch(куда))
            {
                MessageBox.Show("Пункт назначения должен содержать только буквы.", "Ошибка");
                return;
            }

            var товар = new DataTier.Товар
            {
                Транспорт = транспорт,
                ПунктОтправки = откуда,
                ПунктНазначения = куда,
                СтоимостьБилета = стоимость
            };

            var позиция = new LogicTier.ТоварнаяПозиация(товар);
            var магазин = (LogicTier.Магазин)DataContext;
            магазин.СписокТоваров.Add(позиция);

            myComboBox.SelectedIndex = -1;
            PunktOtpravki.Clear();
            PunktNaznachenia.Clear();
            StoimostBileta.Clear();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка при добавлении товара: " + ex.Message, "Ошибка");
        }
    }


    private void btn_delete_Click(object sender, RoutedEventArgs e)
    {
        if (MainList.SelectedItems.Count == 0)
        {
            MessageBox.Show("Пожалуйста, выделите элементы для удаления.", "Предупреждение");
            return;
        }

        var result = MessageBox.Show(
            "Вы действительно хотите удалить выбранные элементы?",
            "Подтверждение удаления",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            var магазин = (LogicTier.Магазин)DataContext;
            var itemsToRemove = MainList.SelectedItems.Cast<LogicTier.ТоварнаяПозиация>().ToList();
            foreach (var item in itemsToRemove)
            {
                магазин.СписокТоваров.Remove(item);
            }

            магазин.ОбновитьАналитику();
        }
    }


    private void btn_save_Click(object sender, RoutedEventArgs e)
    {
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filter = "JSON files (*.json)|*.json";
        saveFileDialog.FileName = "маршруты.json";

        if (saveFileDialog.ShowDialog() == true)
        {
            string путь = saveFileDialog.FileName;

            // Получаем данные из контекста
            if (this.DataContext is Магазин магазин)
            {
                var товары = магазин.СписокТоваров.Select(tp => tp.Товар).ToList();

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };

                var dialog = new SaveFileDialog
                {
                    Filter = "JSON файл (*.json)|*.json",
                    Title = "Сохранить файл как",
                    FileName = "маршруты.json"
                };

                if (dialog.ShowDialog() == true)
                {
                    File.WriteAllText(dialog.FileName, JsonSerializer.Serialize(товары, options), Encoding.UTF8);
                    MessageBox.Show("Файл успешно сохранён!", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }

        }
    }
    private string ПолучитьЗначениеПоКлючу(string json, string ключ)
    {
        try
        {
            using var document = JsonDocument.Parse(json);
            if (document.RootElement.TryGetProperty(ключ, out JsonElement значение))
            {
                return значение.ToString();
            }
            else
            {
                MessageBox.Show($"Ключ '{ключ}' не найден в JSON.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return null;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка при разборе JSON: " + ex.Message, "Ошибка");
            return null;
        }
    }

    private void btn_search_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // Сначала выбираем файл
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "JSON Files (*.json)|*.json"
            };

            if (openFileDialog.ShowDialog() != true)
            {
                return;
            }

            string json = File.ReadAllText(openFileDialog.FileName, Encoding.UTF8);

            // Теперь спрашиваем ключ для поиска
            string ключ = Microsoft.VisualBasic.Interaction.InputBox(
                "Введите ключ для поиска в JSON:",
                "Поиск значения",
                ""
            );

            if (string.IsNullOrWhiteSpace(ключ))
            {
                MessageBox.Show("Ключ не введён.", "Ошибка");
                return;
            }

            // Список для хранения найденных значений
            List<string> найденныеЗначения = new List<string>();

            using var document = JsonDocument.Parse(json);
            JsonElement root = document.RootElement;

            if (root.ValueKind == JsonValueKind.Array)
            {
                // Если корень — массив, ищем в каждом объекте массива
                foreach (var элемент in root.EnumerateArray())
                {
                    if (элемент.ValueKind == JsonValueKind.Object && элемент.TryGetProperty(ключ, out JsonElement значение))
                    {
                        найденныеЗначения.Add(значение.ToString());
                    }
                }

                if (найденныеЗначения.Count == 0)
                {
                    MessageBox.Show($"Ключ '{ключ}' не найден в массиве.", "Информация");
                }
                else
                {
                    // Если что-то нашли — показываем
                    string всеЗначения = string.Join("\n", найденныеЗначения);
                    MessageBox.Show($"Найденные значения для ключа '{ключ}':\n{всеЗначения}", "Результат поиска");
                }
            }
            else
            {
                MessageBox.Show("Ожидался JSON-массив в корне.", "Ошибка");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка поиска: " + ex.Message, "Ошибка");
        }

    }    
}




