using DBI.DataCenter.Raw.Models.WorldGraphs;
using DBI.DataCenter.Raw.Services.WorldGraphs;
using DBI.DataCenter.Structured.Models.Maps;

namespace DBI.DataCenter.Structured.Services.World;

/// <summary>
///     Get world graph data
/// </summary>
public class WorldGraphService(RawWorldGraphService? rawWorldGraphService)
{
    /// <summary>
    ///     Get all the nodes in the given map.
    /// </summary>
    public IEnumerable<MapNode>? GetNodesInMap(long mapId) => rawWorldGraphService?.GetNodesInMap(mapId).Select(node => node.Cook());

    /// <summary>
    ///     Get all the transitions going out of the given map.
    /// </summary>
    public IEnumerable<MapTransition>? GetTransitionsFromMap(long mapId) =>
        rawWorldGraphService?.GetNodesInMap(mapId)
            .SelectMany(
                n => rawWorldGraphService.GetEdgesFrom(n.Id)
                    .SelectMany(
                        e =>
                        {
                            RawWorldGraphNode? fromNode = rawWorldGraphService.GetNode(e.From);
                            RawWorldGraphNode? toNode = rawWorldGraphService.GetNode(e.To);

                            return fromNode != null && toNode != null && e.Transitions != null ? e.Transitions.Select(t => t.Cook(fromNode, toNode)) : [];
                        }
                    )
            );

    /// <summary>
    ///     Get all the transitions going in the given map.
    /// </summary>
    public IEnumerable<MapTransition>? GetTransitionsToMap(long mapId) =>
        rawWorldGraphService?.GetNodesInMap(mapId)
            .SelectMany(
                n => rawWorldGraphService.GetEdgesTo(n.Id)
                    .SelectMany(
                        e =>
                        {
                            RawWorldGraphNode? fromNode = rawWorldGraphService.GetNode(e.From);
                            RawWorldGraphNode? toNode = rawWorldGraphService.GetNode(e.To);

                            return fromNode != null && toNode != null && e.Transitions != null ? e.Transitions.Select(t => t.Cook(fromNode, toNode)) : [];
                        }
                    )
            );
}
