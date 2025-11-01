using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using UsersMS.Core.Repositories;

namespace UsersMS.Infrastructure.Repositories
{
    public class KeycloakService : IKeycloakService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _baseUrl;
        private readonly string _realm;

        public KeycloakService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

            _baseUrl = _configuration["Keycloak:BaseUrl"] ?? throw new Exception("BaseUrl is not defined in configuration.");
            if (!_baseUrl.EndsWith("/"))
                _baseUrl += "/";

            _realm = _configuration["Keycloak:Realm"] ?? throw new Exception("Realm is not defined in configuration.");

            _httpClient.BaseAddress = new Uri(_baseUrl);
        }

        public async Task<string> GetAdminTokenAsync()
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", _configuration["Keycloak:ClientId"]),
                new KeyValuePair<string, string>("client_secret", _configuration["Keycloak:ClientSecret"]),
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            });

            var response = await _httpClient.PostAsync($"realms/{_realm}/protocol/openid-connect/token", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to obtain admin token. Response: {error}");
            }

            var json = JsonSerializer.Deserialize<JsonElement>(await response.Content.ReadAsStringAsync());
            return json.GetProperty("access_token").GetString() ?? throw new Exception("Access token not found in response.");
        }

        public async Task CreateUserAsync(object userDto, string token)
        {
            var requestContent = new StringContent(JsonSerializer.Serialize(userDto), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsync($"admin/realms/{_realm}/users", requestContent);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to create user. Response: {error}");
            }
        }

        public async Task DeleteUserAsync(string username, string token)
        {
            var userResponse = await _httpClient.GetAsync($"admin/realms/{_realm}/users?username={username}");

            if (!userResponse.IsSuccessStatusCode)
            {
                var error = await userResponse.Content.ReadAsStringAsync();
                throw new Exception($"Failed to find user '{username}' in Keycloak. Response: {error}");
            }

            var users = JsonSerializer.Deserialize<JsonElement>(await userResponse.Content.ReadAsStringAsync());
            if (users.GetArrayLength() == 0)
                throw new Exception($"User '{username}' not found in Keycloak.");

            var userId = users[0].GetProperty("id").GetString();
            var deleteResponse = await _httpClient.DeleteAsync($"admin/realms/{_realm}/users/{userId}");

            if (!deleteResponse.IsSuccessStatusCode)
            {
                var error = await deleteResponse.Content.ReadAsStringAsync();
                throw new Exception($"Failed to delete user '{username}'. Response: {error}");
            }
        }

        public async Task DisableUserAsync(string username, string token)
        {
            // Buscar el usuario por username
            var userResponse = await _httpClient.GetAsync($"admin/realms/{_realm}/users?username={username}");
            if (!userResponse.IsSuccessStatusCode)
            {
                var error = await userResponse.Content.ReadAsStringAsync();
                throw new Exception($"Failed to find user '{username}' in Keycloak. Response: {error}");
            }

            var users = JsonSerializer.Deserialize<JsonElement>(await userResponse.Content.ReadAsStringAsync());
            if (users.GetArrayLength() == 0)
            {
                throw new Exception($"User '{username}' not found in Keycloak.");
            }

            // Obtener el ID del usuario
            var userId = users[0].GetProperty("id").GetString();

            // Crear el objeto JSON para deshabilitar al usuario
            var updateUserPayload = new
            {
                enabled = false
            };

            var content = new StringContent(JsonSerializer.Serialize(updateUserPayload), Encoding.UTF8, "application/json");

            // Enviar la solicitud de actualización
            var updateResponse = await _httpClient.PutAsync($"admin/realms/{_realm}/users/{userId}", content);
            if (!updateResponse.IsSuccessStatusCode)
            {
                var error = await updateResponse.Content.ReadAsStringAsync();
                throw new Exception($"Failed to disable user '{username}' in Keycloak. Response: {error}");
            }
        }

        public async Task UpdateUserAsync(string username, object updatePayload, string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var userResponse = await _httpClient.GetAsync($"admin/realms/{_realm}/users?username={username}");
            if (!userResponse.IsSuccessStatusCode)
            {

                var error = await userResponse.Content.ReadAsStringAsync();
                throw new Exception($"Failed to find user '{username}' in Keycloak. Response: {error}");
            }
        
            var users = JsonSerializer.Deserialize<JsonElement>(await userResponse.Content.ReadAsStringAsync());
            if (users.GetArrayLength() == 0)
            {
                throw new Exception($"User '{username}' not found in Keycloak.");
            }

            // Obtener el ID del usuario desde la respuesta
            var userId = users[0].GetProperty("id").GetString();
            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception($"User ID for '{username}' is null or empty.");
            }

            // Preparar el contenido de la solicitud
            var requestContent = new StringContent(JsonSerializer.Serialize(updatePayload), Encoding.UTF8, "application/json");

            // Realizar la solicitud PUT para actualizar el usuario
            var updateResponse = await _httpClient.PutAsync($"admin/realms/{_realm}/users/{userId}", requestContent);

            if (!updateResponse.IsSuccessStatusCode)
            {
                var error = await updateResponse.Content.ReadAsStringAsync();
                throw new Exception($"Failed to update user '{username}' in Keycloak. Response: {error}");
            }
        }

        public async Task<JsonElement?> GetUserAsync(string username, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"admin/realms/{_realm}/users?username={username}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to find user '{username}' in Keycloak. Response: {error}");
            }

            var users = JsonSerializer.Deserialize<JsonElement>(await response.Content.ReadAsStringAsync());
            return users.GetArrayLength() > 0 ? users[0] : null;
        }

        public async Task AssignRoleAsync(string username, string role, string token)
        {
            // Paso 1: Buscar al usuario
            var userSearchResponse = await _httpClient.GetAsync($"admin/realms/{_realm}/users?username={username}");
            if (!userSearchResponse.IsSuccessStatusCode)
            {
                var error = await userSearchResponse.Content.ReadAsStringAsync();
                throw new Exception($"No se pudo encontrar el usuario '{username}' en Keycloak. Respuesta: {error}");
            }

            var userArray = JsonSerializer.Deserialize<JsonElement>(await userSearchResponse.Content.ReadAsStringAsync());
            if (userArray.GetArrayLength() == 0)
                throw new Exception($"Usuario '{username}' no encontrado en Keycloak.");

            var userId = userArray[0].GetProperty("id").GetString();

            // Paso 2: Buscar el cliente
            var clientResponse = await _httpClient.GetAsync($"admin/realms/{_realm}/clients?clientId={_configuration["Keycloak:ClientId"]}");
            if (!clientResponse.IsSuccessStatusCode)
            {
                var error = await clientResponse.Content.ReadAsStringAsync();
                throw new Exception($"No se pudo encontrar el cliente '{_configuration["Keycloak:ClientId"]}' en Keycloak. Respuesta: {error}");
            }

            var clientArray = JsonSerializer.Deserialize<JsonElement>(await clientResponse.Content.ReadAsStringAsync());
            if (clientArray.GetArrayLength() == 0)
                throw new Exception($"Cliente '{_configuration["Keycloak:ClientId"]}' no encontrado.");

            var clientId = clientArray[0].GetProperty("id").GetString();

            // Paso 3: Buscar el rol
            var roleResponse = await _httpClient.GetAsync($"admin/realms/{_realm}/clients/{clientId}/roles/{role}");
            if (!roleResponse.IsSuccessStatusCode)
            {
                var error = await roleResponse.Content.ReadAsStringAsync();
                throw new Exception($"No se pudo encontrar el rol '{role}' en Keycloak. Respuesta: {error}");
            }

            var roleJson = JsonSerializer.Deserialize<JsonElement>(await roleResponse.Content.ReadAsStringAsync());

            // Paso 4: Asignar el rol al usuario
            var assignRoleRequest = new HttpRequestMessage(
                HttpMethod.Post,
                $"admin/realms/{_realm}/users/{userId}/role-mappings/clients/{clientId}"
            );

            assignRoleRequest.Headers.Add("Authorization", $"Bearer {token}");
            assignRoleRequest.Content = new StringContent(JsonSerializer.Serialize(new[] { roleJson }), Encoding.UTF8, "application/json");

            var assignRoleResponse = await _httpClient.SendAsync(assignRoleRequest);

            if (!assignRoleResponse.IsSuccessStatusCode)
            {
                var error = await assignRoleResponse.Content.ReadAsStringAsync();
                throw new Exception($"No se pudo asignar el rol '{role}' al usuario '{username}'. Respuesta: {error}");
            }
        }

    }
}
