### Requisitos y funcionalidades del **Microservicio de Usuarios**

#### 1. Identidad y autenticación

| Función                        | Detalles clave                                                                                                                                                                                    | Eventos / Reglas                                                                                     |
| ------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------- |
| **Registro de usuario**        | • Endpoint **POST /api/users/register**. <br>• Campos obligatorios: nombre, correo único, contraseña segura, teléfono, dirección. <br>• Genera token de confirmación enviado por e-mail (expira). | • Publica **UserCreated**.<br>• Estado inicial = *PendingEmail*; pasa a *Active* tras confirmación.  |
| **Confirmación de correo**     | • Endpoint **GET /api/users/confirm** con token.                                                                                                                                                  | –                                                                                                    |
| **Inicio de sesión**           | • Endpoint **POST /api/users/login** con correo + contraseña. <br>• Sólo cuentas *Active*.                                                                                                        | • Audita intento exitoso/ fallido.                                                                   |
| **Recuperación de contraseña** | • Flujo “Olvidé mi contraseña”: solicita correo → envía enlace temporal → nueva contraseña que cumple política.                                                                                   | • Evento **UserPasswordResetRequested** / **UserPasswordChanged**.                                   |

#### 2. Gestión de perfil

| Función                             | Detalles clave                                                                                                    |
| ----------------------------------- | ----------------------------------------------------------------------------------------------------------------- |
| **Actualizar datos personales**     | Solo el usuario autenticado puede modificar nombre, teléfono, dirección, avatar, etc. Valida formato y longitud.  |
| **Cambiar contraseña (voluntario)** | Requiere contraseña actual; verifica política; notifica por correo.                                               |
| **Historial de actividad**          | Endpoint paginado **GET /api/users/activity** con filtros por fecha y tipo (login, cambios, etc.).                |

#### 3. Roles y permisos

| Función administrativa       | Detalles clave                                                                                    | Eventos                   |
| ---------------------------- | ------------------------------------------------------------------------------------------------- | ------------------------- |
| **Crear rol**                | Sólo **Administradores**. Define nombre, descripción, conjunto de permisos.                       | RoleCreated               |
| **Asignar rol a usuario**    | Selección de uno o varios roles; registra historial.                                              | RoleAssigned              |
| **Modificar / eliminar rol** | Cambia permisos; aplica en cascada a usuarios; prohibido eliminar roles en uso sin reasignación.  | RoleUpdated / RoleDeleted |
| **Listar roles y permisos**  | Búsqueda y filtrado sobre **GET /api/roles**.                                                     |                           |

#### 4. Estados de la cuenta

*Draft → PendingEmail → Active → Suspended → Deleted.*
Cambios de estado se inspiran en eventos de seguridad (bloqueo por intentos fallidos), reglas de retención o petición de eliminación.&#x20;

#### 5. Integraciones y dependencias

* **Keycloak** como Identity Provider (OAuth2/OIDC); el microservicio actúa como *user-profile service* y publicador de eventos de dominio.
* **RabbitMQ + MassTransit** para eventos (`user-events` exchange).
* Otros microservicios (p.ej. Subastas, Pagos) consumen claims y roles vía JWT y escuchan eventos **UserCreated / RoleAssigned** para construcción de vistas locales.

#### 6. Persistencia

* **PostgreSQL**: tabla `users`, `roles`, `user_roles`, `activity_log`.
* **Firebase Storage** (u otra solución de blobs) para avatares opcionales.

#### 7. Requisitos no funcionales específicos

| Categoría      | Especificación                                                                                   |
| -------------- | ------------------------------------------------------------------------------------------------ |
| Arquitectura   | Hexagonal (puertos/adaptadores) + CQRS/Mediator, **.NET Core 8**                                 |
| Seguridad      | Políticas de contraseña (mín. 12 car.), hash Argon2, force HTTPS, rate-limit login, 2FA opcional |
| Rendimiento    | P95 ≤ 150 ms en *login* y ≤ 200 ms en queries de perfil (<50 registros)                          |
| Disponibilidad | ≥ 99.5 % con réplicas en K8s; *graceful-shutdown*                                                |
| Observabilidad | Serilog + OpenTelemetry traces; métricas Prometheus (`login_success_total`, etc.)                |
| Testing        | Cobertura ≥ 90 % en dominio; ≥ 80 % en integración; mutación básica para cifrado                 |
| CI/CD          | GitHub Actions → Docker → K8s; migraciones automatizadas con Flyway                              |
