namespace System.Collections.Generic
{
    internal class DictionaryValueCollectionDebugView<T1, T2>
    {
        private Dictionary<string, object>.ValueCollection values;

        public DictionaryValueCollectionDebugView(Dictionary<string, object>.ValueCollection values)
        {
            this.values = values;
        }
    }
}