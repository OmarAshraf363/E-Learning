using BFCAI.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BFCAI.Api.Controllers.Instructor;

/// <summary>Base for every instructor endpoint: Instructor role required (ownership is checked in the services).</summary>
[ApiController]
[Authorize(Roles = Roles.Instructor)]
public abstract class InstructorControllerBase : ControllerBase;
