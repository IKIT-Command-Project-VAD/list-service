# ShoppingList.List

## Requirements

- Docker
- Docker compose

## Setup

### Setup `.env.dev`

1. Copy `env.dev.sample` → `.env.dev`
2. (Опционально) отредактируйте значения, например `POSTGRES_PORT_EXTERNAL`, `POSTGRES_PASSWORD`.

## Launch

### Run containers

```bash
docker compose --env-file .env.dev -f docker-compose.dev.yml up -d
```

### Connection string for `ShoppingList.List.Web`

Используйте в user-secrets или `appsettings.Development.json`:

```
Host=localhost;Port=${POSTGRES_PORT_EXTERNAL};Database=${POSTGRES_DB};Username=${POSTGRES_USER};Password=${POSTGRES_PASSWORD}
```

### Stop containers

```bash
docker compose --env-file .env.dev -f docker-compose.dev.yml stop
```

### Stop and Delete containers

```bash
docker compose --env-file .env.dev -f docker-compose.dev.yml down
```
