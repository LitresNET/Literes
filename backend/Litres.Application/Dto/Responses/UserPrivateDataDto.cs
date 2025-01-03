﻿namespace Litres.Application.Dto.Responses;

public class UserPrivateDataDto
{
    public long Id { get; set; }
    public string RoleName { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string AvatarUrl { get; set; }
    public decimal Wallet { get; set; }
    public long SubscriptionId { get; set; }
    public List<long> Purchased { get; set; }
    public List<long> Favourites { get; set; }
    public List<long> Reviews { get; set; }
    public List<long> Orders { get; set; }
}