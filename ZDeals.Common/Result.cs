﻿using System.Collections.Generic;
using System.Linq;

namespace ZDeals.Common
{
    public class Result
    {
        public List<Error> Errors { get; private set; }

        public Result()
        {
            Errors = new List<Error>();
        }

        public Result(Error error)
        {
            Errors = new List<Error>();
            if (error != null) Errors.Add(error);
        }

        public Result(IEnumerable<Error>? errors)
        {
            Errors = errors?.ToList() ?? new List<Error>();
        }

        public bool HasError()
        {
            return Errors.Any();
        }

        public bool HasError(ErrorType type)
        {
            return Errors?.Any(x => x.Type == type) ?? false;
        }

        public bool HasError(int errorCode)
        {
            return Errors?.Any(x => x.Code == errorCode) ?? false;
        }

    }

    public class Result<T>: Result
    {
        public T Data { get; set; }

        public Result()
        {
            Data = default!;
        }

        public Result(T data)
        {
            Data = data;
        }

        public Result(Error error): base(error) 
        {
            Data = default!;
        }
        public Result(IEnumerable<Error> errors) : base(errors) 
        {
            Data = default!;
        }
    }
}
