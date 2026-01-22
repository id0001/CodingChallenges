using System.Collections;

namespace CodingChallenge.Utilities.Collections.Trees
{
    public static class GenericTree
    {
        public static GenericTree<T> From<T>(IEnumerable<(T Parent, T Child)> relations, EqualityComparer<T>? comparer = null)
            where T : notnull
        {
            var children = new Dictionary<T, List<T>>(comparer);
            var rootCandidates = new HashSet<T>(comparer);
            var childNodes = new HashSet<T>(comparer);

            int count = 0;
            foreach (var (parent, child) in relations)
            {
                children.TryAdd(parent, []);
                children[parent].Add(child);

                rootCandidates.Add(parent);
                childNodes.Add(child);

                count++;
            }

            if (count == 0)
                throw new ArgumentOutOfRangeException(nameof(relations), @"Collection cannot be empty.");

            rootCandidates.ExceptWith(childNodes);

            if (rootCandidates.Count > 1)
                throw new InvalidOperationException("Cannot contain multiple roots");

            if (rootCandidates.Count == 0)
                throw new InvalidOperationException("Cannot contain circular dependencies");

            var root = new GenericTree<T>(rootCandidates.Single(), comparer);
            BuildTree(root, children);
            return root;
        }

        private static void BuildTree<T>(GenericTree<T> root, Dictionary<T, List<T>> children)
            where T : notnull
        {
            var stack = new Stack<GenericTree<T>>();
            stack.Push(root);

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                if (!children.ContainsKey(current.Value))
                    continue;

                foreach (var child in children[current.Value])
                    stack.Push(current.AddChild(child));
            }
        }
    }


    public class GenericTree<T> : IEnumerable<GenericTree<T>>
        where T : notnull
    {
        private readonly List<GenericTree<T>> _children = [];
        private readonly EqualityComparer<T> _comparer;

        public GenericTree(T value, EqualityComparer<T>? comparer = null)
        {
            Value = value;
            _comparer = comparer ?? EqualityComparer<T>.Default;
        }

        public GenericTree<T>? Parent { get; private set; }

        public T Value { get; }

        public int Depth => Parent?.Depth + 1 ?? 0;

        public IReadOnlyList<GenericTree<T>> Children => _children;

        public IEnumerable<GenericTree<T>> Siblings
        {
            get
            {
                if (Parent is null)
                    yield break;

                foreach (var child in Parent.Children.Where(child => child != this))
                    yield return child;
            }
        }

        public bool TryFindNode(T value, out GenericTree<T>? node)
        {
            node = null;
            if (_comparer.Equals(Value, value))
            {
                node = this;
                return true;
            }

            foreach (var child in Children)
            {
                if (child.TryFindNode(value, out node))
                    return true;
            }

            return false;
        }

        public GenericTree<T> FindNode(T value)
        {
            if (TryFindNode(value, out var node))
                return node!;

            throw new KeyNotFoundException($"A node with value '{value}' could not be found");
        }

        public IEnumerable<GenericTree<T>> FindNodes(Func<GenericTree<T>, bool> selector)
        {
            if (selector(this))
                yield return this;

            foreach (var child in Children)
                foreach (var match in child.FindNodes(selector))
                    yield return match;
        }

        public void MakeRoot() => MakeRoot(this);

        public GenericTree<T> AddChild(T value) => AddChild(new GenericTree<T>(value, _comparer));

        public bool Equals(GenericTree<T>? other) => other is { } && Parent == other.Parent && _comparer.Equals(Value, other.Value);

        public bool RemoveChild(T value) => RemoveChild(new GenericTree<T>(value) { Parent = this });

        public bool RemoveChild(GenericTree<T> node)
        {
            if (node.Parent != this)
                return false;

            node.Parent = null;
            return _children.Remove(node);
        }

        public IEnumerator<GenericTree<T>> GetEnumerator()
        {
            yield return this;
            foreach (var child in Children)
                foreach (var item in child)
                    yield return item;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private GenericTree<T> AddChild(GenericTree<T> node)
        {
            if (_children.Contains(node))
                return node;

            node.Parent?.RemoveChild(node);
            node.Parent = this;

            _children.Add(node);
            return node;
        }

        private static void MakeRoot(GenericTree<T> node)
        {
            if (node.Parent is null)
                return;

            var parent = node.Parent;
            parent.RemoveChild(node);
            MakeRoot(parent);
            node.AddChild(parent);
        }
    }
}
