using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.RTS.PathFinding
{
    public unsafe static class PathFinder
    {
        public unsafe static List<EntityRef> FindPath(Frame f,GridDataLink* grid, int startLine,int startIndex, int targetLine, int targetIndex)
        {
            var nodes = ConvertGridToNode(f, grid);
            var calculatedPath = CalculatePath(nodes, nodes[startLine, startIndex], nodes[targetLine, targetIndex]);
            if (calculatedPath == null) return null;

            List<EntityRef> result = new List<EntityRef>();
            foreach (var node in calculatedPath)
            {
                result.Add(grid->gridLayout[node.X].grids[node.Y]);
            }
            return result;
        }

        private unsafe static Node[,] ConvertGridToNode(Frame f,GridDataLink* grid)
        {
            Node[,] nodes = new Node[grid->gridLayout.Length, grid->gridLayout[0].grids.Length];

            for(int i = 0; i < nodes.GetLength(0); i++)
            {
                for(int j = 0; j < nodes.GetLength(1); j++)
                {
                    if (f.Unsafe.TryGetPointer(grid->gridLayout[i].grids[j], out Grid* g)) {

                        nodes[i, j] = new Node(i, j, !g->isObstacle);
                    }
                }
            }
            return nodes;
        }

        private static List<Node> CalculatePath(Node[,] nodes, Node start, Node goal)
        {
            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(start);

            while (openSet.Count > 0)
            {
                Node current = GetLowestFScoreNode(openSet);
                if (current == goal)
                {
                    return ReconstructPath(current);
                }
                openSet.Remove(current);
                closedSet.Add(current);
                // Consider neighbors based on the grid
                foreach (Node neighbor in GetNeighbors(nodes, current))
                {
                    if (!neighbor.IsWalkable || closedSet.Contains(neighbor))
                        continue;
                    int tentativeGScore = current.G + 1; // Assuming a uniform cost for each step
                    if (!openSet.Contains(neighbor) || tentativeGScore < neighbor.G)
                    {
                        neighbor.Parent = current;
                        neighbor.G = tentativeGScore;
                        neighbor.H = CalculateHeuristic(neighbor, goal);
                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }
                }
            }
            return null;
        }

        // Helper function to get neighbors based on the grid
        private static IEnumerable<Node> GetNeighbors(Node[,] grid, Node node)
        {
            int maxX = grid.GetLength(0);
            int maxY = grid.GetLength(1);

            int x = node.X;
            int y = node.Y;

            // Check and add valid neighbors
            if (x > 0) yield return grid[x - 1, y]; // Left
            if (x < maxX - 1) yield return grid[x + 1, y]; // Right
            if (y > 0) yield return grid[x, y - 1]; // Top
            if (y < maxY - 1) yield return grid[x, y + 1]; // Bottom
        }

        private static Node GetLowestFScoreNode(List<Node> nodes)
        {
            Node lowest = nodes[0];

            foreach (Node node in nodes)
            {
                if (node.F < lowest.F)
                    lowest = node;
            }

            return lowest;
        }

        private static int CalculateHeuristic(Node a, Node b)
        {
            // Manhattan distance as a heuristic
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        private static List<Node> ReconstructPath(Node node)
        {
            List<Node> path = new List<Node>();
            while (node != null)
            {
                path.Insert(0, node);
                node = node.Parent;
            }
            return path;
        }
    }

public class Node
{
    public int X { get; set; }
    public int Y { get; set; }
    public bool IsWalkable { get; set; }
    public List<Node> Neighbors { get; set; }
    public Node Parent { get; set; }
    public int G { get; set; } // Cost from the start node to this node
    public int H { get; set; } // Heuristic estimated cost from this node to the goal node
    public int F => G + H;      // Total cost (F = G + H)

    public Node(int x, int y, bool isWalkable)
    {
        X = x;
        Y = y;
        IsWalkable = isWalkable;
        Neighbors = new List<Node>();
    }
}
}
