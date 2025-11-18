namespace CodingChallenge.Utilities
{
    public abstract class MemoizedBase<TArgs, TResult> where TArgs : notnull
    {
        private Dictionary<TArgs, TResult> _cache = [];
        private Func<TArgs, TResult> _fn = null!;
        private readonly MemoizedBase<TArgs, TResult>? _base = null;

        protected MemoizedBase(MemoizedBase<TArgs, TResult>? @base = null)
        {
            _base = @base;
        }

        protected Dictionary<TArgs, TResult> Cache => _base?.Cache ?? _cache;

        protected Func<TArgs, TResult> Fn
        {
            get => _base?.Fn ?? _fn;
            set => _fn = value;
        }

        protected TResult InvokeInternal(TArgs args)
        {
            if (_base is null)
                _cache.Clear();

            return Fn.Invoke(args);
        }
    }

    public class Memoized<TArgs, TResult> : MemoizedBase<TArgs, TResult>
        where TArgs : notnull
    {
        public Memoized(Func<Memoized<TArgs, TResult>, TArgs, TResult> fn)
        {
            var self = new Memoized<TArgs, TResult>(this);
            Fn = (TArgs args) =>
            {
                if (Cache.ContainsKey(args))
                    return Cache[args];

                var result = fn(self, args);
                Cache.Add(args, result);
                return result;
            };
        }

        protected Memoized(Memoized<TArgs, TResult> self) : base(self)
        {
        }

        public TResult Invoke(TArgs args) => InvokeInternal(args);
    }

    public class Memoized<T1, T2, TResult> : MemoizedBase<(T1, T2), TResult>
    {
        public Memoized(Func<Memoized<T1, T2, TResult>, T1, T2, TResult> fn)
        {
            var self = new Memoized<T1, T2, TResult>(this);
            Fn = ((T1, T2) args) =>
            {
                if (Cache.ContainsKey(args))
                    return Cache[args];

                var result = fn(self, args.Item1, args.Item2);
                Cache.Add(args, result);
                return result;
            };
        }

        protected Memoized(Memoized<T1, T2, TResult> self) : base(self)
        {
        }

        public TResult Invoke(T1 arg1, T2 arg2) => InvokeInternal((arg1, arg2));
    }
}
