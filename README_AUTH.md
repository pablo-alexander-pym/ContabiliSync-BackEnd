# Autenticación Simplificada - ContabiliSync Backend

Este documento describe los endpoints simples de login y registro implementados en la API.

## Endpoints de Autenticación

### 1. Registro de Usuario

**POST** `/api/Usuarios/Registro`

Registra un nuevo usuario en el sistema.

#### Cuerpo de la petición:

```json
{
  "nombre": "Juan Pérez",
  "email": "juan@ejemplo.com",
  "password": "123456",
  "telefono": "+123456789",
  "especialidad": "Tributario",
  "numeroLicencia": "12345",
  "tipo": 0
}
```

#### Campos:

- `nombre`: Nombre completo del usuario (obligatorio)
- `email`: Email único del usuario (obligatorio)
- `password`: Contraseña mínimo 6 caracteres (obligatorio)
- `telefono`: Teléfono (opcional)
- `especialidad`: Especialidad profesional (opcional, para contadores)
- `numeroLicencia`: Número de licencia (opcional, para contadores)
- `tipo`: Tipo de usuario (0=Usuario, 1=Contador, 2=Administrador)

#### Respuesta exitosa (201):

```json
{
  "id": 1,
  "nombre": "Juan Pérez",
  "email": "juan@ejemplo.com",
  "tipo": "Usuario",
  "message": "Usuario registrado exitosamente"
}
```

### 2. Login de Usuario

**POST** `/api/Usuarios/Login`

Autentica un usuario existente.

#### Cuerpo de la petición:

```json
{
  "email": "juan@ejemplo.com",
  "password": "123456"
}
```

#### Respuesta exitosa (200):

```json
{
  "id": 1,
  "nombre": "Juan Pérez",
  "email": "juan@ejemplo.com",
  "tipo": "Usuario",
  "message": "Autenticación exitosa"
}
```

#### Respuesta de error (401):

```json
{
  "message": "Email o contraseña incorrectos"
}
```

## Tipos de Usuario

- **0 - Usuario**: Usuario regular (contribuyente)
- **1 - Contador**: Contador profesional
- **2 - Administrador**: Administrador del sistema

## Ejemplos de Uso

### Registro de un Usuario Regular

```bash
curl -X POST http://localhost:5000/api/Usuarios/Registro \
  -H "Content-Type: application/json" \
  -d '{
    "nombre": "María García",
    "email": "maria@ejemplo.com",
    "password": "123456",
    "tipo": 0
  }'
```

### Registro de un Contador

```bash
curl -X POST http://localhost:5000/api/Usuarios/Registro \
  -H "Content-Type: application/json" \
  -d '{
    "nombre": "Dr. Carlos López",
    "email": "carlos@ejemplo.com",
    "password": "123456",
    "telefono": "+123456789",
    "especialidad": "Tributario",
    "numeroLicencia": "CPC-12345",
    "tipo": 1
  }'
```

### Login

```bash
curl -X POST http://localhost:5000/api/Usuarios/Login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "maria@ejemplo.com",
    "password": "123456"
  }'
```

## Notas Importantes

1. **Seguridad**: Esta implementación es básica, sin JWT ni otros mecanismos de seguridad avanzados, como solicitaste.

2. **Contraseñas**: Las contraseñas se almacenan hasheadas usando BCrypt para seguridad básica.

3. **Validaciones**:

   - Email debe ser único en el sistema
   - Contraseña mínimo 6 caracteres
   - Email debe tener formato válido

4. **Errores comunes**:
   - 400: Email ya existe
   - 400: Datos de entrada inválidos
   - 401: Credenciales incorrectas
