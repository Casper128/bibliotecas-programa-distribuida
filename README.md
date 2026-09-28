## Requisitos

- SDK de .NET 10 (`dotnet --version`).
- Docker Desktop instalado y abierto.
- Terminal zsh o bash.

## 1. Preparar el proyecto

Desde la raíz del repositorio:

```sh
cd Biblioteca
cp -n .env.example .env
dotnet restore Library.slnx
dotnet tool restore
dotnet dev-certs https --trust
```

`cp -n` crea `.env` solo si no existe. Puedes ajustar sus valores para tu entorno local.

La API y los comandos de migración cargan `.env` automáticamente. Las variables
ya definidas en el entorno tienen prioridad. Reinicia la API si modificas `.env`.

## 2. Levantar PostgreSQL y aplicar la migración

```sh
docker compose up -d --wait postgres
dotnet ef database update --project Library.Persistence --startup-project Library.Api
```

## 3. Ejecutar la API

```sh
dotnet run --project Library.Api --no-launch-profile
```

Al iniciar se cargan automáticamente los autores, categorías y libros de ejemplo.

## 4. Probar

Con los valores predeterminados de `.env`:

- Swagger UI: `https://localhost:7240/swagger`
- Todos los libros: `https://localhost:7240/api/books`
- Libro por ID: `https://localhost:7240/api/books/{id}`
- Libros por categoría: `https://localhost:7240/api/books/category/{categoryId}`

Abre Swagger UI y utiliza **Try it out** para probar las consultas.
Obtén `id` y `categoryId` del listado de libros. Swagger está habilitado en `Development`.

## Detener

Detén la API con `Ctrl+C` y luego ejecuta:

```sh
docker compose down
```

Los datos de PostgreSQL se conservan en el volumen de Docker.
