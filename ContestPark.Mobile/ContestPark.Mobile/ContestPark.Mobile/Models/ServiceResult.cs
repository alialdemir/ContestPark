namespace ContestPark.Mobile.Models
{
    public class ServiceResult<TResult>
    {
        public TResult Data { get; set; } = default(TResult);
        public bool IsSuccess { get; set; } = false;
    }
}