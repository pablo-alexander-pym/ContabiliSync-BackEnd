# Cómo usar la colección de Postman (ContabiliSync BackEnd)

Archivos incluidos: `postman_collection.json`, `postman_environment.json`.

Pasos rápidos:

1. Ejecuta la API localmente:

```bash
# Desde la carpeta del proyecto (BackEnd)
dotnet run
```

La aplicación en `launchSettings.json` está configurada para:

- HTTPS: `https://localhost:7021`
- HTTP: `http://localhost:5013`

2. Importa en Postman:

- Importa `postman_collection.json` (Collection)
- Importa `postman_environment.json` (Environment) y actívalo.

3. Usar las peticiones:

- Selecciona el Environment "ContabiliSync Local" -> la variable `baseUrl` apunta a `https://localhost:7021`.
- Ejecuta las peticiones según necesites.

Notas importantes:

- Para endpoints que suben archivos (`POST /api/Documentos`), en Postman selecciona Body -> form-data, usa la key `file` y en el tipo elige "File"; selecciona un archivo desde tu disco.
- Las enums se aceptan por nombre (por ejemplo: `"tipo": "FacturasGastos"`, `"estado": "Pendiente"`, `"tipo": "Contribuyente"` para usuarios) o por su valor numérico.
- Si la API corre en otro puerto, actualiza la variable `baseUrl` en el Environment.

Ejemplos de cuerpos JSON se incluyen en las peticiones de la colección (Usuarios, Citas). Para Documentos se usa form-data con campos: `file`, `contribuyenteId`, `descripcion`, `tipo`.

Si quieres, puedo:

- Añadir ejemplos de respuestas para cada petición.
- Crear una colección de tests en Postman (asserts) para automatizar comprobaciones.
