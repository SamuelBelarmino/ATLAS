using System.ComponentModel.DataAnnotations;
using Invest.Domain.Entities;

namespace Invest.Application.DTOs.Usuarios;

public sealed class UsuarioCreateDto
{
    [Required]
    [MaxLength(150)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string SenhaHash { get; set; } = string.Empty;
}

public sealed class UsuarioUpdateDto
{
    [Required]
    [MaxLength(150)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string SenhaHash { get; set; } = string.Empty;
}

public sealed class UsuarioReadDto
{
    public Guid Id { get; init; }
    public string Nome { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public DateTime DataCriacao { get; init; }

    public static UsuarioReadDto FromEntity(Usuario usuario) => new()
    {
        Id = usuario.Id,
        Nome = usuario.Nome,
        Email = usuario.Email,
        DataCriacao = usuario.DataCriacao
    };
}
