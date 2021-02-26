﻿using FluentAssertions;
using LangAppApi.Domain.Common;
using LangAppApi.Domain.Entities;
using LangAppApi.Domain.Enum;
using LangAppApi.Infrastructure.ViewModel;
using LangAppApi.Service.Features.LangFeatures.Commands;
using LangAppApi.Test.Integration.Base;
using LangAppApi.Test.Integration.Common;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LangAppApi.Test.Integration
{
    public class LangUserControllerTests : BaseClassFixture
    {
        public LangUserControllerTests(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        #region Get Endpoints

        [Trait("LangUsersController", "Scénario Nominal de listing des langues de l'utilisateur connecté")]
        [Fact(DisplayName = "GET /api/v1/langusers 200 OK")]
        public async Task CanGetAllLangUsers()
        {
            //  Arrange
            SetupClaimsViaHeaders();
            var queryString = new Dictionary<string, string>()
            {
                {"IsPaged", "false"}
            };
            var requestUri = QueryHelpers.AddQueryString("/api/v1.0/langusers", queryString);

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

            //  Act
            var response = await Client.SendAsync(request);

            //  Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<Response<IEnumerable<LangUser>>>(content);

            Assert.NotEmpty(result.Data);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Trait("LangUsersController", "Scénario Nominal de listing des langues de l'utilisateur en paginant")]
        [Fact(DisplayName = "GET /api/v1/langusers 200 OK")]
        public async Task CanGetPagingLangUsers()
        {
            //  Arrange
            SetupClaimsViaHeaders();
            var queryString = new Dictionary<string, string>()
            {
                {"IsPaged", "true"},
                {"PageNumber", "1"},
                {"PageSize", "10"},
            };
            var requestUri = QueryHelpers.AddQueryString("/api/v1.0/langusers", queryString);

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

            //  Act
            var response = await Client.SendAsync(request);

            //  Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<OutPutModel<LangUser>>(content);
            Assert.NotEmpty(result.Data);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Trait("LangUsersController", "Scénario Alternatif de listing des langues en paginant, bad request")]
        [Fact(DisplayName = "GET /api/v1/langusers 400 BadRequest")]
        public async Task CantGetPagingLang()
        {
            //  Arrange
            SetupClaimsViaHeaders();
            var queryString = new Dictionary<string, string>()
            {
                {"IsPaged", "true"},
                {"PageNumber", "0"},
                {"PageSize", "10"},
            };
            var requestUri = QueryHelpers.AddQueryString("/api/v1.0/langusers", queryString);

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

            //  Act
            var response = await Client.SendAsync(request);

            //  Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Trait("LangUsersController", "Scénario Nominal de récupération d'une langues par son id")]
        [Fact(DisplayName = "GET /api/v1/langusers/{id} 200 OK")]
        public async Task CanGetLangById()
        {
            //  Arrange
            SetupClaimsViaHeaders();
            var expectedEntity = Utilities.GetSeedingLanguages().First();
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1.0/langusers/{expectedEntity.Id}");

            //  Act
            var response = await Client.SendAsync(request);

            //  Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<Response<LangUser>>(content);

            Assert.Equal(expectedEntity.Id, result.Data.Id);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Trait("LangUsersController", "Scénario Alternatif de récupération d'une langue par son id, langue introuvable")]
        [Fact(DisplayName = "GET /api/v1/langusers/{id} 404 NotFound")]
        public async Task CantGetLangByIdNotFound()
        {
            //  Arrange
            SetupClaimsViaHeaders();
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1.0/langusers/{new Guid()}");

            //  Act
            var response = await Client.SendAsync(request);

            //  Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion Get Endpoints

        #region Post Endpoints

        [Trait("LangUsersController", "Scénario Nominal de création d'une langue")]
        [Fact(DisplayName = "POST /api/v1/langusers 201 Created")]
        public async Task CanCreateLang()
        {
            //  Arrange
            SetupClaimsViaHeaders();
            var command = new CreateLangCommand()
            {
                Language = Lang.French
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1.0/langusers")
            {
                Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8,
                    "application/json")
            };

            //  Act
            var response = await Client.SendAsync(request);

            //  Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Trait("LangUsersController", "Scénario Alternatif de création d'une langue, command invalide")]
        [Fact(DisplayName = "POST /api/v1/langusers 400 BadRequest")]
        public async Task CantCreateLangBadRequest()
        {
            //  Arrange
            SetupClaimsViaHeaders();
            var command = new CreateLangCommand();

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1.0/langusers")
            {
                Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8,
                    "application/json")
            };

            //  Act
            var response = await Client.SendAsync(request);

            //  Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion Post Endpoints

        #region Put Endpoints

        [Trait("LangUsersController", "Scénario Nominal de modification d'une langue")]
        [Fact(DisplayName = "PUT /api/v1/langusers 200 Ok")]
        public async Task CanUpdateLang()
        {
            //  Arrange
            SetupClaimsViaHeaders();

            var expectedEntity = Utilities.GetSeedingLanguages().First();
            var command = new UpdateLangCommand()
            {
                Id = expectedEntity.Id
            };

            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/v1.0/langusers/{expectedEntity.Id}")
            {
                Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8,
                    "application/json")
            };

            //  Act
            var response = await Client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<Response<LangUser>>(content);

            //  Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Assert.Equal(expectedEntity.Id, result.Data.Id);
        }

        [Trait("LangUsersController", "Scénario Alternatif de modification d'une langue, introuvable")]
        [Fact(DisplayName = "PUT /api/v1/langusers 404 NotFound")]
        public async Task CantUpdateLangNotFound()
        {
            //  Arrange
            SetupClaimsViaHeaders();

            var id = new Guid();
            var command = new UpdateLangCommand
            {
                Id = id
            };

            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/v1.0/langusers/{id}")
            {
                Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8,
                    "application/json")
            };

            //  Act
            var response = await Client.SendAsync(request);

            //  Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Trait("LangUsersController", "Scénario Alternatif de modification d'une langue, commande invalide")]
        [Fact(DisplayName = "PUT /api/v1/langusers 400 BadRequest")]
        public async Task CantUpdateLangBadRequest()
        {
            //  Arrange
            SetupClaimsViaHeaders();
            var expectedEntity = Utilities.GetSeedingLanguages().First();
            var command = new UpdateLangCommand
            {
                Id = expectedEntity.Id
            };

            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/v1.0/langusers/{expectedEntity.Id}")
            {
                Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8,
                    "application/json")
            };

            //  Act
            var response = await Client.SendAsync(request);

            //  Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion Put Endpoints

        #region Delete Endpoints

        [Trait("LangUsersController", "Scénario Nominal de suppression d'une langue")]
        [Fact(DisplayName = "DELETE /api/v1/langusers 200 Ok")]
        public async Task CanDeleteLang()
        {
            //  Arrange
            SetupClaimsViaHeaders();
            var expectedEntity = Utilities.GetSeedingLanguages().First();

            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1.0/langusers/{expectedEntity.Id}");

            //  Act
            var response = await Client.SendAsync(request);

            //  Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Trait("LangUsersController", "Scénario Alternatif de suppression d'une langue, introuvable")]
        [Fact(DisplayName = "DELETE /api/v1/langusers 404 NotFound")]
        public async Task CantDeleteLangNotFound()
        {
            //  Arrange
            SetupClaimsViaHeaders();

            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1.0/langusers/{new Guid()}");

            //  Act
            var response = await Client.SendAsync(request);

            //  Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion Delete Endpoints
    }
}