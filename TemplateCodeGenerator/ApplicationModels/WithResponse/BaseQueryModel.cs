using MediatR;

namespace BaseNamespace
{
    /// <summary>
    /// BaseDescriptionQueryPrefix BaseDescriptionMethod
    /// </summary>
    public class BaseQueryModel : IRequest<BaseResponseModel>
    {
        /// <summary>
        /// Идентификатор объекта
        /// </summary>
        public int ObjectId { get; set; }
    }
}