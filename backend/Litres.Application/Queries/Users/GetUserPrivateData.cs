using Litres.Application.Dto;
using Litres.Application.Dto.Responses;
using Litres.Domain.Abstractions.Queries;

namespace Litres.Application.Queries.Users;

public record GetUserPrivateData(long UserId) : IQuery<UserPrivateDataDto>;