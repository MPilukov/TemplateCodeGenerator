using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BaseNamespace.WithoutResponseNamespace
{
    /// <summary>
    /// Класс-обработчик для BaseDescriptionMethod
    /// </summary>
    public class BaseHandler : AsyncRequestHandler<BaseQueryModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseHandler"/> class.
        /// </summary>
        public BaseHandler()
        {
        }

        /// <inheritdoc />
        protected override async Task Handle(BaseQueryModel BaseQueryName, CancellationToken cancellationToken)
        {
        }
    }
}