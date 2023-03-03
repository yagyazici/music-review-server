using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using MusicReview.Domain.Settings;
using MusicReview.Domain.DTOs;
using MusicReview.Domain.Services;
using MusicReview.Domain.UserServices;
using MusicReview.Domain.Auth;
using MusicReview.Domain.Models.Base;

namespace MusicReview.Applications.Applications;

public class AuthApplications
{
    private readonly IOptions<TokenSettings> _tokenSettings;
    private readonly IGenericMongoRepository<User> _mongoRepository;
    private readonly IHttpUserService _httpUserService;

    public AuthApplications(
        IOptions<TokenSettings> tokenSettings,
        IGenericMongoRepository<User> mongoRepository,
        IHttpUserService httpUserService)
    {
        _mongoRepository = mongoRepository;
        _tokenSettings = tokenSettings;
        _httpUserService = httpUserService;
    }
    
    public AuthToken CreateToken(User user)
    {
        string userStr = JsonConvert.SerializeObject(user);
        List<Claim> claims = new(){
            new Claim("Id", user.Id)
            
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _tokenSettings.Value.Token
        ));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        var expires = DateTime.Now.AddDays(1);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: expires,
            signingCredentials: creds,
            notBefore: DateTime.Now
        );
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return new AuthToken(jwt, expires);
    }

    public void CreatePassword(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (HMACSHA512 hmac = new())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
    
    public RefreshToken GenerateRefreshToken()
    {
        var refreshToken = new RefreshToken {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64))
        };
        return refreshToken;
    }

    public void SetRefreshToken(RefreshToken newRefreshToken, User user)
    {
        user.RefreshToken = newRefreshToken.Token;
        user.TokenCreated = newRefreshToken.Created;
        user.TokenExpires = newRefreshToken.Expires;
    }

    public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }

    public List<string> RegisterSendErrorList(UserDTO user, string username, string email, string password)
    {
        List<string> errorList = new List<string>();
        if (!this.UsernameRestrictions(username)) errorList.Add("Invalid username");
        if (!this.EmailRestrictions(email)) errorList.Add("Invalid Email");
        if (!this.PasswordRestrictions(password)) errorList.Add("Invalid Password");
        if (_mongoRepository.AnyAsync(user => user.Username == username).Result)
        {
            errorList.Add("This username already in use!");
        }
        if (_mongoRepository.AnyAsync(user => user.Email == email).Result)
        {
            errorList.Add("This email already in use!");
        }
        return errorList;
    }

    public List<string> UpdateUserSendErrorList(User user, string username, string email)
    {
        List<string> errorList = new List<string>();
        if (!this.UsernameRestrictions(username)) errorList.Add("Invalid username");
        if (!this.EmailRestrictions(email)) errorList.Add("Invalid Email");
        if (user.Username != username && _mongoRepository.AnyAsync(user => user.Username == username).Result)
        {
            errorList.Add("This username already in use!");
        }
        if (user.Email != email && _mongoRepository.AnyAsync(user => user.Email == email).Result)
        {
            errorList.Add("This email already in use!");
        }
        return errorList;
    }

    public string UpdatePasswordSendError(string password)
    {
        if (!this.PasswordRestrictions(password)) return "Invalid Password";
        return "true";
    }

    public Task<User> GetCurrentUser()
    {
        var userId = _httpUserService.GetCurrentUserId();
        return _mongoRepository.GetByIdAsync(userId);
    }

    private bool UsernameRestrictions(string username)
    {
        // 3-15 characters long
        // no __ or _. or ._ or .. inside
        // no special chars
        var regex = new Regex(@"^(?=.{3,15}$)(?![\s])(?!.*[_.]{2})[a-zA-Z0-9._ ]+(?<![\s])$");
        return regex.IsMatch(username);
    }

    private bool EmailRestrictions(string email)
    {
        var regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
        return regex.IsMatch(email);
    }

    private bool PasswordRestrictions(string password)
    {
        // Minimum eight characters, at least one letter and one number:
        var regex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$");
        return regex.IsMatch(password);
    }
}