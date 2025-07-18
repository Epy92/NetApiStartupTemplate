﻿using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace WebAPI.JwtToken
{
    public class JwtTokenBuilder
    {
        private SymmetricSecurityKey securityKey = null;
        private string subject = "";
        private string issuer = "";
        private string audience = "";
        private Dictionary<string, string> claims = new Dictionary<string, string>();
        private int expiryInHours = 24;

        public JwtTokenBuilder AddSecurityKey(SymmetricSecurityKey securityKey)
        {
            this.securityKey = securityKey;
            return this;
        }

        public JwtTokenBuilder AddSubject(string subject)
        {
            this.subject = subject;
            return this;
        }

        public JwtTokenBuilder AddIssuer(string issuer)
        {
            this.issuer = issuer;
            return this;
        }

        public JwtTokenBuilder AddAudience(string audience)
        {
            this.audience = audience;
            return this;
        }

        public JwtTokenBuilder AddClaim(string type, string value)
        {
            this.claims.Add(type, value);
            return this;
        }

        public JwtTokenBuilder AddClaims(Dictionary<string, string> claims)
        {
            this.claims.Union(claims);
            return this;
        }

        public JwtTokenBuilder AddExpiry(int expiryInHours)
        {
            this.expiryInHours = expiryInHours;
            return this;
        }

        public JwtToken Build()
        {
            EnsureArguments();

            var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, this.subject),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }
                .Union(this.claims.Select(item => new Claim(item.Key, item.Value)));

            var token = new JwtSecurityToken(
                issuer: this.issuer,
                audience: this.audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(expiryInHours),
                signingCredentials: new SigningCredentials(
                    this.securityKey,
                    SecurityAlgorithms.HmacSha256));

            return new JwtToken(token);
        }

        private void EnsureArguments()
        {
            if (this.securityKey == null)
                throw new ArgumentNullException("Security Key");

            if (string.IsNullOrEmpty(this.subject))
                throw new ArgumentNullException("Subject");

            if (string.IsNullOrEmpty(this.issuer))
                throw new ArgumentNullException("Issuer");

            if (string.IsNullOrEmpty(this.audience))
                throw new ArgumentNullException("Audience");
        }
    }
}
