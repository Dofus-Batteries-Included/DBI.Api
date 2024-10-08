[![Discord](https://img.shields.io/discord/1279846081550618696?label=discord)](https://discord.gg/d2fapbcjvv)

# Dofus Batteries Included (DBI) - API

APIs used by the [DBI Plugins](https://github.com/Dofus-Batteries-Included/DBI.Plugins).

__Summary__
- [Data Center](https://github.com/Dofus-Batteries-Included/DBI.Api?tab=readme-ov-file#treasure-solver)
  - [Game versions](https://github.com/Dofus-Batteries-Included/DBI.Api?tab=readme-ov-file#game-versions) - [Try it!](https://api.dofusbatteriesincluded.fr/swagger/index.html?urls.primaryName=data-center#/Game%20versions/GameVersions_GetAvailableVersions)
  - [Raw data](https://github.com/Dofus-Batteries-Included/DBI.Api?tab=readme-ov-file#raw-data) - [Try it!](https://api.dofusbatteriesincluded.fr/swagger/index.html?urls.primaryName=data-center#/Raw%20data)
  - [Structured data](https://github.com/Dofus-Batteries-Included/DBI.Api?tab=readme-ov-file#structured-data) - [Try it!](https://api.dofusbatteriesincluded.fr/swagger/index.html?urls.primaryName=data-center)
- [Path Finder](https://github.com/Dofus-Batteries-Included/DBI.Api?tab=readme-ov-file#path-finder)
  - [The tricky part about paths](https://github.com/Dofus-Batteries-Included/DBI.Api?tab=readme-ov-file#the-tricky-part-about-paths)
  - [How to search for a path](https://github.com/Dofus-Batteries-Included/DBI.Api?tab=readme-ov-file#how-to-search-for-a-path) - [Try it!](https://api.dofusbatteriesincluded.fr/swagger/index.html?urls.primaryName=path-finder#/Path%20Finder)
- [Treasure Solver](https://github.com/Dofus-Batteries-Included/DBI.Api?tab=readme-ov-file#treasure-solver)
  - [How to use the treasure solver](https://github.com/Dofus-Batteries-Included/DBI.Api?tab=readme-ov-file#how-to-use-the-treasure-solver)
    - [Find next clue](https://github.com/Dofus-Batteries-Included/DBI.Api?tab=readme-ov-file#find-next-clue) - [Try it!](https://api.dofusbatteriesincluded.fr/swagger/index.html?urls.primaryName=treasure-solver#/Treasure%20Solver/TreasureSolver_FindNextPosition)
    - [Export clues data](https://github.com/Dofus-Batteries-Included/DBI.Api?tab=readme-ov-file#export-clues-data) - [Try it!](https://api.dofusbatteriesincluded.fr/swagger/index.html?urls.primaryName=treasure-solver#/Clues/Clues_ExportClues)
  - [How to register clues](https://github.com/Dofus-Batteries-Included/DBI.Api?tab=readme-ov-file#how-to-register-clues)
    - [Register account](https://github.com/Dofus-Batteries-Included/DBI.Api?tab=readme-ov-file#register-account) - [Try it!](https://api.dofusbatteriesincluded.fr/swagger/index.html?urls.primaryName=identity#/Registration/Registration_Register)
    - [Register clues](https://github.com/Dofus-Batteries-Included/DBI.Api?tab=readme-ov-file#register-clues) - [Try it!](https://api.dofusbatteriesincluded.fr/swagger/index.html?urls.primaryName=treasure-solver#/Clues/Clues_RegisterClues)

## Data Center

The [data center API](https://api.dofusbatteriesincluded.fr/swagger/index.html?urls.primaryName=data-center) exposes data from the [DDC](https://github.com/Dofus-Batteries-Included/DDC) repository. The underlying data is the foundation on which the other APIs rest.

### Game versions

[Try it!](https://api.dofusbatteriesincluded.fr/swagger/index.html?urls.primaryName=data-center#/Game%20versions/GameVersions_GetAvailableVersions)

Most endpoints ask for a version to use when getting the data.
It can either be a version of the game for which the extractor has released data (see releases of the [DDC](https://github.com/Dofus-Batteries-Included/DDC) repository) or the special value `latest` that will get the data from the higher version.

The [Get available versions](https://api.dofusbatteriesincluded.fr/swagger/index.html?urls.primaryName=data-center#/Game%20versions/GameVersions_GetAvailableVersions) endpoint returns the list of available versions, and the latest one.

<details>
  <summary>
    <b>Example</b>
  </summary>

__Request__
  ```
  curl -X 'GET' \
    'https://api.dofusbatteriesincluded.fr/data-center/game-versions' \
    -H 'accept: application/json'
  ```

__Response__
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
</details>

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

<details>
  <summary>
    <b>Example</b>
  </summary>

__Request__
  ```
  curl -X 'GET' \
    'https://api.dofusbatteriesincluded.fr/data-center/versions/latest/world/maps/75497730' \
    -H 'accept: application/json'
  ```

__Response__
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
</details>

## Path Finder

The [path finder API](https://api.dofusbatteriesincluded.fr/swagger/index.html?urls.primaryName=path-finder) computes paths between maps of the game.

### The tricky part about paths

The first issue is that map coordinates are not unique: there are multiple worlds, and multiple levels in a given world, so multiple maps can have the same map coordinates. 
The unique identifier of a map is its `mapId`. 
The path finder cannot use the coordinates, it needs the map ID.

The second issue is that the maps of the game can be subdivided into multiple zones dissociated zones: the player cannot go from one zone to the other without going through other maps.
For example, this is a common occurence in the wabbit islands. \
It means that we need to know for each map the available zones and how they connect with the zones of the adjacent maps, and we need to know the zones from which we start and end the path searches.

Thankfully, the creators of the game have included what they call a `worldgraph` in the game files. It is extracted by the [DDC](https://github.com/Dofus-Batteries-Included/DDC) project and saved to a file called `world-graph`.\
The graph is defined as follows:
- Nodes are zones of a map. Most maps have only one zone but maps that have multiple dissociated areas have multiple zones.
  <details>
    <summary>
      <b>Example</b>: the data center API exposes an endpoint to get all the nodes of a given map, <a href="https://api.dofusbatteriesincluded.fr/swagger/index.html?urls.primaryName=data-center#/World%20-%20Maps/Maps_GetNodesInMap">try it!</a>
    </summary>
  
    __Request__
    ```
    curl -X 'GET' \
      'https://api.dofusbatteriesincluded.fr/data-center/versions/latest/world/maps/106693122/nodes' \
      -H 'accept: application/json'
    ```
    
    __Response__
    ```json
    [
      {
        "id": 7911,
        "mapId": 106693122,
        "zoneId": 2
      },
      {
        "id": 10115,
        "mapId": 106693122,
        "zoneId": 1
      }
    ]
    ```
  </details>

- Edges are connections between two maps. Edges have transitions: they are all the ways a player can move from the first map to the second.
  <details>
    <summary>
      <b>Example</b>: the data center API exposes an endpoint to get all the edges from a given map (<a href="https://api.dofusbatteriesincluded.fr/swagger/index.html?urls.primaryName=data-center#/World%20-%20Maps/Maps_GetTransitionsFromMap">try it!</a>) or to a given map (<a href="https://api.dofusbatteriesincluded.fr/swagger/index.html?urls.primaryName=data-center#/World%20-%20Maps/Maps_GetTransitionsToMap">try it!</a>)
    </summary>
  
    __Request__
    ```
    curl -X 'GET' \
    'https://api.dofusbatteriesincluded.fr/data-center/versions/latest/world/maps/99615238/transitions/outgoing' \
    -H 'accept: application/json'
    ```
    
    __Response__
    ```json
    [
      {
        "$type": "scroll",
        "direction": "west",
        "from": {
          "id": 5239,
          "mapId": 99615238,
          "zoneId": 1
        },
        "to": {
          "id": 5240,
          "mapId": 99614726,
          "zoneId": 1
        }
      },
      {
        "$type": "scroll",
        "direction": "south",
        "from": {
          "id": 5239,
          "mapId": 99615238,
          "zoneId": 1
        },
        "to": {
          "id": 6586,
          "mapId": 99615239,
          "zoneId": 1
        }
      }
    ]
  ```
  </details>
  
The only missing piece is how to find the nodes of the graph that correspond to the start and end map of a path search.
The data is located in the cells of the maps, that are also extracted by the [DDC](https://github.com/Dofus-Batteries-Included/DDC) project and saved to a file called `maps`. \
Each cell has a `linkedZone` field that is a 2-bytes value, the first byte is the `zoneId`.

__Note__: the fact that `zoneId` is the first byte of `linkedZone` is a guess, it seems to be the case but I have no guarantees.

<details>
    <summary>
      <b>Example</b>: the data center API exposes an endpoint to get all the cells of a given map, <a href="https://api.dofusbatteriesincluded.fr/swagger/index.html?urls.primaryName=data-center#/World%20-%20Maps/Maps_GetMapCells">try it!</a>
    </summary>

  __Request__
  ```
  curl -X 'GET' \
    'https://api.dofusbatteriesincluded.fr/data-center/versions/latest/world/maps/106693122/cells' \
    -H 'accept: application/json'
  ```

  __Response__
  ```json
  [
    {
      "mapId": 106693122,
      "cellNumber": 0,
      "floor": 0,
      "moveZone": 0,
      "linkedZone": 0,
      "speed": 0,
      "los": true,
      "visible": false,
      "nonWalkableDuringFight": false,
      "nonWalkableDuringRp": false,
      "havenbagCell": false
    },
    ...,
    {
      "mapId": 106693122,
      "cellNumber": 155,
      "floor": 0,
      "moveZone": 0,
      "linkedZone": 32,
      "speed": 0,
      "los": true,
      "visible": false,
      "nonWalkableDuringFight": true,
      "nonWalkableDuringRp": false,
      "havenbagCell": false
    },
    ...,
    {
      "mapId": 106693122,
      "cellNumber": 264,
      "floor": 0,
      "moveZone": 0,
      "linkedZone": 17,
      "speed": 0,
      "los": true,
      "visible": false,
      "nonWalkableDuringFight": false,
      "nonWalkableDuringRp": false,
      "havenbagCell": false
    },
    ...
  ]
  ```
</details>

### How to search for a path

[Try it!](https://api.dofusbatteriesincluded.fr/swagger/index.html?urls.primaryName=path-finder#/Path%20Finder)

Internally, the path finder requires the start and end node in the graph to search for a path between them.
The path finder API exposes multiple ways to provide that information, they are the different schemas accepted by the [Find nodes](https://api.dofusbatteriesincluded.fr/swagger/index.html?urls.primaryName=path-finder#/Path%20Finder/PathFinderPaths_FindNodesAll) endpoint :

- `FindNodeById`, from the `nodeId`: the easiest for the path finder, it is the unique identifier of a node. This shifts the burden of finding the right node to the caller of the API.
  <details>
    <summary>
      <b>Example</b>
    </summary>

    __Request__
    ```
    curl -X 'POST' \
      'https://api.dofusbatteriesincluded.fr/path-finder/path/find-nodes' \
      -H 'accept: application/json' \
      -H 'Content-Type: application/json' \
      -d '{
      "search": "by-id",
      "nodeId": 7911
    }'
    ```
    
    __Response__
    ```json
    [
      {
        "mapPosition": {
          "x": 26,
          "y": -9
        },
        "nodeId": 7911,
        "mapId": 106693122,
        "zoneId": 2
      }
    ]
    ```
  </details>

- `FindNodeByMap`, from the `mapId` and `cellNumber`: the second-best option because it always leads to a unique node. The path finder can extract the nodes in the map and the `zoneId` of the cell, using both these information it can find a unique node.
  <details>
    <summary>
      <b>Example</b>
    </summary>

    __Request__
    ```
    curl -X 'POST' \
      'https://api.dofusbatteriesincluded.fr/path-finder/path/find-nodes' \
      -H 'accept: application/json' \
      -H 'Content-Type: application/json' \
      -d '{
      "search": "by-map",
      "mapId": 106693122,
      "cellNumber": 425
    }'
    ```
    
    __Response__
    ```json
    [
      {
        "mapPosition": {
          "x": 26,
          "y": -9
        },
        "nodeId": 10115,
        "mapId": 106693122,
        "zoneId": 1
      }
    ]
    ```
  </details>

- `FindNodeByMap`, from the `mapId` alone: there might be multiple nodes in a map, but usually there is only one.
  <details>
    <summary>
      <b>Example</b>
    </summary>
  
    __Request__
    ```
    curl -X 'POST' \
      'https://api.dofusbatteriesincluded.fr/path-finder/path/find-nodes' \
      -H 'accept: application/json' \
      -H 'Content-Type: application/json' \
      -d '{
      "search": "by-map",
      "mapId": 106693122
    }'
    ```
    
    __Response__
    ```json
    [
      {
        "mapPosition": {
          "x": 26,
          "y": -9
        },
        "nodeId": 7911,
        "mapId": 106693122,
        "zoneId": 2
      },
      {
        "mapPosition": {
          "x": 26,
          "y": -9
        },
        "nodeId": 10115,
        "mapId": 106693122,
        "zoneId": 1
      }
    ]
    ```
  </details>

- `FindNodeAtPosition` from the map coordinates: the path finder can extract all the maps at those coordinates, and all the nodes in those maps. There are high changes that multiple nodes match the coordinates.
  <details>
    <summary>
      <b>Example</b>
    </summary>
  
    __Request__
    ```
    curl -X 'POST' \
      'https://api.dofusbatteriesincluded.fr/path-finder/path/find-nodes' \
      -H 'accept: application/json' \
      -H 'Content-Type: application/json' \
      -d '{
      "search": "at-position",
      "position": {
        "x": 26,
        "y": -9
      }
    }'
    ```
  
    __Response__
    ```json
    [
      {
        "mapPosition": {
          "x": 26,
          "y": -9
        },
        "nodeId": 10112,
        "mapId": 99615745,
        "zoneId": 1
      },
      {
        "mapPosition": {
          "x": 26,
          "y": -9
        },
        "nodeId": 7911,
        "mapId": 106693122,
        "zoneId": 2
      },
      {
        "mapPosition": {
          "x": 26,
          "y": -9
        },
        "nodeId": 10115,
        "mapId": 106693122,
        "zoneId": 1
      }
    ]
    ```
  </details>

The [Find paths](https://api.dofusbatteriesincluded.fr/swagger/index.html?urls.primaryName=path-finder#/Path%20Finder/PathFinderPaths_FindNodes) endpoint uses the same schemas to specify the start and end nodes of the search.
It then extract all the candidates for the start and the end and computes all the paths between all the candidates.

<details>
  <summary>
  <b>Example</b>: in this example we provide both the map ids and the cell numbers, there is only one candidate for the start and end node so we get the only possible path between them
  </summary>

  __Request__
  ```
  curl -X 'POST' \
    'https://api.dofusbatteriesincluded.fr/path-finder/path/find-paths' \
    -H 'accept: application/json' \
    -H 'Content-Type: application/json' \
    -d '{
    "start": {
      "search": "by-map",
      "mapId": 75497730,
      "cellNumber": 425
    },
    "end": {
      "search": "by-map",
      "mapId": 75498242,
      "cellNumber": 430
    }
  }'
  ```
  
  __Response__
  ```json
  {
    "paths": [
      {
        "from": {
          "mapPosition": {
            "x": -20,
            "y": -5
          },
          "nodeId": 5609,
          "mapId": 75497730,
          "zoneId": 1
        },
        "to": {
          "mapPosition": {
            "x": -20,
            "y": -5
          },
          "nodeId": 1667,
          "mapId": 75498242,
          "zoneId": 1
        },
        "steps": [
          {
            "node": {
              "mapPosition": {
                "x": -20,
                "y": -5
              },
              "nodeId": 5609,
              "mapId": 75497730,
              "zoneId": 1
            },
            "transition": {
              "type": "scroll",
              "direction": "north"
            }
          },
          {
            "node": {
              "mapPosition": {
                "x": -20,
                "y": -6
              },
              "nodeId": 7076,
              "mapId": 75497731,
              "zoneId": 1
            },
            "transition": {
              "type": "scroll",
              "direction": "south"
            }
          },
          {
            "node": {
              "mapPosition": {
                "x": -20,
                "y": -5
              },
              "nodeId": 7095,
              "mapId": 75497730,
              "zoneId": 2
            },
            "transition": {
              "type": "scroll",
              "direction": "east"
            }
          }
        ]
      }
    ]
  }
  ```
</details>

<details>
  <summary>
  <b>Example</b>: in this example we provide the map ids without specifying the cell numbers, two paths are possible between the nodes: a path from the first node's zone 1 to the second node, and a path from the first node's zone 2 to the second node.
  </summary>

  __Request__
  ```
  curl -X 'POST' \
    'https://api.dofusbatteriesincluded.fr/path-finder/path/find-paths' \
    -H 'accept: application/json' \
    -H 'Content-Type: application/json' \
    -d '{
    "start": {
      "search": "by-map",
      "mapId": 75497730
    },
    "end": {
      "search": "by-map",
      "mapId": 75498242
    }
  }'
  ```

  __Response__
  ```json
  {
    "paths": [
      {
        "from": {
          "mapPosition": {
            "x": -20,
            "y": -5
          },
          "nodeId": 5609,
          "mapId": 75497730,
          "zoneId": 1
        },
        "to": {
          "mapPosition": {
            "x": -20,
            "y": -5
          },
          "nodeId": 1667,
          "mapId": 75498242,
          "zoneId": 1
        },
        "steps": [
          {
            "node": {
              "mapPosition": {
                "x": -20,
                "y": -5
              },
              "nodeId": 5609,
              "mapId": 75497730,
              "zoneId": 1
            },
            "transition": {
              "type": "scroll",
              "direction": "north"
            }
          },
          {
            "node": {
              "mapPosition": {
                "x": -20,
                "y": -6
              },
              "nodeId": 7076,
              "mapId": 75497731,
              "zoneId": 1
            },
            "transition": {
              "type": "scroll",
              "direction": "south"
            }
          },
          {
            "node": {
              "mapPosition": {
                "x": -20,
                "y": -5
              },
              "nodeId": 7095,
              "mapId": 75497730,
              "zoneId": 2
            },
            "transition": {
              "type": "scroll",
              "direction": "east"
            }
          }
        ]
      },
      {
        "from": {
          "mapPosition": {
            "x": -20,
            "y": -5
          },
          "nodeId": 7095,
          "mapId": 75497730,
          "zoneId": 2
        },
        "to": {
          "mapPosition": {
            "x": -20,
            "y": -5
          },
          "nodeId": 1667,
          "mapId": 75498242,
          "zoneId": 1
        },
        "steps": [
          {
            "node": {
              "mapPosition": {
                "x": -20,
                "y": -5
              },
              "nodeId": 7095,
              "mapId": 75497730,
              "zoneId": 2
            },
            "transition": {
              "type": "scroll",
              "direction": "east"
            }
          }
        ]
      }
    ]
  }
  ```
</details>

## Treasure Solver

The [treasure solver API](https://api.dofusbatteriesincluded.fr/swagger/index.html?urls.primaryName=treasure-solver) can solve Dofus treasure hunts using data collected by the players.
In addition to the endpoints to find treasure hunt clues or export all of them as JSON, the API exposes endpoints to register new clues. \
The [DBI.TreasureSolver](https://github.com/Dofus-Batteries-Included/DBI.Plugins/tree/main/src/TreasureSolver) plugins automatically registers clues while the player performs their hunts.

### How to use the treasure solver

#### Find next clue

[Try it!](https://api.dofusbatteriesincluded.fr/swagger/index.html?urls.primaryName=treasure-solver#/Treasure%20Solver/TreasureSolver_FindNextPosition)

The treasure solver uses the path finder to find the adjacent maps during the treasure hunts. It also uses the same `FindNodeRequest` system to find the starting position.
See [Path Finder - The tricky part about paths](https://github.com/Dofus-Batteries-Included/DBI.Api#the-tricky-part-about-paths).

To solve a step of the treasure hunt, call the [Find next clue](https://api.dofusbatteriesincluded.fr/swagger/index.html?urls.primaryName=treasure-solver#/Treasure%20Solver/TreasureSolver_FindNextClue) endpoint with the request to find the start node, the direction and the clue ID.

<details>
  <summary>
    <b>Example</b>
  </summary>

  __Request__
  ```
  curl -X 'POST' \
    'https://api.dofusbatteriesincluded.fr/treasure-solver/find-next-clue' \
    -H 'accept: application/json' \
    -H 'Content-Type: application/json' \
    -d '{
    "start": {
      "search": "by-map",
      "mapId": 68552449
    },
    "direction": "west",
    "clueId": 968
  }'
  ```

  __Response__
  ```json
  {
    "found": true,
    "map": {
      "mapPosition": {
        "x": 4,
        "y": -9
      },
      "nodeId": 9826,
      "mapId": 190579205,
      "zoneId": 1
    },
    "distance": 1
  }
  ```
</details>

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

<details>
  <summary>
    <b>Example</b>
  </summary>

__Request__
  ```
  curl -X 'GET' \
    'https://api.dofusbatteriesincluded.fr/treasure-solver/clues/export' \
    -H 'accept: application/json'
  ```
</details>

### How to register clues

The `Register clues` endpoint requires an API key.
API keys are used to associate clues to accounts, which allow to deduplicate records: the `(author, clue-id, map-id)` tuple is unique in the records database.

#### Register account

[Try it!](https://api.dofusbatteriesincluded.fr/swagger/index.html?urls.primaryName=identity#/Registration/Registration_Register)

First call the `Register account` endpoint and provide the dofus account ID and dofus account nickname that will be the author of the clues. The endpoint returns an API key that should be provided to the server in the `Authorization` header.

<details>
  <summary>
    <b>Example</b>
  </summary>

__Request__
  ```
  curl -X 'GET' \
    'https://api.dofusbatteriesincluded.fr/identity/register?accountId={ACCOUNT_ID}&accountName={ACCOUNT_NAME}' \
    -H 'accept: application/json'
  ```

__Response__
  ```json
  "ed2defea-5925-45e5-b286-d31d10194e6f"
  ```
</details>

#### Register clues

[Try it!](https://api.dofusbatteriesincluded.fr/swagger/index.html?urls.primaryName=treasure-solver#/Clues/Clues_RegisterClues)

Once the API key has been retrieved, it can be used to register clues.
In order to minimize chattiness, multiple clues can be registered in a single request. Each clue can be marked as found or not.
Clues that are marked as not found are removed from the data sets.

<details>
  <summary>
    <b>Example</b>
  </summary>

__Request__
  ```
  curl -X 'POST' \
    'https://api.dofusbatteriesincluded.fr/treasure-solver/clues' \
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
</details>