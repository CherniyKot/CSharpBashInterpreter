using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using CSharpBashInterpreter.Semantics.Abstractions;

namespace CSharpBashInterpreter.Semantics.Context;

public class DefaultContext : IContext
{
    private class ExternalVariables : IEnumerable<KeyValuePair<string, string>>
    {
        public bool TryGetValue(string name, [MaybeNullWhen(false)] out string value)
        {
            var nullableValue = Environment.GetEnvironmentVariable(name);
            value = Environment.GetEnvironmentVariable(name)!;
            return nullableValue != null;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            foreach(DictionaryEntry variable in Environment.GetEnvironmentVariables())
                yield return new KeyValuePair<string, string>(variable.Key.ToString()!, variable.Value!.ToString()!);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
    private class VariablesHandler : IContext.IVariablesHandler
    {
        private readonly ExternalVariables _externalVariables = new();

        public IDictionary<string, string> Internals { get; } = new ConcurrentDictionary<string, string>();

        public int Count => Internals.Count + _externalVariables.Count();
        public bool IsReadOnly => Internals.IsReadOnly;

        public void Add(string name, string value) => Internals.Add(name, value);
        public bool ContainsKey(string name) => Internals.ContainsKey(name);
        public bool Remove(string name) => Internals.Remove(name, out _);

        public bool TryGetValue(string name, out string value)
        {
            if (Internals.TryGetValue(name, out value) == false)
                _externalVariables.TryGetValue(name, out value);
            return value != null;
        }

        public string this[string name]
        {
            get
            {
                if (TryGetValue(name, out var value) == false)
                    throw new KeyNotFoundException(
                        $"This variable: '{name}' is not represented either in the internal or external storage.");
                return value;
            }
            set => Internals[name] = value;
        }

        public ICollection<string> Keys =>
            Internals.Keys.Concat(_externalVariables.Select((var) => var.Key)).ToArray();
        public ICollection<string> Values =>
            Internals.Keys.Concat(_externalVariables.Select((var) => var.Key)).ToArray();

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            foreach (var variable in Internals)
                yield return variable;
            foreach (var variable in _externalVariables)
                yield return variable;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(KeyValuePair<string, string> item) => Internals.Add(item);
        public void Clear() => Internals.Clear();
        public bool Contains(KeyValuePair<string, string> item) => Internals.Contains(item);

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex) =>
            Internals.CopyTo(array, arrayIndex);

        public bool Remove(KeyValuePair<string, string> item) => Internals.Remove(item);
    }

    public IContext.IVariablesHandler EnvironmentVariables { get; } = new VariablesHandler();
}