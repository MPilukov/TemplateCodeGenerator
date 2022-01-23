namespace TemplateCodeGenerator
{
    public static class NewMethodConstants
    {
        public static string NewMethodTemplateInController =>
@"

        /// <summary>
        /// Метод для BaseDescriptionMethod
        /// </summary>
        /// <param name=""userLogin"">Логин пользователя</param>
        /// <param name=""BaseQueryName""><see cref=""BaseQueryModel""/></param>
        /// <param name=""cancellationToken""><see cref=""CancellationToken""/></param>
        /// <returns><see cref=""BaseResponseModel""/></returns>
        [HttpPost(""" + BaseNameMethodTemplate + @""")]
        [SwaggerOperation(
            OperationId = nameof(" + BaseNameMethodTemplate + @"Async),
            Summary = ""Метод для BaseDescriptionMethod"")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(BaseResponseModel))]
        public Task<BaseResponseModel> " + BaseNameMethodTemplate + @"Async(
            [FromHeader, Required(ErrorMessage = ""Требуется логин пользователя"")] string userLogin,
            [FromBody, SwaggerParameter(nameof(BaseQueryModel))]BaseQueryModel BaseQueryName,
            CancellationToken cancellationToken)
            => _mediator.Send(BaseQueryName, cancellationToken);
    }
}";

        public const string BaseNameMethodTemplate = "BaseNameMethod";

        // todo: get/ delete
    }
}