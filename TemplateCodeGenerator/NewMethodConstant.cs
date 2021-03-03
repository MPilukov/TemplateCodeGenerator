namespace TemplateCodeGenerator
{
    public static class NewMethodConstant
    {
        public static string NewMethodInController =>
@"

        /// <summary>
        /// Метод для BaseDescriptionMethod
        /// </summary>
        /// <param name=""userLogin"">Логин пользователя</param>
        /// <param name=""baseQueryName""><see cref=""BaseQueryModel""/></param>
        /// <param name=""cancellationToken""><see cref=""CancellationToken""/></param>
        /// <returns><see cref=""BaseResponseModel""/></returns>
        [HttpPost(""BaseNameMethod"")]
        [SwaggerOperation(
            OperationId = nameof(BaseNameMethodAsync),
            Summary = ""Метод для BaseDescriptionMethod"")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(BaseResponseModel))]
        public Task<BaseResponseModel> BaseNameMethodAsync(
            [FromHeader, Required(ErrorMessage = ""Требуется логин пользователя"")] string userLogin,
            [FromBody, SwaggerParameter(nameof(BaseQueryModel))]BaseQueryModel baseQueryName,
            CancellationToken cancellationToken)
            => _mediator.Send(baseQueryName, cancellationToken);
    }
}";
    }
}