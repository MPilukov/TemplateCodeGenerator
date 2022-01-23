using MediatR;

namespace BaseNamespace.WithoutResponseNamespace
{
    /// <summary>
    /// BaseDescriptionQueryPrefix BaseDescriptionMethod
    /// </summary>
    public class BaseQueryModel : IRequest
    {
        /// <summary>
        /// Идентификатор объекта
        /// </summary>
        public int ObjectId { get; set; }
    }
}