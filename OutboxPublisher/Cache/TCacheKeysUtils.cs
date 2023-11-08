namespace Cache
{
    public class TCacheKeysUtils
    {
        public static string KeyOperation(string id)
        {
            return $"KeyOperation_{id}";
        }

        public static string ClosedBalance(string id)
        {
            return $"ClosedBalanc_{id}";
        }
    }
}