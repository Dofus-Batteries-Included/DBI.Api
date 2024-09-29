# Dofus Batteries Included (DBI) - API

APIs used by the [DBI Plugins](https://github.com/Dofus-Batteries-Included/DBI.Plugins).

- [Treasure Solver](https://github.com/Dofus-Batteries-Included/DBI.Api?tab=readme-ov-file#treasure-solver)
  - [How to use the treasure solver](https://github.com/Dofus-Batteries-Included/DBI.Api?tab=readme-ov-file#how-to-use-the-treasure-solver)
    - [Find next clue](https://github.com/Dofus-Batteries-Included/DBI.Api?tab=readme-ov-file#find-next-clue) - [Try it!](https://api.dofusbatteriesincluded.fr/swagger/index.html?urls.primaryName=treasure-solver#/Treasure%20Solver/TreasureSolver_FindNextPosition)
    - [Export clues data](https://github.com/Dofus-Batteries-Included/DBI.Api?tab=readme-ov-file#export-clues-data) - [Try it!](https://api.dofusbatteriesincluded.fr/swagger/index.html?urls.primaryName=treasure-solver#/Clues/Clues_ExportClues)
  - [How to register clues](https://github.com/Dofus-Batteries-Included/DBI.Api?tab=readme-ov-file#how-to-register-clues)
    - [Register account](https://github.com/Dofus-Batteries-Included/DBI.Api?tab=readme-ov-file#register-account) - [Try it!](https://api.dofusbatteriesincluded.fr/swagger/index.html?urls.primaryName=identity#/Registration/Registration_Register)
    - [Register clues](https://github.com/Dofus-Batteries-Included/DBI.Api?tab=readme-ov-file#register-clues) - [Try it!](https://api.dofusbatteriesincluded.fr/swagger/index.html?urls.primaryName=treasure-solver#/Clues/Clues_RegisterClues)
- [Data Center](https://github.com/Dofus-Batteries-Included/DBI.Api?tab=readme-ov-file#treasure-solver)

## Treasure Solver

Dofus Treasure Hunt solver using data collected by the players. 
In addition to the endpoints to find treasure hunt clues, the API exposes endpoints to register new clues.
 The [DBI.TreasureSolver](https://github.com/Dofus-Batteries-Included/DBI.Plugins/tree/main/src/TreasureSolver) plugins automatically registers clues while the player performs their hunts.

### How to use the treasure solver

#### Find next clue

[Try it!](https://api.dofusbatteriesincluded.fr/swagger/index.html?urls.primaryName=treasure-solver#/Treasure%20Solver/TreasureSolver_FindNextPosition)

Call the `Find next position` (or the `Find next map`) endpoint with the start position, the direction and the clue ID.

__Example__
- Request
```
curl -X 'GET' \
  'https://api.dofusbatteriesincluded.fr/treasure-solver/-26/29/West/965' \
  -H 'accept: application/json'
```
- Response
```json
{
  "found": true,
  "mapPosition": {
    "x": -30,
    "y": 29
  }
}
```

#### Export clues data

[Try it!](https://api.dofusbatteriesincluded.fr/swagger/index.html?urls.primaryName=treasure-solver#/Clues/Clues_ExportClues)

Call the `Export clues` endpoint to download a JSON file containing all the clues that have been registered.
The exported file looks like:
```json
{
  "version": "1.2.3", # API version
  "last-modification-date": "2024-09-08T22:35:48.3123551+02:00",
  "clues": [
   {
      "clue-id": 123,
      "name-fr": "...",
      "name-en": "...",
      "name-es": "...",
      "name-de": "...",
      "name-pt": "..."
   },
   ...
  ],
  "maps": {
    "456": { # Map ID
      "position": { "x": 45, "y": 67 },
      "clues": [
        147,
        258
      ]
    },
    ...
  }
}
```

__Example__
- Request
```
curl -X 'GET' \
  'https://api.dofusbatteriesincluded.fr/treasure-solver/clues/export' \
  -H 'accept: application/json'
```

### How to register clues

The `Register clues` endpoint requires an API key.
API keys are used to associate clues to accounts, which allow to deduplicate records: the `(author, clue-id, map-id)` tuple is unique in the records database.

#### Register account

[Try it!](https://api.dofusbatteriesincluded.fr/swagger/index.html?urls.primaryName=identity#/Registration/Registration_Register)

First call the `Register account` endpoint and provide the dofus account ID and dofus account nickname that will be the author of the clues. The endpoint returns an API key that should be provided to the server in the `Authorization` header.

__Example__
- Request
```
curl -X 'GET' \
  'https://api.dofusbatteriesincluded.fr/identity/register?accountId={ACCOUNT_ID}&accountName={ACCOUNT_NAME}' \
  -H 'accept: application/json'
```
- Response
```json
"ed2defea-5925-45e5-b286-d31d10194e6f"
```

#### Register clues

[Try it!](https://api.dofusbatteriesincluded.fr/swagger/index.html?urls.primaryName=treasure-solver#/Clues/Clues_RegisterClues)

Once the API key has been retrieved, it can be used to register clues. 
In order to minimize chattiness, multiple clues can be registered in a single request. Each clue can be marked as found or not. 
Clues that are marked as not found are removed from the data sets.

__Example__
- Request
```
curl -X 'POST' \
  'http://localhost:5274/treasure-solver/clues' \
  -H 'accept: */*' \
  -H 'Authorization: ed2defea-5925-45e5-b286-d31d10194e6f' \
  -H 'Content-Type: application/json' \
  -d '{
  "clues": [
    {
      "mapId": 123,
      "clueId": 456,
      "found": true
    },
    {
      "mapId": 147,
      "clueId": 258,
      "found": true
    }
  ]
}'
```

## Data Center

The data center API exposes data from the [DDC](https://github.com/Dofus-Batteries-Included/DDC) repository.

### Game versions

[Try it!](https://api.dofusbatteriesincluded.fr/swagger/index.html?urls.primaryName=data-center#/Game%20versions/GameVersions_GetAvailableVersions)

Most endpoints ask for a version to use when getting the data. 
It can either be a version of the game for which the extractor has released data (see releases of the [DDC](https://github.com/Dofus-Batteries-Included/DDC) repository) or the special value `latest` that will get the data from the higher version.

The `GET /data-center/versions` endpoints returns the list of available versions, and the latest one.

__Example__
- Request
```
curl -X 'GET' \
  'https://api.dofusbatteriesincluded.fr/data-center/game-versions' \
  -H 'accept: application/json'
```
- Response
```json
{
  "latest": "2.73.38.36",
  "versions": [
    "2.73.22.22",
    "2.73.31.26",
    "2.73.32.27",
    "2.73.34.30",
    "2.73.35.32",
    "2.73.36.33",
    "2.73.37.34",
    "2.73.38.36"
  ]
}
```

### Raw data

[Try it!](https://api.dofusbatteriesincluded.fr/swagger/index.html?urls.primaryName=data-center#/Raw%20data)

Raw data from the repository exposed as JSON files.

__Note__: the data for `maps` is huge because it is a JSON containing all the maps of the game and all their cells. 
It is approx 1.5 GB if uncompressed, and merely 40 MB when compressed. 
The API server handles Brotli, GZip and Deflate compression, use it!

### Structured data

[Try it!](https://api.dofusbatteriesincluded.fr/swagger/index.html?urls.primaryName=data-center)

All the other endpoints are for structured data. For example the `World - Maps` endpoint read data from `maps`, `map-positions`, `sub-areas`, `areas`, `super-areas`, `world-maps` and `i18n**` raw files to build their response.

For now there are only a few structured data endpoints as an example of how they can be added to the data center APIs project. 
There are a lot more that could be implemented.\
Suggestions are very welcome, please open an issue if you'd like a specific piece of data exposed through a structured API. 
Alternatively, you can open a PR and contribute yourself. Please open an issue or [join the discord](https://discord.com/invite/HzE9RgYPW5) to get help in doing so.

__Example__
- Request
```
curl -X 'GET' \
  'https://api.dofusbatteriesincluded.fr/data-center/versions/latest/world/maps/75497730' \
  -H 'accept: application/json'
```
- Response
```json
{
  "worldMapId": 1,
  "worldMapName": {
    "french": "Monde des Douze",
    "english": "World of Twelve",
    "spanish": "Mundo de los Doce",
    "german": "Die Welt der Zwölf",
    "portuguese": "Mundo dos Doze"
  },
  "superAreaId": 0,
  "superAreaName": {
    "french": "Monde des Douze",
    "english": "World of Twelve",
    "spanish": "Mundo de los Doce",
    "german": "Die Welt der Zwölf",
    "portuguese": "Mundo dos Doze"
  },
  "areaId": 28,
  "areaName": {
    "french": "Montagne des Koalaks",
    "english": "Koalak Mountain",
    "spanish": "Montaña de los koalaks",
    "german": "Koalak-Gebirge",
    "portuguese": "Montanha dos Koalaks"
  },
  "subAreaId": 231,
  "subAreaName": {
    "french": "Lacs enchantés",
    "english": "Enchanted Lakes",
    "spanish": "Lagos encantados",
    "german": "Verzauberte Seen",
    "portuguese": "Lagos Encantados"
  },
  "mapId": 75497730,
  "position": {
    "x": -20,
    "y": -5
  },
  "cellsCount": 560
}
```