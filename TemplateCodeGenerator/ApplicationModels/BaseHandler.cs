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

        /// <summary>
        /// Метод-обработчик для BaseDescriptionMethod
        /// </summary>
        /// <param name="BaseQueryName"><see cref="BaseQueryModel"/></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns><see cref="BaseResponseModel"/></returns>
        public async Task<BaseResponseModel> Handle(BaseQueryModel BaseQueryName, CancellationToken cancellationToken)
        {
            return new BaseResponseModel { };
        }
    }
}