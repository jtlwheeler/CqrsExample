namespace CommandProcessor.Result
{
    public class Result<T>
    {
        public T Value { get; private set; }
        public bool Success { get; private set; }

        protected Result(bool success, T value)
        {
            Success = success;
            Value = value;
        }

        public static Result<T> Ok(T value)
        {
            return new Result<T>(true, value);
        }

        public static Result<T> Fail(T value)
        {
            return new Result<T>(false, value);
        }
    }
}