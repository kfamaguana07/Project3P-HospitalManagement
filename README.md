# 🏥 Gestión Hospitalaria  

📌 **Proyecto de Programación Web - Parcial 3**  

## 👥 Integrantes  
- **Daniel Guaman**  
- **Kevin Amaguana**  

🚀 Desarrollo de una plataforma web para la gestión hospitalaria, incluyendo administración de usuarios, asignación de roles y manejo de citas médicas.  


> [!IMPORTANT]
> # Cuentas de Prueba
>
> A continuación, encontrarás las credenciales de prueba para los distintos roles del sistema.  
> Úsalas para acceder y probar las funcionalidades según el rol asignado.

### 🔑 Credenciales de Prueba  

| Rol                  | Email                           | Contraseña              |
|----------------------|--------------------------------|-------------------------|
| **Admin**    | `admin@admin.com`             | `Admin1234,`             |
| **Doctor**          | `adguaman29@gmail.com`             | `Dguaman2025!`           |
| **Staff**   | `receptionist@test.com`       | `Receptionist1234!`     |
| **Patient**   | `sdlasso@gmail.com`       | `Slasso2003!`     |




## Seguridad en la autenticación y autorización dentro del sistema.

### Resumen de Roles y Permisos

| **Rol**               | **Pacientes** | **Médicos** | **Especialidades** | **Citas** | **Tratamientos** | **Facturación** |
|------------------------|---------------|-------------|--------------------|-----------|------------------|------------------|
| **Admin**      | CRUD*         | CRUD        | CRUD               | CRUD      | CRUD             | CRUD             |
| **Patient**           | R (propio)    | -           | -                  | R (propias)| -                | R (propias)      |
| **Doctor**             | R (pacientes) | R (propio)  | -                  | RUD (propias)| CRUD (pacientes)| -                |
| **Staff**      | CRUD          | R           | R                  | CRUD      | CRUD                | CRUD                |


---

### Leyenda:
- **CRUD**: Permisos completos (Crear, Leer, Actualizar, Eliminar).
- **R**: Solo lectura.
- **-**: Sin acceso.
- **(propio)**: Acceso solo a su propia información.
- **(pacientes)**: Acceso solo a la información de los pacientes asociados.
- **(propias)**: Acceso solo a sus propias citas o facturas.


## Gestión de Pacientes

### Guardar Paciente

Al llamar al método `GuardarPaciente(paciente)`, se realizarán las siguientes acciones:

1. **Guardar el paciente** en la base de datos.
2. **Generar la contraseña** para el paciente (por ejemplo, `Jperez1990!`).
3. **Crear un usuario** en la tabla `AspNetUsers` con el **email** `juan.perez@example.com` y la **contraseña** `Jperez1990!`.
4. **Asignar el rol** "Patient" al usuario.

### Sincronización de Pacientes Manualmente Ingresados

Si se ingresan nuevos pacientes directamente en la base de datos (por ejemplo, mediante un gestor de bases de datos o scripts SQL), será necesario crear las cuentas de usuario correspondientes en la aplicación.

Para sincronizar estos pacientes, puedes ejecutar el siguiente endpoint:

https://dominio/Paciente/CrearCuentasParaPacientesExistentes

Este endpoint realizará lo siguiente:

- Recorrerá todos los registros de la tabla de **pacientes** que tengan el campo `BHABILITADO = 1`.
- Generará una **contraseña** para cada paciente utilizando la función `GenerarContrasena` definida en `AccountController`.
- Creará un **usuario** en la tabla `AspNetUsers` y asignará el rol **Patient** a cada cuenta.

>[!IMPORTANT]  
> Esta acción está diseñada para ejecutarse **una sola vez**. Si se ejecuta nuevamente, podría intentar crear duplicados en la base de datos.
>
> Por lo tanto, se recomienda:
> - Ejecutar este endpoint **solo cuando se hayan agregado pacientes manualmente** a la base de datos.



