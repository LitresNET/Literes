using AutoMapper;
using Litres.Application.Abstractions.Repositories;
using Litres.Domain.Abstractions.Commands;
using Litres.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Litres.Application.Commands.SignUp.Handlers;

public class SignUpUserCommandHandler(
    IContractRepository contractRepository,
    IPublisherRepository publisherRepository,
    IUnitOfWork unitOfWork,
    UserManager<User> userManager,
    IMapper mapper) : ICommandHandler<SignUpUserCommand, IdentityResult>
{
    //TODO: add some more logic for Publisher role
    public async Task<IdentityResult> HandleAsync(SignUpUserCommand command)
    {
        var user = mapper.Map<User>(command);
        await using var transaction = await unitOfWork.BeginTransactionAsync();
        var createResult = await userManager.CreateAsync(user, user.PasswordHash!);
        if (createResult.Succeeded)
        {
            var roleResult = await userManager.AddToRoleAsync(user, command.Role);
            if (roleResult.Succeeded) await transaction.CommitAsync();
            else await transaction.RollbackAsync();
            return roleResult;
        }
        await transaction.RollbackAsync();
        return createResult;
        
    }
}