# Dofus Batteries Included - Path Finder

Implementation of the dofus path finder used by [the DBI.Api project](https://github.com/Dofus-Batteries-Included/DBI.Api).
This project is also published as a standalone nuget package so that it can be reused without the need of calling the API.

## The tricky part about paths

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

The first step is to retrieve the game data. The easiest way is to download it from the DDC repository releases. That data can then be used to instantiate the `NodeFinder` 
and the `PathFinder`.

```csharp
IWorldDataProvider worldData = WorldDataBuilder.FromDdcGithubRepository().BuildAsync();
NodeFinder nodeFinder = new NodeFinder(worldData);
PathFinder nodeFinder = new PathFinder(worldData);
```

The path finder requires the start and end node in the graph to search for a path between them.
The node finder is used to find the start and end node. There are multiple ways to look for them:

- `FindNodeById`, from the `nodeId`: the easiest for the node finder, it is the unique identifier of a node. This shifts the burden of finding the right node to the caller of the API.
  <details>
    <summary>
      <b>Example</b>
    </summary>

  __Code__
  ```csharp
  IEnumerable<RawWorldGraphNode> nodes = nodeFinder.FindNodes(new FindNodesById { NodeId = 7911 });
  ```
  
  __Result__
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

- `FindNodeByMap`, from the `mapId` and `cellNumber`: the second-best option because it always leads to a unique node. The node finder can extract the nodes in the map and the `zoneId` of the cell, using both these information it can find a unique node.
  <details>
    <summary>
      <b>Example</b>
    </summary>

  __Code__
  ```csharp
  IEnumerable<RawWorldGraphNode> nodes = nodeFinder.FindNodes(new FindNodesByMap { MapId = 106693122, CellNumber = 425 });
  ```

  __Result__
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

  __Code__
  ```csharp
  IEnumerable<RawWorldGraphNode> nodes = nodeFinder.FindNodes(new FindNodesByMap { MapId = 106693122 });
  ```

  __Result__
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

- `FindNodeAtPosition` from the map coordinates: the node finder can extract all the maps at those coordinates, and all the nodes in those maps. There are high changes that multiple nodes match the coordinates.
  <details>
    <summary>
      <b>Example</b>
    </summary>

  __Code__
  ```csharp
  IEnumerable<RawWorldGraphNode> nodes = nodeFinder.FindNodes(new FindNodeAtPosition { Position = new Position(26, -9) });
  ```

  __Result__
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

Using the results, we can then use the path finder to find a path between two nodes.

<details>
  <summary>
  <b>Example</b>: in this example we provide both the map ids and the cell numbers, there is only one candidate for the start and end node
  </summary>

  __Code__
  ```csharp
  RawWorldGraphNode from = nodeFinder.FindNodes(new FindNodesByMap { MapId = 75497730, CellNumber = 425 }).Single();
  RawWorldGraphNode to = nodeFinder.FindNodes(new FindNodesByMap { MapId = 75498242, CellNumber = 430 }).Single();
  Path? GetShortestPath = pathFinder.GetShortestPath(from, to);
  ```

  __Result__
  ```json
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
  ```
</details>