using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ValidationFilter : IAsyncActionFilter
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationFilter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var dto = context.ActionArguments.Values.FirstOrDefault();
        if (dto == null)
        {
            await next();
            return;
        }

        var validatorType = typeof(IValidator<>).MakeGenericType(dto.GetType());
        var validator = _serviceProvider.GetService(validatorType) as IValidator;

        if (validator == null)
        {
            await next();
            return;
        }

        var contextValidation = new ValidationContext<object>(dto);
        var result = await validator.ValidateAsync(contextValidation);

        if (!result.IsValid)
        {
            var errors = result.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            context.Result = new BadRequestObjectResult(new ValidationProblemDetails(errors));
            return;
        }

        await next();
    }
}
