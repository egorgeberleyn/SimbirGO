﻿using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Simbir.GO.Application.Interfaces.Auth;
using Simbir.GO.Application.Interfaces.Persistence.Repositories;
using Simbir.GO.Domain.Accounts;

namespace Simbir.GO.Infrastructure.Auth.Utils;

public class CurrentUserContext : IUserContext
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IAccountRepository _accountRepository;

    public CurrentUserContext(IHttpContextAccessor contextAccessor, IAccountRepository accountRepository)
    {
        _contextAccessor = contextAccessor;
        _accountRepository = accountRepository;
    }

    public bool TryGetUserId(out long result)
    {
        var stringId = _contextAccessor.HttpContext?.User
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return long.TryParse(stringId, out result);
    }

    public async Task<Account?> GetUserAsync()
    {
        if (!TryGetUserId(out var userId))
            return null;
        return await _accountRepository.GetByIdAsync(userId);
    }
}