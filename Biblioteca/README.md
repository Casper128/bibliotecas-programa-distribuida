# Biblioteca — Seguimiento 1

Aplicación de consulta del catálogo de una biblioteca, basada en el proyecto
`../Microservices/Properties`. Se desarrolla por etapas pequeñas y con commits en español.

## Estado actual

Se crearon la solución y sus cuatro proyectos, las referencias entre capas, el
mediador propio, los contratos de repositorio y Unit of Work, el contexto de EF Core,
el registro del proveedor PostgreSQL y el mecanismo de ejecución de semillas.
El dominio incluye los modelos `Book`, `Author` y `Category`, y los value objects
`Isbn` y `PublicationYear`. Los mapeos, las migraciones, las semillas concretas y
las consultas aún están pendientes.
La API puede iniciar en desarrollo, pero todavía no tiene endpoints de catálogo.
El Seguimiento 1 no exige pruebas; se omite el proyecto Tests del ejemplo.

El enunciado solicita SQL Server. Por indicación del usuario esta solución utilizará
PostgreSQL. Las tres consultas previstas son listar libros, consultar por ID y
filtrar por categoría. Devolverán ID, título, ISBN, año de publicación, autor y
categoría. No se expondrán operaciones de creación, edición o eliminación.

## Misma arquitectura del ejemplo

| Proyecto | Responsabilidad | Referencias a proyectos |
| --- | --- | --- |
| `Library.Domain` | Entidades, value objects y reglas de negocio | Ninguna |
| `Library.Application` | Contratos, DTO, consultas, casos de uso y mediador propio | Domain |
| `Library.Persistence` | EF Core, configuraciones, repositorios, Unit of Work y semillas | Application, Domain |
| `Library.Api` | Controladores y composición de dependencias | Application, Persistence |

`Library.slnx` conserva los grupos Core, Infrastructure, Presenters y scripts.
Se mantienen las carpetas `Entities`, `Common/ValueObjects`, `Exceptions`,
`Contracts/Repositories`, `Contracts/Persistence`, `Utilities/Mediator`,
`Utilities/Pagination`, `UseCases/Books/Queries`, `Configurations`, `Repositories`,
`UnitOfWorks`, `Seeds` y `Controllers`.

Se conserva `BussinesRuleException`, incluido su nombre original, para seguir la
convención del ejemplo. `DataBaseSeeder` resuelve las implementaciones de
`IDataSeeder` por inyección de dependencias y las ejecuta según `Order`, igual
que en la referencia. Actualmente no hay implementaciones registradas ni datos
insertados. `EfCoreUnitOfWork.RollbackAsync` descarta cambios pendientes del contexto;
no deshace un commit ya guardado.

