using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Results
{
    public class SuccessDataResult<T> : DataResult<T>
    {
        public SuccessDataResult(T data, string message) : base(data, true, message)
        {

        }

        public SuccessDataResult(T data) : base(data, true)
        {

        }

        // çok fazla kullanılmaz
        public SuccessDataResult(string message) : base(default, true, message)
        {

        }

        // çok fazla kullanılmaz
        public SuccessDataResult() : base(default, true)
        {

        }
    }
}
