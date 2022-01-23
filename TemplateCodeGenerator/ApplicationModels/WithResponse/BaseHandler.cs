using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BaseNamespace
{
    /// <summary>
    /// Класс-обработчик для BaseDescriptionMethod
    /// </summary>
    public class BaseHandler : IRequestHandler<BaseQueryModel, BaseResponseModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseHandler"/> class.
        /// </summary>
        public BaseHandler()
        {
        }

        /// <inheritdoc />
        public async Task<BaseResponseModel> Handle(BaseQueryModel BaseQueryName, CancellationToken cancellationToken)
        {
            return new BaseResponseModel { };
        }
    }
}