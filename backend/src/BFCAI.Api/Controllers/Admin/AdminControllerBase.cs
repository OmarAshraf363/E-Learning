using BFCAI.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BFCAI.Api.Controllers.Admin;

/// <summary>Base for every admin endpoint: [ApiController] + Admin role required.</summary>
[ApiController]
[Authorize(Roles = Roles.Admin)]
public abstract class AdminControllerBase : ControllerBase;
