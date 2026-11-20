using BFCAI.Application.Common.Models;
using BFCAI.Application.Features.Dashboard;
using Microsoft.AspNetCore.Mvc;

namespace BFCAI.Api.Controllers.Admin;

[Route("api/admin/dashboard")]
public class DashboardController(IAdminDashboardService service) : AdminControllerBase
{
    [HttpGet]
    public Task<AdminDashboardDto> Get([FromQuery] PaginationParams pagination) => service.GetAsync(pagination);
}
