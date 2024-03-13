using Domain;
using Domain.Entities;
using GameServerAPI;

public interface IPlayerService
{
    Task<PlayerDto?> Create(string connectionId);
}