﻿using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace App.Server.Security;

public class AllowAllHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public AllowAllHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory                                                  logger,
        UrlEncoder                                                      encoder,
        ISystemClock                                                    clock)
        : base(options, logger, encoder, clock) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // 模拟一个已认证的用户（可选：设置默认用户名）
        var claims = new[] { new Claim(ClaimTypes.Name, "DebugUser") };
        var identity = new ClaimsIdentity(claims, "AllowAll");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "AllowAll");

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}