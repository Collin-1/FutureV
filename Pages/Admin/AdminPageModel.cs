using FutureV.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FutureV.Pages.Admin;

public abstract class AdminPageModel : PageModel
{
    private readonly AdminAccessService _adminAccessService;

    protected AdminPageModel(AdminAccessService adminAccessService)
    {
        _adminAccessService = adminAccessService;
    }

    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        if (_adminAccessService.HasAccess(context.HttpContext))
        {
            base.OnPageHandlerExecuting(context);
            return;
        }

        var returnUrl = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
        context.Result = new RedirectToPageResult("/Admin/Login", new { returnUrl });
    }
}
