namespace System.Collections.Generic
{
    internal class IDictionaryDebugView<T1, T2>
    {
        private IDictionary<string, object?> actionArguments;

        public IDictionaryDebugView(IDictionary<string, object?> actionArguments)
        {
            this.actionArguments = actionArguments;
        }

        public object Items { get; internal set; }
    }
}