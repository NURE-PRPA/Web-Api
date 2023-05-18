using DL;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Response.Models;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using WebAPI.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Mysqlx;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/certificates")]
public class CertificatesController : ControllerBase
{
    private QuantEdDbContext _dbContext;
    private IAuthService _auth;
    private IUserService _userService;
    private IUserAttemptService _userAttemptService;
    
    public CertificatesController(QuantEdDbContext dbContext, IAuthService auth, IUserService userService, IUserAttemptService userAttemptService)
    {
        _dbContext = dbContext;
        _auth = auth;
        _userService = userService;
        _userAttemptService = userAttemptService;
    }

    [HttpGet]
    [Route("my")]
    public async Task<ActionResult> GetMyCertificates()
    {
        return await Task.Run(async () =>
        {
            var user = _auth.GetCookieAuthInfo();

            if (user.Email == null)
                return Ok(new Response<object>(OperationResult.ERROR, "Not authenticated"));

            var certificates = _dbContext.Certificates
                .Include(c => c.Subscription)
                .ThenInclude(s => s.Listener)
                .Include(c => c.Subscription)
                .ThenInclude(s => s.Course)
                .ThenInclude(c => c.Lecturer)
                .ThenInclude(l => l.Organization)
                .Where(c => c.Subscription.Listener.Email == user.Email)
                .Select(s => s)
                .ToList();

            if (certificates == null)
                return Ok(new Response<object>(OperationResult.ERROR, "Certificates load error"));
            else
            {
                foreach( var certificate in certificates)
                {
                    certificate.RemoveCycles();
                    certificate.Subscription.Course.Lecturer.RemoveCycles();
                }
                return Ok(new Response<List<Certificate>>(OperationResult.OK, certificates, "Certificates load successful"));
            }
        });
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult> GetCertificate(string id)
    {
        return await Task.Run(async () =>
        {
            var certificate = _dbContext.Certificates
                .Include(c => c.Subscription)
                .ThenInclude(s => s.Listener)
                .Include(c => c.Subscription)
                .ThenInclude(s => s.Course)
                .ThenInclude(c => c.Lecturer)
                .ThenInclude(l => l.Organization)
                .FirstOrDefault(c => c.Id == id);

            if (certificate == null)
                return Ok(new Response<object>(OperationResult.ERROR, "Certificate load error"));
            else
            {
                certificate.RemoveCycles();
                certificate.Subscription.Course.Lecturer.RemoveCycles();
                return Ok(new Response<Certificate>(OperationResult.OK, certificate, "Certificate load successful"));
            }
        });
    }

    [HttpGet]
    [Route("course/{id}")]
    public async Task<ActionResult> GetCourseCertificate(string id)
    {
        return await Task.Run(async () =>
        {
            var certificate = _dbContext.Certificates
                .Include(c => c.Subscription)
                .ThenInclude(s => s.Listener)
                .Include(c => c.Subscription)
                .ThenInclude(s => s.Course)
                .ThenInclude(c => c.Lecturer)
                .ThenInclude(l => l.Organization)
                .FirstOrDefault(c => c.Subscription.Course.Id == id);

            if (certificate == null)
                return Ok(new Response<object>(OperationResult.ERROR, "Certificate load error"));
            else
            {
                certificate.RemoveCycles();
                certificate.Subscription.Course.Lecturer.RemoveCycles();
                return Ok(new Response<Certificate>(OperationResult.OK, certificate, "Certificate load successful"));
            }
        });
    }
}