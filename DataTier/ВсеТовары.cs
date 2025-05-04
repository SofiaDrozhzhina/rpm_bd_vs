using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace DataTier
{
    public class ВсеТовары
    {
        public ObservableCollection<Товар> СписокТоваров { get; set; }

        public ВсеТовары()
        {
            СписокТоваров = new ObservableCollection<Товар>();
        }

        public void ЗагрузитьИзФайла(string путь)
        {
            if (File.Exists(путь))
            {
                string json = File.ReadAllText(путь);
                var товары = JsonSerializer.Deserialize<List<Товар>>(json);

                if (товары != null)
                {
                    СписокТоваров = new ObservableCollection<Товар>(товары);
                }
                else
                {
                    MessageBox.Show("Ошибка чтения файла: товары не найдены", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Файл не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
