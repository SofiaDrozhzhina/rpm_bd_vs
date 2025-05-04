namespace DataTier
{
    public class Товар
    {
        public String Транспорт {  get; set; }
        public string ПунктОтправки { get; set; }
        public string ПунктНазначения { get; set; }
        public double СтоимостьБилета { get; set; }
        public string ПредставлениеТовара => $"{Транспорт} ({ПунктОтправки} - {ПунктНазначения},{СтоимостьБилета}₽)";
    }

}
