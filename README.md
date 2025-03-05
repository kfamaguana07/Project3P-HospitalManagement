# 🏥 Gestión Hospitalaria  

📌 *Proyecto de Programación Web - Parcial 3*  

## 👥 Integrantes  
- *Daniel Guaman*  
- *Kevin Amaguana*  

🚀 Desarrollo de una plataforma web para la gestión hospitalaria, incluyendo administración de usuarios, asignación de roles y manejo de citas médicas.  

## Objetivos del Proyecto

- Desarrollar una plataforma web para la gestión de pacientes, médicos y citas médicas.
- Implementar una interfaz de usuario moderna y responsiva utilizando Bootstrap.
- Proporcionar un sistema seguro con autenticación y autorización basada en roles.
- Desplegar el sistema en la nube utilizando un servicio gratuito de hosting como Somee.com.

> [!IMPORTANT]
> # Cuentas de Prueba
> 
>A continuación, encontrarás las credenciales de prueba para los distintos roles del sistema.  
> Úsalas para acceder y probar las funcionalidades según el rol asignado.

### 🔑 Credenciales de Prueba  

| Rol                  | Email                           | Contraseña              |
|----------------------|--------------------------------|-------------------------|
| *Admin*    | admin@admin.com             | Admin1234,             |
| *Doctor*          | adguaman29@gmail.com             | Dguaman2025!           |
| *Staff*   | receptionist@test.com       | Receptionist1234!     |
| *Patient*   | sdlasso@gmail.com       | Slasso2003!     |




## Seguridad en la autenticación y autorización dentro del sistema.

### Resumen de Roles y Permisos

| *Rol*               | *Pacientes* | *Médicos* | *Especialidades* | *Citas* | *Tratamientos* | *Facturación* |
|------------------------|---------------|-------------|--------------------|-----------|------------------|------------------|
| *Admin*      | CRUD*         | CRUD        | CRUD               | CRUD      | CRUD             | CRUD             |
| *Patient*           | R (propio)    | -           | -                  | R (propias)| -                | R (propias)      |
| *Doctor*             | R (pacientes) | R (propio)  | -                  | RUD (propias)| CRUD (pacientes)| -                |
| *Staff*      | CRUD          | R           | R                  | CRUD      | CRUD                | CRUD                |


---

### Leyenda:
- *CRUD*: Permisos completos (Crear, Leer, Actualizar, Eliminar).
- *R*: Solo lectura.
- *-*: Sin acceso.
- *(propio)*: Acceso solo a su propia información.
- *(pacientes)*: Acceso solo a la información de los pacientes asociados.
- *(propias)*: Acceso solo a sus propias citas o facturas.

- *Pacientes*: Registro, actualización y consulta de pacientes.
- *Médicos*: Administración de la información de los médicos.
- *Citas*: Programación y gestión de citas médicas.
- *Tratamientos*: Información sobre tratamientos aplicados a pacientes.
- *Facturación*: Generación de facturas y consultas sobre pagos realizados.

## Gestión de Pacientes

### Guardar Paciente

Al llamar al método GuardarPaciente(paciente), se realizarán las siguientes acciones:

1. *Guardar el paciente* en la base de datos.
2. *Generar la contraseña* para el paciente (por ejemplo, Jperez1990!).
3. *Crear un usuario* en la tabla AspNetUsers con el *email* juan.perez@example.com y la *contraseña* Jperez1990!.
4. *Asignar el rol* "Patient" al usuario.

### Sincronización de Pacientes Manualmente Ingresados

Si se ingresan nuevos pacientes directamente en la base de datos (por ejemplo, mediante un gestor de bases de datos o scripts SQL), será necesario crear las cuentas de usuario correspondientes en la aplicación.

Para sincronizar estos pacientes, puedes ejecutar el siguiente endpoint:

https://dominio/Paciente/CrearCuentasParaPacientesExistentes

Este endpoint realizará lo siguiente:

- Recorrerá todos los registros de la tabla de *pacientes* que tengan el campo BHABILITADO = 1.
- Generará una *contraseña* para cada paciente utilizando la función GenerarContrasena definida en AccountController.
- Creará un *usuario* en la tabla AspNetUsers y asignará el rol *Patient* a cada cuenta.

>[!IMPORTANT]  
> Esta acción está diseñada para ejecutarse *una sola vez*. Si se ejecuta nuevamente, podría intentar crear duplicados en la base de datos.
> 
>Por lo tanto, se recomienda:
> - Ejecutar este endpoint *solo cuando se hayan agregado pacientes manualmente* a la base de datos.

---


## Creación de la Base de Datos

El sistema se apoya en una base de datos relacional en SQL Server, que incluye las siguientes tablas para la gestión hospitalaria y autenticación de usuarios utilizando *ASP.NET Core Identity*:

### Tablas del Sistema de Gestión Hospitalaria:
- *Pacientes*: Información personal y médica de los pacientes.
- *Médicos*: Datos de los médicos registrados en el hospital.
- *Especialidades*: Listado de especialidades médicas disponibles.
- *Citas*: Registro de las citas médicas programadas.
- *Tratamientos*: Información sobre los tratamientos aplicados a los pacientes.
- *Facturación*: Registro de los pagos y facturas generadas.

### Tablas de Identidad para Autenticación y Gestión de Usuarios:
- *AspNetUsers*: Almacena la información de los usuarios (incluye pacientes, médicos, administradores, etc.) como el nombre, email, contraseñas, etc.
- *AspNetRoles*: Almacena los roles disponibles en el sistema (Admin, Doctor, Patient, Staff).
- *AspNetUserRoles*: Relaciona a los usuarios con los roles asignados, determinando qué permisos tiene cada usuario.
- *AspNetUserClaims*: Almacena las afirmaciones adicionales sobre los usuarios, como roles o derechos personalizados.
- *AspNetUserLogins*: Almacena la información de los proveedores de autenticación externa si es necesario (ej. login con Google o Facebook).
- *AspNetRoleClaims*: Almacena los permisos y derechos asociados a cada rol.
- *AspNetSessions*: Almacena las sesiones activas de los usuarios para asegurar el inicio y cierre de sesión.

### Creación Automática de Tablas de Identity
Cuando se configura *ASP.NET Core Identity* en el proyecto, las tablas mencionadas (como AspNetUsers, AspNetRoles, etc.) se crean automáticamente en la base de datos durante la inicialización del sistema, sin necesidad de que el desarrollador cree manualmente estas tablas. Esto se realiza mediante la migración de la base de datos utilizando *Entity Framework Core*.

*Entity Framework Core* facilita la gestión de la base de datos, permitiendo la creación, actualización y migración de las tablas a través de comandos en la línea de comandos, sin necesidad de intervención manual en los scripts SQL. Esto hace que el proceso de administración de la base de datos sea más eficiente y flexible.

Estas tablas gestionan la autenticación y autorización de los usuarios, permitiendo la asignación de roles y permisos según el acceso permitido en el sistema.

La base de datos también incluye relaciones entre las tablas de gestión hospitalaria y las tablas de identidad, lo que permite vincular la información de los pacientes, médicos y demás usuarios con sus roles y permisos dentro del sistema.
