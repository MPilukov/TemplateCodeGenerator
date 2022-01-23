namespace TemplateCodeGenerator
{
    public static class NewMethodConstants
    {
        public static string NewPostMethodTemplateInController =>
@"

        /// <summary>
        /// Метод для BaseDescriptionMethod
        /// </summary>
        /// <param name=""userLogin"">Логин пользователя</param>
        /// <param name=""BaseQueryName""><see cref=""BaseQueryModel""/></param>
        /// <param name=""cancellationToken""><see cref=""CancellationToken""/></param>
        /// <returns><see cref=""BaseResponseModel""/></returns>
        [HttpPost(""" + BaseUriAddressTemplate + @""")]
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
        public static string NewPostWithoutResponseMethodTemplateInController =>
@"

        /// <summary>
        /// Метод для BaseDescriptionMethod
        /// </summary>
        /// <param name=""userLogin"">Логин пользователя</param>
        /// <param name=""BaseQueryName""><see cref=""BaseQueryModel""/></param>
        /// <param name=""cancellationToken""><see cref=""CancellationToken""/></param>
        /// <returns><see cref=""Task""/></returns>
        [HttpPost(""" + BaseUriAddressTemplate + @""")]
        [SwaggerOperation(
            OperationId = nameof(" + BaseNameMethodTemplate + @"Async),
            Summary = ""Метод для BaseDescriptionMethod"")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        public Task " + BaseNameMethodTemplate + @"Async(
            [FromHeader, Required(ErrorMessage = ""Требуется логин пользователя"")] string userLogin,
            [FromBody, SwaggerParameter(nameof(BaseQueryModel))]BaseQueryModel BaseQueryName,
            CancellationToken cancellationToken)
            => _mediator.Send(command, cancellationToken);
    }
}";

        public static string NewGetMethodTemplateInController =>
@"

        /// <summary>
        /// Метод для BaseDescriptionMethod
        /// </summary>
        /// <param name=""BaseQueryName""><see cref=""BaseQueryModel""/></param>
        /// <param name=""cancellationToken""><see cref=""CancellationToken""/></param>
        /// <returns><see cref=""BaseResponseModel""/></returns>
        [HttpGet(""" + BaseUriAddressTemplate + @""")]
        [SwaggerOperation(
            OperationId = nameof(" + BaseNameMethodTemplate + @"Async),
            Summary = ""Метод для BaseDescriptionMethod"")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(BaseResponseModel))]
        public Task<BaseResponseModel> " + BaseNameMethodTemplate + @"Async(
            [FromQuery, SwaggerParameter(nameof(BaseQueryModel))]BaseQueryModel BaseQueryName,
            CancellationToken cancellationToken)
            => _mediator.Send(BaseQueryName, cancellationToken);
    }
}";

        public static string NewDeleteMethodTemplateInController =>
@"

        /// <summary>
        /// Метод для BaseDescriptionMethod
        /// </summary>
        /// <param name=""userLogin"">Логин пользователя</param>
        /// <param name=""BaseQueryName""><see cref=""BaseQueryModel""/></param>
        /// <param name=""cancellationToken""><see cref=""CancellationToken""/></param>
        /// <returns>A <see cref=""Task""/> representing the asynchronous operation.</returns>
        [HttpDelete]
        [SwaggerOperation(
            OperationId = nameof(" + BaseNameMethodTemplate + @"Async),
            Summary = ""Метод для BaseDescriptionMethod"")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        public Task " + BaseNameMethodTemplate + @"Async(
            [FromHeader, Required(ErrorMessage = ""Требуется логин пользователя"")] string userLogin,
            [FromBody, SwaggerParameter(nameof(BaseQueryModel))] BaseQueryModel BaseQueryName,
            CancellationToken cancellationToken) => _mediator.Send(command, cancellationToken);
    }
}";
        
        public const string BaseUriAddressTemplate = "BaseUriAddress";
        public const string BaseNameMethodTemplate = "BaseNameMethod";
    }
}