using System;
using System.Threading.Tasks;
using CafeOrderManager.Infrastructure.Exceptions;
using CafeOrderManager.Infrastructure.Extensions;
using CafeOrderManager.Infrastructure.Interfaces;
using CafeOrderManager.Infrastructure.Models;
using CafeOrderManager.Service.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace CafeOrderManager.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AuthenticationControlAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var svcProvider = context.HttpContext.RequestServices;
            var service = svcProvider.GetService<AuthService>();

            var canUserTakeAction = await service.CanUserTakeAction();

            // işlem başarılı
            if (canUserTakeAction.EnsureSucces())
            {
                if (canUserTakeAction.Data)
                    await next();
                else
                {
                    Result<bool> res = new Result<bool>();
                    res.Error(new BaseException("Kullanıcı Aktif Değil!"));
                    var response = new JsonResult(res);
                    response.StatusCode = 401;
                    context.Result = response;
                }
            }
            // işlem sırasında bir hata oluştu, sistem yoğun veya veritabanı erişilemez durumda
            else
            {
                Result<bool> res = new Result<bool>();
                res.Error(new BaseException("System is busy or Database is not accessible."));
                var response = new JsonResult(res);
                response.StatusCode = 500;
                context.Result = response;
            }
        }
    }
}