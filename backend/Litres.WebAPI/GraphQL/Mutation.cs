using HotChocolate;
using Litres.Application.Commands.SignIn;
using Litres.Application.Commands.SignUp;
using Litres.Application.Dto.Responses;
using Litres.Application.Queries.Users;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Abstractions.Queries;
using Litres.Domain.Entities;
using Litres.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Litres.WebAPI.GraphQL;

public class Mutation
{
    public async Task<AuthResponseDto> SignIn(
        [Service] ICommandDispatcher commandDispatcher, 
        [Service] IQueryDispatcher queryDispatcher,
        string email, string password)
    {
        try
        {
            var authCmd = new SignInUserCommand(email, password);
            var cmdRes = await commandDispatcher.DispatchReturnAsync<SignInUserCommand, string>(authCmd);

            var q = new GetUserByEmail(email);
            var qRes = queryDispatcher.QueryAsync<GetUserByEmail, User>(q);

            var dto = new AuthResponseDto { UserId = qRes.Id, Token = cmdRes };
            return dto;
        }
        catch (Exception ex)
        {
            return new AuthResponseDto { UserId = -1, Token = "" };
        }
    }

    public async Task<AuthResponseDto> SignUp(
        [Service] ICommandDispatcher commandDispatcher,
        [Service] IQueryDispatcher queryDispatcher,
        string name, string email, string password
        )
    {
        var regCmd = new SignUpUserCommand(name, email, password);
        var regRes = await commandDispatcher.DispatchReturnAsync<SignUpUserCommand, IdentityResult>(regCmd);

        if (!regRes.Succeeded)
            return new AuthResponseDto { UserId = -1, Token = "" };

        var q = new GetUserByEmail(email);
        var qRes = await queryDispatcher.QueryAsync<GetUserByEmail, User>(q);
        
        var authCmd = new SignInUserCommand(email, password);
        var authRes = await commandDispatcher.DispatchReturnAsync<SignInUserCommand, string>(authCmd);
        
        return new AuthResponseDto {UserId = qRes.Id, Token = authRes};
    }
}