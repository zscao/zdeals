using System;
using System.Collections.Generic;
using System.Linq;

namespace ZDeals.Common
{
    public class Result
    {
        public List<Error> Errors { get; set; }

        public Result()
        {
            Errors = new List<Error>();
        }

        public Result(Error error)
        {
            Errors = new List<Error>();
            if (error != null) Errors.Add(error);
        }

        public Result(IEnumerable<Error> errors)
        {
            Errors = errors?.ToList() ?? new List<Error>();
        }

        public bool HasError()
        {
            return Errors?.Any() ?? false;
        }

        public bool HasError(Type type)
        {
            return Errors?.Any(x => x.GetType() == type) ?? false;
        }

    }

    public class Result<T>: Result
    {
        public T Data { get; set; }

        public Result()
        {
        }

        public Result(T data)
        {
            Data = data;
        }

        public Result(Error error): base(error) { }
        public Result(IEnumerable<Error> errors) : base(errors) { }
    }
}
