# Dofus Batteries Included (DBI) - API

APIs used by the [DBI Plugins](https://github.com/Dofus-Batteries-Included/DBI.Plugins).

## Data Center

Data from the [DDC](https://github.com/Dofus-Batteries-Included/DDC) repository exposed through REST APIs.

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
```
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
```
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