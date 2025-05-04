
using DataTier;

namespace LogicTier
{
    public class ТоварнаяПозиация
    {
        private Товар _товар;

        public ТоварнаяПозиация(Товар p)
        {
            _товар = p;
        }
        public Товар Товар 
        {
            get { return _товар; }
        }
        public string Транспорт
        {
            get { return _товар.Транспорт; }
            set { _товар.Транспорт = value; }
        }
        public string ПунктОтправки
        {
            get { return _товар.ПунктОтправки; }
            set { _товар.ПунктОтправки = value; }
        }
        public string ПунктНазначения
        {
            get { return _товар.ПунктНазначения; }
            set { _товар.ПунктНазначения = value; }
        }
        public double СтоимостьБилета
        {
            get { return _товар.СтоимостьБилета; }
            set { _товар.СтоимостьБилета = value; }
        }

        public double СуммарнаяСтоимостьПозиции
        {
            get { return _товар.СтоимостьБилета; }
        }
        public string ПредставлениеТовара
        {
            get
            {
                return _товар.Транспорт + " : " + _товар.ПунктОтправки
                    + " - " + _товар.ПунктНазначения+" "+_товар.СтоимостьБилета+"руб.";
            }
        }

    }
}
