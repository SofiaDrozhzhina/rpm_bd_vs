using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using DataTier;

namespace LogicTier
{
    public class Магазин : INotifyPropertyChanged
    {
        private ObservableCollection<ТоварнаяПозиация> _товары;

        public Магазин(List<ТоварнаяПозиация> позиции)
        {
            _товары = new ObservableCollection<ТоварнаяПозиация>(позиции);
            _товары.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(КоличествоАвтобусныхРейсов));
                OnPropertyChanged(nameof(СуммарнаяСтоимостьСамолётов));
                OnPropertyChanged(nameof(СамыйДорогойБилет));
            };
        }

        public ObservableCollection<ТоварнаяПозиация> СписокТоваров => _товары;

        public int КоличествоАвтобусныхРейсов =>
            _товары.Count(p => p.Транспорт == "Автобус");

        public double СуммарнаяСтоимостьСамолётов =>
            _товары.Where(p => p.Транспорт == "Самолет")
                   .Sum(p => p.СуммарнаяСтоимостьПозиции);

        public double СамыйДорогойБилет =>
            _товары.Count == 0 ? 0 : _товары.Max(p => p.СуммарнаяСтоимостьПозиции);

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public void ОбновитьАналитику()
        {
            OnPropertyChanged(nameof(КоличествоАвтобусныхРейсов));
            OnPropertyChanged(nameof(СуммарнаяСтоимостьСамолётов));
            OnPropertyChanged(nameof(СамыйДорогойБилет));
        }

    }
}