Se fija `Microsoft.OpenApi` en 2.7.5 para corregir la advertencia NU1903 de la
dependencia transitiva del ejemplo, conservando la misma arquitectura.
Referencia: [aviso de Microsoft.OpenAPI](https://github.com/advisories/GHSA-v5pm-xwqc-g5wc).

## Requisitos y ejecución local

- SDK de .NET 10 (el mismo framework del proyecto base).
- Docker con Compose y el motor iniciado.

Desde esta carpeta:

```sh
cp .env.example .env
docker compose config --quiet
docker compose up -d --wait postgres
dotnet restore Library.slnx
dotnet tool restore
dotnet build Library.slnx
dotnet run --project Library.Api --launch-profile https
```

OpenAPI en desarrollo: `http://localhost:5240/openapi/v1.json` (aún sin operaciones).
Para usar el perfil HTTPS debe confiar en el certificado local de desarrollo:
`dotnet dev-certs https --trust`.

PostgreSQL usa `localhost:5433`, base `library` y usuario `library`. La contraseña
de ejemplo `library_dev_only` es exclusivamente local y coincide con
`appsettings.Development.json`. Si cambias valores en `.env`, también debes
actualizar la conexión de la API, por ejemplo mediante la variable de entorno
`ConnectionStrings__MyConnection`. ASP.NET Core no lee automáticamente `.env`.
`appsettings.json` no incluye credenciales; fuera de desarrollo debes proporcionar
la conexión en la configuración del entorno.

El volumen `postgres_data` conserva los datos al detener el servicio:

```sh
docker compose down
```

Las variables `POSTGRES_*` inicializan un volumen nuevo; cambiarlas después no
modifica usuarios ni contraseñas de una base ya existente.

## Code First: primero código, después base de datos

1. Definir `Book`, `Author` y `Category`, incluyendo sus invariantes y value objects.
2. Crear los mapeos `IEntityTypeConfiguration<T>` y los `DbSet` en `DataContext`.
3. Generar y revisar una migración de EF Core.
4. Levantar PostgreSQL y aplicar la migración.
5. Registrar y ejecutar los seeders de autores, categorías y libros, en ese orden.
6. Implementar las tres consultas siguiendo `Query`, `UseCase`, DTO y perfiles de AutoMapper.

Docker crea el servidor y la base vacía. Las tablas de la aplicación se crearán con
migraciones de EF Core; no se usarán scripts SQL de inicialización ni `EnsureCreated`.
No generes la migración inicial hasta que hayamos completado el modelo.
Los siguientes comandos se ejecutarán **en esa etapa**, desde esta carpeta:

```sh
ASPNETCORE_ENVIRONMENT=Development dotnet ef migrations add InitialCreate \
  --project Library.Persistence --startup-project Library.Api --output-dir Migrations

ASPNETCORE_ENVIRONMENT=Development dotnet ef database update \
  --project Library.Persistence --startup-project Library.Api
```

La API llama a `DataBaseSeeder.SeedAsync` al iniciar, como el ejemplo. Cuando existan
seeders concretos, será necesario aplicar las migraciones antes de iniciar la API.
Cada seeder comprobará la existencia de sus registros para evitar duplicados.

## Dominio implementado

Los modelos se encuentran en `Library.Domain/Entities`, agrupados en `Books`,
`Authors` y `Categories`. Los value objects específicos de libro están en
`Entities/Books/ValueObjects`, siguiendo la ubicación de `PropertyDetails` en el
ejemplo. `Common/ValueObjects` queda reservado para conceptos compartidos.

Se conservan clases selladas, identificadores `Guid.CreateVersion7()`, setters
privados, constructores privados para materialización y métodos `Apply...Rules`
que lanzan `BussinesRuleException`. Los value objects son `sealed record`, con
igualdad por valor y sin modificación pública, como `PropertyDetails`.

Reglas adoptadas para esta versión del catálogo (los límites de longitud y año
son decisiones de implementación, no requisitos adicionales del enunciado):

| Modelo o value object | Datos y reglas |
| --- | --- |
| `Author` | Nombre obligatorio, máximo 128 caracteres y colección de libros. |
| `Category` | Nombre obligatorio, máximo 64 caracteres y colección de libros. |
| `Book` | Título obligatorio, máximo 256 caracteres; ISBN, año, autor y categoría obligatorios. Los IDs relacionados no pueden ser `Guid.Empty`. |
| `Isbn` | Acepta ISBN-10 e ISBN-13; elimina espacios y guiones, normaliza `x` a `X` y valida longitud y caracteres (solo dígitos, con `X` permitida al final de ISBN-10). |
| `PublicationYear` | Acepta años desde 1 hasta el año actual en UTC; no admite publicaciones futuras. |

Títulos y nombres se guardan sin espacios al inicio ni al final. Cada libro se
relaciona con un autor y una categoría mediante IDs y propiedades de navegación.
El constructor establece los IDs; las navegaciones se cargarán con EF Core en la
etapa de persistencia. No se añadieron operaciones de actualización para esta
versión de consulta.

Para mantener esta etapa sencilla, el ISBN solo valida su formato básico; no
calcula el dígito de control ni verifica registros editoriales. La igualdad usa el
valor normalizado; no se convierte entre ISBN-10 e ISBN-13.

## Próxima etapa

AutoMapper está registrado en `ApplicationServicesRegistry` para descubrir los
perfiles de `Library.Application`. Al implementar las consultas se crearán los
DTO y sus clases `Profile` junto a cada caso de uso; los `UseCase` recibirán
`IMapper` por inyección de dependencias. Todavía no hay mapeos concretos porque
los DTO están pendientes. Se usará AutoMapper en lugar de `MapperExtensions`
por indicación del usuario; las dependencias entre capas se mantienen.

AutoMapper 16 usa licencia. La clave, cuando corresponda, se configura mediante
`AUTOMAPPER_LICENSE_KEY`, sin guardarla en el repositorio. Consulta la
[configuración oficial de licencia](https://docs.automapper.io/en/latest/License-configuration.html).

Crear las configuraciones de EF Core para los tres modelos y sus value objects,
agregar los `DbSet` y generar la migración inicial para PostgreSQL. Después se
implementarán las semillas ordenadas y las tres consultas. Los nombres de código
seguirán en inglés como el ejemplo; la documentación y los commits, en español.

## Validación de esta etapa

- Compilación de los cuatro proyectos: cero errores y cero advertencias.
- Restauración de la herramienta local `dotnet-ef` 10.0.0: correcta.
- `ef dbcontext info`: reconoce el proveedor Npgsql, base `library` y puerto 5433.
- Arranque de la API en desarrollo: OpenAPI responde HTTP 200 y no contiene operaciones.
- `docker compose --env-file .env.example config --quiet`: correcto.
- PostgreSQL no se inició ni se probó una conexión real: el motor de Docker estaba apagado.

La validación se realizó con un SDK .NET 10 temporal en `/tmp/inmoby-dotnet10`.
El SDK habitual del equipo sigue siendo .NET 9; instala .NET 10 para ejecutar
normalmente los comandos documentados.

## Referencias técnicas

- [Proveedor Npgsql para Entity Framework Core](https://www.npgsql.org/efcore/).
- [Imagen oficial de PostgreSQL](https://hub.docker.com/_/postgres).
